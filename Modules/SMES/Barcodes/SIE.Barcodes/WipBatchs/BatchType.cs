using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次类型
    /// </summary>
    public enum BatchType
    {
        /// <summary>
        /// 返工
        /// </summary>
        [Label("返工")]
        Rework = 1,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scraped = 2,

        /// <summary>
        /// 可疑品
        /// </summary>
        [Label("可疑品")]
        Suspect = 3,

        /// <summary>
        /// 良品
        /// </summary>
        [Label("良品")]
        Good = 4
    }
}
