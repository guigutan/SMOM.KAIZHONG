using SIE.CrossPlatform.Collect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 工单包装规则
    /// </summary>
    [Serializable]
    public class XPWorkOrderPackageRuleDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal LevelQty { get; set; }

        /// <summary>
        /// 是否装箱
        /// </summary>
        public bool IsPackage { get; set; }

        /// <summary>
        /// 是否出库标签
        /// </summary>
        public bool IsOutStockLabel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 是否入库标签
        /// </summary>
        public bool IsInStockLabel { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId { get; set; }

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? NumberRuleId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 打印模板ID
        /// </summary>
        public double? PrintTemplateId { get; set; }

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint { get; set; }

        /// <summary>
        /// 物料包装规则明细Id
        /// </summary>
        public double DetailId { get; set; }

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackageUnitName { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string NumberRuleName { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit { get; set; }
    }
}
