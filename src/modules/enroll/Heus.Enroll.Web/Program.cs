

using Heus.Enroll.Web;

await WebApplication.CreateBuilder(args).RunWithModuleAsync<WebModuleInitializer>();
    // .UseCoreService(typeof(WebServiceModule))
    // .Build()
    // .RunAsync();
public partial class Program { }