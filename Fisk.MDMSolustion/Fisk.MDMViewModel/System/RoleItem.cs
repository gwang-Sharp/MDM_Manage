using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.ViewModel.System
{
    public  class RoleItem
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public string RoleDesc { get; set; }

        public int Page { get; set; }

        public int limit { get; set; }

        public string Where { get; set; }

        public int UserId { get; set; }

        public string ParmsId { get; set; }
    }
}
