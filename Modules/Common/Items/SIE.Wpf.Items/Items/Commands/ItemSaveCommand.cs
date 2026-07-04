using SIE.Domain;
using SIE.EventMessages.ItemPlanExs;
using SIE.Items;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Items.Items.Commands
{
    /// <summary>
    /// 物料保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存数据", Gestures = "Ctrl+S", GroupType = 10)]
    public class ItemSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            IEnumerable<Item> data = view.Data.OfType<Item>().Where(p => p.Precision == null && p.PersistenceStatus == PersistenceStatus.New);
            data.ForEach(p => p.Precision = p.Unit?.Precision);

            base.Execute(view);
        }

        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        protected override void OnSaving(ListLogicalView view)
        {
            var item = view.Current as Item;
            ItemPlanExData itemPlanExData = new ItemPlanExData();
            itemPlanExData.item = item;
            if (item != null && item.Photo == null)
                item.Photo = new byte[0];

            RT.EventBus.Publish<ItemPlanExData>(itemPlanExData);
            base.OnSaving(view);
        }

        /// <summary>
        /// 执行后事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnExecuted(CommandExecutedArgs e)
        {
            base.OnExecuted(e);
            CRT.MessageService.ShowInstantMessage("保存成功！".L10N(), "提示信息".L10N(), 3);
        }
    }
}
