using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 物料单位
    /// </summary>
    [Serializable]
    public class ItemUnit : EbsDataBase
    {        
        /// <summary>
        /// 单位代码
        /// </summary>
        public string Uom_Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Unit_Of_Measure { get; set; }

        /// <summary>
        /// 计量单位类别，需要手动建立跟ERP一样的单位类型快码
        /// </summary>
        public string Uom_Class { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
       
        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public SIE.Items.Unit? Unit { get; set; }
    }

}
