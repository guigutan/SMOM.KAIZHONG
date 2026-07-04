using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Items.Items.Commands
{
    /// <summary>
    /// 删除指定物料的指定产品等级
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", GroupType = CommandGroupType.Edit)]
    class ProductGradeDeleteCommand : ListEditCommand
    {
        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && view.SelectedEntities.Count > 0;
        }

        /// <summary>
        /// 删除执行方法
        /// </summary>
        /// <param name="view"></param>
        public override void Execute(ListLogicalView view)
        {
            if (!CRT.MessageService.AskQuestion(string.Format("你确认删除选择的{0}条数据吗？".L10N(), view.SelectedEntities.Count)))
                return;

            var selectedItems = view.SelectedEntities.OfType<ProductGrade>().ToList();
            var ctl = RT.Service.Resolve<ItemController>();
            var pagingData = view.PagingData;
            var pagingInfo = new PagingInfo(pagingData.PageNumber, pagingData.PageSize);
            var list = ctl.RemoveProductGrade(selectedItems, pagingInfo);

            view.Data = list;
            view.RefreshControl();
        }
    }
}
