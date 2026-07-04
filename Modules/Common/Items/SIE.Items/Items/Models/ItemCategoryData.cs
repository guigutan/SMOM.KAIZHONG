using SIE.EventMessages;
using System;

namespace SIE.Items
{
    /// <summary>
    /// ERP物料分类接口数据
    /// </summary>
    [Serializable]
    public class ItemCategoryData : ErpInfoData
    {
        /// <summary>
        /// 分类类型0-库存类别;1-质量类别; 2-齐套类别
        /// </summary>
        public int CategoryType { get; set; }

        /// <summary>
        /// 物料类型0-成品 1-原材料 2-半成品
        /// </summary>
        public int ItemType { get; set; }

        /// <summary>
        /// 分类层级
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode { get; set; }
    }

    /// <summary>
    /// 更新父Id数据
    /// </summary>
    public class UpdateItemCategoryTreeData
    {
        /// <summary>
        ///  物料分类
        /// </summary>
        public ItemCategory ItemCategory { get; set; }

        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Infkey { get; set; }
    }
}
