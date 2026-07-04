using SIE.MetaModel.View;
using SIE.Wpf.Command;
using SIE.Wpf.Resources.ProcessTechs.ViewModels;
using System;

namespace SIE.Wpf.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺定义数据导入
    /// </summary>
    [Command(ImageName = "Import", Label = "导入", ToolTip = "导入", Location = CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class ImportProcessTechCommand : ListViewCommand
    {
        /// <summary>
        /// 执行数据导入逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(ImportProcessTechViewModel), null);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var template = new DetailsUITemplate<ImportProcessTechViewModel>(view.ModuleKey);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                detailView.Data = new ImportProcessTechViewModel();

                return ui;
            });
        }
    }
}
