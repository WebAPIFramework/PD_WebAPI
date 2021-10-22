using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Api.Application.Worker;

namespace Api.Web.Entry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

           
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //furion（主机注入）
                    webBuilder.Inject()
                              .UseStartup<Startup>();
                    //webBuilder.UseUrls("http://*.80");
                    webBuilder.ConfigureKestrel(options =>
                    {   //Kestrel的默认监听端口是http5000、https5001。
                        //增加该配置后，到时以独立方式发布后，打开http://server:80 能直接访问该api，无需另外部署IIS
                        //容器化部署所用
                        options.ListenAnyIP(80);
                    });
                })
                
               //启用日志（添加日志拓展）
               .UseSerilogDefault();
    }
}