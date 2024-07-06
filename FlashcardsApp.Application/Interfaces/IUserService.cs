using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FlashcardsApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(string username, string email, string password);
        Task<SignInResult> AuthenticateUserAsync(string email, string password);
    }
}
