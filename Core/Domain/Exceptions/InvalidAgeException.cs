using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Exceptions;

public class InvalidAgeException : Exception
{
    public override string Message => "Age must be greater than 0";
}
