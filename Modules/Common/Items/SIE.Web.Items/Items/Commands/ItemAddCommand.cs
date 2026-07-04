using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Utils;
using SIE.Web.Command;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ItemAddCommand")]
    public class ItemAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var item = args.Data.ToJsonObject<Item>();
            item.Code = RT.Service.Resolve<ItemController>().GetItemCode();
            item.State = State.Enable;
            var itemCategoryList = item.GetLazyList(ItemExtCategoryListProperty.ItemCategoryListProperty);
            if (itemCategoryList == null) return false;
            foreach (EnumViewModel enumViewModel in EnumViewModel.GetByEnumType(typeof(CategoryType)))
            {
                ItemCategoryRelation categoryRelation = new ItemCategoryRelation();
                categoryRelation.GenerateId();
                categoryRelation.Type = (CategoryType)enumViewModel.EnumValue;
                itemCategoryList.Add(categoryRelation);
            }
            return item;
        }
    }
}
