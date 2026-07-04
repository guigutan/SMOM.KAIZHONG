using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Utils;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 物料添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加", GroupType = CommandGroupType.Edit)]
    public class ItemAddCommand : ListAddCommand
    {
        /// <summary>
        /// 重写实体创建后方法
        /// </summary>
        /// <param name="entity">Entity</param>
        protected override void OnItemCreated(Entity entity)
        {
            Item item = entity as Item;
            item.Code = RT.Service.Resolve<ItemController>().GetItemCode();
            item.State = State.Enable;
            var itemCategoryList = item.GetLazyList(ItemExtCategoryListProperty.ItemCategoryListProperty);
            if (itemCategoryList == null) return;
            foreach (EnumViewModel enumViewModel in EnumViewModel.GetByEnumType(typeof(CategoryType)))
            {
                itemCategoryList.Add(new ItemCategoryRelation() { Type = (CategoryType)enumViewModel.EnumValue });
            }
        }
    }
}