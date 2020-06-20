using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fisk.MDMAPISolution.Controllers
{
    public class OAuthController : Controller
    {
        [AllowAnonymous]
        public JsonResult Token(string name ,string pwd)
        {
            //从数据库验证用户名，密码 
            //验证通过 否则 返回Unauthorized

            //创建claim
            var claims = new[]
               {
                   new Claim(ClaimTypes.Name, "test"),
                   new Claim("userId","value")
               };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fiskmdmsolution"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                   issuer: "fisksoftmdm.issuer.com",
                   audience: "fisksoftmdm.com",
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(5),
                   signingCredentials: creds);
            return Json(new {
                Authorization = $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}"
            });
        }
    }
}