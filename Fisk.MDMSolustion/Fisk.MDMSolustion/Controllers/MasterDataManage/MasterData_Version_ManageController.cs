using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisk.MDM.Interface;
using Fisk.MDMSolustion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Fisk.MDMSolustion.Controllers.MasterDataManage
{
    public class MasterData_Version_ManageController : Controller
    {
        private readonly IMasterData_Version_Manage _Version_Manage;
        private readonly IHubContext<VersionHub> _VersionHub;
        public MasterData_Version_ManageController(IMasterData_Version_Manage _Version_Manage, IHubContext<VersionHub> hub)
        {
            this._Version_Manage = _Version_Manage;
            this._VersionHub = hub;
        }
        #region 视图管理

        #endregion

        #region 版本管理

        /// <summary>
        /// 获取实体版本快照记录
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InitVersionTable(int EntityID, int page, int rows)
        {
            return Json(this._Version_Manage.InitVersionTable(EntityID, page, rows));
        }
        /// <summary>
        /// 删除对应版本信息
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="versionName"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult VersionDel(int entityID, string versionName)
        {
            return Json(this._Version_Manage.VersionDel(entityID, versionName));
        }
        /// <summary>
        /// 获取实体下的熟悉列表(跟踪)
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IActionResult InitAttrStraceTable(int EntityID, string AttrID, int page, int rows)
        {
            return Json(this._Version_Manage.InitAttrStraceTable(EntityID, AttrID, page, rows));
        }

        #endregion
    }
}