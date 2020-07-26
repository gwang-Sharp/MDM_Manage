using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fisk.MDM.Utility.Middleware
{
    public class RefuseStealingMiddleWare
    {
        private readonly RequestDelegate _next;

        public RefuseStealingMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string url = context.Request.Path.Value.ToLower();
            if (!url.Contains(".jpg")|| !url.Contains(".png"))
            {
                await _next(context);//走正常流程
            }
            else {
                string urlReferrer = context.Request.Headers["Referer"].ToString()?? context.Request.Headers["referer"].ToString();
                if (string.IsNullOrWhiteSpace(urlReferrer))//直接访问
                {
                    await this.SetForbiddenImage(context);//返回404图片
                }
                else if (!urlReferrer.Contains("rfc-china.com"))//非当前域名
                {
                    await this.SetForbiddenImage(context);//返回404图片
                }
                else
                {
                    await _next(context);//走正常流程
                }
            }

        }
        /// <summary>
        /// 设置拒绝图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task SetForbiddenImage(HttpContext context)
        {
            string defaultImagePath = "wwwroot/Img/Home/error.jpg";
            string path = Path.Combine(Directory.GetCurrentDirectory(), defaultImagePath);

            FileStream fs = File.OpenRead(path);
            byte[] bytes = new byte[fs.Length];
            await fs.ReadAsync(bytes, 0, bytes.Length);
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
