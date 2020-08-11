using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ChatBot.Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(options =>
                        {
                            options.ListenAnyIP(8080);
                            options.ListenAnyIP(8443, listenOptions => listenOptions.UseHttps("certificate.pfx"));
                        })
                        .UseStartup<Startup>();
                });
        }
    }
}