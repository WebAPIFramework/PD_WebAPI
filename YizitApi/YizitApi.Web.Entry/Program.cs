using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using YizitApi.Application.Worker;

namespace YizitApi.Web.Entry
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
                    //furion������ע�룩
                    webBuilder.Inject()
                              .UseStartup<Startup>();
                    //webBuilder.UseUrls("http://*.80");
                    webBuilder.ConfigureKestrel(options =>
                    {   //Kestrel��Ĭ�ϼ����˿���http5000��https5001��
                        //���Ӹ����ú󣬵�ʱ�Զ�����ʽ�����󣬴�http://server:80 ��ֱ�ӷ��ʸ�api���������ⲿ��IIS
                        //��������������
                        options.ListenAnyIP(80);
                    });
                })
                
               //������־�������־��չ��
               .UseSerilogDefault();
    }
}