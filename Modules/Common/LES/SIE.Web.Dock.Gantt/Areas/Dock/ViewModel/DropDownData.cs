using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.Gantt.Areas.Dock.ViewModel
{
    /// <summary>
    /// 下拉选择数据格式
    /// </summary>
    [Serializable]
    public class DropValue
    {
        /// <summary>
        /// value
        /// </summary>
        public double? value { get; set; }

        /// <summary>
        /// text
        /// </summary>
        public string text { get; set; }
    }
}
