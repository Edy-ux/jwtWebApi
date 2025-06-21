using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtWebApi.Dto
{
    public class UserDtoResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }

        public string[]? Roles { get; set; } = [];





    }
}