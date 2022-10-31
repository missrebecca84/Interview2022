using Application.Functions;
using Core.Domain.Models;
using Core.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;
using UnitTests.Helpers;

namespace UnitTests.Application.Functions;

[TestFixture]
public class GetCustomersByAgeFunctionTests
{
    private ListLogger _logger;
    private Mock<ICustomerService> _mockService;
    private Customer _testModel;
    private string _serializedTestModel;
    private FakeHttpRequestData _requestData;

    [SetUp]
    public void Setup()
    {
        _logger = new ListLogger();
        _testModel = new Customer()
        {
            CustomerId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
            FullName = "FullName",
            DateOfBirth = DateTime.Now.Date
        };
        var resultList = new List<Customer>() { _testModel };
        _serializedTestModel = JsonConvert.SerializeObject(resultList);
        _mockService = new Mock<ICustomerService>();

        var body = new MemoryStream(Encoding.ASCII.GetBytes(" "));
        var context = new Mock<FunctionContext>();
        _requestData = new FakeHttpRequestData(
                        context.Object,
                        new Uri("https://localhost"),
                        body);
    }

    [Test]
    public async Task GetCustomersByAge_Success_Returns_Customer()
    {
        // Arrange
       
        var resultList = new List<Customer>() { _testModel };
        _mockService.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Returns(Task.FromResult(resultList.AsEnumerable()));
        // Act
        var function = new GetCustomersByAgeFunction(_logger, _mockService.Object);
        var result = await function.Run(_requestData, 38);
        result.Body.Position = 0;
        // Assert
        var reader = new StreamReader(result.Body);
        var responseBody = await reader.ReadToEndAsync();
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(_serializedTestModel, responseBody);
    }

    [Test]
    public async Task GetCustomersByAge_Success_Logs_AsExpected()
    {
        // Arrange
        var resultList = new List<Customer>() { _testModel };
        _mockService.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Returns(Task.FromResult(resultList.AsEnumerable()));
        // Act
        var function = new GetCustomersByAgeFunction(_logger, _mockService.Object);
        await function.Run(_requestData, 38);

        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsFalse(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }

    [Test]
    public async Task GetCustomersByAge_Success_CallsDomainService_AsExpected()
    {
        // Arrange
   
        var resultList = new List<Customer>() { _testModel };
        _mockService.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Returns(Task.FromResult(resultList.AsEnumerable()));
        // Act
        var function = new GetCustomersByAgeFunction(_logger, _mockService.Object);
        await function.Run(_requestData, 38);

        _mockService.Verify(a => a.GetCustomersByAgeAsync(It.IsAny<int>()), Times.Once, "Function Failed to Call Domain Service when expected");
    }

    [Test]
    public async Task GetCustomersByAge_Success_NoResults_NotFound()
    {
        // Arrange
        var resultList = new List<Customer>() { };
        _mockService.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Returns(Task.FromResult(resultList.AsEnumerable()));
        // Act
        var function = new GetCustomersByAgeFunction(_logger, _mockService.Object);
        var result = await function.Run(_requestData, 38);
        result.Body.Position = 0;
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }


    [Test]
    public async Task GetCustomersByAge_CatchesException_Returns_BadRequest_LogsExpected()
    {
        // Arrange
        _mockService.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Throws<NullReferenceException>();
        // Act
        var function = new GetCustomersByAgeFunction(_logger, _mockService.Object);
        var result = await function.Run(_requestData, 10);
        result.Body.Position = 0;
        // Assert
        var reader = new StreamReader(result.Body);
        var responseBody = await reader.ReadToEndAsync();
        Assert.IsNotNull(result);
        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.AreEqual(new NullReferenceException().Message, responseBody);
        Assert.AreEqual(3, _logger.Logs.Count);
        Assert.IsTrue(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }
}