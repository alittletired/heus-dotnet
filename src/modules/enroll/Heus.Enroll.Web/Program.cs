using Heus.Enroll.Web;
await WebApplication.CreateBuilder(args)
    .RunWithModuleAsync<WebModuleInitializer>();
public partial class Program { }