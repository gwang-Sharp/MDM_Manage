using Fisk.MDM.DataAccess.Models;
using Fisk.MDM.ViewModel.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.Interface
{
    public interface IMasterData_Quality_Manage
    {

        #region 业务规则--数据维护 wg
        object AddDatamaintenance(int EntityID, string _Datamaintenance);
        object GetDatamaintenance(int page, int rows);
        object UpdataDatamaintenance(string FormModel);
        object DelEntityDataRule(int id);
        object Attributes_GetAll_ByEntityID(int EntityID);
        #endregion
        object GetModelSelectRule();
        object GetEntitySelectRule(string modelID);
        object GetAttributeSelectRule(string entityID);
        object SearchRuleBase(int page, int limit, string where);
        object SearchRuleBase(system_businessrule_attribute file);
        object DeleteRuleBase(string ruleID);
        object EiteRuleBase(system_businessrule_attribute file);
        object AttributeRuleRelease(int modelID, int entityID);

        object InsertDataValidation(system_datavalidation data);
        object UpdateDataValidation(system_datavalidation data);
        object SearchDataValidation(int page, int limit, string where);
        object DeleteDataValidation(string dataID);
        object InsertFieldValidation(system_fieldvalidation data);

        object UpdateFieldValidation(system_fieldvalidation data);

        object SearchFieldValidation(int page, int limit, string where);

        object DeleteFieldValidation(string dataID);
        object SearchMergingrules(int page, int limit, string where);
        void MergeData(string entityID, string mergingCode);
        object DeleteMergingrules(int ruleID);
        object InsertMergingrules(MergingrulesItem item);
        object EiteMergingrules(MergingrulesItem item);
        object ChangeThreshold(system_mergingrules item);
    }
}
