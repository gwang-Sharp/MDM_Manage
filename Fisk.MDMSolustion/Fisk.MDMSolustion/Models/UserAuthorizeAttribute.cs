using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Fisk.MDM.Business
{
    public class UserAuthorizeAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public  void OnActionExecuting(ActionExecutingContext context)
        {
            bool IsAuthenticated = false;
            var requestURL = context.HttpContext.Request.Path;
            //如果HttpContext.User.Identity.IsAuthenticated为true，
            //或者HttpContext.User.Claims.Count()大于0表示用户已经登录
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                IsAuthenticated = true;
            }
            if (IsAuthenticated)
            {
                //这里通过 HttpContext.User.Claims 可以将我们在Login这个Action中存储到cookie中的所有
                //claims键值对都读出来，比如我们刚才定义的UserName的值admin就在这里读取出来了
                var userName = context.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            }
            else
            {
                context.Result = new RedirectResult("/Login/Index?returnURL=" + UrlEncoder.Default.Encode(requestURL));
                return;
            }
        }
    }
}
