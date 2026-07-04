using SIE.Domain.Validation;
using SIE.Items;
using System.ComponentModel;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 被备件基础数据引用的分类不能删除
    /// </summary>
    [DisplayName("被备件基础数据引用的分类不能删除")]
    [Description("被备件基础数据引用的分类不能删除")]
    public class ItemCategoryNoReferencedRule : NoReferencedRule<ItemCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemCategoryNoReferencedRule()
        {
            Properties.Add(SparePart.ItemCategoryIdProperty);
        }
    }
}
