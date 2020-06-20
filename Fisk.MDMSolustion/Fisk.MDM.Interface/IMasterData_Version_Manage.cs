using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fisk.MDM.Interface
{
    public interface IMasterData_Version_Manage
    {
        #region 版本管理 wg
        object InitVersionTable(int entityID, int page, int rows);

        object InitAttrStraceTable(int entityID, string attrID, int page, int rows);

        object VersionDel(int EntityID, string versionName);
        Task<bool> CreateVersion(string linkTable, int entityId, string versionId);

        #endregion

    }
}
