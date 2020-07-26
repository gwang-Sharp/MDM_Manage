/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：全局异常监控和记录                                                    
*│　作    者：Dennyhui                                             
*│　版    本：1.0                                                 
*│　创建时间：2020年5月22日18:42:35               
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间：Fisk.MDM.Business            
*│　类       名：GlobalExceptionFilter                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Fisk.MDMSolustion.Models
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly MDMDBContext _dbContext;
        public GlobalExceptionFilter(MDMDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 发生异常时进入
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                Result result = new Result();
                result.success = false;
                result.message = "发生错误，" + context.Exception.Message;
                result.data = "";
                context.Result = new ContentResult
                {
                    //Content = context.Exception.Message,//这里是把异常抛出。也可以不抛出。
                    Content = JsonConvert.SerializeObject(result),
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "text/html;charset=utf-8"
            };
                system_globalexception_log sgel = new system_globalexception_log();
                sgel.ID = Guid.NewGuid().ToString();
                sgel.Creater = CurrentUser.UserAccount;
                sgel.CreateTime = DateTime.Now;
                sgel.Controller = context.RouteData.Values["controller"].ToString();
                sgel.Action = context.RouteData.Values["action"].ToString();
                //sgel.ErrorMsg = context.Exception.StackTrace.ToString();
                sgel.ErrorMsg = context.Exception.Message;
                _dbContext.system_globalexception_log.Add(sgel);
                _dbContext.SaveChanges();
            }
            context.ExceptionHandled = true;
        }

        /// <summary>
                /// 异步发生异常时进入
                /// </summary>
                /// <param name="context"></param>
                /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
