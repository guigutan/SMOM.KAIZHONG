using DevExpress.Xpf.Grid;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Packages.Boxs;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Packages.Boxs.Commands
{
    /// <summary>
    /// 产品容量添加命令
    /// </summary>
    [Command(Label = "添加", ImageName = "AddEntity", ToolTip = "添加产品容量", GroupType = CommandGroupType.Edit)]
    public class AddCpaacityCommand : ListViewCommand
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Parent.Current != null;
        }

        /// <summary>
        /// 命令执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate<Item>(view.ModuleKey);
            template.BlocksDefined += Template_BlocksDefined;

            var defaultCap = (View.Parent.Current as TurnoverBox).Capacity;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            listView.CommandsContainer.Visibility = System.Windows.Visibility.Collapsed;
            listView.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            (listView.Control.View as TableView).ShowCheckBoxSelectorColumn = true;

            listView.QueryView.Querying += QueryView_Querying;

            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString("N"), ui.Control, v =>
            {
                v.Title = "选择物料".L10N();
                v.Closed += (o, e) =>
                {
                    if (v.Result == 0)
                    {
                        var items = listView.SelectedEntities.OfType<Item>();
                        items.ForEach(p =>
                        {
                            view.Data.Add(new ProductCapacity
                            {
                                Item = p,
                                Capacity = defaultCap
                            });
                        });
                    }
                };
            });

            template.BlocksDefined -= Template_BlocksDefined;
            listView.QueryView.Querying -= QueryView_Querying;
        }

        /// <summary>
        /// 查询时事件
        /// </summary>
        /// <param name="sender">查询视图</param>
        /// <param name="e">事件参数</param>
        private void QueryView_Querying(object sender, QueryEventArgs e)
        {
            var queryView = sender as QueryLogicalView;
            var criteria = queryView.Data as ProdCpyItemCriteria;
            criteria.FilterIds = View.Data.OfType<ProductCapacity>().Select(p => p.ItemId).ToArray();
        }

        /// <summary>
        /// 模板块定义事件
        /// </summary>
        /// <param name="sender">模板对象</param>
        /// <param name="e">事件参数</param>
        private void Template_BlocksDefined(object sender, MetaModel.View.CodeBlocksDefinedEventArgs e)
        {
            var queryBlock = e.Blocks.Surrounders.Find(typeof(ItemCriteria));
            if (queryBlock != null)
                e.Blocks.Surrounders.Remove(queryBlock);
            var conditionBlock = new ConditionBlock(typeof(ProdCpyItemCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);

            if (e.Blocks.Children.Count > 0)
                e.Blocks.Children.Clear();
        }
    }
}
