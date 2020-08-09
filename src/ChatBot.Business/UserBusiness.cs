using System;
using System.Threading.Tasks;
using ChatBot.AuthProvider.Contract;
using ChatBot.Business.Contracts.User;
using ChatBot.Business.Contracts.User.Models;
using ChatBot.Repository.Contracts;
using Shouldly;

namespace ChatBot.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IAuthProvider _authProvider;
        private readonly IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository, IAuthProvider authProvider)
        {
            userRepository.ShouldNotBeNull();
            authProvider.ShouldNotBeNull();
            _userRepository = userRepository;
            _authProvider = authProvider;
        }

        public async Task<UserModel> CreateUserAsync(UserModel createUserRequest)
        {
            try
            {
                createUserRequest.ShouldNotBeNull();
                createUserRequest.Email.ShouldNotBeNullOrWhiteSpace();
                createUserRequest.Name.ShouldNotBeNullOrWhiteSpace();
                createUserRequest.Department.ShouldNotBeNullOrWhiteSpace();
                createUserRequest.Password.ShouldNotBeNullOrWhiteSpace();

                var userId =
                    await _authProvider.CreateUserWithUsernameAndPasswordAsync(createUserRequest.Email,
                        createUserRequest.Password);

                var user = createUserRequest.Map();
                user.Uid = userId;

                await _userRepository.CreateUserDetailsAsync(user);
                return user.Map();
            }
            catch (ShouldAssertException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> CheckAdminPrivilegesAsync(string userId)
        {
            var user = await _userRepository
                .GetUserDetailsAsync(userId)
                .ConfigureAwait(false);

            return user.IsAdmin;
        }

        public Task DeleteUserAsync(string userId)
        {
            userId.ShouldNotBeNullOrWhiteSpace();

            return Task.WhenAll(
                _authProvider.DeleteUserAsync(userId),
                _userRepository.DeleteUserDetailsAsync(userId));
        }
    }
}