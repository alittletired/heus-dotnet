
using Heus.AspNetCore;
using Heus.Enroll.Web;


await WebApplicationHelper.RunAsync(args, typeof(WebModuleInitializer));
    // .UseCoreService(typeof(WebServiceModule))
    // .Build()
    // .RunAsync();
public partial class Program { }