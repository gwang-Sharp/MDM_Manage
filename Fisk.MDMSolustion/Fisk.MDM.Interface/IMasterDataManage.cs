using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.ViewModel;
using Fisk.MDM.ViewModel.System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fisk.MDM.Interface
{
    public interface IMasterDataManage
    {
        #region 模型管理
        object SearchModel(int limit, int page, string where);

        object InserModel(system_model model);

        object UpdateModel(system_model model);

        object DeleteModel(system_model model);
        #endregion
        #region 属性管理
        object Attributes_GetAll();
        object AddAttribute(system_attribute attribute);

        object AttributesGet(int id, int page, int rows);

        object Attributes_ModelsGet();

        object Attributes_EntitysGet();
        object AttributeUpdate(system_attribute attribute);

        object AttributeDel(int Id);
        #endregion
        #region 实体管理
        object RefreshEntity();
        object SearchEntity(EntityManageItem model);

        object InserEntity(EntityManageItem model);

        object UpdateEntity(EntityManageItem model);

        object DeleteEntity(EntityManageItem model);

        object DelStageData(int EntityID, int id);

        object GetAll_Attributes(int EntityID);
        #endregion
        #region 登录管理
        Result Login(string Account, string pwd);
        #endregion


        #region mdm接口
        object DataSaveEntityMember(string body);

        byte[] OutputExcel(string entityID,string type,string Where);

        byte[] ExportMergeDataResult(string entityID, string mergingCode);

        object UploadExcel(IFormFile file, string entityName);

        object GetErrorLogs(string BatchID, int page, int rows);

        byte[] DownError(string BatchID, string entityid);

        object GetWorkFlow_ApproveData(string workflow_instanceid, string entityid,int page,int rows);

        object ApproveData_OK(string workflow_instanceid, string entityid);

        object testGlobalException();
        #endregion

    }
}
