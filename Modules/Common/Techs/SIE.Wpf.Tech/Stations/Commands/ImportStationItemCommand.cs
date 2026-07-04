using SIE.Tech.Stations;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Tech.Stations.Commands
{
    /// <summary>
    /// 数据导入 工单
    /// </summary>
    [Command(ImageName = "Import", Label = "导入", ToolTip = "导入", GroupType = CommandGroupType.Edit)]
    public class ImportStationItemCommand : ListViewCommand
    {
        /// <summary>
        /// 没数据不能使用
        /// </summary>
        /// <param name="view">view</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Parent.Current != null;
        }

        /// <summary>
        /// 执行数据导入逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(ImportStationItemViewModel), null);
            CRT.Workbench.ShowView(key, v =>
            {
                var station = view.Parent.Current as Station;
                v.Title = (station.Name + "工位物料导入").L10N();
                var template = new DetailsUITemplate<ImportStationItemViewModel>(view.ModuleKey);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                detailView.Data = new ImportStationItemViewModel() { Station = station };

                //退出时，数据已被修改且未保存时，提示用户
                v.Closing += (o, e) =>
                {
                    if (ui.MainView.Data.IsDirty)
                    {
                        if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()))
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                };

                return ui;
            });
        }
    }
}
