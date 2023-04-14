using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizHut.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}
