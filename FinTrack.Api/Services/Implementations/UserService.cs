using CSharpFunctionalExtensions;
using FinTrack.Api.Infrastructure.Interfaces;
using FinTrack.Api.Repository.Interfaces;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher _hasher;

        public UserService(IRepository<User> userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }
        public async Task<Result<User>> LoginAsync(string name, string password)
        {
            if (!await _userRepository.IsItemExistsAsync(name))
                return Result.Failure<User>("User with this username does not exists");

            var user = await _userRepository.GetItemByNameAsync(name);

            var isCorrect = _hasher.Verify(password, user.Password);
            if (!isCorrect)
            {
                return Result.Failure<User>("Incorrectly entered password");
            }

            return Result.Success(new User { Id = user.Id, Name = user.Name});  
        }

        public Task<Result> UpdatePasswordAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> RegisterUserAsync(string username, string password)
        {
            if (await _userRepository.IsItemExistsAsync(username))
                return Result.Failure("The user with the same username already exists");
            var hashedPassword = _hasher.Generate(password);
            var userCreate = new User { Name = username, Password = hashedPassword };
 
            if (!await _userRepository.CreateAsync(userCreate))
                return Result.Failure("Something went wrong while saving");

            return Result.Success();
        }

        public async Task<Result<User>> GetUserById(int userId)
        {
            var user = await _userRepository.GetItemAsync(userId);
            if (user == null)
                return Result.Failure<User>("User is not exists");

            return user;
        }
        public async Task<Result<User>> GetUserByName(string userName)
        {
            var user = await _userRepository.GetItemByNameAsync(userName);
            if (user == null)
                return Result.Failure<User>("User is not exists");

            return user;
        }
        public async Task<Result> DeleteUserAccount(User user)
        {
            var isSuccess = await _userRepository.DeleteAsync(user);
            if (isSuccess)
                return Result.Success("Deleting was successful");
            else 
                return Result.Failure("Something went wrong while deleting account");
        }


    }
}
