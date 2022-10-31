using Application.Functions;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;
using UnitTests.Helpers;

namespace UnitTests.Application.Functions
{
    [TestFixture]
    public class GetByIdFunctionTests
    {
        private ListLogger _logger;
        private readonly Guid _testId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        private Customer _testModel;
        private string _serializedTestModel;
        private Mock<ICustomerService> _mockCustomerService; 
        private FakeHttpRequestData _requestData;

        [SetUp]
        public void Setup()
        {
            _logger = new ListLogger();
            _testModel = new Customer()
            {
                CustomerId = _testId,
                FullName = "FullName",
                DateOfBirth = DateTime.Now.Date
            };
            _serializedTestModel = JsonConvert.SerializeObject(_testModel);
            _mockCustomerService = new Mock<ICustomerService>();
            var body = new MemoryStream(Encoding.ASCII.GetBytes(" "));
            var context = new Mock<FunctionContext>();
            _requestData = new FakeHttpRequestData(
                            context.Object,
                            new Uri("https://localhost"),
                            body);
        }

        [Test]
        public async Task GetById_Success_Returns_Customer()
        {
            // Arrange         
            _mockCustomerService.Setup(a => a.GetCustomerByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(_testModel));
            // Act
            var function = new GetByIdFunction(_logger, _mockCustomerService.Object);
            var result = await function.Run(_requestData, _testId);
            result.Body.Position = 0;
            // Assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(_serializedTestModel, responseBody);
        }

        [Test]
        public async Task GetById_Success_Logs_AsExpected()
        {
            // Arrange
            _mockCustomerService.Setup(a => a.GetCustomerByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(_testModel));
            // Act
            var function = new GetByIdFunction(_logger, _mockCustomerService.Object);
            var result = await function.Run(_requestData, _testId);

            Assert.AreEqual(2, _logger.Logs.Count);
            Assert.IsFalse(_logger.Logs.Any(a => a.ToLower().Contains("error")));
        }

        [Test]
        public async Task GetById_Success_CallsDomainService_AsExpected()
        {
            _mockCustomerService.Setup(a => a.GetCustomerByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(_testModel));
            // Act
            var function = new GetByIdFunction(_logger, _mockCustomerService.Object);
            var result = await function.Run(_requestData, _testId);

            _mockCustomerService.Verify(a => a.GetCustomerByIdAsync(It.IsAny<Guid>()), Times.Once, "Function Failed to Call Domain Service when expected");
        }

        [Test]
        public async Task GetById_UnableToLocateCustomer_Returns_NotFound_LogsExpected()
        {
            // Arrange
            _mockCustomerService.Setup(a => a.GetCustomerByIdAsync(It.IsAny<Guid>())).Throws<CustomerNotFoundException>();
            // Act
            var function = new GetByIdFunction(_logger, _mockCustomerService.Object);
            var result = await function.Run(_requestData, _testId);
            result.Body.Position = 0;
            // Assert
            var reader = new StreamReader(result.Body);
            var responseBody = await reader.ReadToEndAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(new CustomerNotFoundException().Message, responseBody);
            Assert.AreEqual(3, _logger.Logs.Count);
            Assert.IsTrue(_logger.Logs.Any(a => a.ToLower().Contains("error")));
        }

        [Test]
        public async Task SaveCustomer_CatchesException_Returns_BadRequest_LogsExpected()
        {
            // Arrange
            _mockCustomerService.Setup(a => a.GetCustomerByIdAsync(It.IsAny<Guid>())).Throws<ArgumentNullException>();
            // Act
            var function = new GetByIdFunction(_logger, _mockCustomerService.Object);
            var result = await function.Run(_requestData, _testId);
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