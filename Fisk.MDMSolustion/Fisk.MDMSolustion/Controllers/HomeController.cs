using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fisk.MDMSolustion.Models;
using Fisk.MDM.Business;
using Fisk.MDM.Interface;
using Fisk.MDM.DataAccess.Models;

namespace Fisk.MDMSolustion.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMasterDataManage _modelmanage;

        public HomeController(IMasterDataManage ModelManage)
        {
            this._modelmanage = ModelManage;
        }
        [TypeFilter(typeof(UserAuthorizeAttribute))]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ErrorRequest()
        {
            return View();
        }
    }
}
