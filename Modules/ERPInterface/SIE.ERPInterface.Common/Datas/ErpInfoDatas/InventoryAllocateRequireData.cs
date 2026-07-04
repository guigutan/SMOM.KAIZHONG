using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 需求数据
    /// </summary>
    [Serializable]
    public class InventoryAllocateRequireData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 指定批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 指定任务号
        /// </summary>
        public string AppointTaskNo
        {
            get; set;
        }

        /// <summary>
        /// 指定库位
        /// </summary>
        public string AppointLoc
        {
            get; set;
        }

        /// <summary>
        /// 指定LPN
        /// </summary>
        public string AppointLPN
        {
            get; set;
        }

        /// <summary>
        /// 指定项目号
        /// </summary>
        public string AppointProjectNo
        {
            get; set;
        }

        /// <summary>
        /// 指定批次属性01(生产日期)
        /// </summary>
        public DateTime? LotAtt01 { get; set; }

        /// <summary>
        /// 指定批次属性02（失效日期）
        /// </summary>
        public DateTime? LotAtt02 { get; set; }

        /// <summary>
        /// 指定批次属性03（收货日期）
        /// </summary>
        public DateTime? LotAtt03 { get; set; }

        /// <summary>
        /// 指定批次属性04(生产批次)
        /// </summary>
        public string LotAtt04 { get; set; }
    }
}
