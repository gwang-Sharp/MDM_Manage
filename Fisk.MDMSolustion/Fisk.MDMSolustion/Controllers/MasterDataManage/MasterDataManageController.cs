using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.ViewModel.System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fisk.MDMSolustion.Controllers.MasterDataManage
{
    public class MasterDataManageController : Controller
    {
        private readonly IMasterDataManage _masterdatamanage;
        public MasterDataManageController(IMasterDataManage MasterDataManage)
        {
            this._masterdatamanage = MasterDataManage;
        }
        #region 属性管理 wg
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AttributeAdd(string attribute)
        {
            var attributeModel = JsonConvert.DeserializeObject<system_attribute>(attribute);
            return Json(this._masterdatamanage.AddAttribute(attributeModel));
        }

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AttributesGet(int id, int page, int rows)
        {
            return Json(this._masterdatamanage.AttributesGet(id, page, rows));
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AttributeUpdate(string attribute)
        {
            var attributeModel = JsonConvert.DeserializeObject<system_attribute>(attribute);
            return Json(this._masterdatamanage.AttributeUpdate(attributeModel));
        }
        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AttributeDel(int Id)
        {
            return Json(this._masterdatamanage.AttributeDel(Id));
        }
        #endregion


        #region 实体管理 hhyang
        /// <summary>
        /// 查询实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">查询实体所需要的参数</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult SearchEntity(EntityManageItem Params)
        {
            var resert = this._masterdatamanage.SearchEntity(Params);
            return Json(resert);
        }
        /// <summary>
        /// 添加实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">添加实体所需要的参数</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult InserEntity(EntityManageItem Params)
        {
            var resert = this._masterdatamanage.InserEntity(Params);

            return Json(resert);
        }

        /// <summary>
        /// 更新实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">更新实体所需要的参数</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult UpdateEntity(EntityManageItem Params)
        {
            var resert = this._masterdatamanage.UpdateEntity(Params);
            return Json(resert);
        }

        /// <summary>
        /// 删除实体  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">删除实体所需要的参数</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult DeleteEntity(EntityManageItem Params)
        {
            var resert = this._masterdatamanage.DeleteEntity(Params);
            return Json(resert);
        }

        #endregion


        #region 模型管理 wyt
        [HttpPost]

        public JsonResult SearchModel(int limit, int page, string where)
        {
            return Json(_masterdatamanage.SearchModel(limit, page, where));
        }
        /// <summary>
        /// 删除模型数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <param name="logRetentionDays"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteModel(int id)
        {
            system_model Model = new system_model()
            {
                Id = id
            };
            return Json(_masterdatamanage.DeleteModel(Model));
        }

        /// <summary>
        /// 编辑模型数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateModel(int id, string name, string remark, int logRetentionDays)
        {
            system_model Model = new system_model()
            {
                Id = id,
                Name = name,
                Remark = remark,
                LogRetentionDays = logRetentionDays
            };
            return Json(_masterdatamanage.UpdateModel(Model));
        }
        /// <summary>
        /// 添加模型数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <param name="logRetentionDays"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult InserModel(string name, string remark, int logRetentionDays)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //这里通过 HttpContext.User.Claims 可以将我们在Login这个Action中存储到cookie中的所有
                //claims键值对都读出来，比如我们刚才定义的UserName的值Wangdacui就在这里读取出来了
                var userName = HttpContext.User.Claims.First().Value;
            }

            system_model Model = new system_model()
            {
                Name = name,
                Remark = remark,
                LogRetentionDays = logRetentionDays,
                Creater = "System",
                CreateTime = DateTime.Now
            };
            return Json(_masterdatamanage.InserModel(Model));
        }

        #endregion
    }
}