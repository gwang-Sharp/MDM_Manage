using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_version_snapshot_detail
    {
        public int id { get; set; }
        public int VersionID { get; set; }
        public string LinkEntityTable { get; set; }
        public int LinkEntityID { get; set; }
    }
}
