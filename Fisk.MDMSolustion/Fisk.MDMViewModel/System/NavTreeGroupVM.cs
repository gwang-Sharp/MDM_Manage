using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.ViewModel.System
{
    public class NavTreeGroupVM
    {
        public string id { get; set; }
        public string label { get; set; }
        public string NavName { get; set; }
        public string NavCode { get; set; }
        public string ParentNavCode { get; set; }
        public string NavDesc { get; set; }
        public string SequenceNo { get; set; }
        public string disabled { get; set; }

        public string NavURL { get; set; }
        public bool checkeds { get; set; }

        public string isLeaf { get; set; }
        public int? Grade { get; set; }

        public List<NavTreeGroupVM> children { get; set; }
        public int? ConfirmDelete { get; set; }
       

    }
}
