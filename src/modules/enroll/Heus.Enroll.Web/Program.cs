using Heus.Enroll.Web;
await WebApplication.CreateBuilder(args)
    .UseModuleAndRunAsync<WebModuleInitializer>();
public partial class Program { }