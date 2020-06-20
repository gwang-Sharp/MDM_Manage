using Fisk.MDM.Business;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Utility.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Fisk.MDMSolustion.Models
{
    /// <summary>
    /// 操作日志拦截器 2020年4月21日11:26:43  Dennyhui
    /// </summary>
    public class OperationLog : System.Attribute, IActionFilter
    {
        private readonly ILogger<MasterDataManage> _logger;
        MDMDBContext md = new MDMDBContext();
        /// <summary>
        /// Action执行后
        /// </summary>
        public  void  OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var Request = filterContext;//获取控制器传入的参数
                //var result = ((JsonObject)filterContext.Result).Data.ToString();//获取控制器返回的结果
                system_log s_log = new system_log
                {
                    ID = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    ActionName = filterContext.RouteData.Values["Action"].ToString(),
                    ControllerName = filterContext.RouteData.Values["Controller"].ToString(),
                    Parameters = JsonConvert.SerializeObject(filterContext.HttpContext.Request.Form),
                    OperateResult = JsonConvert.SerializeObject(filterContext.Result),
                    UserHostAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserAccount = CurrentUser.UserAccount
                };
                md.system_log.Add(s_log);
                md.SaveChangesAsync();
                //this._dbContext.Add(operation);
                //this._dbContext.SaveChanges();
                //System_Log sl = new System_Log();
                //sl.Id = Guid.NewGuid().ToString();
                //sl.Tkey = Enum.GetName(typeof(operate), Key);
                //sl.UserAccount = CurrentUser.UserAccount;
                //sl.OperateResult = result;
                //sl.Description = Description;
                //sl.CreateTime = DateTime.Now;
                //sl.ActionName = filterContext.RouteData.Values["Action"].ToString();
                //sl.ControllerName = filterContext.RouteData.Values["Controller"].ToString();
                //sl.Parameters = filterContext.HttpContext.Request.Form;
                //sl.UserHostAddress = Request.HttpContext.Request.Host;
                //sl.BrowserInformation = Request.HttpContext.+ "_" + Request.Browser.Version;
            }
            catch (Exception ex)
            {
                this._logger.LogError($@"operation error ,detail desc：{ex}");
            }

            //logService.Save(model);
        }

        /// <summary>
        /// Action执行前
        /// </summary>
        public  void OnActionExecuting(ActionExecutingContext context)
        {
            
        }


        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
