using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_version_zipper
    {
        public int EntityID { get; set; }
        public string AttributeID { get; set; }
        public string Value { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
        public string AttributeName { get; set; }
    }
}
