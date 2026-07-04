using SIE.MES.LoadItems;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Editors;
using System;

namespace SIE.Wpf.MES.LoadItems.Commands
{
    /// <summary>
    /// 正常下料命令
    /// </summary>
    [Command(ImageName = "ArrowUpBoldHexagonOutline", Label = "正常下料", ToolTip = "正常下料", GroupType = CommandGroupType.Edit)]
    public class UnloadItemCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            return view.Current != null && mainView?.Current != null && mainView.Current is ILoadableItem;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var editor = new Calculator();
            var loadItem = view.Current as LoadItem;
            if (loadItem != null)
            {
                editor.Value = loadItem.Qty.ConvertTo<double>();
                editor.Tag= loadItem.Id;
            }

            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
            {
                w.Title = "正常下料数录入".L10N();
                w.Height = 400;
                w.Width = 400;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (editor.HasError || editor.Value <= 0)
                        {
                            e.Cancel = true;
                            return;
                        }

                        var qty = editor.Value.ConvertTo<decimal>();
                        try
                        {
                            var loadItemId = editor.Tag.ConvertTo<double>();
                            RT.Service.Resolve<LoadItemController>().UnloadItem(loadItemId, qty);

                            var mainView = view.Relations.Find("mainView").Current as ILoadableItem;
                            mainView.RefreshLoadItem();
                            mainView.RefreshUnoadItem();
                        }
                        catch (Exception exc)
                        {
                            e.Cancel = true;
                            exc.Alert();
                        }
                    }
                };
            });
        }
    }
}
