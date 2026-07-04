using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Items.ProductModels.Commands
{
    /// <summary>
    /// 选择产品机型命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择产品机型", GroupType = CommandGroupType.Edit, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    class ProductModelLookUpCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Parent.Current != null && view.Parent.Current.PersistenceStatus != PersistenceStatus.New;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(ProductModel), ProductModelViewConfig.ButtonSelectView);
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            var control = listView.Control as GridControl;
            var tableView = control.View as TableView;
            tableView.ShowCheckBoxSelectorColumn = true;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            ui.MainView.QueryView.DataProvider = (c) =>
            {
                var ctl = RT.Service.Resolve<ItemController>();
                var criteria = ui.MainView.QueryView.Data as ProductModelCriteria;
                var list = ctl.GetProductModelList(criteria, criteria.PagingInfo);
                return list;
            };

            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "选择 产品机型".L10N();
                w.MinWidth = 500;
                w.MinHeight = 400;
                ui.MainView.QueryView.TryExecuteQuery();
            });
            if (result == 0)
            {
                SetProductModelItem(view, listView);
            }
        }

        /// <summary>
        /// 保存选择的产品机型 并刷新
        /// </summary>
        /// <param name="view">视图</param>
        /// <param name="listView">列表视图</param>
        private void SetProductModelItem(ListLogicalView view, ListLogicalView listView)
        {
            var productFamilyCategory = view.Parent.Current as ProductFamily;
            var selectedItems = listView.SelectedEntities.OfType<ProductModel>().ToList();
            var ctl = RT.Service.Resolve<ItemController>();
            var criteria = new ProductModelCriteria();
            var list = ctl.AddRangeProductModel(selectedItems, criteria, productFamilyCategory.Id);

            view.Data = list;
            view.RefreshControl();
        }
    }
}
