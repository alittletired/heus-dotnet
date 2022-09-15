
using Heus.Web;
using Heus.AspNetCore;

await WebApplicationExtensions.RunAsync(args, typeof(WebServiceModule));
    // .UseCoreService(typeof(WebServiceModule))
    // .Build()
    // .RunAsync();
