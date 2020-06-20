using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.ViewModel.System;
using Fisk.MDMSolustion.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fisk.MDMSolustion.Controllers.MasterDataManage
{
    public class MasterData_Quality_ManageController : Controller
    {
        private readonly IMasterData_Quality_Manage  quality_Manage;
        public MasterData_Quality_ManageController(IMasterData_Quality_Manage _Quality_Manage)
        {
            this.quality_Manage = _Quality_Manage;
        }

        #region 数据维护 wg
        /// <summary>
        /// 获取维护数据表信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetDatamaintenance(int page, int rows)
        {
            return Json(this.quality_Manage.GetDatamaintenance(page, rows));
        }
        /// <summary>
        /// 添加数据维护规则
        /// </summary>
        /// <param name="_Datamaintenance"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddDatamaintenance(int entityid, string _Datamaintenance)
        {
            return Json(this.quality_Manage.AddDatamaintenance(entityid, _Datamaintenance));
        }
        /// <summary>
        /// 删除实体数据维护规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelEntityDataRule(int id)
        {
            return Json(this.quality_Manage.DelEntityDataRule(id));
        }
        /// <summary>
        /// 编辑实体数据维护规则
        /// </summary>
        /// <param name="FormModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdataDatamaintenance(string FormModel)
        {
            return Json(this.quality_Manage.UpdataDatamaintenance(FormModel));
        }
        /// <summary>
        /// 根据实体ID获取开启变更的属性
        /// </summary>
        /// <param name="EntityID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Attributes_GetAll_ByEntityID(int EntityID)
        {
            return Json(this.quality_Manage.Attributes_GetAll_ByEntityID(EntityID));
        }
        #endregion

        #region 合并规则

        /// <summary>
        /// 编辑 2020年4月27日   hhyang
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        public JsonResult ChangeThreshold(system_mergingrules item)
        {
            return Json(quality_Manage.ChangeThreshold(item));
        }

        /// <summary>
        /// 编辑合并规则 2020年4月27日   hhyang
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperationLog))]
        public JsonResult EiteMergingrules(MergingrulesItem item)
        {
            return Json(quality_Manage.EiteMergingrules(item));
        }

        /// <summary>
        /// 添加合并规则  2020年4月27日   hhyang
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        [HttpPost]
        [ServiceFilter(typeof(OperationLog))]
        public JsonResult InsertMergingrules(MergingrulesItem item)
        {
            return Json(quality_Manage.InsertMergingrules(item));
        }

        /// <summary>
        /// 删除合并规则  2020年4月27日   hhyang
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        [HttpPost]
        [ServiceFilter(typeof(OperationLog))]
        public JsonResult DeleteMergingrules(int RuleID)
        {

            return Json(quality_Manage.DeleteMergingrules(RuleID));
        }

        /// <summary>
        /// 合并数据  2020年4月30日14:26:31 Dennyhui
        /// </summary>
        public void MergeData(string entityID, string mergingCode)
        {
            quality_Manage.MergeData(entityID, mergingCode);
        }

        /// <summary>
        /// 查看合并规则  2020年4月27日   hhyang
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        [HttpPost]
        [ServiceFilter(typeof(OperationLog))]
        public JsonResult SearchMergingrules(int page, int limit, string where)
        {

            return Json(quality_Manage.SearchMergingrules(page, limit, where));
        }

        #endregion


        #region 业务规则
        /// <summary>
        /// 删除数据验证
        /// </summary>
        /// <param name="DataID"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult DeleteFieldValidation(string DataID)
        {
            return Json(quality_Manage.DeleteFieldValidation(DataID));
        }


        /// <summary>
        /// 查询数据验证表格信息 2020年4月28日 10:54 hhyang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult SearchFieldValidation(int page, int limit, string where)
        {
            return Json(quality_Manage.SearchFieldValidation(page, limit, where));
        }
        /// <summary>
        /// 编辑数据验证信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult UpdateFieldValidation(system_fieldvalidation data)
        {
            return Json(quality_Manage.UpdateFieldValidation(data));
        }


        /// <summary>
        /// 添加数据验证信息 2020年4月28日 10:54 hhyang
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult InsertFieldValidation(system_fieldvalidation data)
        {
            return Json(quality_Manage.InsertFieldValidation(data));
        }

        /// <summary>
        /// 删除数据验证
        /// </summary>
        /// <param name="DataID"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult DeleteDataValidation(string DataID)
        {
            return Json(quality_Manage.DeleteDataValidation(DataID));
        }


        /// <summary>
        /// 查询数据验证表格信息 2020年4月28日 10:54 hhyang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult SearchDataValidation(int page, int limit, string where)
        {
            return Json(quality_Manage.SearchDataValidation(page, limit, where));
        }

        /// <summary>
        /// 编辑数据验证信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult UpdateDataValidation(system_datavalidation data)
        {
            return Json(quality_Manage.UpdateDataValidation(data));
        }

        /// <summary>
        /// 添加数据验证信息 2020年4月28日 10:54 hhyang
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult InsertDataValidation(system_datavalidation data)
        {
            return Json(quality_Manage.InsertDataValidation(data));
        }

        [HttpPost]
        public JsonResult AttributeRuleRelease(int modelID, int entityID)
        {
            return Json(quality_Manage.AttributeRuleRelease(modelID, entityID));

        }


        /// <summary>
        /// 编辑业务规则数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult EiteRuleBase(system_businessrule_attribute file)
        {
            return Json(quality_Manage.EiteRuleBase(file));
        }

        /// <summary>
        /// 添加业务规则数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult InsertRuleBase(system_businessrule_attribute file)
        {
            return Json(quality_Manage.SearchRuleBase(file));
        }

        /// <summary>
        /// 删除规则数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult DeleteRuleBase(string RuleID)
        {
            return Json(quality_Manage.DeleteRuleBase(RuleID));
        }
        /// <summary>
        /// 查询业务规则数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult SearchRuleBase(int page, int limit, string where)
        {
            return Json(quality_Manage.SearchRuleBase(page, limit, where));
        }

        /// <summary>
        /// 获取属性下拉数据
        /// </summary>
        /// <param name="EntityID"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult GetAttributeSelectRule(string EntityID)
        {
            return Json(quality_Manage.GetAttributeSelectRule(EntityID));
        }


        /// <summary>
        /// 获取模型下拉数据
        //
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetModelSelectRule()
        {
            return Json(quality_Manage.GetModelSelectRule());
        }
        /// <summary>
        /// 获取实体下拉数据
        //
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEntitySelectRule(string ModelID)
        {
            return Json(quality_Manage.GetEntitySelectRule(ModelID));
        }
        #endregion
    }
}