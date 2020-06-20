using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_attribute
    {
        public int Id { get; set; }
        public int? EntityID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Remark { get; set; }
        public string DataType { get; set; }
        public string Type { get; set; }
        public int? TypeLength { get; set; }
        public string StartTrace { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? LinkEntityID { get; set; }
        public int? LinkModelID { get; set; }
        public int? IsDefault { get; set; }
        public int? IsDisplay { get; set; }
        public int? IsSystem { get; set; }
    }
}
