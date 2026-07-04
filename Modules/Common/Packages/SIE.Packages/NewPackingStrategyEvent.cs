using SIE.Packages.Packings.Strategys;
using System;

namespace SIE.Packages.Packings
{
    /// <summary>
    /// 包装流程控制事件
    /// </summary>
    [Serializable]
    public class NewPackingStrategyEvent
    {
        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 包装数量是否已满
        /// </summary>
        public bool IsPackageQtyFull { get; set; }

        /// <summary>
        /// 包装物料数量是否已满
        /// </summary>
        public bool IsPackageItemFull { get; set; }

        /// <summary>
        /// 整个包装层次已满
        /// </summary>
        public bool IsPackageRuleFull { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        public bool IsSucess
        {
            get
            {
                return Error.IsNullOrWhiteSpace();
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 外包装关系
        /// </summary>
        public PackingRelation OuterPackingRelation { get; set; }

        /// <summary>
        /// 内包装码，可能是SN或包装号
        /// </summary>
        public string CollectBarcode { get; set; }

        /// <summary>
        /// 策略类型
        /// </summary>
        public ScanStrategyMode StrategyType { get; set; }
    }
}
