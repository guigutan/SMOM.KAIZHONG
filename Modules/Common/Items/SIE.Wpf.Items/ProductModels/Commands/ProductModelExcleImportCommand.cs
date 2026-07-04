using SIE.MetaModel.View;
using SIE.Wpf.Command;
using SIE.Wpf.Items.ProductModels.ViewModels;
using System;

namespace SIE.Wpf.Items.ProductModels.Commands
{
    /// <summary>
    /// 数据导入
    /// </summary>
    [Command(ImageName = "Import", Label = "导入", ToolTip = "导入", Location = CommandLocation.All, GroupType = 20)]
    public class ProductModelExcleImportCommand : ListViewCommand
    {
        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(ImportProductModelCheckViewModel), null);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var template = new DetailsUITemplate<ImportProductModelCheckViewModel>(view.ModuleKey);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                detailView.Data = new ImportProductModelCheckViewModel();

                return ui;
            });
        }
    }
}
