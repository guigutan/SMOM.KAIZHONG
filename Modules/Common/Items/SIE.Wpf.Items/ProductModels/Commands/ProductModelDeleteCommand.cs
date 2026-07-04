using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Items.ProductModels.Commands
{
    /// <summary>
    /// 移除产品机型与产品族的绑定按钮
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", GroupType = CommandGroupType.Edit)]
    class ProductModelDeleteCommand : ListEditCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && view.SelectedEntities.Count > 0;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            if (!CRT.MessageService.AskQuestion(string.Format("你确认删除选择的{0}条数据吗？".L10N(), view.SelectedEntities.Count)))
                return;

            var selectedItems = view.SelectedEntities.OfType<ProductModel>().ToList();
            var ctl = RT.Service.Resolve<ItemController>();
            var criteria = new ProductModelCriteria();
            var list = ctl.RemoveProductModel(selectedItems, criteria);

            view.Data = list;
            view.RefreshControl();
        }
    }
}
