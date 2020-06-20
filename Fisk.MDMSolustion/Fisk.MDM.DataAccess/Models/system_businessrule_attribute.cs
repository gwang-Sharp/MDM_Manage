using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_businessrule_attribute
    {
        public int ID { get; set; }
        public int? ModelID { get; set; }
        public int? EntityID { get; set; }
        public int? AttributeID { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
        public string Expression { get; set; }
        public string Required { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Validity { get; set; }
    }
}
