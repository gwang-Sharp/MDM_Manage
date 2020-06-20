using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fisk.MDM.Interface;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Fisk.MDMSolustion.Controllers.System
{
    public class XlsxController : Controller
    {
        private readonly IMasterDataManage _masterdatamanage;


        public XlsxController(IMasterDataManage MasterDataManage)
        {
            this._masterdatamanage = MasterDataManage;
        }
        public IActionResult XlsxIndex()
        {
            return View();
        }
        /// <summary>
        /// 导出Excel文件  2020年4月30日14:50:08  DennyHui
        /// </summary>
        /// <returns></returns>
        public FileResult OutputExcel()
        {
            string entityid = Request.Query["entityID"].FirstOrDefault();
            string Type = Request.Query["Type"].FirstOrDefault();
            string Where = Request.Query["where"].FirstOrDefault();
            var result = _masterdatamanage.OutputExcel(entityid, Type, Where);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UploadTemplate.xlsx");
        }

        /// <summary>
        /// 获取上传操作错误日志列表
        /// </summary>
        /// <param name="BatchID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetErrorLogs(string BatchID, int page, int rows)
        {
            return Json(_masterdatamanage.GetErrorLogs(BatchID, page, rows));
        }
        /// <summary>
        /// 下载上传错误文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DownError()
        {
            var batchID = Request.Query["batchid"].ToString();
            var entityid = Request.Query["EID"].ToString();
            var result = _masterdatamanage.DownError(batchID, entityid);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ErrorFile.xlsx");
        }

    }
}