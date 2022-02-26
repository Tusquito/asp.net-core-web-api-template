using System.Threading.Tasks;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Login;

namespace Backend.Api.Services.Account;

public interface IUserAuthenticationService
{
    Task<(AccountDTO, AuthenticationResultType)> AuthenticateAsync(LoginForm form);
}