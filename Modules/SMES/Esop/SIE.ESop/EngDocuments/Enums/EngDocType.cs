using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Enums
{
    /// <summary>
    /// 工程文件维护类型
    /// </summary>
    public enum EngDocType
    {
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        Product = 1,

        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        WorkOrder = 2,
    }
}
