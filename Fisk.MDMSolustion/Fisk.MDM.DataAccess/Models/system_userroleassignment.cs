using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_userroleassignment
    {
        public int Id { get; set; }
        public int? RoleID { get; set; }
        public int? UserID { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Creater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Updater { get; set; }
        public string Validity { get; set; }
    }
}
