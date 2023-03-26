using Heus.Enroll.Web;
await WebApplication.CreateBuilder(args)
    .RunWithModuleAsync<MallWebModuleInitializer>();
public partial class Program { }