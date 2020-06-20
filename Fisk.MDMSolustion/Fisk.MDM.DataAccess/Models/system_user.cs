using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_user
    {
        public int Id { get; set; }
        public string UserAccount { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string Icon { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Creater { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Validity { get; set; }
        public string Type { get; set; }
        public string ADID { get; set; }
    }
}
