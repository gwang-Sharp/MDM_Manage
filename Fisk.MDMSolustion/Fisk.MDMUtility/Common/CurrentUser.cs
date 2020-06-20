//*********************************************************************************
//Description:偶去当前登录用户帮助类
//Author:DennyHui
//Create Date: 2020年4月21日16:33:47
//*********************************************************************************

namespace Fisk.MDM.Utility.Common
{
    public class CurrentUser
    {
        /// <summary>
        /// 得到当前登录用户的域帐号
        /// </summary>
        public static string UserAccount
        {
            get
            {
                try
                {
                    string _userAccount = string.Empty;
                    string AuthenticationType = AppsettingsHelper.GetSection("AuthenticationType");
                    switch (AuthenticationType)
                    {
                        case "form":
                            if (SessionHelper.GetSession("UserName") != null)
                            {
                                _userAccount = SessionHelper.GetSession("UserName").ToString();
                            }
                            break;
                        case "windows":
                            {
                                _userAccount = HttpContext.Current.User.Identity.Name;
                                _userAccount = _userAccount.Substring(_userAccount.IndexOf("\\") + 1);
                                if (string.IsNullOrEmpty(_userAccount))
                                {
                                    _userAccount = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                                }
                                if (string.IsNullOrEmpty(_userAccount))
                                {
                                    _userAccount = AppsettingsHelper.GetConfigStr("DevelopUser");
                                }
                                break;
                            }
                    }
                    ////判断session是否为空
                    if (string.IsNullOrEmpty(_userAccount))
                    {
                        //读登录Cookie 获取user
                        //CookieHelper co = new CookieHelper();
                        _userAccount = CookieHelper.getCurrentUser();
                        SessionHelper.SetSession("UserName", _userAccount); //重新添加用户
                    }
                    //_userAccount = System.Web.HttpContext.Current.User.Identity.Name;

                    return _userAccount;
                }
                catch (System.Exception ex)
                {
                    return null;
                }

            }
        }
    }
}