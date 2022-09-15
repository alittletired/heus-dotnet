using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Core.Modularity;

public class ApplicationConfigurationContext
{
    public IHost Host { get; }
    public ApplicationConfigurationContext(IHost host)
    {
        Host = host;
    }
    public IServiceProvider ServiceProvider => Host.Services;
    public IConfiguration Configuration => Host.Services.GetRequiredService<IConfiguration>();
    public IHostEnvironment Environment => Host.Services.GetRequiredService<IHostEnvironment>();
}