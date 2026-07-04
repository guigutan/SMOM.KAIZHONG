using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.Gantt.Areas.Dock.ViewModel
{
    /// <summary>
    /// 保存返回信息
    /// </summary>
    [Serializable]
    public class SaveReturnMsg
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 失败的信息
        /// </summary>
        public string Message { get; set; }
    }
}
