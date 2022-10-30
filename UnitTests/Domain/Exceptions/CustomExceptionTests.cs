using Core.Domain.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnitTests.Domain.Exceptions;

[TestFixture]
public class CustomExceptionTests
{
    [Test]
    public void CustomerNotFoundException_Returns_Expected_ErrorMessage()
    {
        var sut = new CustomerNotFoundException();
        Assert.AreEqual("Customer was not found in database", sut.Message);
    }

    [Test]
    public void InvalidAgeException_Returns_Expected_ErrorMessage()
    {
        var sut = new InvalidAgeException();
        Assert.AreEqual("Age must be greater than 0", sut.Message);
    }
}
