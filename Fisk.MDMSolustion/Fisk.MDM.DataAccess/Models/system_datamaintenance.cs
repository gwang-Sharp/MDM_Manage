using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_datamaintenance
    {
        public int Id { get; set; }
        public int EntityID { get; set; }
        public string AttributeID { get; set; }
        public string RuleName { get; set; }
        public string RuleType { get; set; }
        public string RuleRemark { get; set; }
        public string Apiaddress { get; set; }
        public string BpmName { get; set; }
        public string ApplyTitle { get; set; }
        public string MergeAPI { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
