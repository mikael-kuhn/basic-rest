using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using FinanceApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace FinanceApiTest.Controllers
{
    using FinanceApi.Mappers;
    using Builders.Models;
    using Builders.Domain;
    using FinanceApi.Controllers;
    using Domain = Finance.Domain.Domain;
    using Finance.Domain.Repositories;
    using FinanceApi.Models;


    public sealed class UnknownDto
    {
        public string UnknownProperty { get; set; }
    }

    public sealed class InvoicesControllerTest
    {
        private readonly InvoicesController testSubject;
        private readonly Mock<IRepository<Domain.Invoice>> invoiceRepositoryMock;
        private readonly Mock<IModelDomainMapper<GetInvoice, Domain.Invoice>> getInvoiceMapperMock;
        private readonly Mock<IModelDomainMapper<UpdateInvoice, Domain.Invoice>> updateInvoiceMapperMock;
        private const string Id = "1";

        public InvoicesControllerTest()
        {
            invoiceRepositoryMock = new Mock<IRepository<Domain.Invoice>>();
            getInvoiceMapperMock = new Mock<IModelDomainMapper<GetInvoice, Domain.Invoice>>();
            updateInvoiceMapperMock = new Mock<IModelDomainMapper<UpdateInvoice, Domain.Invoice>>();

            testSubject = new InvoicesController(invoiceRepositoryMock.Object,
                getInvoiceMapperMock.Object,
                updateInvoiceMapperMock.Object);

            testSubject.ControllerContext = new ControllerContext(new ActionContext(new DefaultHttpContext(),
                new RouteData(), new ControllerActionDescriptor()));
        }

        [Fact]
        public void Get_should_return_NotFound_when_invoice_does_not_exist()
        {
            // Act
            var statusCode = ((NotFoundResult) testSubject.Get("Unknown")).StatusCode;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void Get_should_return_invoice()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);

            var expected = new GetInvoiceBuilder().Build();
            getInvoiceMapperMock.Setup(map => map.ToModel(invoice)).Returns(expected);

            // Act
            var response = testSubject.Get(Id) as ObjectResult;

            // Assert
            Assert.IsType<GetInvoice>(response.Value);
            Assert.Equal((int) HttpStatusCode.OK, response.StatusCode.Value);
            Assert.Equal(expected, response.Value);
        }

        [Fact]
        public void Post_should_not_accept_malformed_request()
        {
            // Act
            var response = testSubject.Post(null);

            // Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public void Post_should_not_accept_invalid_request()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            testSubject.ModelState.AddModelError("name", "some validation failed");

            // Act
            var response = testSubject.Post(invoice);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Post_should_return_created_response()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            var domainInvoice = new InvoiceBuilder().Build();
            var getInvoice = new GetInvoiceBuilder().Build();

            invoiceRepositoryMock.Setup(r => r.Create(It.IsAny<Domain.Invoice>())).Returns(domainInvoice);
            getInvoiceMapperMock.Setup(mapper => mapper.ToModel(domainInvoice)).Returns(getInvoice);

            // Act
            var response = testSubject.Post(invoice);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(response);
        }

        [Fact]
        public void Put_should_not_accept_malformed_request()
        {
            // Act
            var response = testSubject.Put(Id, null);

            // Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public void Put_should_not_accept_invalid_request()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            testSubject.ModelState.AddModelError("name", "some validation failed");

            // Act
            var response = testSubject.Put(Id, invoice);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Put_should_return_NoContent()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Exists(It.IsAny<string>())).Returns(true);

            // Act
            var response = testSubject.Put(Id, invoice);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Put_should_return_NotFound_if_invoice_does_not_exist()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Exists(It.IsAny<string>())).Returns(false);

            // Act
            var response = testSubject.Put(Id, invoice);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void Patch_should_not_accept_malformed_request()
        {
            // Act
            var response = testSubject.Patch(Id, null);

            // Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public void Patch_should_return_NotFound_if_invoice_does_not_exist()
        {
            // Arrange
            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns((Domain.Invoice)null);

            // Act
            var response = testSubject.Patch(Id, new JsonPatchDocument<UpdateInvoice>());

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void Patch_should_return_NoContent()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);
            var updateInvoice = new UpdateInvoiceBuilder().Build();
            updateInvoiceMapperMock.Setup(mapper => mapper.ToModel(invoice)).Returns((updateInvoice));

            // Act
            var response = testSubject.Patch(Id, new JsonPatchDocument<UpdateInvoice>());

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Patch_with_illegal_request_should_return_BadRequest()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);
            var updateInvoice = new UpdateInvoiceBuilder().Build();
            updateInvoiceMapperMock.Setup(mapper => mapper.ToModel(invoice)).Returns((updateInvoice));


            var patchDocument = new JsonPatchDocument<UpdateInvoice>(
                new List<Operation<UpdateInvoice>>
                {
                    new Operation<UpdateInvoice>("replace", @"/unknown", null, 32)
                },
                new DefaultContractResolver());

            // Act
            var response = testSubject.Patch(Id, patchDocument);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public void Options_should_return_NoContent()
        {
            // Act
            var response = testSubject.Options();

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Options_should_return_allow_header()
        {
            // Act
            testSubject.Options();

            // Assert
            Assert.True(testSubject.HttpContext.Response.Headers.ContainsKey("Allow"));
        }

        [Fact]
        public void Options_should_return_allow_POST_OPTIONS()
        {
            // Act
            testSubject.Options();

            // Act
            var methodsAllowed = testSubject.HttpContext.Response.Headers["Allow"].FirstOrDefault().Split(',');

            // Assert
            Assert.True(methodsAllowed.Contains(HttpVerbs.Post));
            Assert.True(methodsAllowed.Contains(HttpVerbs.Options));
        }

        [Fact]
        public void OptionsForInvoice_should_return_GET_PUT_OPTIONS_if_invoice_exists()
        {
            // Act
            invoiceRepositoryMock.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            testSubject.OptionsForInvoice(Id);

            // Assert
            var methodsAllowed = testSubject.HttpContext.Response.Headers["Allow"].FirstOrDefault().Split(',');
            Assert.True(methodsAllowed.Contains(HttpVerbs.Get));
            Assert.True(methodsAllowed.Contains(HttpVerbs.Put));
            Assert.True(methodsAllowed.Contains(HttpVerbs.Patch));
            Assert.True(methodsAllowed.Contains(HttpVerbs.Options));
            Assert.False(methodsAllowed.Contains(HttpVerbs.Post));
        }

        [Fact]
        public void OptionsForInvoice_should_return_GET_PUT_if_invoice_does_not_exist()
        {
            // Arrange
            invoiceRepositoryMock.Setup(r => r.Exists(Id)).Returns(false);

            // Act
            testSubject.OptionsForInvoice(Id);

            // Assert
            var methodsAllowed = testSubject.HttpContext.Response.Headers["Allow"].FirstOrDefault().Split(',');
            Assert.True(methodsAllowed.Contains(HttpVerbs.Options));
            Assert.True(methodsAllowed.Contains(HttpVerbs.Get));
            Assert.False(methodsAllowed.Contains(HttpVerbs.Put));
            Assert.False(methodsAllowed.Contains(HttpVerbs.Patch));
            Assert.False(methodsAllowed.Contains(HttpVerbs.Post));
        }
    }
}