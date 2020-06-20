using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_datavalidation
    {
        public int Id { get; set; }
        public int? ModelID { get; set; }
        public int? EntityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FunctionName { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Validity { get; set; }
    }
}
