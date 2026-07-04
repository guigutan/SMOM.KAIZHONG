using SIE.Items;
using SIE.xUnit.Core;

namespace SIE.xUnit.Items.Items.Fixtures
{
    /// <summary>
    /// 物料(物料分类为质量分类)
    /// </summary>
    public class ItemFixture : FixtureBase
    {

        /// <summary>
        /// 物料
        /// </summary>
        public Item FixPropItem { get; set; }

        /// <summary>
        /// 质量分类
        /// </summary>
        public ItemCategory FixPropItemCategory { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemFixture()
        {
            FixPropItem = CreateItem();
            FixPropItemCategory = CreateItemCategory(FixPropItem);
        }

        /// <summary>
        /// 创建物料与质量分类
        /// </summary>
        /// <param name="item"></param>
        protected virtual ItemCategory CreateItemCategory(Item item)
        {
            return RT.Service.Resolve<ItemTestController>().CreateItemCategoryWithRelationQuality(item.Id);
        }

        /// <summary>
        /// 创建物料
        /// </summary>
        /// <returns></returns>
        protected virtual Item CreateItem()
        {
            var item = RT.Service.Resolve<ItemTestController>().CreateItem();
            return item;
        }

    }
}
