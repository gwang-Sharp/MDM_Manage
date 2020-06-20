using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_rolenavassignment
    {
        public int Id { get; set; }
        public string RoleID { get; set; }
        public string NavCode { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Creater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Updater { get; set; }
        public string Validity { get; set; }
    }
}
