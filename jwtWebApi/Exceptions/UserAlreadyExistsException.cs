namespace jwtWebApi.Exceptions;

public class UserAlreadyExistsException : InvalidOperationException
{
    public UserAlreadyExistsException(string message) : base(message) { }
}
