using Core.DataAccess.Entities;
using Infrastructure.DataAccess.Data;
using Infrastructure.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace UnitTests.DataAccess.Repository;

/// <summary>
/// Test Class for testing the Async Repository Class
/// Since it is an assignment to portray my skills, I used both Mocking and the In memory Provider
/// Typically, I would mock all of these classes. I just wanted to show both techniques
/// </summary>
[TestFixture]
public class BusinessRepositoryTests
{
    private DbContextOptions<BusinessContext> _contextOptions;

    [SetUp]
    public void Setup()
    {
        var dbName = $"Customers_{DateTime.Now.ToFileTimeUtc()}";
        _contextOptions = new DbContextOptionsBuilder<BusinessContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Test]
    public void GetByIdAsync_Returns_Customer_UsingMocks()
    {

        var dbSetMock = new Mock<DbSet<Customer>>();
        dbSetMock.Setup(s => s.FindAsync(It.IsAny<Guid>())).Returns(() => ValueTask.FromResult(new Customer()));
        var mockContext = new Mock<BusinessContext>(_contextOptions);
        mockContext.Setup(s => s.Set<Customer>()).Returns(dbSetMock.Object);

        var customerRepository = new BusinessRepository<Customer>(mockContext.Object);
        var customer = customerRepository.GetById(Guid.NewGuid()).Result;

        //Assert  
        Assert.NotNull(customer);
        Assert.IsAssignableFrom<Customer>(customer);
    }

    [Test]
    public async Task Add_Saves_Without_Throwing_Exception_InMemoryContext()
    {
        Exception? exception = null;
        //Arrange
        var customer = new Customer
        {
            FullName = "Test Customer",
            DateOfBirth = DateTime.Now.Date
        };
        var context = new BusinessContext(_contextOptions);
        var customerRepository = new BusinessRepository<Customer>(context);

        //Act
        try
        {
            await customerRepository.Add(customer);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        Assert.IsNull(exception);

    }

    [Test]
    public async Task GetWhere_Retrieves_Expected_UsingInMemoryContext()
    {
        //Arrange
        var customer = new Customer
        {
            FullName = "Test Customer",
            DateOfBirth = DateTime.Now.Date
        };
        var context = new BusinessContext(_contextOptions);
        var customerRepository = new BusinessRepository<Customer>(context);

        //Act
        await customerRepository.Add(customer);
        var retrievedCustomer = (await customerRepository.GetWhere(a => a.FullName == customer.FullName).ConfigureAwait(false)).FirstOrDefault();

        //Assert  
        Assert.NotNull(retrievedCustomer);
        Assert.IsAssignableFrom<Customer>(retrievedCustomer);
        Assert.IsNotNull(retrievedCustomer?.Id);
    }
}
