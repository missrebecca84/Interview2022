using Core.DataAccess.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UnitTests.Helpers;

namespace UnitTests.Repository;

/// <summary>
/// Unit Tests related to Customer specific repository logic
/// </summary>
[TestFixture]
public class CustomerRepositoryTests
{
    private CustomerRepository _customerRepository;
    private ListLogger _logger;
    [SetUp]
    public void Setup()
    {
        _logger = new ListLogger();
        var mockLoggerFactory = new Mock<LoggerFactory>();
        var dbName = $"Customers_{DateTime.Now.ToFileTimeUtc()}";
        var contextOptions = new DbContextOptionsBuilder<MicrogrooveContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
        var context = new MicrogrooveContext(contextOptions);
        
        _customerRepository = new CustomerRepository(context, _logger);
    }

    [Test]
    public async Task SaveCustomer_Returns_PopulatedCustomerId_AsExpected()
    {
        var customer = new Customer()
        {
            FullName = "Test Person1",
            DateOfBirth = DateTime.Now
        };
        var id = await _customerRepository.SaveCustomer(customer);
        Assert.IsNotNull(id);
        Assert.IsInstanceOf<Guid>(id);
    }

    [Test]
    public async Task SaveCustomer_Logs_Information_AsExpected()
    {
        var customer = new Customer()
        {
            FullName = "Test Person1",
            DateOfBirth = DateTime.Now
        };
        await _customerRepository.SaveCustomer(customer);
        Assert.AreEqual(2, _logger.Logs.Count);
    }

    [Test]
    public async Task SaveCustomer_NullValues_ThrowsException_Logs_AsExpected()
    {
        Exception exception = null;
        //Act
        try
        {
           await _customerRepository.SaveCustomer(null);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        Assert.IsNotNull(exception);
        Assert.AreEqual(2, _logger.Logs.Count);
        Assert.IsTrue(_logger.Logs.Any(a => a.ToLower().Contains("error")));
    }

    [Test]
    public async Task GetCustomerByAgeAsync_Returns_ExpectedResults()
    {
        //arrange
        var customer1 = new Customer()
        {
            FullName = "Test Person1",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(6) //age 38
        };
        var customer2 = new Customer()
        {
            FullName = "Test Person2",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(7) //age 38
        };
         var customer3 = new Customer()
        {
            FullName = "Test Person3",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(-6) //age 39.5
        };
        Task.WaitAll(new Task[]
        {
            _customerRepository.Add(customer1),
            _customerRepository.Add(customer2),
            _customerRepository.Add(customer3)
        });
        var customersAged38 = await _customerRepository.GetCustomerByAgeAsync(38).ConfigureAwait(false);
        Assert.IsNotNull(customersAged38);
        Assert.AreEqual(2, customersAged38.Count());
        Assert.IsFalse(customersAged38.Any(a => a.FullName == "Test Person3"));
    }

    [Test]
    public async Task GetCustomerByAgeAsync_Logs_Information_AsExpected()
    {
        //arrange
        var customer1 = new Customer()
        {
            FullName = "Test Person1",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(6) //age 38
        };
        var customer2 = new Customer()
        {
            FullName = "Test Person2",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(7) //age 38
        };
        var customer3 = new Customer()
        {
            FullName = "Test Person3",
            DateOfBirth = DateTime.Now.AddYears(-39).AddMonths(-6) //age 39.5
        };
        Task.WaitAll(new Task[]
        {
            _customerRepository.Add(customer1),
            _customerRepository.Add(customer2),
            _customerRepository.Add(customer3)
        });

        //act
        await _customerRepository.GetCustomerByAgeAsync(38).ConfigureAwait(false);

        //Assert
        Assert.AreEqual(2, _logger.Logs.Count);
    }
}
