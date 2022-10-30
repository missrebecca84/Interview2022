namespace Core.Domain.Exceptions;
public class CustomerNotFoundException : Exception
{
    public override string Message => "Customer was not found in database";

}
