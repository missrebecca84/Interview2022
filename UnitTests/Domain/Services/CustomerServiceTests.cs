using AutoMapper;
using Core.DataAccess.Repositories;
using Core.Domain.Exceptions;
using Infrastructure.Domain.Mappers;
using Infrastructure.Domain.Services;
using Moq;
using NUnit.Framework;
using UnitTests.Helpers;
using DomainModels = Core.Domain.Models;
using Entities = Core.DataAccess.Entities;

namespace UnitTests.Domain.Services;

[TestFixture]
public class CustomerServiceTests
{
    private ListLogger _logger;
    private IMapper _mapper;
    private Mock<ICustomerRepository> _mockRepository;
    private readonly Guid _testId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
    private readonly Guid _testId2 = new Guid("62FA647A-AD54-4BCC-A860-E5A2664B019E");

    [SetUp]
    public void Setup()
    {
        _logger = new ListLogger();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMapper>());
        _mapper = config.CreateMapper();
        _mockRepository = new Mock<ICustomerRepository>();
    }

    [Test]
    public async Task GetCustomerByIdAsync_Successful_Logs_AsExcepted()
    {
        _mockRepository.Setup(a => a.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(new Entities.Customer()));

        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        await service.GetCustomerByIdAsync(_testId);
        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsFalse(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }

    [Test]
    public async Task GetCustomerByIdAsync_Successful_CallsRepository_AsExcepted()
    {
        _mockRepository.Setup(a => a.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(new Entities.Customer()));

        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        await service.GetCustomerByIdAsync(_testId);
        _mockRepository.Verify(a => a.GetById(It.IsAny<Guid>()), Times.Once, "Service did not call into repository");
    }

    [Test]
    public async Task GetCustomerByIdAsync_Successful_ReturnsValue_AsExcepted()
    {
        var testEntity = new Entities.Customer()
        {
            Id = _testId,
            FullName = "FullName",
            DateOfBirth = DateTime.Now.Date
        };
        _mockRepository.Setup(a => a.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(testEntity));

        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        var result = await service.GetCustomerByIdAsync(_testId);
        Assert.IsNotNull(result);
        Assert.IsAssignableFrom<DomainModels.Customer>(result);
        Assert.AreEqual(testEntity.Id, result.CustomerId);
        Assert.AreEqual(testEntity.FullName, result.FullName);
        Assert.AreEqual(testEntity.DateOfBirth, result.DateOfBirth);
    }

    [Test]
    public async Task GetCustomerByIdAsync_Failure_ThrowsException_Logs_AsExcepted()
    {
        _mockRepository.Setup(a => a.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Entities.Customer)null));
        Exception exception = null;
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        try
        {
            await service.GetCustomerByIdAsync(_testId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        Assert.IsNotNull(exception);
        Assert.IsAssignableFrom<CustomerNotFoundException>(exception);
        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsTrue(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }

    [Test]
    public async Task GetCustomersByAgeAsync_Failure_ThrowsException_Logs_AsExcepted()
    {

        Exception exception = null;
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        try
        {
            await service.GetCustomersByAgeAsync(0);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        Assert.IsNotNull(exception);
        Assert.IsAssignableFrom<InvalidAgeException>(exception);
        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsTrue(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }

    [Test]
    public async Task GetCustomersByAgeAsync_Success_Logs_AsExcepted()
    {
        var returnValue = new List<Entities.Customer>();
        _mockRepository.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Returns(Task.FromResult(returnValue.AsEnumerable()));
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        await service.GetCustomersByAgeAsync(10);
        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsFalse(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }

    [Test]
    public async Task GetCustomersByAgeAsync_Success_CallsRepository_AsExcepted()
    {
        var returnValue = new List<Entities.Customer>()
        {
             new Entities.Customer()
            {
                Id = _testId,
                FullName = "Test Person1",
                DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(6) //age 38
            },
            new Entities.Customer()
            {
                Id = _testId2,
                FullName = "Test Person2",
                DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(7) //age 38
            }
        };
        _mockRepository.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Returns(Task.FromResult(returnValue.AsEnumerable()));
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        await service.GetCustomersByAgeAsync(10);

        _mockRepository.Verify(a => a.GetCustomersByAgeAsync(It.IsAny<int>()), Times.Once, "Service did not call into repository");

    }

    [Test]
    public async Task GetCustomersByAgeAsync_Success_ReturnsData_AsExcepted()
    {
        //arrange
        var entity1 = new Entities.Customer()
        {
            Id = _testId,
            FullName = "Test Person1",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(6) //age 38
        };
        var entity2 = new Entities.Customer()
        {
            Id = _testId2,
            FullName = "Test Person2",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(7) //age 38
        };
        var returnValue = new List<Entities.Customer>()
        {
             entity1, entity2
        };
        _mockRepository.Setup(a => a.GetCustomersByAgeAsync(It.IsAny<int>())).Returns(Task.FromResult(returnValue.AsEnumerable()));
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        
        //act
        var resultList = await service.GetCustomersByAgeAsync(38).ConfigureAwait(false);

        //assert
        Assert.IsNotNull(resultList);

        Assert.IsAssignableFrom<List<DomainModels.Customer>>(resultList);
        Assert.AreEqual(2, resultList.Count());

        var customer1 = resultList.FirstOrDefault(a => a.CustomerId == entity1.Id);
        Assert.IsNotNull(customer1);
        Assert.AreEqual(entity1.FullName, customer1?.FullName, "Customer1 Full Name does not match");
        Assert.AreEqual(entity1.DateOfBirth, customer1?.DateOfBirth, "Customer1 Date Of Birth does not match");

        var customer2 = resultList.FirstOrDefault(a => a.CustomerId == entity2.Id);
        Assert.IsNotNull(customer2);
        Assert.AreEqual(entity2.FullName, customer2?.FullName, "Customer2 Full Name does not match");
        Assert.AreEqual(entity2.DateOfBirth, customer2?.DateOfBirth, "Customer2 Date Of Birth does not match");


    }

    [Test]
    public async Task SaveCustomerAsync_Success_ReturnsGuid_AsExcepted()
    {
        _mockRepository.Setup(a => a.SaveCustomer(It.IsAny<Entities.Customer>())).Returns(Task.FromResult(_testId));
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        var result = await service.SaveCustomerAsync(new DomainModels.Customer());
        Assert.IsNotNull(result);
        Assert.AreEqual(_testId, result);
    }

    [Test]
    public async Task SaveCustomerAsync_Success_Logs_AsExcepted()
    {
        _mockRepository.Setup(a => a.SaveCustomer(It.IsAny<Entities.Customer>())).Returns(Task.FromResult(_testId));
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        await service.SaveCustomerAsync(new DomainModels.Customer());
        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsFalse(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }

    [Test]
    public async Task SaveCustomerAsync_Success_CallsRepository_AsExcepted()
    {
        _mockRepository.Setup(a => a.SaveCustomer(It.IsAny<Entities.Customer>())).Returns(Task.FromResult(_testId));
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        await service.SaveCustomerAsync(new DomainModels.Customer());

        _mockRepository.Verify(a => a.SaveCustomer(It.IsAny<Entities.Customer>()), Times.Once, "Service did not call into repository");

    }

    [Test]
    public async Task SaveCustomerAsync_Failure_ThrowsException_Logs_AsExcepted()
    {
        Exception exception = null;
        var service = new CustomerService(_logger, _mapper, _mockRepository.Object);
        try
        {
            await service.SaveCustomerAsync((DomainModels.Customer)null);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        Assert.IsNotNull(exception);
        Assert.IsAssignableFrom<ArgumentNullException>(exception);
        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsTrue(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }
}