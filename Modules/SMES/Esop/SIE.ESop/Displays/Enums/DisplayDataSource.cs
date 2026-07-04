using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.Displays.Enums
{
    /// <summary>
    /// 显示点数据源
    /// </summary>
    public enum DisplayDataSource
    {
        /// <summary>
        /// 文档集
        /// </summary>
        [Label("文档集")]
        Document = 1,

        /// <summary>
        /// 工程文件维护
        /// </summary>
        [Label("工程文件维护")]
        Engineer = 2,
    }
}
