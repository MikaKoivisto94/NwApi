using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWApi.Models;

namespace NWApi.Services.Interfaces
{
    public interface IAuthenticateService
    {
        LoggedUser Authenticate(string username, string password);
    }
}
