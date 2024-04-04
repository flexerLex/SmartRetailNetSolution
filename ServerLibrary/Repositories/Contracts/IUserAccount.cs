using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using Microsoft.Win32;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> SignInAsync(Login user);
    }
}