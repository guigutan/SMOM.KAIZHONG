using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 替代料编辑器
    /// </summary>
    public class AlternativeEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// AlternativeEditor
        /// </summary>
        public const string EditorName = "AlternativeEditor";

        /// <summary>
        /// 替代料列表
        /// </summary>
        public EntityList<Item> AlternativeList { get; }

        /// <summary> 
        /// 选中替代料列表
        /// </summary>
        public EntityList<Item> SelectedValueList { get; }

        /// <summary>
        /// 查询面板
        /// </summary>
        private QueryLogicalView queryView;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AlternativeEditor()
        {
            AlternativeList = new EntityList<Item>();
            SelectedValueList = new EntityList<Item>();
        }

        /// <summary>
        /// 替代料选择
        /// </summary>
        protected void PopupSelection()
        {
            var grid = CreateControl();
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), grid, w =>
            {
                w.Title = "替代料选择".L10N();
                w.WindowState = WindowState.Maximized;
                w.Closed += (o, e) =>
                {
                    if (w.Result == 0)
                    {
                        var detail = this.Context.CurrentObject as ProductBomDetail;
                        detail.AlternativeList.Clear();
                        foreach (Item item in SelectedValueList)
                        {
                            ProductBomDetailAlternative alternative = new ProductBomDetailAlternative() { Item = item };
                            detail.AlternativeList.Add(alternative);
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <returns>FrameworkElement</returns>
        FrameworkElement CreateControl()
        {
            AlternativeList.Clear();
            SelectedValueList.Clear();
            var grid = new Grid();
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(5, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            var listView = CreateListControl();
            var selectedView = CreateSelectedListControl();
            var itemGroup = new GroupBox() { Header = "替代料" };
            itemGroup.Content = listView;
            var selectedGroup = new GroupBox() { Header = "选中值" };
            selectedGroup.Content = selectedView;
            grid.Children.Add(itemGroup);
            grid.Children.Add(selectedGroup);
            Grid.SetColumn(itemGroup, 0);
            Grid.SetColumn(selectedGroup, 2);

            //var txt = new TextBox
            //{
            //    AcceptsReturn = false,
            //    TextWrapping = TextWrapping.Wrap,
            //    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            //    Name = base.Meta.Name,
            //    IsReadOnly = true
            //};
            ////OnTextBoxCreated(txt);
            //txt.MouseDoubleClick += (o, e) => PopupSelection();
            //ResetBinding(txt);
            //grid.Children.Add(txt);
            //SetAutomationElement(txt);
            BindingData();
            return grid;
        }

        /// <summary>
        /// 选中属性值控件
        /// </summary>
        /// <returns>FrameworkElement</returns>
        FrameworkElement CreateSelectedListControl()
        {
            var uiTemplate = new ListUITemplate(typeof(Item), AlternativeViewConfig.AlternativeView/*typeof(AlternativeViewConfig)*/);
            var ui = uiTemplate.CreateUI();

            var listView = ui.MainView as ListLogicalView;
            (listView.Control.View as TableView).ShowCheckBoxSelectorColumn = false;
            (listView.Control.View as TableView).ShowTotalSummary = false;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            listView.Data = SelectedValueList;

            listView.Control.MouseDoubleClick += (s, e) =>
            {
                if (listView.SelectedEntities.Count <= 0)
                {
                    return;
                }

                listView.SelectedEntities.ForEach(p => SelectedValueList.Remove(p));
                listView.UnSelectEntities(listView.SelectedEntities.ToArray());
                listView.RefreshControl();
                queryView.TryExecuteQuery();
            };

            return listView.Control; //ui.Control;
        }

        /// <summary>
        /// 属性值控件
        /// </summary>
        /// <returns>FrameworkElement</returns>
        FrameworkElement CreateListControl()
        {
            var template = new ListUITemplate(typeof(Item), AlternativeViewConfig.AlternativeView);
            template.BlocksDefined += Template_BlocksDefined;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            (listView.Control.View as TableView).ShowCheckBoxSelectorColumn = false;
            (listView.Control.View as TableView).ShowTotalSummary = false;
            listView.Control.SelectionMode = MultiSelectMode.Row;

            if (listView.CommandsContainer != null)
            {
                listView.CommandsContainer.Visibility = Visibility.Collapsed;
            }

            queryView = listView.QueryView;
            if (queryView != null)
            {
                queryView.Querying += QueryView_Querying;
                queryView.QueryCompleted += QueryView_QueryCompleted;
            }

            listView.Control.MouseDoubleClick += (s, e) =>
            {
                if (listView.SelectedEntities.Count <= 0)
                {
                    return;
                }
                listView.SelectedEntities.ForEach(p =>
                {
                    if (!SelectedValueList.OfType<Entity>().Any(t => t.GetId().Equals(p.GetId())))
                    {
                        SelectedValueList.Add(p);
                    }
                });
                //SelectedValueList.AddRange(listView.SelectedEntities);
                listView.SelectedEntities.ForEach(p => listView.Data.Remove(p));
                listView.SelectedEntities.ForEach(p => listView.UnSelectEntities(p));
                listView.RefreshControl();
            };
            return ui.Control;
        }

        /// <summary>
        /// 查询完成事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void QueryView_QueryCompleted(object sender, QueryEventArgs e)
        {
            var data = e.ResultView.Data.CastTo<EntityList<Item>>();
            AlternativeList.Clear();
            AlternativeList.AddRange(data);
        }

        /// <summary>
        /// 查询前事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void QueryView_Querying(object sender, QueryEventArgs e)
        {
            var criteria = (sender as ConditionQueryLogicalView).Current as AlternativeCriteria;
            criteria.FilterId = null;
            if (SelectedValueList.Count > 0)
                criteria.FilterId = SelectedValueList.Select(p => p.Id).ToArray(); //指定过滤替代料ID
        }

        /// <summary>
        /// 替代料块定义，更换查询面板
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            var queryBlock = e.Blocks.Surrounders.Find(typeof(ItemCriteria));
            if (queryBlock != null)
                e.Blocks.Surrounders.Remove(queryBlock);

            var conditionBlock = new ConditionBlock(typeof(AlternativeCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);
            if (e.Blocks.Children.Count > 0)
            {
                e.Blocks.Children.Clear();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindingData()
        {
            var detail = this.Context.CurrentObject as ProductBomDetail;
            if (detail.AlternativeList.Count > 0)
                SelectedValueList.AddRange(detail.AlternativeList.Select(p => p.Item));
        }

        /// <summary>
        /// 创建编辑器样式
        /// </summary>
        /// <returns>FrameworkElement</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var panel = new DockPanel();
            Button element = new Button();
            element.SetResourceReference(Button.StyleProperty, "ImageButtonBaseStyle");
            element.Padding = new Thickness(0);
            var binding = new Binding("ActualHeight");
            binding.RelativeSource = RelativeSource.Self;
            element.SetBinding(Button.WidthProperty, binding);
            element.Content = IconManager.GetPackIcon("Search", 16, 16); //img;
            element.SetValue(DockPanel.DockProperty, Dock.Right);
            element.Click += (s, e) => PopupSelection();
            KeyboardNavigation.SetIsTabStop(element, false);
            panel.Children.Add(element);

            var txt = new TextBox
            {
                AcceptsReturn = false,
                TextWrapping = TextWrapping.NoWrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Name = base.Meta.Name,
                IsReadOnly = true
            };

            txt.MouseDoubleClick += (o, e) => PopupSelection();
            ResetBinding(txt);
            panel.Children.Add(txt);
            SetAutomationElement(txt);

            RaisePropertyChangeEvents = true;
            return panel;
        }

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return TextBox.TextProperty;
        }
    }
}
