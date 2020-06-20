using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.ViewModel
{
    public class Result
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 状态信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 数据信息
        /// </summary>
        public dynamic data { get; set; }
    }
    public class UploadResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 状态信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 批次编号
        /// </summary>
        public string batchid { get; set; }
        /// <summary>
        /// 数据信息
        /// </summary>
        public dynamic data { get; set; }
    }

    public class tableResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 状态信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 数据信息
        /// </summary>
        public dynamic data { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public dynamic total { get; set; }
        /// <summary>
        /// 额外参数
        /// </summary>
        public dynamic ExtraData { get; set; }
    }
}
