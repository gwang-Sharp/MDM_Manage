using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.ViewModel.System
{
    public class MergingrulesItem
    {
        public int? ID { get; set; }
        public int? ModelID { get; set; }
        public int? EntityID { get; set; }

        public List<Rulesdetail> RulesOfTheData { get; set; }

        public int Page { get; set; }

        public int limit { get; set; }

        public string where { get; set; }


    }
    public class Rulesdetail 
    {
       public int AttributeID { get; set; }

        public int Weight { get; set; }

        public string AttributeValue { get; set; }


    }
}
