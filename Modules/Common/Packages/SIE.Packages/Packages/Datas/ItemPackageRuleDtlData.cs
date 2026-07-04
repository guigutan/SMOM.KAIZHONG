using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Packages.Packages.Datas
{
    /// <summary>
    /// 包装规则明细数据
    /// </summary>
    [Serializable]
    public class ItemPackageRuleDtlData
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 包装规则ID
        /// </summary>
        public double ItemPackageRuleId { get; set; }

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 包装单位
        /// </summary>
        public double PackageUnitId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        /// 编码规则
        /// </summary>
        public double? NumberRuleId { get; set; }

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit { get; set; }
    }
}
