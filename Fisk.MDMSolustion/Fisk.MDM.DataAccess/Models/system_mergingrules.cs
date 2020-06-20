using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_mergingrules
    {
        public int Id { get; set; }
        public int? ModelID { get; set; }
        public int? EntityID { get; set; }
        public string MergingCode { get; set; }
        public int? IsGroop { get; set; }
        public string SelfMotion { get; set; }
        public string Manual { get; set; }
        public string NoProcessing { get; set; }
        public int? State { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Validity { get; set; }
    }
}
