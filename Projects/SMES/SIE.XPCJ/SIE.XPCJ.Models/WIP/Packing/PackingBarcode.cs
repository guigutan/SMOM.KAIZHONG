using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Packing
{
    /// <summary>
    /// 包装号
    /// </summary>
    [Label("包装号")]
    public partial class PackingBarcode
    {
        /// <summary>
        /// 包装号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime PrintDate { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// 工单包装规则Id
        /// </summary>
        public double PackageRuleDetailId { get; set; }

        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintTimes { get; set; }

        /// <summary>
        /// 打印状态
        /// </summary>
        public BarcodeState PrintedState { get; set; }

        /// <summary>
        /// 打印人Id
        /// </summary>
        public double PrintById { get; set; }

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId { get; set; }

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnitName { get; set; }
    }
}
