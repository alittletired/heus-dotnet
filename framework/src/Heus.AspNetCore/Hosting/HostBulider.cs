using Microsoft.AspNetCore.Hosting;

namespace Heus.AspNetCore.Hosting
{
    public static class HostBulider
    {
        public static IHostBuilder CreateWebBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices(services => { });
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(options =>
                {
                    options.Limits.MaxRequestBufferSize = 302768;
                    options.Limits.MaxRequestLineSize = 302768;
                });
                //webBuilder.UseStartup<Startup>();
                webBuilder.Configure(app =>
                {

                });
            });
            return builder;
        }
    }
}
