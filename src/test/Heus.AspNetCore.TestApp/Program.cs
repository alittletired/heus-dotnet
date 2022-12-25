using Heus.AspNetCore.TestApp;

await WebApplication.CreateBuilder(args)
    .RunWithModuleAsync<TestAppModuleInitializer>();
public partial class Program { }