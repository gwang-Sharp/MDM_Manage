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
            CreateHostBuilder(args).Build().Run();//准备一个web服务器然后运行起来
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>//创建默认builder-会完成各种配置
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((Context, loggingBuillder) =>
                {
                    //loggingBuillder.AddFilter("System", LogLevel.Error);//过滤掉命名空间
                    //loggingBuillder.AddFilter("Microsoft", LogLevel.Error);
                    //loggingBuillder.AddLog4Net();//使用Log4Net
                })//扩展日志
                .ConfigureWebHostDefaults(webBuilder =>//指定一个web服务器--Kestrel
                {
                    //webBuilder.UseKestrel(options =>
                    //{
                    //    options.Limits.MaxRequestBodySize = int.MaxValue; //接触文件上传限制 2020年4月30日13:03:00 Dennyhui
                    //}
                    //);
                    webBuilder.UseUrls("http://*:5004");
                    webBuilder.UseStartup<Startup>();//就是跟MVC流程串起来
                    //webBuilder.UseKestrel(options =>
                    //{
                    //    //获取或设置保持活动状态超时。 默认值为 20 分钟
                    //    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                    //    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);
                    //});
                    webBuilder.ConfigureKestrel((context, serverOptions) =>
                    {
                        //为整个应用设置并发打开的最大 TCP 连接数,默认情况下，最大连接数不受限制 (NULL)
                        serverOptions.Limits.MaxConcurrentConnections = 1000;
                        //对于已从 HTTP 或 HTTPS 升级到另一个协议（例如，Websocket 请求）的连接，有一个单独的限制。 连接升级后，不会计入 MaxConcurrentConnections 限制
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = 1000;
                        //获取或设置保持活动状态超时。 默认值为 2 分钟。
                        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                        //  serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                    });
                });
    }
}
