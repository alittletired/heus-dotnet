

namespace Heus.Ioc
{
    public abstract class ServiceModuleBase : IServiceModule
    {


        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        public virtual void ConfigureServices(ConfigureServicesContext context)
        {
        }
        /// <summary>
        /// 配置应用
        /// </summary>
        /// <param name="context"></param>
        public virtual void Configure(ConfigureContext context)
        {
        }

    }
}
