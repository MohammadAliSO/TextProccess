
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asynchronous_TextProcessing.Auth.Services
{
    public interface IUserService
    {
        bool ValidateCredentials(string username, string password);
    }
}
