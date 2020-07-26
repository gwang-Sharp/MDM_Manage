using Fisk.MDM.Business;
using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.Interface;
using Fisk.MDM.Utility.Common;
using Fisk.MDM.ViewModel.System;
using Fisk.MDMSolustion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Fisk.MDMSolustion.Controllers.System
{
    [Authorize]//登录验证
    public class SystemController : Controller
    {
        //private DbContext DbContext;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMasterDataManage _masterdatamanage;
        private readonly ISystemManage system;


        public SystemController(IMasterDataManage MasterDataManage, ISystemManage systemManage)
        {
            this._masterdatamanage = MasterDataManage;
            this.system = systemManage;

        }

        /// <summary>
        /// 反权限关联
        /// </summary>
        /// <returns></returns>
        public IActionResult AssociateRole()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        ///<summary>
        /// 属性管理
        /// </summary>
        /// <returns></returns>
        public IActionResult AttributesManagement()
        {
            return View();
        }
        /// <summary>
        /// 业务规则管理 
        /// </summary>
        /// <returns></returns>
        public IActionResult RuleBaseFactory()
        {
            return View();
        }
        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Attributes_GetAll()
        {
            return Json(this._masterdatamanage.Attributes_GetAll());
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Attributes_EntitysGet()
        {
            return Json(this._masterdatamanage.Attributes_EntitysGet());
        }
        /// <summary>
        /// 获取所有模型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Attributes_ModelsGet()
        {
            return Json(this._masterdatamanage.Attributes_ModelsGet());
        }
        #region 用户管理
        public IActionResult UserManagement()
        {
            return View();
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public IActionResult UserGet(int page, int rows, string where)
        {
            return Json(this.system.UserGet(page, rows, where));
        }

        [HttpPost]
        public IActionResult UserSearch(string name, int page, int rows)
        {
            return Json(this.system.UserSearch(name, page, rows));
        }
        [HttpPost]
        public JsonResult getRoleInUserPaginationList(int page, int rows, string RoleID)
        {
            return Json(this.system.getRoleInUserPaginationList(page, rows, RoleID));
        }

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult UserAdd(string user)
        {
            var userModel = JsonConvert.DeserializeObject<system_user>(user);
            return Json(this.system.UserAdd(userModel));
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult UserUpdate(string user)
        {
            var userModel = JsonConvert.DeserializeObject<system_user>(user);
            return Json(this.system.UserUpdate(userModel));
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult UserDel(int Id)
        {
            return Json(this.system.UserDel(Id));
        }
        #endregion


        public IActionResult AuthorityCenter()
        {

            return View();
        }

        [TypeFilter(typeof(UserAuthorizeAttribute))]
        public IActionResult ModelManagement()
        {

            return View();
        }
        public IActionResult SubscriptionManage()
        {
            return View();
        }
        /// <summary>
        /// 版本快照管理
        /// </summary>
        /// <returns></returns>
        public IActionResult VersionManagement()
        {
            return View();
        }
        /// <summary>
        /// 合并规则页面管理
        /// </summary>
        /// <returns></returns>
        public IActionResult ParentScopedRules()
        {
            return View();
        }
        /// <summary>
        /// 菜单
        /// </summary>
        /// <returns></returns>
        public IActionResult NavManage()
        {
            return View();
        }

        /// <summary>
        /// 角色管理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleManage()
        {
            return View();
        }

        #region 实体管理   hhyang
        /// <summary>
        /// 实体管理页面  2020年4月16日  hhyang
        /// </summary>
        /// <returns></returns>
        public IActionResult EntityManagement()
        {
            return View();
        }
        #region 实体数据维护管理 wg
        /// <summary>
        /// 实体管理公用页面
        /// </summary>
        /// <returns></returns>
        public IActionResult CommonToolView()
        {
            ViewBag.entity = RouteData.Values["id"]?.ToString();
            return View();
        }
        /// <summary>
        /// 删除stage表数据
        /// </summary>
        /// <param name="EntityId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelStageData(int EntityId, int Id)
        {
            return Json(this._masterdatamanage.DelStageData(EntityId, Id));
        }
        [HttpPost]
        public IActionResult GetAll_Attributes(int EntityID)
        {
            return Json(this._masterdatamanage.GetAll_Attributes(EntityID));
        }

        #endregion
        #endregion

        #region 菜单管理   hhyang
        /// <summary>
        /// 查询菜单  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">所需要的参数</param>
        /// <returns></returns>
        [HttpPost]
        //[ServiceFilter(typeof(OperationLog))]
        public JsonResult SearchMenu(MenuItem model)
        {
            return Json(system.SearchMenu(model));
        }



        /// <summary>
        /// 添加菜单  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">添加所需要的参数</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult InserMenu(MenuItem model)
        {
            return Json(system.InserMenu(model));
        }
        /// <summary>
        /// 删除菜单  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">删除所需要的参数</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult DeleteMenu(MenuItem model)
        {
            return Json(system.DeleteMenu(model));
        }

        /// <summary>
        /// 编辑菜单  2020年 4月 20日 hhyang
        /// </summary>
        /// <param name="model">编辑所需要的参数</param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult UpdateMenu(MenuItem model)
        {
            return Json(system.UpdateMenu(model));
        }
        #endregion

        #region   角色管理

        // 查询角色数据  2020年4月21日 hhyang
        [HttpPost]

        public JsonResult SearchRole(RoleItem Parmas)
        {
            return Json(system.SearchRole(Parmas));
        }

        //更新角色 2020年4月21日 hhyang
        [HttpPost]

        public JsonResult UpdateRole(RoleItem Parmas)
        {
            return Json(system.UpdateRole(Parmas));
        }

        /// <summary>
        /// 删除角色 2020年4月21日 hhyang
        /// </summary>
        /// <param name="Parmas"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult DeleteRole(RoleItem Parmas)
        {
            return Json(system.DeleteRole(Parmas));
        }

        /// <summary>
        /// 添加所需要的的参数 2020年4月21日 hhyang
        /// </summary>
        /// <param name="Parmas"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult InserRole(RoleItem Parmas)
        {
            var resert = system.InserRole(Parmas);

            return Json(resert);
        }
        #endregion

        /// <summary>
        /// 查询所有的角色 hhyang
        /// </summary>
        /// <param name="Parmas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RoleRelation(RoleItem Parmas)
        {
            return Json(system.RoleRelation(Parmas));
        }

        /// <summary>
        /// 得到用户拥有的角色id hhyang
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserHaveRole(string UserId)
        {
            var resert = system.UserHaveRole(UserId);
            return Json(resert);
        }

        /// <summary>
        /// 设置角色 hhyang
        /// </summary>
        /// <param name="RoleList"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserConfigRole(string[] RoleList, string UserId)
        {
            var resert = system.UserConfigRole(RoleList, UserId);

            return Json(resert);
        }


        /// <summary>
        /// 得到角色里面的用户 hhyang
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RoleHaveUser(string RoleID)
        {
            return Json(system.RoleHaveUser(RoleID));
        }

        /// <summary>
        /// 用户点击所有用户 hhyang
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllUser(string where)
        {
            return Json(system.GetAllUser(where));
        }

        /// <summary>
        ///点击用户获取有权限的角色id hhyang
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserAllCheckedRole(string UserID)
        {
            return Json(system.GetUserAllCheckedRole(UserID));
        }
        /// <summary>
        /// 获取角色列表并且用户权限选中排头排序分页列表  2020年4月22日18:41:28 hhyang
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRolByUserDataOrderByDesc(RoleItem parmas)
        {
            return Json(system.GetRolByUserDataOrderByDesc(parmas));
        }

        /// <summary>
        /// 修改用户对应的权限 hhyang
        /// </summary>
        /// <param name="FilNoList"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifUserRole(List<ModifRoleRelation> FilNoList)
        {
            return Json(system.ModifUserRole(FilNoList));
        }

        /// <summary>
        /// 得到角色对应的菜单权限 hhyang
        /// </summary>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMenuNodeCheckRoleList(string MenuId)
        {
            return Json(system.GetMenuNodeCheckRoleList(MenuId));
        }

        /// <summary>
        /// 查询 hhyang
        /// </summary>
        /// <param name="parmas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMenuNodeOrderByDesc(RoleItem parmas)
        {
            return Json(system.GetMenuNodeOrderByDesc(parmas));
        }

        /// <summary>
        /// 反关联权限修改菜单 hhyang
        /// </summary>
        /// <param name="FilNoList"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifMenuRole(List<ModifRoleRelation> FilNoList)
        {
            return Json(system.ModifMenuRole(FilNoList));
        }

        /// <summary>
        /// 对用户增加新的角色 hhyang
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult NewUserAssignmentRole(string data)
        {
            return Json(system.NewUserAssignmentRole(data));
        }

        /// <summary>
        /// 修改菜单权限 hhyang
        /// </summary>
        /// <param name="NavCodeObj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult NewEditRoleMenuByNavCode(string NavCodeObj)
        {
            return Json(system.NewEditRoleMenuByNavCode(NavCodeObj));
        }

        /// <summary>
        /// 点击角色获取菜单选中 hhyang
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRoleCheckedMenu(string RoleID)
        {
            return Json(system.GetRoleCheckedMenu(RoleID));
        }

        /// <summary>
        /// 点击全选查询所有的id  hhyang
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRolAllDataList()
        {
            return Json(system.GetRolAllDataList());
        }

        /// <summary>
        /// 获取菜单 hhyang 
        /// </summary>
        /// <returns></returns>
        public JsonResult getMenuList()
        {
            return Json(system.getMenuList());
        }

        /// <summary>
        /// 保存实体成员  2020年4月30日14:26:52  Dennyhui
        /// </summary>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        [HttpPost]
        public JsonResult DataSaveEntityMember()
        {
            string body = Request.Form["body"].ToString();
            return Json(_masterdatamanage.DataSaveEntityMember(body));
        }


        /// <summary>
        /// 导出数据合并结果的Excel文件 2020年5月9日15:05:01 Dennyhui
        /// </summary>
        /// <returns></returns>
        public FileResult ExportMergeDataResult()
        {
            string entityid = Request.Query["entityID"].FirstOrDefault();
            string mergingCode = Request.Query["mergingCode"].FirstOrDefault();
            var result = _masterdatamanage.ExportMergeDataResult(entityid, mergingCode);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MergeDataResult.xlsx");
        }
        /// <summary>
        /// excel上传  2020年4月30日12:51:53 Dennyhui
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object UploadExcel()
        {
            var file = Request.Form.Files[0];
            string entityName = Request.Form["entityname"].ToString();
            //string filename = file.FileName;
            //filename = filename.Substring(filename.LastIndexOf("."));
            var result = _masterdatamanage.UploadExcel(file, entityName);
            return result;
        }
        public object testGlobalException()
        {
            return _masterdatamanage.testGlobalException();
        }


    }
}