
using Heus.AspNetCore;
using Heus.Enroll.Web;


await WebApplicationHelper.RunAsync(args, typeof(WebServiceModule));
    // .UseCoreService(typeof(WebServiceModule))
    // .Build()
    // .RunAsync();