using AutoMapper;
using Infrastructure.Domain.Mappers;
using NUnit.Framework;
using DomainModels = Core.Domain.Models;
using Entities = Core.DataAccess.Entities;

namespace UnitTests.Domain.Mappers;

[TestFixture]
public class CustomerMapperTests
{
    private IMapper _mapper;
    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMapper>());
        _mapper = config.CreateMapper();
    }

    [Test]
    public void AutoMapper_Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMapper>());
        config.AssertConfigurationIsValid();
    }

    [Test]
    public void DomainModel_Without_Id_Maps_To_Entity_AsExpected()
    {
        var model = new DomainModels.Customer()
        {
            FullName = "Test",
            DateOfBirth = DateTime.Now.Date
        };
        var entity = _mapper.Map<DomainModels.Customer, Entities.Customer>(model);
        DomainAndEntityAreEqualAssertions(model, entity);
    }

    [Test]
    public void DomainModel_With_Id_Maps_To_Entity_AsExpected()
    {
        var model = new DomainModels.Customer()
        {
            CustomerId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
            FullName = "Test",
            DateOfBirth = DateTime.Now.Date
        };
        var entity = _mapper.Map<DomainModels.Customer, Entities.Customer>(model);
        DomainAndEntityAreEqualAssertions(model, entity);
    }

    [Test]
    public void Entity_Without_Id_Maps_To_Domain_AsExpected()
    {
        var entity = new Entities.Customer()
        {
            FullName = "Test",
            DateOfBirth = DateTime.Now.Date
        };
        var model = _mapper.Map<Entities.Customer, DomainModels.Customer>(entity);
        DomainAndEntityAreEqualAssertions(model, entity);
    }

    [Test]
    public void Entity_With_Id_Maps_To_Domain_AsExpected()
    {
        var entity = new Entities.Customer()
        {
            Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D"),
            FullName = "Test",
            DateOfBirth = DateTime.Now.Date
        };
        var model = _mapper.Map<Entities.Customer, DomainModels.Customer>(entity);
        DomainAndEntityAreEqualAssertions(model, entity);
    }

    private void DomainAndEntityAreEqualAssertions(DomainModels.Customer model, Entities.Customer entity)
    {
        Assert.AreEqual(model.CustomerId, entity.Id);
        Assert.AreEqual(model.DateOfBirth, entity.DateOfBirth);
        Assert.AreEqual(model.FullName, entity.FullName);
    }
}
