using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_entitydatchlogs
    {
        public int Id { get; set; }
        public int EntityID { get; set; }
        public string BatchID { get; set; }
        public string Code { get; set; }
        public string ErrorMessages { get; set; }
        public string State { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
