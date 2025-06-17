using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtWebApi.Exeptions
{
    public class UserAlreadyExistsException : InvalidOperationException
    {
        public UserAlreadyExistsException(string message) : base(message) { }
    }
}