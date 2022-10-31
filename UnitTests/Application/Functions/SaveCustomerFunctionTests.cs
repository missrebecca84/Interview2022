using Application.Functions;
using Core.Domain.Models;
using Core.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Text;
using UnitTests.Helpers;

namespace UnitTests.Application.Functions
{
    [TestFixture]
    public class SaveCustomerFunctionTests
    {
        private ListLogger _logger;
        private readonly Guid _testId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        [SetUp]
        public void Setup()
        {
            _logger = new ListLogger();
        }

        [Test]
        public async Task SaveCustomer_Success_Returns_Guid_Id()
        {
            // Arrange
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"FullName\":\"Rebecca Clark\",\"DateOfBirth\":\"1984-3-8\"}"));
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://localhost"),
                            body);
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(a => a.SaveCustomerAsync(It.IsAny<Customer>())).Returns(Task.FromResult(_testId));
            // Act
            var function = new SaveCustomerFunction(_logger, mockService.Object);
            var result = await function.Run(request);
            result.Body.Position = 0;
            // Assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(_testId.ToString(), responseBody);
        }

        [Test]
        public async Task SaveCustomer_Success_Logs_AsExpected()
        {
            // Arrange
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"FullName\":\"Rebecca Clark\",\"DateOfBirth\":\"1984-3-8\"}"));
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://localhost"),
                            body);
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(a => a.SaveCustomerAsync(It.IsAny<Customer>())).Returns(Task.FromResult(_testId));
            // Act
            var function = new SaveCustomerFunction(_logger, mockService.Object);
            await function.Run(request);

            Assert.AreEqual(2, _logger.Logs.Count);
            Assert.IsFalse(_logger.Logs.Any(a => a.ToLower().Contains("error")));
        }

        [Test]
        public async Task SaveCustomer_Success_CallsDomainService_AsExpected()
        {
            // Arrange
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"FullName\":\"Rebecca Clark\",\"DateOfBirth\":\"1984-3-8\"}"));
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://localhost"),
                            body);
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(a => a.SaveCustomerAsync(It.IsAny<Customer>())).Returns(Task.FromResult(_testId));
            // Act
            var function = new SaveCustomerFunction(_logger, mockService.Object);
            await function.Run(request);

            mockService.Verify(a => a.SaveCustomerAsync(It.IsAny<Customer>()), Times.Once, "Function Failed to Call Domain Service when expected");
        }

        [Test]
        public async Task SaveCustomer_InvalidName_Returns_BadRequest()
        {
            // Arrange
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"FullName\":\" \",\"DateOfBirth\":\"1984-3-8\"}"));
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://localhost"),
                            body);
            var mockService = new Mock<ICustomerService>();
            
            // Act
            var function = new SaveCustomerFunction(_logger, mockService.Object);
            var result = await function.Run(request);
            result.Body.Position = 0;
            // Assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("Full Name is Required", responseBody);
        }

        [Test]
        public async Task SaveCustomer_CatchesException_Returns_BadRequest_LogsExpected()
        {
            // Arrange
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"FullName\":\"Rebecca Clark\",\"DateOfBirth\":\"1984-3-8\"}"));
            var context = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://localhost"),
                            body);
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(a => a.SaveCustomerAsync(It.IsAny<Customer>())).Throws<ArgumentNullException>();
            // Act
            var function = new SaveCustomerFunction(_logger, mockService.Object);
            var result = await function.Run(request);
            result.Body.Position = 0;
            // Assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(new ArgumentNullException().Message, responseBody);
            Assert.AreEqual(3, _logger.Logs.Count);
            Assert.IsTrue(_logger.Logs.Any(a => a.ToLower().Contains("error")));
        }
    }
}
