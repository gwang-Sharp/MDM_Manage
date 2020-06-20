namespace Fisk.MDM.Utility.Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// 一次能读取的最大行数   //因为DataTable的行数不是无限的，有容量限制
        /// </summary>
        public const int  maxRow = 100000;
    }
}
