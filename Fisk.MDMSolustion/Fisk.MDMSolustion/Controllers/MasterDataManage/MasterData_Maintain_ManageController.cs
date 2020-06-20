using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisk.MDM.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Fisk.MDMSolustion.Controllers.MasterDataManage
{
    public class MasterData_Maintain_ManageController : Controller
    {
        private readonly IMasterData_Maintain_Manage maintain_Manage;
        public MasterData_Maintain_ManageController(IMasterData_Maintain_Manage _Maintain_Manage)
        {
            this.maintain_Manage = _Maintain_Manage;
        }
        #region 实体维护
        /// <summary>
        /// 初始化实体表格
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InitEntityTable(int Entity, string where, int page, int rows)
        {
            return Json(this.maintain_Manage.InitEntityTable(Entity, where, page, rows));
        }
        #endregion
    }
}