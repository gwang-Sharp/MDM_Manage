//*********************************************************************************
//Description:session帮助类
//Author:DennyHui
//Create Date: 2020年4月21日16:02:22
//*********************************************************************************
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Fisk.MDM.Utility.Common
{
    public  class SessionHelper
    {

        private static IHttpContextAccessor _httpContextAccessor;
        private static ISession _session => _httpContextAccessor.HttpContext.Session;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetSession(string key, string value)
        {
            _session.SetString(key, value);  
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        public static string GetSession(string key)
        {
            var value = _session.GetString(key);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
        /// <summary>
        /// 删除session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void RemoveSession(string key, string value)
        {
            _session.Remove(key);
        }
    }
    /// <summary>
    /// session扩展 2020年4月21日16:02:35  Dennyhui
    /// </summary>
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}

