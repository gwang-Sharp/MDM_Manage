using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Creater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Updater { get; set; }
        public string Validity { get; set; }
    }
}
