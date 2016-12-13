using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FinanceApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
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
        public void Get_should_return_NotModified_when_ETag_match_current_version()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);

            var expected = new GetInvoiceBuilder().Build();
            getInvoiceMapperMock.Setup(map => map.ToModel(invoice)).Returns(expected);

            testSubject.Request.Headers.Add(HeaderNames.IfNoneMatch, $"\"{Guid.Empty}\"");

            // Act
            var response = testSubject.Get(Id) as StatusCodeResult;

            // Assert
            Assert.Equal((int) HttpStatusCode.NotModified, response.StatusCode);
        }

        [Fact]
        public void Get_should_have_etag_header()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();

            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);

            var expected = new GetInvoiceBuilder().Build();
            getInvoiceMapperMock.Setup(map => map.ToModel(invoice)).Returns(expected);

            // Act
            testSubject.Get(Id);

            // Assert
            var headers = testSubject.Response.GetTypedHeaders();
            Assert.NotNull(headers.ETag);
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
            invoiceRepositoryMock.Setup(r => r.Update(It.IsAny<Domain.Invoice>())).Returns(new Version("2"));

            // Act
            var response = testSubject.Put(Id, invoice);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Put_should_return_ETag()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Exists(It.IsAny<string>())).Returns(true);
            invoiceRepositoryMock.Setup(r => r.Update(It.IsAny<Domain.Invoice>())).Returns(new Version("2"));

                // Act
            testSubject.Put(Id, invoice);

            // Assert
            var headers = testSubject.Response.GetTypedHeaders();
            Assert.NotNull(headers.ETag);
        }

        [Fact]
        public void Put_should_return_PreconditionFailed_When_Etag_Is_Wrong()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Exists(It.IsAny<string>())).Returns(true);
            invoiceRepositoryMock.Setup(r => r.Update(It.IsAny<Domain.Invoice>())).Returns(new Version("2"));
            testSubject.Request.Headers.Add(HeaderNames.IfMatch, $"\"unknown\"");

            // Act
            var response = testSubject.Put(Id, invoice);

            // Assert
            Assert.Equal((int)HttpStatusCode.PreconditionFailed, ((StatusCodeResult)response).StatusCode);
        }

        [Fact]
        public void Put_should_return_NoContent_When_Etag_Is_Wildcard()
        {
            // Arrange
            var invoice = new UpdateInvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Exists(It.IsAny<string>())).Returns(true);
            invoiceRepositoryMock.Setup(r => r.Update(It.IsAny<Domain.Invoice>())).Returns(new Version("2"));
            testSubject.Request.Headers.Add(HeaderNames.IfMatch, EntityTagHeaderValue.Any.Tag);

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
        public void Patch_should_return_NoContent_When_Etag_Is_Wildcard()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);
            invoiceRepositoryMock.Setup(r => r.Update(It.IsAny<Domain.Invoice>())).Returns(new Version("2"));

            var updateInvoice = new UpdateInvoiceBuilder().Build();
            updateInvoiceMapperMock.Setup(mapper => mapper.ToModel(invoice)).Returns((updateInvoice));

            testSubject.Request.Headers.Add(HeaderNames.IfMatch, EntityTagHeaderValue.Any.Tag);

            // Act
            var response = testSubject.Patch(Id, new JsonPatchDocument<UpdateInvoice>());

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Patch_should_return_ETag()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();
            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);
            invoiceRepositoryMock.Setup(r => r.Update(It.IsAny<Domain.Invoice>())).Returns(new Version("2"));
            var updateInvoice = new UpdateInvoiceBuilder().Build();
            updateInvoiceMapperMock.Setup(mapper => mapper.ToModel(invoice)).Returns((updateInvoice));

            // Act
            testSubject.Patch(Id, new JsonPatchDocument<UpdateInvoice>());

            // Assert
            var headers = testSubject.Response.GetTypedHeaders();
            Assert.NotNull(headers.ETag);
        }

        [Fact]
        public void Patch_should_return_PreconditionFailed_When_Etag_Is_Wrong()
        {
            // Arrange
            var invoice = new InvoiceBuilder().Build();

            invoiceRepositoryMock.Setup(r => r.Get(Id)).Returns(invoice);
            testSubject.Request.Headers.Add(HeaderNames.IfMatch, $"\"unknown\"");

            var updateInvoice = new UpdateInvoiceBuilder().Build();
            updateInvoiceMapperMock.Setup(mapper => mapper.ToModel(invoice)).Returns((updateInvoice));

            // Act
            var response = testSubject.Patch(Id, new JsonPatchDocument<UpdateInvoice>());

            // Assert
            Assert.Equal((int)HttpStatusCode.PreconditionFailed, ((StatusCodeResult)response).StatusCode);
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
            Assert.True(testSubject.HttpContext
                .Response.Headers.ContainsKey(HeaderNames.Allow));
        }

        [Fact]
        public void Options_should_return_allow_POST_OPTIONS()
        {
            // Act
            testSubject.Options();

            // Act
            var methodsAllowed = testSubject.HttpContext.Response
                .Headers[HeaderNames.Allow].FirstOrDefault().Split(',');

            // Assert
            Assert.True(methodsAllowed.Contains(HttpVerbs.Post));
            Assert.True(methodsAllowed.Contains(HttpVerbs.Options));
        }

        [Fact]
        public void OptionsForInvoice_should_return_GET_PUT_OPTIONS_HEAD_if_invoice_exists()
        {
            // Act
            invoiceRepositoryMock.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            testSubject.OptionsForInvoice(Id);

            // Assert
            var methodsAllowed = testSubject.HttpContext.Response
                .Headers[HeaderNames.Allow].FirstOrDefault().Split(',');
            Assert.True(methodsAllowed.All(ma => new [] { HttpVerbs.Get, HttpVerbs.Put,
                HttpVerbs.Patch, HttpVerbs.Options, HttpVerbs.Head }.Contains(ma)));
            Assert.False(methodsAllowed.Contains(HttpVerbs.Post));
        }

        [Fact]
        public void OptionsForInvoice_should_return_GET_HEAD_OPTIONS_if_invoice_does_not_exist()
        {
            // Arrange
            invoiceRepositoryMock.Setup(r => r.Exists(Id)).Returns(false);

            // Act
            testSubject.OptionsForInvoice(Id);

            // Assert
            var methodsAllowed = testSubject.HttpContext.Response
                .Headers[HeaderNames.Allow].FirstOrDefault().Split(',');
            Assert.True(methodsAllowed.All(ma => new [] { HttpVerbs.Get,
                HttpVerbs.Options, HttpVerbs.Head }.Contains(ma)));
            Assert.False(methodsAllowed.Any(ma => new [] { HttpVerbs.Put,
                HttpVerbs.Patch, HttpVerbs.Post }.Contains(ma)));
        }

        [Fact]
        public void HeadForInvoice_should_return_NoContent_for_existing_invoice()
        {
            // Arrange
            invoiceRepositoryMock.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            var response = testSubject.HeadForInvoice(Id);

            // Assert
            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void HeadForInvoice_should_return_NotFound_for_nonexisting_invoice()
        {
            // Arrange
            invoiceRepositoryMock.Setup(r => r.Exists(Id)).Returns(false);

            // Act
            var response = testSubject.HeadForInvoice(Id);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void HeadForInvoice_should_return_ContentType()
        {
            // Arrange
            invoiceRepositoryMock.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            testSubject.HeadForInvoice(Id);

            // Assert
            Assert.Equal(testSubject.Response.ContentType, ApiDefinition.ApiMediaType);
        }
    }
}