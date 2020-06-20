using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_navigation
    {
        public int Id { get; set; }
        public string NavCode { get; set; }
        public string NavName { get; set; }
        public string NavDesc { get; set; }
        public string ParentNavCode { get; set; }
        public string NavURL { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Creater { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Updater { get; set; }
        public string Grade { get; set; }
        public int? SequenceNo { get; set; }
        public int? ConfirmDelete { get; set; }
    }
}
