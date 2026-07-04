using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CSM.Suppliers.Datas
{
    /// <summary>
    /// 供应商配置项值
    /// </summary>
    [Serializable]
    public class SupplierConfigValue
    {
        /// <summary>
        /// 委外调入库位Id
        /// </summary>
        public double OutsourcingInLocId { get; set; }

        /// <summary>
        /// 委外调入库位名称
        /// </summary>
        public string OutsourcingInLocName { get; set; }

        /// <summary>
        /// 委外调入库位编码
        /// </summary>

        public string OutsourcingInLocCode { get; set; }

        /// <summary>
        /// 委外扣料库位Id
        /// </summary>
        public double OutsourcingOutLocId { get; set; }

        /// <summary>
        /// 委外扣料库位名称
        /// </summary>
        public string OutsourcingOutLocName { get; set; }

        /// <summary>
        /// 委外扣料库位编码
        /// </summary>

        public string OutsourcingOutLocCode { get; set; }

        /// <summary>
        /// 扣料时点
        /// </summary>
        public OutsourcingTimeType OutsourcingUseTime { get; set; }

        /// <summary>
        /// 委外扣料处理
        /// </summary>
        public OutsourcingReceiveType OutsourcingReceiveType { get; set; }

        /// <summary>
        /// 库存是否管理货主
        /// </summary>
        public bool IsHasStorer { get; set; }
    }
}
