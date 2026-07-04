using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Units;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ItemSaveCommand")]
    public class ItemSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data">实体</param>
        protected override void OnSaving(EntityList data)
        {
            var items = data as EntityList<Item>;
            items = RT.Service.Resolve<ItemController>().OnSavingItems(items);
            base.OnSaving(items);
        }
    }
}
