using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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

            return Ok(getInvoiceMapper.ToModel(invoice));
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

            invoiceRepository.Update(updateInvoiceMapper.ToDomain(updateInvoice, id));

            return NoContent();
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

            var updateInvoice = updateInvoiceMapper.ToModel(invoice);
            patchDocument.ApplyTo(updateInvoice, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedDomainInvoice = updateInvoiceMapper.ToDomain(updateInvoice, id);
            invoiceRepository.Update(updatedDomainInvoice);

            return NoContent();
        }

        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", string.Join(",", HttpVerbs.Options, HttpVerbs.Post));
            return NoContent();
        }

        [HttpOptions("{id}")]
        public IActionResult OptionsForInvoice(string id)
        {
            if (!invoiceRepository.Exists(id))
            {
                Response.Headers.Add("Allow", string.Join(",",
                    HttpVerbs.Options,
                    HttpVerbs.Get));
            }
            else
            {
                Response.Headers.Add("Allow", string.Join(",",
                    HttpVerbs.Options,
                    HttpVerbs.Get,
                    HttpVerbs.Put,
                    HttpVerbs.Patch));
            }
            return NoContent();
        }
    }
}