using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_mergingrules_similarresult
    {
        public string SimilarFlag { get; set; }
        public string EntityID { get; set; }
        public string Code { get; set; }
        public string SimilarNum { get; set; }
        public string MergingCode { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Creator { get; set; }
        public string IdentityFlag { get; set; }
        public int Id { get; set; }
    }
}
