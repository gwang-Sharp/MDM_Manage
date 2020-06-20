using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisk.MDM.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Fisk.MDMSolustion.Controllers.MasterDataManage
{
    public class MasterData_Subscription_ManageController : Controller

    {
        private readonly IMasterData_Subscription_Manage subscription_Manage;
        public MasterData_Subscription_ManageController(IMasterData_Subscription_Manage _Subscription_Manage)
        {
            this.subscription_Manage = _Subscription_Manage;
        }

        #region 订阅管理 wg
        /// <summary>
        /// 初始化数据订阅列表
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IActionResult InitSubscriptionTable(string EntityID, int page, int rows)
        {
            return Json(this.subscription_Manage.InitSubscriptionTable(EntityID, page, rows));
        }
        /// <summary>
        /// 获取实体相关属性
        /// </summary>
        /// <param name="EntityID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AttributesGet_ByEntityID(int EntityID)
        {
            return Json(this.subscription_Manage.AttributesGet_ByEntityID(EntityID));
        }
        /// <summary>
        /// 添加实体订阅数据
        /// </summary>
        /// <param name="FormModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddSubscription(string FormModel)
        {
            return Json(this.subscription_Manage.AddSubscription(FormModel));
        }
        /// <summary>
        /// 更新实体订阅数据
        /// </summary>
        /// <param name="FormModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdataSubscription(string FormModel)
        {
            return Json(this.subscription_Manage.UpdataSubscription(FormModel));
        }
        /// <summary>
        /// 删除实体订阅数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelSubscription(int Id)
        {
            return Json(this.subscription_Manage.DelSubscription(Id));
        }

        #endregion
    }
}