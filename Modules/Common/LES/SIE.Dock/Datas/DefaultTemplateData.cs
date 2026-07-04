using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Datas
{
    /// <summary>
    /// 默认模板数据
    /// </summary>
    [Serializable]
    public class DefaultTemplateData
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public double TemplateId { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateFileName { get; set; }
    }
}
