using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_globalexception_log
    {
        public string ID { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ErrorMsg { get; set; }
        public string IP { get; set; }
    }
}
