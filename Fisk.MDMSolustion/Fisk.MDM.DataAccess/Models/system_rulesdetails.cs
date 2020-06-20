using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_rulesdetails
    {
        public int Id { get; set; }
        public string MerginfCode { get; set; }
        public int? AttributeID { get; set; }
        public int? IsGroop { get; set; }
        public int? Weight { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Validity { get; set; }
    }
}
