using DevExpress.Xpf.Grid;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Tech.Routings.Commands
{
    /// <summary>
    /// 添加工序BOM命令（分类添加）
    /// </summary> 
    [Command(ImageName = "AddEntity", Label = "分类添加", ToolTip = "按分类添加BOM", GroupType = 10)]
    public class AddProcessBomByCtgCommand : ListAddCommand
    {
        /// <summary>
        /// 工序BOM命令能否执行 
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>流程状态为保存且选中的活动节点非开始结束节点且节点工序类型为装配返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return (bool)view["CanExecute"];
        }

        /// <summary>
        /// 添加工序BOM命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(ItemSmallCategory), "AddProcessBomView");
            template.BlocksDefined += Template_BlocksDefined;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            if (listView.CommandsContainer != null)
                listView.CommandsContainer.Visibility = System.Windows.Visibility.Collapsed;
            var control = listView.Control as GridControl;
            var tableView = control.View as TableView;
            tableView.ShowCheckBoxSelectorColumn = true;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "选择 物料小类".L10N();
                w.MinWidth = 500;
                w.MinHeight = 400;
            });
            if (result == 0)
            {
                var ctgIds = listView.SelectedEntities.OfType<ItemSmallCategory>().Select(p => p.Id).ToList();
                var items = RT.Service.Resolve<ItemController>().GetItemFromCategoryIds(ctgIds);
                var currentItems = view.Data.OfType<Item>();
                int addCount = 0;
                items.ForEach(p =>
                {
                    if (currentItems.FirstOrDefault(q => q.Id == p.Id) == null)
                    {
                        view.Data.Add(p);
                        addCount++;
                    }
                });

                CRT.MessageService.ShowMessage("已选择分类包括{0}条物料，其中过滤已存在的物料后添加了{1}条！".L10nFormat(currentItems.Count(), addCount), "提示".L10N());
            }
        }

        /// <summary>
        /// 块定义，替换查询实体
        /// </summary>
        /// <param name="sender">模板</param>
        /// <param name="e">参数</param>
        private void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            var criteriaBlock = e.Blocks.Surrounders.Find(typeof(ItemSmallCategory));
            if (criteriaBlock != null)
                e.Blocks.Surrounders.Remove(criteriaBlock);
            var conditionBlock = new ConditionBlock(typeof(ItemSmallCategoryCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);

            var childBlock = e.Blocks.Children.Find(typeof(Item));
            if (childBlock != null)
                childBlock.Children.Clear();
        }
    }

    /// <summary>
    /// 添加工序BOM命令（分类添加）
    /// </summary> 
    [Command(ImageName = "AddEntity", Label = "物料添加", ToolTip = "按物料添加BOM", GroupType = 10)]
    public class AddProcessBomByItemCommand : ListAddCommand
    {
        /// <summary>
        /// 工序BOM命令能否执行 
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>流程状态为保存且选中的活动节点非开始结束节点且节点工序类型为装配返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return (bool)view["CanExecute"];
        }

        /// <summary>
        /// 添加工序BOM命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var template = new ListUITemplate(typeof(Item), "SmallCategoryView");
            template.BlocksDefined += Template_BlocksDefined;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            if (listView.CommandsContainer != null)
                listView.CommandsContainer.Visibility = System.Windows.Visibility.Collapsed;
            var control = listView.Control as GridControl;
            var tableView = control.View as TableView;
            tableView.ShowCheckBoxSelectorColumn = true;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "选择 物料".L10N();
                w.MinWidth = 500;
                w.MinHeight = 400;
            });
            if (result == 0)
            {
                var currentItems = view.Data.OfType<Item>();
                int addCount = 0;

                foreach (Item item in listView.SelectedEntities.OfType<Item>())
                {
                    if (currentItems.FirstOrDefault(q => q.Id == item.Id) == null)
                    {
                        view.Data.Add(item);
                        addCount++;
                    }
                }

                CRT.MessageService.ShowMessage("已选择{0}条物料，其中过滤已存在的物料后添加了{1}条！".L10nFormat(listView.SelectedEntities.Count(), addCount), "提示".L10N());
            }
        }

        /// <summary>
        /// 块定义，替换查询实体
        /// </summary>
        /// <param name="sender">模板</param>
        /// <param name="e">参数</param>
        private void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            var criteriaBlock = e.Blocks.Surrounders.Find(typeof(ItemCriteria));
            if (criteriaBlock != null)
                e.Blocks.Surrounders.Remove(criteriaBlock);
            var conditionBlock = new ConditionBlock(typeof(AlternativeCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);

            e.Blocks.Children.Clear();
        }
    }
}