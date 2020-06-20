using Fisk.MDM.DataAccess.Models;
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
    //����webӦ������ķ�����м��
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";//

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //���ÿ�������  2020��5��21��10:50:22  Dennyhui
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
                .AllowAnyOrigin());
            });

            //����JWT������֤ 2020��5��7��13:55:00  Dennyhui
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,//�Ƿ���֤Issuer
              ValidateAudience = true,//�Ƿ���֤Audience
              ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
              ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
              ValidAudience = "fisksoftmdm.com",
              //�����������ǩ��tokenʱ��issuer,Audienceһ��
              ValidIssuer = "fisksoftmdm.issuer.com",
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2020fiskmdmsolution"))//�õ�token������Կ.������16���ַ�
          };
      });
            //����authorrize
            services.AddAuthentication(b =>
            {
                b.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                b.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                b.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).
            AddCookie(option =>
            {
                option.LoginPath = new PathString("/Login/Index"); //���õ�½ʧ�ܻ���δ��¼��Ȩ������£�ֱ����ת��·������
                option.AccessDeniedPath = new PathString("/Error/Forbidden"); //û��Ȩ��ʱ����תҳ��
                option.Cookie.Name = "My_SessionId";
                //����cookieֻ�����
                //option.Cookie.HttpOnly = true;
                //cookie����ʱ��
                //option.Cookie.Expiration = TimeSpan.FromSeconds(10);//�������Ѿ����ں��ԣ�ʹ�����������
                //option.ExpireTimeSpan = new TimeSpan(1, 0, 0);//Ĭ��14��
            });
            //ʹ��SignalR
            services.AddSignalR(options =>
            {
                //������������ʱ����
                options.HandshakeTimeout = TimeSpan.FromSeconds(1800);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(1800);
                //options.KeepAliveInterval = TimeSpan.FromSeconds(1800);
                options.EnableDetailedErrors = true;
            }).AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//����json���ص����ı�ת��
                options.PayloadSerializerOptions.Converters.Add(new DateTimeConverter());//����json���ص�ʱ���ʽ
            });
            //�������� IOC(���Ʒ�ת)���� ע�����ͣ�����ʵ��
            services.AddSession(); //����ôȥ��session
            services.AddControllersWithViews();//���ӿ���������ͼ
            services.AddHttpClient();//����http
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //ע��ȫ���쳣����  2020��5��22��18:10:09  Dennyhui
            services.AddMvc(
                options =>
                {
                    options.Filters.Add<GlobalExceptionFilter>();
                }
                ).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//����json���ص����ı�ת��
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());//����json���ص�ʱ���ʽ
            });
            //����ļ��ϴ���С���� 2020��4��30��13:02:51 Dennyhui
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddScoped<OperationLog>();
            //����ע�����
            foreach (var item in GetClassName("Fisk.MDM.Business"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
            services.AddDbContext<MDMDBContext>(options => options.UseMySql(Configuration.GetConnectionString("MysqlConnection")));
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); //��� "ExcelDataReader����NotSupportedException û�����ݿ����ڱ���1252" ������   Dennyhui
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MDMDBContext mysqlDbContext)
        {
            //һ��������߶������
            //RequestDelegate

            #region middleware
            //app.Use(next =>
            //{
            //    Console.WriteLine("Middleware 1 out");
            //    return new RequestDelegate(
            //       async context =>
            //        {
            //            await context.Response.WriteAsync("This is Middleware 1 start");
            //            await next.Invoke(context);
            //            await context.Response.WriteAsync("This is Middleware 1 end");
            //        });
            //});
            //app.Use(next =>
            //{
            //    Console.WriteLine("Middleware 2 out");
            //    return new RequestDelegate(
            //       async context =>
            //       {
            //           await context.Response.WriteAsync("This is Middleware 2 start");
            //           await next.Invoke(context);
            //           await context.Response.WriteAsync("This is Middleware 2 end");
            //       });
            //});
            //app.Use(
            //    next =>
            //    {
            //        Console.WriteLine("Middleware 3 out");
            //        return new RequestDelegate(
            //           async context =>
            //           {
            //               await context.Response.WriteAsync("This is Middleware 3 start");
            //               //next.Invoke(context);//404
            //               await context.Response.WriteAsync("This is Middleware 3 end");
            //           });
            //    }
            //);
            //1 ��һ�Ű�ֽ��û���κ����õĶ�����Ҫʲôдʲô��pay-for-what-you-use
            //2 ��������չ---���⻷������˳�򶼿�����չ

            //��һ�׶��ǳ�ʼ��������Build---�õ�RequestDelegate-middleware1(������next����)--����ǹܵ�
            //Ȼ�����������---Context--һ�����ã�Ȼ��Ͷ���˹����(revserse˳�������)

            //д�����������С���̣���ר�Ž���await async +ί��
            #endregion


            #region Ȩ����֤
            //app.Use(next =>
            //{
            //    return async c =>
            //    {
            //        if (c.Request.Query.ContainsKey("Name"))
            //        {
            //            await next.Invoke(c);
            //        }
            //        else
            //        {
            //            await c.Response.WriteAsync("No Authorization");
            //        }
            //    };
            //});
            #endregion
            //����http����
            app.UseCookiePolicy();

            //app.UseHttpsRedirection();

            app.UseHttpMethodOverride();
            //������
            app.UseMiddleware<RefuseStealingMiddleWare>();
            app.UseSession();//��ʾ������Ҫʹ��session
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);//���ÿ���CORS �м����������Ϊ�ڶ� UseRouting �� UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������С� 2020��5��21��10:52:09 DennyHui
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
                endpoints.MapHub<VersionHub>("/VersionHub");//����SignalR
            });
            mysqlDbContext.Database.EnsureCreated();//���ݿⲻ���ڵĻ������Զ�����
        }

        /// <summary>  
        /// ��ȡ�����е�ʵ�����Ӧ�Ķ���ӿ�
        /// </summary>  
        /// <param name="assemblyName">����</param>
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