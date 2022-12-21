using Heus.Enroll.Web;

await WebApplication.CreateBuilder(args)
    .RunWithModuleAsync<TestAppModuleInitializer>();
public partial class Program { }