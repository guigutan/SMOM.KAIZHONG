using SIE.MES.LoadItems;
using SIE.MES.LoadItems.ViewModels;
using SIE.Security;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP;
using System;
using System.Linq;

namespace SIE.Wpf.MES.LoadItems.Commands
{
    /// <summary>
    /// 一键下料命令
    /// </summary>
    [Command(ImageName = "ArrowWithCircleDown", Label = "一键下料", ToolTip = "一键下料", GroupType = CommandGroupType.Edit)]
    public class UnloadAllItemCommand : ListViewCommand
    {
        private const string MAIN_VIEW = "mainView";

        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view == null)
            {
                return false;
            }
            var mainView = view.Relations.Find(MAIN_VIEW);
            return view.Data.OfType<LoadItem>().Any() && mainView?.Current != null && mainView.Current is ILoadableItem;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var type = view.Relations.Find(MAIN_VIEW)?.EntityType;
            if (type == null)
            {
                return;
            }
            var vm = view.Relations.Find(MAIN_VIEW)?.Current as DataCollectionViewModel;
            if (vm == null)
            {
                throw new ArgumentNullException("主视图模型为空".L10N());
            }
            var workCell = vm.GetWorkcell();
            if (workCell == null || workCell.ProcessId == 0 || workCell.ResourceId == 0 || workCell.StationId == 0)
            {
                return;
            }
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(type);
            var matchItems = RT.Service.Resolve<LoadItemController>().GetUnloadAllItems( workCell.ResourceId, workCell.StationId);
            var template = new ListUITemplate(typeof(UnloadAllItemViewModel), ViewConfig.ListView, moduleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = matchItems;
            var result = CRT.Workbench.ShowDialog(moduleKey, ui.Control, w =>
            {
                w.Title = "一键下料".L10N();
                w.Commands[0] = "下料".L10N();
                w.Height = 500;
                w.Width = 800;
                w.Closing += (s, e) =>
                {
                    try
                    {
                        if (w.Result == 0 && matchItems.Any(p => p.IsLoadItem))
                        {
                            var loadItems = matchItems.Where(p => p.IsLoadItem).ToList();
                            RT.Service.Resolve<LoadItemController>().UnloadAllItem(loadItems);
                            var mainView = view.Relations.Find(MAIN_VIEW).Current as ILoadableItem;
                            mainView?.RefreshLoadItem();
                            mainView?.RefreshUnoadItem();
                        }
                    }
                    catch (Exception exc)
                    {
                        var baseExc = exc.GetBaseException();
                        CRT.MessageService.ShowError(baseExc.Message);
                        e.Cancel = true;
                    }
                };
            });
            if (result == 0)
                CRT.MessageService.ShowMessage("一键下料成功".L10N());
        }
    }
}