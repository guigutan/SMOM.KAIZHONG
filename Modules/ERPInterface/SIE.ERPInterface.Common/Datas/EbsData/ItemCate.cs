using SIE.Items;
using System;

namespace SIE.ERPInterface.Common.Datas.EbsData
{
    /// <summary>
    /// 物料分类
    /// </summary>
    [Serializable]
    public class ItemCate : EbsDataBase
    {        
        /// <summary>
        /// ERP物料分类ID
        /// </summary>
        public double Category_Id { get; set; }

        /// <summary>
        /// 连接的段
        /// </summary>
        public string Concatenated_Segments { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
             
        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 物料分类
        /// </summary>
        public ItemCategory? ItemCategory { get; set; }

        /// <summary>
        /// 分类层级ID
        /// </summary>
        public double? LevelId { get; set; }

        /// <summary>
        /// 分类父ID
        /// </summary>
        public double? TreePId { get; set; }
    }

}
