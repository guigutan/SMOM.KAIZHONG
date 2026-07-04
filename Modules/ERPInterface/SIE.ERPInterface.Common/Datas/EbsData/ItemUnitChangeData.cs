using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 单位转换
    /// </summary>
    [Serializable]
    public class ItemUnitChangeData : EbsDataBase
    {
        /// <summary>
        /// 物品编码,为空则是EBS通用的转换
        /// </summary>
        public string Item_Code { get; set; }

        /// <summary>
        /// 单位分子
        /// </summary>
        public decimal Unit_Molecule { get; set; }

        /// <summary>
        /// 源单位编码
        /// </summary>
        public string From_Uom_Code { get; set; }

        /// <summary>
        /// 单位分母
        /// </summary>
        public decimal Unit_Denominator { get; set; }

        /// <summary>
        /// 目标单位编码
        /// </summary>
        public string To_Uom_Code { get; set; }

        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 物料单位
        /// </summary>
        public SIE.Items.ItemUnit? ItemUnit { get; set; }
    }


}
