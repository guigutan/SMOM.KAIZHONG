using SIE.Defects;
using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Controls;
using System;
using System.Linq;

namespace SIE.Wpf.MES.LoadItems.Commands
{
    /// <summary>
    /// 不良下料命令
    /// </summary>
    [Command(ImageName = "AlertCircleOutline", Label = "不良下料", ToolTip = "不良下料", GroupType = CommandGroupType.Edit)]
    public class UnloadDefectItemCommand : ListViewCommand
    {
        /// <summary>
        /// 命令执行判断
        /// </summary>
        /// <param name="view">表格逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            return view.Current != null && mainView?.Current is ILoadableItem;
        }

        /// <summary>
        /// 命令执行代码
        /// </summary>
        /// <param name="view">表格逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var editor = DefectControlFactory.CreateControl();
            editor.AllowMultiple = true;
            editor.AllowQty = true;
            editor.Defects.AddRange(RF.GetAll<Defect>());
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
            {
                w.Title = "不良下料录入".L10N();
                w.Height = 500;
                w.Width = 650;
                w.MinHeight = 350;
                w.MinWidth = 400;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (editor.SelectedValue.Count == 0)
                        {
                            e.Cancel = true;
                            return;
                        }

                        try
                        {
                            var loadItemId = view.Current.GetId().ConvertTo<double>();
                            var defects = editor.SelectedValue.Select(p => new DefectData { Qty = p.Qty, DefectId = p.Defect.Id }).ToList();
                            RT.Service.Resolve<LoadItemController>().UnloadItem(loadItemId, defects);

                            var mainView = view.Relations.Find("mainView").Current as ILoadableItem;
                            mainView.RefreshLoadItem();
                            mainView.RefreshUnoadItem();
                        }
                        catch (Exception exc)
                        {
                            exc.Alert();
                            e.Cancel = true;
                        }
                    }
                };
            });
        }
    }
}
