using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 物料数据
    /// </summary>
    [Serializable]
    public class ItemDataEbs : EbsDataBase
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        public int Item_Id { get; set; }

        /// <summary>
        /// 物品编码
        /// </summary>
        public string Item_Code { get; set; }

        /// <summary>
        /// 物品名称
        /// </summary>
        public string Item_Name { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string Unit_Code { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification_Model { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 最小包装数量
        /// </summary>
        public string Minpacking_Qty { get; set; }

        /// <summary>
        /// 英文描述
        /// </summary>
        public string English_Description { get; set; }

        /// <summary>
        /// 简短描述
        /// </summary>
        public string Short_Description { get; set; }

        /// <summary>
        /// 单位长度
        /// </summary>
        public decimal? Unit_Length { get; set; }

        /// <summary>
        /// 单位宽度
        /// </summary>
        public decimal? Unit_Width { get; set; }

        /// <summary>
        /// 单位高度
        /// </summary>
        public decimal? Unit_Height { get; set; }

        /// <summary>
        /// 单位体积
        /// </summary>
        public decimal? Unit_Volume { get; set; }

        /// <summary>
        /// 单位重量
        /// </summary>
        public decimal? Unit_Weight { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public string Is_Delete { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public int Organization_Id { get; set; }

        /// <summary>
        /// 物品类型(约定0成品 1原材料 2半成品 3其他)
        /// </summary>
        public int? Item_Type { get; set; }

        /// <summary>
        /// 物品来源类型
        /// </summary>
        public string Item_Source_Type { get; set; }

        /// <summary>
        /// 采购员
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        /// 计划员
        /// </summary>
        public string Planner { get; set; }


        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public double Category_Id { get; set; }

        /// <summary>
        /// 批次管理 =2的时候是批次管理
        /// </summary>
        public int? Lot_Control_Code { get; set; }

        /// <summary>
        /// 物料分类编码
        /// </summary>
        public string ItemCategoryCode { get; set; }
    }


}
