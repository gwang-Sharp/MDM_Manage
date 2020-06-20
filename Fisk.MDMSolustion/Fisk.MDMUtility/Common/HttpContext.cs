//*********************************************************************************
//Description:HttpContext帮助类
//Author:DennyHui
//Create Date: 2020年4月21日16:05:56
//*********************************************************************************

using Microsoft.AspNetCore.Http;

namespace Fisk.MDM.Utility.Common
{
    /// <summary>
    /// http上下文
    /// </summary>
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 当前上下文
        /// </summary>
        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;


        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }

}
