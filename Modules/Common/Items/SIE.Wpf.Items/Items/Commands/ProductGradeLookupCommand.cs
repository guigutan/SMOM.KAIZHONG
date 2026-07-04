using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Items.Items.Commands
{
    /// <summary>
    /// 选择产品等级命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择产品等级", GroupType = CommandGroupType.Edit, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    class ProductGradeLookupCommand : ListViewCommand
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
        /// 选择的执行方法
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(ProductGrade), ProductGradeViewConfig.ButtonSelectView);
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            var control = listView.Control as GridControl;
            var tableView = control.View as TableView;
            tableView.ShowCheckBoxSelectorColumn = true;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            var item = view.Parent.Current as Item;
            ui.MainView.QueryView.DataProvider = (c) =>
            {
                var criteria = ui.MainView.QueryView.Data as ProductGradeCriteria;
                var ctl = RT.Service.Resolve<ItemController>();
                var list = ctl.GetProductGrades(criteria, item.Id);
                list.SetTotalCount(list.Count);
                return list;
            };

            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "选择 产品等级".L10N();
                w.MinWidth = 300;
                w.MinHeight = 300;
                ui.MainView.QueryView.TryExecuteQuery();
            });
            if (result == 0)
            {
                SetProductGradeItem(view, listView);
            }
        }

        /// <summary>
        /// 保存选择数据方法
        /// </summary>
        /// <param name="view">父列表视图</param>
        /// <param name="listView">子列表视图</param>
        private void SetProductGradeItem(ListLogicalView view, ListLogicalView listView)
        {
            var item = view.Parent.Current as Item;
            var selectedItems = listView.SelectedEntities.OfType<ProductGrade>().ToList();
            var ctl = RT.Service.Resolve<ItemController>();
            ////var criteria = new ProductGradeCriteria();
            ////var list = ctl.AddRangeProductGrade(selectedItems, item.Id, criteria);
            var pagingData = view.PagingData;
            var pagingInfo = new PagingInfo(pagingData.PageNumber, pagingData.PageSize);
            var list = ctl.AddRangeProductGrade(selectedItems, item.Id, pagingInfo);

            view.Data = list;
            view.RefreshControl();
        }
    }
}
