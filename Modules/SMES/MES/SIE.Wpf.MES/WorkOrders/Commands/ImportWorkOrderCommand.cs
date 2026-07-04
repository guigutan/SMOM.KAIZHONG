using SIE.Wpf.Command;
using SIE.Wpf.MES.WorkOrders.ViewModels;
using System;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 数据导入 工单
    /// </summary>
    [Command(Label = "导入工单", ToolTip = "导入工单", Hierarchy = "工单生成", GroupType = CommandGroupType.Edit)]
    public class ImportWorkOrderCommand : ListViewCommand
    {
        /// <summary>
        /// 执行数据导入逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(ImportWorkOrderCheckViewModel), null);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var template = new DetailsUITemplate<ImportWorkOrderCheckViewModel>(view.ModuleKey);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                detailView.Data = new ImportWorkOrderCheckViewModel();

                //退出时，数据已被修改且未保存时，提示用户
                v.Closing += (o, e) =>
                {
                    if (ui.MainView.Data.IsDirty)
                    {
                        if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()) == true)
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