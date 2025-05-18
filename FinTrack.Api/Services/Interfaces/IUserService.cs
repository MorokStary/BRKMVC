using CSharpFunctionalExtensions;
using FinTrack.Models;

namespace FinTrack.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterUserAsync(string username, string password);
        Task<Result<User>> LoginAsync(string login, string password);
        Task<Result> UpdatePasswordAsync(User user);
        Task<Result> DeleteUserAccount(User user);
        Task<Result<User>> GetUserById(int userId);
        Task<Result<User>> GetUserByName(string userName);
    }
}
