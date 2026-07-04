using DevExpress.Xpf.Grid;
using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Workbench.Experiences;
using SIE.MetaModel.View;
using SIE.Security;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.Workbench.Experiences.Commands
{
    /// <summary>
    /// 重写添加按钮
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class ExperienceAddCommand : ListAddCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">view</param>
        public override void Execute(ListLogicalView view)
        {
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(view.EntityType);
            var template = new ListUITemplate(typeof(Item), ExperienceItemViewConfig.ExperienceItemView, moduleKey);
            template.BlocksDefined += Template_BlocksDefined;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            listView.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            (listView.Control.View as TableView).ShowCheckBoxSelectorColumn = true;
            var result = CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "选择物料".L10N();
                w.MinWidth = 300;
                w.MinHeight = 300;
                ui.MainView.QueryView.TryExecuteQuery();
            });
            if (result == 0)
            {
                var viewList = view.Data.OfType<HistoryExperience>().AsEntityList();
                var ids = viewList.Select(p => p.ItemId).ToList();
                var items = listView.SelectedEntities.OfType<Item>().AsEntityList();
                items.Where(p => !ids.Contains(p.Id)).ForEach(item => view.Data.Add(new HistoryExperience() { Item = item }));
            }
        }

        /// <summary>
        /// 查询块定义
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Template_BlocksDefined(object sender, MetaModel.View.CodeBlocksDefinedEventArgs e)
        {
            e.Blocks.Surrounders.Clear();
            var conditionBlock = new ConditionBlock(typeof(ExperienceItemCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);
        }
    }

    /// <summary>
    /// 历史经验库选择物料视图
    /// </summary>
    internal class ExperienceItemViewConfig : WPFViewConfig<Item>
    {
        /// <summary>
        /// 历史经验库物料视图
        /// </summary>
        public const string ExperienceItemView = "ExperienceItemView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ExperienceItemView);
            if (ViewGroup == ExperienceItemView)
                ConfigExperienceItemView();
        }

        /// <summary>
        /// 视图配置
        /// </summary>
        private void ConfigExperienceItemView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList();
                View.Property(p => p.Name).ShowInList();
                View.Property(p => p.Type).ShowInList();
                View.ChildrenProperty(Item.PropertyValueListProperty).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(Item.UnitListProperty).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
