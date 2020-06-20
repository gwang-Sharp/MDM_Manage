using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_log
    {
        public string ID { get; set; }
        public string UserAccount { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Parameters { get; set; }
        public string OperateResult { get; set; }
        public string UserHostAddress { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
