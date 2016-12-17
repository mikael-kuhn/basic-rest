using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace FinanceApi.Controllers
{
    using Finance.Domain.Domain;
    using Finance.Domain.Repositories;

    /// <summary>
    /// This is not a complete implementation. It is for blog purposes only
    /// </summary>
    [Route("files")]
    public sealed class FilesController : Controller
    {
        private readonly IRepository<File> fileRepository;

        public FilesController(IRepository<File> fileRepository)
        {
            this.fileRepository = fileRepository;
        }

        [HttpGet("{id}", Name = "GetFile")]
        [ResponseCache(CacheProfileName =  "Default")]
        public IActionResult Get(string id)
        {
            var file = fileRepository.Get(id);

            if (file == null)
            {
                return NotFound();
            }

            var requestHeaders = Request.GetTypedHeaders();

            if (requestHeaders.IfNoneMatch == null &&
                requestHeaders.IfModifiedSince.HasValue
                && requestHeaders.IfModifiedSince.Value >= file.LastModified)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            if (!file.IsAvailable)
            {
                Response.Headers.Add(HeaderNames.RetryAfter, "60");
                return NoContent();
            }

            var responseHeaders = Response.GetTypedHeaders();
            responseHeaders.LastModified = file.LastModified;

            return File(file.Content, file.ContentType);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (!fileRepository.Exists(id))
            {
                return NotFound();
            }

            // Yeah this you would never do in real life
            var file = fileRepository.Get(id);

            var requestHeaders = Request.GetTypedHeaders();
            if (requestHeaders.IfMatch != null)
            {
                if (!requestHeaders.IfMatch.All(match => match.Equals(EntityTagHeaderValue.Any)))
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }
            }
            else
            {
                if (requestHeaders.IfUnmodifiedSince.HasValue
                    && requestHeaders.IfUnmodifiedSince.Value < file.LastModified)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }
            }

            fileRepository.Delete(id);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Post()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
            {
                return BadRequest();
            }

            File newFile;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                newFile = new File(stream.ToArray(), DateTimeOffset.Now, file.ContentType);
                fileRepository.Create(newFile);
            }

            return new AcceptedAtRouteResult("GetFile", new { id = newFile.Id}, null);
        }
    }
}