using System.Threading.Tasks;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Login;

namespace Backend.Api.Authentication.Services.Account;

public interface IUserAuthenticationService
{
    Task<(AccountDTO, AuthenticationResultType)> AuthenticateAsync(LoginRequest request);
}