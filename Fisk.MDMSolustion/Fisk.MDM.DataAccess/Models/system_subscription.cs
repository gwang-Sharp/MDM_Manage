using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EntityID { get; set; }
        public string AttributeID { get; set; }
        public string SubscriptionRemark { get; set; }
        public string Apiaddress { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
