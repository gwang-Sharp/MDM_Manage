//*********************************************************************************
//Description:Cookie帮助类
//Author:DennyHui
//Create Date: 2020年4月21日16:31:29
//*********************************************************************************
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;



namespace Fisk.MDM.Utility.Common
{
    public class CookieHelper : Controller
    {
        private static IHttpContextAccessor _httpContextAccessor;
        internal static void Configure(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        private static ISession _session => _httpContextAccessor.HttpContext.Session;
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes),
            });
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
        /// <summary>
        /// 从Cookie中获取当前登录用户  2020年4月23日21:39:03 
        /// </summary>
        /// <returns></returns>
        public static string getCurrentUser()
        {
            string CurrentUser = _httpContextAccessor.HttpContext.User.Claims.First().Value;
            return CurrentUser;
        }
    }
}
