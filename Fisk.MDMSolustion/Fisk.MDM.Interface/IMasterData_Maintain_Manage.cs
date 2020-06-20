using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.Interface
{
    public interface IMasterData_Maintain_Manage
    {
        //系统数据维护 WG
        object InitEntityTable(int EntityID, string where, int page, int rows);
        //对外数据维护 WG
        object InitViewTable(string ModelType, string where, int Page, int limit);

    }
}
