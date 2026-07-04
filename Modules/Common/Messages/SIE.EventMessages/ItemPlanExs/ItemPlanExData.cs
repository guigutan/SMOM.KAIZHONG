using SIE.Core.Items;
using System;

namespace SIE.EventMessages.ItemPlanExs
{
    /// <summary>
    /// 物料计划资料数据
    /// </summary>
    [Serializable]
    public class ItemPlanExData
    {
        /// <summary>
        /// 物料
        /// </summary>
        public Item item { get; set; }
    }
}
