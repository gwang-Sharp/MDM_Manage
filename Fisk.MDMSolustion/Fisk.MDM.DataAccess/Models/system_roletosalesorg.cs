﻿using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_roletosalesorg
    {
        public int id { get; set; }
        public string RoleID { get; set; }
        public string OrgLevelName { get; set; }
        public int IsValid { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
