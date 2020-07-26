using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Fisk.MDMSolustion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();//׼��һ��web������Ȼ����������
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>//����Ĭ��builder-����ɸ�������
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((Context, loggingBuillder) =>
                {
                    //loggingBuillder.AddFilter("System", LogLevel.Error);//���˵������ռ�
                    //loggingBuillder.AddFilter("Microsoft", LogLevel.Error);
                    //loggingBuillder.AddLog4Net();//ʹ��Log4Net
                })//��չ��־
                .ConfigureWebHostDefaults(webBuilder =>//ָ��һ��web������--Kestrel
                {
                    //webBuilder.UseKestrel(options =>
                    //{
                    //    options.Limits.MaxRequestBodySize = int.MaxValue; //�Ӵ��ļ��ϴ����� 2020��4��30��13:03:00 Dennyhui
                    //}
                    //);
                    webBuilder.UseUrls("http://*:5004");
                    webBuilder.UseStartup<Startup>();//���Ǹ�MVC���̴�����
                    //webBuilder.UseKestrel(options =>
                    //{
                    //    //��ȡ�����ñ��ֻ״̬��ʱ�� Ĭ��ֵΪ 20 ����
                    //    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                    //    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);
                    //});
                    webBuilder.ConfigureKestrel((context, serverOptions) =>
                    {
                        //Ϊ����Ӧ�����ò����򿪵���� TCP ������,Ĭ������£������������������ (NULL)
                        serverOptions.Limits.MaxConcurrentConnections = 1000;
                        //�����Ѵ� HTTP �� HTTPS ��������һ��Э�飨���磬Websocket ���󣩵����ӣ���һ�����������ơ� ���������󣬲������ MaxConcurrentConnections ����
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = 1000;
                        //��ȡ�����ñ��ֻ״̬��ʱ�� Ĭ��ֵΪ 2 ���ӡ�
                        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                        //  serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                    });
                });
    }
}
