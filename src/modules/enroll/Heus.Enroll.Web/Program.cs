

using Heus.Enroll.Web;

await WebApplication.CreateBuilder(args).UsingModule<WebModuleInitializer>().Build().RunAsync();
    // .UseCoreService(typeof(WebServiceModule))
    // .Build()
    // .RunAsync();
public partial class Program { }