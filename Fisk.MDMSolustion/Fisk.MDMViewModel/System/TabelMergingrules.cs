using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.ViewModel.System
{
    public class TabelMergingrules
    {
        public int Id { get; set; }
        public int? ModelID { get; set; }
        public int? EntityID { get; set; }
        public string State { get; set; }
        public string ModelName { get; set; }
        public string EntityName{ get; set; }
        public string CleaningRules { get; set; }
        public string MergingCode { get; set; }
        public string Creater { get; set; }
        public string CreateTime { get; set; }
        public string Updater { get; set; }
        public string UpdateTime { get; set; }
        public string Validity { get; set; }

        public string SelfMotion { get; set; }
        public string Manual { get; set; }
        public string NoProcessing { get; set; }

        public List<DetailsItem> DetailsItem { get; set; }
    }

    public class DetailsItem 
    {
       public int? ID { get; set; }
        
       public string Name { get; set; }
        
        public int? weight { get; set; }

        public int? IsGroop { get; set; }
    }
}
