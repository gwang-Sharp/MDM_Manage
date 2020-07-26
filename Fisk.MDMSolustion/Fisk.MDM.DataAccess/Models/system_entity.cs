using System;
using System.Collections.Generic;

namespace Fisk.MDM.DataAccess.Models
{
    public partial class system_entity
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public int? BeganIn { get; set; }
        public string StageTable { get; set; }
        public string EntityTable { get; set; }
        public string HistoryTable { get; set; }
        public string ValiditeProc { get; set; }
        public string BusinessProc { get; set; }
        public string DataImportProc { get; set; }
        public sbyte? AutoCreateCode { get; set; }
        public string Creater { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
