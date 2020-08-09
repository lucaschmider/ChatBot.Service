using ChatBot.AuthProvider.Contract;
using ChatBot.AuthProvider.Firebase.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChatBot.AuthProvider.Firebase
{
    public static class FirebaseAuthModule
    {
        public static IServiceCollection AddFirebaseAuthModule(this IServiceCollection services, FirebaseAuthConfiguration configuration)
        {
            services
                .AddSingleton<IAuthProvider, FirebaseAuthProvider>()
                .AddSingleton(configuration)
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://securetoken.google.com/" + configuration.ProjectId;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://securetoken.google.com/" + configuration.ProjectId,
                        ValidateAudience = true,
                        ValidAudience = configuration.ProjectId,
                        ValidateLifetime = true
                    };
                });
            return services;
        }
    }
}