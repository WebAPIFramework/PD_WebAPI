using Core.Infrastructure.Authentication;
using Furion;
using Furion.DatabaseAccessor;
using Microsoft.Extensions.DependencyInjection;
using Api.Core;

namespace Api.EntityFramework.Core
{
    public class DBContextStartup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDatabaseAccessor(options =>
            {
                options.CustomizeMultiTenants("CompanyId");//启用自定义多租户类型，有一个默认参数，配置多租户表字段名
                //options.AddDb<ShipmentDbContext>(DbProvider.SqlServer);
                options.AddDb<TubroDbContext, TubroDbContextLocator>(DbProvider.SqlServer);
                //options.AddDbPool<MultiTenantDbContext, MultiTenantDbContextLocator>(DbProvider.SqlServer);

            }, "Api.Database.Migrations");


        }
    }
}