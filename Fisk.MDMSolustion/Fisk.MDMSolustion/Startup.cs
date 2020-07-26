using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.Utility.Middleware;
using Fisk.MDMSolustion.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Fisk.MDMSolustion
{
    //配置web应用所需的服务和中间件
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public string conStr { get; set; }
        public string Env { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Env = Configuration.GetSection("Env").Value;
            if (Env == "Uat" || Env == "Prd")
            {
                conStr = Configuration.GetSection("ConnectionString" + Env + ":MysqlConnection").Value;
            }
            else
            {
                conStr = Configuration.GetSection("ConnectionStringLocalhost:MysqlConnection").Value;
            }
        }


        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";//

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //配置跨域请求  2020年5月21日10:50:22  Dennyhui
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
                .AllowAnyOrigin());
            });

            //添加JWT身份验证 2020年5月7日13:55:00  Dennyhui
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,//是否验证Issuer
              ValidateAudience = true,//是否验证Audience
              ValidateLifetime = true,//是否验证失效时间
              ValidateIssuerSigningKey = true,//是否验证SecurityKey
              ValidAudience = "fisksoftmdm.com",
              //上下这两项和签发token时的issuer,Audience一致
              ValidIssuer = "fisksoftmdm.issuer.com",
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2020fiskmdmsolution"))//拿到token加密密钥.必须是16个字符
          };
      });
            //配置authorrize
            services.AddAuthentication(b =>
            {
                b.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                b.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                b.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).
            AddCookie(option =>
            {
                option.LoginPath = new PathString("/Login/Index"); //设置登陆失败或者未登录授权的情况下，直接跳转的路径这里
                option.AccessDeniedPath = new PathString("/Error/Forbidden"); //没有权限时的跳转页面
                option.Cookie.Name = "My_SessionId";
                //设置cookie只读情况
                //option.Cookie.HttpOnly = true;
                //cookie过期时间
                //option.Cookie.Expiration = TimeSpan.FromSeconds(10);//此属性已经过期忽略，使用下面的设置
                //option.ExpireTimeSpan = new TimeSpan(1, 0, 0);//默认14天
            });
            //使用SignalR
            services.AddSignalR();
            //服务容器 IOC(控制反转)容器 注册类型，请求实例
            services.AddSession(); //该怎么去用session
            services.AddControllersWithViews();//添加控制器和视图
            services.AddHttpClient();//添加http
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //注入全局异常处理  2020年5月22日18:10:09  Dennyhui
            services.AddMvc(
                options =>
                {
                    options.Filters.Add<GlobalExceptionFilter>();
                }
                ).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//处理json返回的中文被转义
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());//处理json返回的时间格式
            });
            //解决文件上传大小限制 2020年4月30日13:02:51 Dennyhui
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddScoped<OperationLog>();
            //集中注册服务
            foreach (var item in GetClassName("Fisk.MDM.Business"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
            services.AddDbContext<MDMDBContext>(options => options.UseMySql(conStr));
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); //解决 "ExcelDataReader引发NotSupportedException 没有数据可用于编码1252" 的问题   Dennyhui
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MDMDBContext mysqlDbContext)
        {
            //初始化SessionHelper、CookieHelper实例成员 
            app.UseStaticHttpContext();
            //添加http请求
            app.UseCookiePolicy();
            //app.UseHttpsRedirection();

            app.UseHttpMethodOverride();
            //防盗链
            app.UseMiddleware<RefuseStealingMiddleWare>();
            app.UseSession();//表示请求需要使用session
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages(new StatusCodePagesOptions
                {
                    HandleAsync = (request) =>
                    {
                        if (request.HttpContext.Response.StatusCode.ToString().StartsWith("4"))
                        {
                            request.HttpContext.Response.Redirect("/Home/ErrorRequest");
                        }
                        else
                        {
                            request.Next.Invoke(request.HttpContext);
                        }
                        return Task.CompletedTask;
                    }
                });
                app.UseHsts();
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);//启用跨域；CORS 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行。 配置不正确将导致中间件停止正常运行。 2020年5月21日10:52:09 DennyHui
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
                endpoints.MapHub<VersionHub>("/VersionHub");//配置SignalR
            });
            mysqlDbContext.Database.EnsureCreated();//数据库不存在的话，会自动创建
        }

        /// <summary>  
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>  
        /// <param name="assemblyName">程序集</param>
        public Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }
                return result;
            }
            return new Dictionary<Type, Type[]>();
        }
    }
}
