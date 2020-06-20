/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：JWT身份验证                                          
*│　作    者：Dennyhui                                             
*│　版    本：1.0                                                 
*│　创建时间：2020年5月1日16:35:29                   
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Fisk.MDMSolustion.Controllers                      
*│　类       名： OAuthController                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Fisk.MDM.Interface;
using Fisk.MDM.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fisk.MDMSolustion.Controllers
{
    public class OAuthController : Controller
    {
        private readonly IMasterDataManage _masterdatamanage;
        public OAuthController(IMasterDataManage MasterDataManage)
        {
            this._masterdatamanage = MasterDataManage;

        }
        /// <summary>
        /// 获取JWT Token进行身份验证  Dennyhui  2020年5月7日15:05:06
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Token(string name, string pwd)
        {
            //从数据库验证用户名，密码 
            //验证通过 否则 返回Unauthorized
            Result result = _masterdatamanage.Login(name, pwd);//验证用户名密码
            if (result.success)
            {
                //创建claim
                var claims = new[]
               {
                   new Claim(ClaimTypes.Name, name),
                   new Claim("password", pwd)
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2020fiskmdmsolution"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                       issuer: "fisksoftmdm.issuer.com",
                       audience: "fisksoftmdm.com",
                       claims: claims,
                       expires: DateTime.Now.AddSeconds(5),
                       signingCredentials: creds);
                //生成Token
                return Json(new
                {
                    Authorization = $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}"
                });
            }
            else
            {
                return Json("failure");
            }
        }

    }
}