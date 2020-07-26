using Fisk.MDM.Utility.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.Utility.Middleware
{
    /// <summary>
    /// 获取IHttpContextAccessor服务 初始化SessionHelper、CookieHelper实例成员   WG
    /// </summary>
    public static class StaticHttpContextExtensions
    {
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>();
            CookieHelper.Configure(httpContextAccessor);
            SessionHelper.Configure(httpContextAccessor);
            return app;
        }
    }
}
