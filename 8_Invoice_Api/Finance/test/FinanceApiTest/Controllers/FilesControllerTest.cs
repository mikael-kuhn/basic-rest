using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Net.Http.Headers;
using Moq;
using Xunit;

namespace FinanceApiTest.Controllers
{
    using Finance.Domain.Domain;
    using Finance.Domain.Repositories;
    using FinanceApi.Controllers;
    using Builders;

    public sealed class FilesControllerTest
    {
        private readonly FilesController testSubject;
        private readonly Mock<IRepository<File>> fileRepository;
        public const string Id = "1";

        public FilesControllerTest()
        {
            fileRepository = new Mock<IRepository<File>>();
            testSubject = new FilesController(fileRepository.Object);
            testSubject.ControllerContext = new ControllerContext(new ActionContext(new DefaultHttpContext(),
                new RouteData(), new ControllerActionDescriptor()));

        }

        [Fact]
        public void Get_should_return_LastModified_header()
        {
            // Arrange
            var file = new FileBuilder().Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);

            // Act
            testSubject.Get(Id);

            // Assert
            var headers = testSubject.Response.GetTypedHeaders();
            Assert.NotNull(headers.LastModified);
        }

        [Fact]
        public void Get_should_return_NotFound()
        {
            // Act
            var response = testSubject.Get("unknown");

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void Get_should_return_file()
        {
            // Arrange
            var file = new FileBuilder().Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);

            // Act
            var response = testSubject.Get(Id);

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public void Get_with_IfModifiedSince_should_return_NotModified()
        {
            // Arrange
            var lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var requestHeaders = testSubject.Request.GetTypedHeaders();
            requestHeaders.IfModifiedSince = new DateTimeOffset(2016, 12, 13, 0, 0, 0, TimeSpan.Zero);

            var file = new FileBuilder().WithLastModified(lastModified).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);

            // Act
            var response = (StatusCodeResult)testSubject.Get(Id);

            // Assert
            Assert.Equal(StatusCodes.Status304NotModified, response.StatusCode);
        }

        [Fact]
        public void Get_with_IfModifiedSince_and_IfNoneMatch_should_disregard_IfModifiedSince()
        {
            // Arrange
            var lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var requestHeaders = testSubject.Request.GetTypedHeaders();
            requestHeaders.IfModifiedSince = new DateTimeOffset(2016, 12, 13, 0, 0, 0, TimeSpan.Zero);
            requestHeaders.IfNoneMatch = new List<EntityTagHeaderValue> { new EntityTagHeaderValue($"\"{Guid.Empty}\"")};

            var file = new FileBuilder().WithLastModified(lastModified).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);

            // Act
            var response = testSubject.Get(Id);

            // Assert
            Assert.IsType<FileContentResult>(response);
        }

        [Fact]
        public void Get_with_IfModifiedSince__should_return_file()
        {
            // Arrange
            var lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var requestHeaders = testSubject.Request.GetTypedHeaders();
            requestHeaders.IfModifiedSince = new DateTimeOffset(2016, 12, 11, 0, 0, 0, TimeSpan.Zero);

            var file = new FileBuilder().WithLastModified(lastModified).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);

            // Act
            var response = testSubject.Get(Id);

            // Assert
            Assert.IsType<FileContentResult>(response);
        }

        [Fact]
        public void Get_should_return_NoContent_when_not_available()
        {
            // Accept
            var file = new FileBuilder().SetIsAvailable(false).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);

            // Act
            var response = testSubject.Get(Id);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Get_should_return_RetryAfter_when_not_available()
        {
            // Accept
            var file = new FileBuilder().SetIsAvailable(false).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);

            // Act
            testSubject.Get(Id);

            // Assert
            Assert.True(testSubject.Response.Headers.ContainsKey(HeaderNames.RetryAfter));
        }

        [Fact]
        public void Delete_should_return_NoContent()
        {
            // Arrange
            var file = new FileBuilder().Build();
            fileRepository.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            var response = testSubject.Delete(Id);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Delete_should_return_NotFound()
        {
            // Act
            var response = testSubject.Delete("unknown");

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void Delete_should_return_PreconditionFailed_when_it_has_been_modified()
        {
            // Arrange
            var lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var requestHeaders = testSubject.Request.GetTypedHeaders();
            requestHeaders.IfUnmodifiedSince = new DateTimeOffset(2016, 12, 11, 0, 0, 0, TimeSpan.Zero);

            var file = new FileBuilder().WithLastModified(lastModified).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);
            fileRepository.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            var response = (StatusCodeResult) testSubject.Delete(Id);

            // Assert
            Assert.Equal(StatusCodes.Status412PreconditionFailed, response.StatusCode);
        }

        [Fact]
        public void Delete_should_return_NoContent_when_unmodified()
        {
            // Arrange
            var lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var requestHeaders = testSubject.Request.GetTypedHeaders();
            requestHeaders.IfUnmodifiedSince = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var file = new FileBuilder().WithLastModified(lastModified).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);
            fileRepository.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            var response = testSubject.Delete(Id);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public void Delete_should_return_PreconditionFailed_when_ETag_is_used()
        {
            // Arrange
            var lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var requestHeaders = testSubject.Request.GetTypedHeaders();
            requestHeaders.IfUnmodifiedSince = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            requestHeaders.IfMatch = new List<EntityTagHeaderValue> {new EntityTagHeaderValue($"\"{Guid.Empty}\"")};

            var file = new FileBuilder().WithLastModified(lastModified).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);
            fileRepository.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            var response = (StatusCodeResult) testSubject.Delete(Id);

            // Assert
            Assert.Equal(StatusCodes.Status412PreconditionFailed, response.StatusCode);
        }

        [Fact]
        public void Delete_should_return_NoContent_when_ETag_is_any()
        {
            // Arrange
            var lastModified = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);

            var requestHeaders = testSubject.Request.GetTypedHeaders();
            requestHeaders.IfUnmodifiedSince = new DateTimeOffset(2016, 12, 12, 0, 0, 0, TimeSpan.Zero);
            requestHeaders.IfMatch = new List<EntityTagHeaderValue> {EntityTagHeaderValue.Any};

            var file = new FileBuilder().WithLastModified(lastModified).Build();
            fileRepository.Setup(r => r.Get(Id)).Returns(file);
            fileRepository.Setup(r => r.Exists(Id)).Returns(true);

            // Act
            var response = testSubject.Delete(Id);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }
    }
}