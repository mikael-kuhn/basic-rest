using System.Linq;
using System.Net;
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

        [HttpGet("{id}")]
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
    }
}