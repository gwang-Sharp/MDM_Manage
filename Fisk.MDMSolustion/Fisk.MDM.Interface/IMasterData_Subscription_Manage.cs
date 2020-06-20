using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.Interface
{
    public interface IMasterData_Subscription_Manage
    {

        #region 订阅管理 wg
        object InitSubscriptionTable(string EntityID, int page, int rows);
        object AttributesGet_ByEntityID(int entityid);

        object AddSubscription(string FormModel);
        object UpdataSubscription(string FormModel);
        object DelSubscription(int Id);
        #endregion

    }
}
