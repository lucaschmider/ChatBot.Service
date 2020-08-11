using System;
using System.Threading.Tasks;
using ChatBot.AuthProvider.Contract;
using ChatBot.AuthProvider.Contract.Exceptions;
using ChatBot.AuthProvider.Firebase.Configurations;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Shouldly;

namespace ChatBot.AuthProvider.Firebase
{
    public class FirebaseAuthProvider : IAuthProvider
    {
        private readonly FirebaseAuth _auth;

        public FirebaseAuthProvider(FirebaseAuthConfiguration configuration)
        {
            configuration.ShouldNotBeNull();

            var app = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(configuration.ServiceAccountJson)
            });
            _auth = FirebaseAuth.GetAuth(app);
        }

        public async Task<string> CreateUserWithUsernameAndPasswordAsync(string username, string password)
        {
            try
            {
                var userRecord = await _auth.CreateUserAsync(new UserRecordArgs
                {
                    Email = username,
                    Password = password
                }).ConfigureAwait(false);
                return userRecord.Uid;
            }
            catch (Exception)
            {
                throw new UserAlreadyExistsException();
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _auth.DeleteUserAsync(userId);
        }
    }
}