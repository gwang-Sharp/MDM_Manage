using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fisk.MDMSolustion.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMasterDataManage _masterdatamanage;

        public LoginController(IMasterDataManage MasterDataManage, IHttpContextAccessor httpContextAccessor)
        {
            _masterdatamanage = MasterDataManage;
            //new SessionHelper(httpContextAccessor);
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> doLogin(string userAccount, string pwd)
        {
            Result result = _masterdatamanage.Login(userAccount, pwd);

            if (result.success)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, userAccount));
                identity.AddClaim(new Claim("password", pwd));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //验证是否授权成功
                if (principal.Identity.IsAuthenticated)
                {
                    SessionHelper.SetSession("UserName", userAccount);
                    return Json("/Home/Index");
                }
            }
            return Json("failure");
        }

        /// <summary>
        /// 登出  2020年4月23日22:41:56 Dennyhui
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login/Index");
        }
    }
}