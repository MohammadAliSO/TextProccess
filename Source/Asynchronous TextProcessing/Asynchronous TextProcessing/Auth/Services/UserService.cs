
using Asynchronous_TextProcessing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asynchronous_TextProcessing.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly DBContext _context;

        public UserService(DBContext context)
        {
            _context = context;
        }
        public bool ValidateCredentials(string username, string password)
        {
            //return username.Equals("admin") && password.Equals("123");
            return _context.UserTs.Any(u => u.UserName == username && u.Password == password);
        }
    }
}
