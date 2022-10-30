using DomainModels = Core.Domain.Models;
using Core.Domain.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet.Frameworks;

namespace UnitTests.Domain.Validations;

[TestFixture]
public class ValidationWrapperTests
{
    [Test]
    public void Validation_Wrapper_Assigns_Value_With_Valid_Json()
    {
        var customer = new DomainModels.Customer()
        {
            FullName = "Test",
            DateOfBirth = DateTime.Now.Date
        };
        var serialized = JsonConvert.SerializeObject(customer);
        var wrapper = ValidationWrapper<DomainModels.Customer>.BuildValidationWrapper(serialized);
        Assert.IsNotNull(wrapper);
        Assert.IsNotNull(wrapper.Value);
        Assert.AreEqual(customer.CustomerId, wrapper.Value.CustomerId);
        Assert.AreEqual(customer.FullName, wrapper.Value.FullName);
        Assert.AreEqual(customer.DateOfBirth, wrapper.Value.DateOfBirth);
    }

    [Test]
    public void Customer_WithoutId_IsValid()
    {
        var customer = new DomainModels.Customer()
        {
            FullName = "Test",
            DateOfBirth = DateTime.Now.Date
        };
        var serialized = JsonConvert.SerializeObject(customer);
        var wrapper = ValidationWrapper<DomainModels.Customer>.BuildValidationWrapper(serialized);
        Assert.IsNotNull(wrapper);
        Assert.IsTrue(wrapper.IsValid);
    }

    [Test]
    public void Customer_WithoutFullName_IsNotValid_HasExpected_Error_Message()
    {
        var customer = new DomainModels.Customer()
        {
           DateOfBirth = DateTime.Now.Date
        };
        var serialized = JsonConvert.SerializeObject(customer);
        var wrapper = ValidationWrapper<DomainModels.Customer>.BuildValidationWrapper(serialized);
        Assert.IsNotNull(wrapper);
        Assert.IsFalse(wrapper.IsValid);
        Assert.IsTrue(wrapper.ValidationResults.Any(a => a.ErrorMessage == "Full Name is Required"));
    }

    [Test]
    public void Customer_FullName_StringEmpty_IsNotValid_HasExpected_Error_Message()
    {
        var customer = new DomainModels.Customer()
        {
            FullName = " ",
            DateOfBirth = DateTime.Now.Date
        };
        var serialized = JsonConvert.SerializeObject(customer);
        var wrapper = ValidationWrapper<DomainModels.Customer>.BuildValidationWrapper(serialized);
        Assert.IsNotNull(wrapper);
        Assert.IsFalse(wrapper.IsValid);
        Assert.IsTrue(wrapper.ValidationResults.Any(a => a.ErrorMessage == "Full Name is Required"));
    }

    [Test]
    public void Customer_WithoutDateOfBirth_HasExpected_Error_Message()
    {
        var customer = new DomainModels.Customer()
        {
            FullName = "Test",
        };
        var serialized = JsonConvert.SerializeObject(customer);
        var wrapper = ValidationWrapper<DomainModels.Customer>.BuildValidationWrapper(serialized);
        Assert.IsNotNull(wrapper);
        Assert.IsFalse(wrapper.IsValid);
        Assert.IsTrue(wrapper.ValidationResults.Any(a => a.ErrorMessage.Contains("The DateOfBirth field is required.")));
    }
}
