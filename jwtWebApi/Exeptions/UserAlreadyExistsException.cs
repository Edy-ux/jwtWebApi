namespace jwtWebApi.Exeptions
{
    public class UserAlreadyExistsException : InvalidOperationException
    {
        public UserAlreadyExistsException(string message) : base(message) { }
    }
}