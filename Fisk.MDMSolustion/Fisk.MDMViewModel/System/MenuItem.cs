using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.ViewModel.System
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Sorting { get; set; }

        public string Describ { get; set; }

        public string ConfirmDelete { get; set; }
    
        public string where { get; set; }
        public string ParentNavCode { get; set; }


        public string Grade { get; set; }

    }
}
