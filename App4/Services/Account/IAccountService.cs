using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App4.Models;

namespace App4.Services.Account
{
    public interface IAccountService
    {

        Task<bool> LoginAsync(string username, string password);
        Task<AuthenticatedUser> GetUserAsync();
    }
}
