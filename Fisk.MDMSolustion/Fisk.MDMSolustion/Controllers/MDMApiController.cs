/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：MDM共享API                                                    
*│　作    者：Dennyhui                                             
*│　版    本：1.0                                                 
*│　创建时间：2020年5月1日16:35:29                   
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Fisk.MDMSolustion.Controllers                      
*│　类       名： MDMApiController                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Fisk.MDM.Interface;
using Fisk.MDMSolustion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Fisk.MDMSolustion.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer,Cookies")] //使用JWT和Cookies双重身份验证架构
    public class MDMApiController : Controller
    {
        private readonly IMasterDataManage _masterdatamanage;
        private readonly IMasterData_Maintain_Manage maintain_Manage;
        public MDMApiController(IMasterDataManage MasterDataManage, IMasterData_Maintain_Manage _Maintain_Manage)
        {
            this._masterdatamanage = MasterDataManage;
            this.maintain_Manage = _Maintain_Manage;
        }
        [HttpPost]
        public string Test()
        {
            //获取当前用户信息
            var claims = User.Claims;
            var userName = User.Identity.Name;
            var userId = claims.FirstOrDefault(t => t.Type == "userId");
            return "OK";
        }
        [HttpPost]
        /// <summary>
        /// 导出Excel文件  2020年4月30日14:50:08  DennyHui
        /// </summary>
        /// <returns></returns>
        public FileResult OutputExcel()
        {
            string entityid = Request.Form["entityID"];
            string Type = Request.Form["Type"].FirstOrDefault();
            string Where = Request.Form["where"].FirstOrDefault();

            var result = _masterdatamanage.OutputExcel(entityid, Type, Where);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
        }
        [HttpPost]
        /// <summary>
        /// 保存实体成员  2020年4月30日14:26:52  Dennyhui
        /// </summary>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        public JsonResult DataSaveEntityMember()
        {
            string body = Request.Form["body"].ToString();
            return Json(_masterdatamanage.DataSaveEntityMember(body));
        }
        [HttpPost]
        /// <summary>
        /// excel上传  2020年4月30日12:51:53 Dennyhui
        /// </summary>
        /// <returns></returns>
        [ServiceFilter(typeof(OperationLog))]
        public object UploadExcel()
        {
            var file = Request.Form.Files[0];
            string entityName = Request.Form["entityname"].ToString();
            //string filename = file.FileName;
            //filename = filename.Substring(filename.LastIndexOf("."));
            var result = _masterdatamanage.UploadExcel(file, entityName);
            return result;
        }
        [HttpPost]
        /// <summary>
        /// 查询需要审批的数据  2020年5月20日16:46:37  Dennyhui
        /// </summary>
        /// <param name="workflow_instanceid"></param>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public object GetWorkFlow_ApproveData()
        {
            string entityid = Request.Query["entityID"].FirstOrDefault();
            string workflow_instanceid = Request.Query["workflow_instanceid"].FirstOrDefault();
            int Page = int.Parse(Request.Form["Page"].ToString());
            int Rows = int.Parse(Request.Form["Rows"].ToString());
            var result = _masterdatamanage.GetWorkFlow_ApproveData(workflow_instanceid, entityid, Page, Rows);
            return result;
        }
        [HttpPost]
        /// <summary>
        /// 审批数据成功，导入数据  2020年5月20日18:26:35  Dennyhui
        /// </summary>
        /// <param name="workflow_instanceid"></param>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public object ApproveData_OK()
        {
            string entityid = Request.Query["entityID"].FirstOrDefault();
            string workflow_instanceid = Request.Query["workflow_instanceid"].FirstOrDefault();
            var result = _masterdatamanage.ApproveData_OK(workflow_instanceid, entityid);
            return result;
        }

        /// <summary>
        /// 页面表格渲染
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InitViewTable(string ModelType, string where, int Page, int limit)
        {
            return Json(this.maintain_Manage.InitViewTable(ModelType, where, Page, limit));
        }

    }
}