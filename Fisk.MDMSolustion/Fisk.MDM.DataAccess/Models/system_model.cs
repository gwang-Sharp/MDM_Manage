using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public int? LogRetentionDays { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
