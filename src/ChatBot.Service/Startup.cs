using ChatBot.AuthProvider.Firebase;
using ChatBot.AuthProvider.Firebase.Configurations;
using ChatBot.Business;
using ChatBot.Repository.MongoDb;
using ChatBot.Repository.MongoDb.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatBot.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoConfiguration = new MongoDbConfiguration();
            Configuration.GetSection(MongoDbConfiguration.SectionKey).Bind(mongoConfiguration);

            var firebaseConfiguration = new FirebaseAuthConfiguration();
            Configuration.GetSection(FirebaseAuthConfiguration.SectionKey).Bind(firebaseConfiguration);

            services
                .AddFirebaseAuthModule(firebaseConfiguration)
                .AddMongoDbModule(mongoConfiguration)
                .AddChatBotBusinessModule()
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}