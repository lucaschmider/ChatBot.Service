using System.Linq;
using System.Threading;
using ChatBot.AuthProvider.Firebase;
using ChatBot.AuthProvider.Firebase.Configurations;
using ChatBot.Business;
using ChatBot.MessageInterpreter.DialogFlow;
using ChatBot.Repository.MongoDb;
using ChatBot.Repository.MongoDb.Configurations;
using ChatBot.StatisticsProvider.Influx;
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

            var dialogFlowConfiguration = new DialogFlowConfiguration();
            Configuration.GetSection(DialogFlowConfiguration.SectionKey).Bind(dialogFlowConfiguration);

            var influxConfiguration = new InfluxDbConfiguration();
            Configuration.GetSection(InfluxDbConfiguration.SectionKey).Bind(influxConfiguration);

            services
                .AddCors()
                .AddFirebaseAuthModule(firebaseConfiguration)
                .AddMongoDbModule(mongoConfiguration)
                .AddDialogFlowModule(dialogFlowConfiguration)
                .AddInfluxStatisticsModule(influxConfiguration)
                .AddChatBotBusinessModule()
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var hosts = Configuration
                .GetSection("AllowedHosts")
                .GetChildren()
                .Select(child => child.Value)
                .ToArray();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCors(options => options.WithOrigins(hosts).AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}