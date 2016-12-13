using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace FinanceApi.Controllers
{
    using Mappers;
    using Finance.Domain.Repositories;
    using Domain = Finance.Domain.Domain;
    using Models;

    [Route("invoices")]
    public sealed class InvoicesController : Controller
    {
        private readonly IRepository<Domain.Invoice> invoiceRepository;
        private readonly IModelDomainMapper<GetInvoice, Domain.Invoice> getInvoiceMapper;
        private readonly IModelDomainMapper<UpdateInvoice, Domain.Invoice> updateInvoiceMapper;

        public InvoicesController(IRepository<Domain.Invoice> invoiceRepository,
            IModelDomainMapper<GetInvoice, Domain.Invoice> getInvoiceMapper,
            IModelDomainMapper<UpdateInvoice, Domain.Invoice> updateInvoiceMapper)
        {
            this.invoiceRepository = invoiceRepository;
            this.getInvoiceMapper = getInvoiceMapper;
            this.updateInvoiceMapper = updateInvoiceMapper;
        }

        [HttpGet("{id}", Name = "GetInvoice")]
        public IActionResult Get(string id)
        {
            var invoice = invoiceRepository.Get(id);

            if (invoice == null)
            {
                return NotFound();
            }

            var currentETag = new EntityTagHeaderValue($"\"{invoice.Version}\"");
            if (IfMatchGivenIfNoneMatch(currentETag))
            {
                return StatusCode((int)HttpStatusCode.NotModified);
            }

            var responseHeaders = Response.GetTypedHeaders();
            responseHeaders.ETag = currentETag;

            return Ok(getInvoiceMapper.ToModel(invoice));
        }

        private bool IfMatchGivenIfNoneMatch(EntityTagHeaderValue currentETag)
        {
            var requestHeaders = Request.GetTypedHeaders();
            return requestHeaders.IfNoneMatch != null &&
                   requestHeaders.IfNoneMatch.Contains(currentETag);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UpdateInvoice updateInvoice)
        {
            if (updateInvoice == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdInvoice = invoiceRepository.Create(updateInvoiceMapper.ToDomain(updateInvoice));

            return CreatedAtRoute("GetInvoice",
                new { id = createdInvoice.Id },
                getInvoiceMapper.ToModel(createdInvoice));
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] UpdateInvoice updateInvoice)
        {
            if (updateInvoice == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!invoiceRepository.Exists(id))
            {
                return NotFound();
            }

            if (IfMatchIsInvalid(invoiceRepository.GetCurrentVersion(id)))
            {
                return StatusCode((int) HttpStatusCode.PreconditionFailed);
            }

            var newVersion = invoiceRepository.Update(updateInvoiceMapper.ToDomain(
                updateInvoice, id)).Value;

            var responseHeaders = Response.GetTypedHeaders();
            responseHeaders.ETag = new EntityTagHeaderValue($"\"{newVersion}\"");;

            return NoContent();
        }

        private bool IfMatchIsInvalid(string currentVersion)
        {
            var requestHeaders = Request.GetTypedHeaders();
            var currentETag = new EntityTagHeaderValue($"\"{currentVersion}\"");

            return requestHeaders.IfMatch != null &&
                   !requestHeaders.IfMatch.Any(ifm => ifm.Equals(EntityTagHeaderValue.Any))
                   && requestHeaders.IfMatch.Contains(currentETag);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id, [FromBody] JsonPatchDocument<UpdateInvoice> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var invoice = invoiceRepository.Get(id);
            if (invoice == null)
            {
                return NotFound();
            }

            if (IfMatchIsInvalid(invoice.Version))
            {
                return StatusCode((int) HttpStatusCode.PreconditionFailed);
            }

            var updateInvoice = updateInvoiceMapper.ToModel(invoice);
            patchDocument.ApplyTo(updateInvoice, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedDomainInvoice = updateInvoiceMapper.ToDomain(updateInvoice, id);
            var newVersion = invoiceRepository.Update(updatedDomainInvoice);

            var responseHeaders = Response.GetTypedHeaders();
            responseHeaders.ETag = new EntityTagHeaderValue($"\"{newVersion}\"");;

            return NoContent();
        }

        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add(HeaderNames.Allow, string.Join(",", HttpVerbs.Options, HttpVerbs.Post));
            return NoContent();
        }

        [HttpOptions("{id}")]
        public IActionResult OptionsForInvoice(string id)
        {
            if (!invoiceRepository.Exists(id))
            {
                Response.Headers.Add(HeaderNames.Allow, string.Join(",",
                    HttpVerbs.Options,
                    HttpVerbs.Get,
                    HttpVerbs.Head));
            }
            else
            {
                Response.Headers.Add(HeaderNames.Allow, string.Join(",",
                    HttpVerbs.Options,
                    HttpVerbs.Get,
                    HttpVerbs.Head,
                    HttpVerbs.Put,
                    HttpVerbs.Patch));
            }
            return NoContent();
        }

        [HttpHead("{id}")]
        public IActionResult HeadForInvoice(string id)
        {
            Response.ContentType = ApiDefinition.ApiMediaType;
            Response.GetTypedHeaders();

            if (!invoiceRepository.Exists(id))
            {
                return NotFound();
            }
            return Ok();
        }
    }
}