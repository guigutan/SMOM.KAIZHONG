using DevExpress.Xpf.Grid;
using Resources.IconPacks;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.Items;
using SIE.Items.ProductBoms;
using SIE.Items.ViewModels;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 产品属性值编辑器
    /// </summary>
    public class PropertyValueEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// PropertyValueEditor
        /// </summary>
        public readonly static string EditorName = "PropertyValueEditor";

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return TextBox.TextProperty;
        }

        /// <summary>
        /// 属性值列表
        /// </summary>
        public EntityList<ItemPropertyValue> PropertyValueList { get; }

        /// <summary>
        /// 选中属性值列表
        /// </summary>
        public EntityList<ItemPropertyValue> SelectedValueList { get; }

        ////public ListLogicalView selectedView { get; set; }

        /// <summary>
        /// 查询面板
        /// </summary>
        private QueryLogicalView queryView;

        /// <summary>
        /// PropertyValueEditor
        /// </summary>
        public PropertyValueEditor()
        {
            PropertyValueList = new EntityList<ItemPropertyValue>();
            SelectedValueList = new EntityList<ItemPropertyValue>();
        }

        /// <summary>
        /// PopupSelection
        /// </summary>
        protected void PopupSelection()
        {
            var model = (this.Context.CurrentObject as PropertyValueViewModel);
            if (model.DefinitionId == 0)
                throw new ValidationException("请选择属性".L10N());
            var grid = CreateControl();
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), grid, w =>
            {
                w.Title = "产品属性选择".L10N();
                w.Height = 600;
                w.Width = 1000;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        var property = this.Context.CurrentObject as PropertyValueViewModel;
                        property.Values.Clear();
                        if (SelectedValueList.Count > 0)
                        {
                            property.Values.AddRange(SelectedValueList.Select(p => p.Value).ToArray());
                        }

                        property.ResetData();
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
            PropertyValueList.Clear();
            SelectedValueList.Clear();
            var grid = new Grid();
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(5, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            var selectedView = CreateSelectedListControl();
            var listView = CreateListControl();
            var itemGroup = new GroupBox() { Header = "物料属性" };
            itemGroup.Content = listView;
            var selectedGroup = new GroupBox() { Header = "选中值" };
            selectedGroup.Content = selectedView;
            grid.Children.Add(itemGroup);
            grid.Children.Add(selectedGroup);
            Grid.SetColumn(itemGroup, 0);
            Grid.SetColumn(selectedGroup, 2);
            BindingData();
            return grid;
        }

        /// <summary>
        /// 属性值控件
        /// </summary>
        /// <returns>FrameworkElement</returns>
        FrameworkElement CreateListControl()
        {
            var template = new ListUITemplate(typeof(ItemPropertyValue), ViewConfig.ListView); //SearchTemplate();
            template.BlocksDefined += ListTemplate_BlocksDefined;
            template.EntityType = typeof(ItemPropertyValue);
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            listView.CommandsContainer.Visibility = Visibility.Collapsed;
            listView.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;
            (listView.Control.View as TableView).ShowCheckBoxSelectorColumn = false;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            ////listView.Relations.Add(new RelationView("selectedView", selectedView));

            queryView = ui.MainView.Relations.Select(p => p.View).OfType<QueryLogicalView>().FirstOrDefault();
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

                ////SelectedValueList.Add(e.NewItem);
                //SelectedValueList.AddRange(listView.SelectedEntities);
                listView.SelectedEntities.ForEach(p => listView.Data.Remove(p));
                listView.SelectedEntities.ForEach(p => listView.UnSelectEntities(p));
                ////listView.UnSelectEntities(e.NewItem as ItemPropertyValue);
                ////listView.Data.Remove(e.NewItem as ItemPropertyValue);
                ////PropertyValueList.Remove(e.NewItem);
                ////listView.Data = PropertyValueList;
                ////queryView.TryExecuteQuery();
                listView.RefreshControl();
            };

            return ui.Control;
        }

        /// <summary>
        /// 聚合快元数据生成后事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ListTemplate_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            var conditionBlock = new ConditionBlock(typeof(ItemPropertyValueCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);
            if (e.Blocks.Children.Count > 0)
            {
                e.Blocks.Children.Clear();
            }
        }

        /// <summary>
        /// 查询后事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void QueryView_QueryCompleted(object sender, QueryEventArgs e)
        {
            var data = e.ResultView.Data.CastTo<EntityList<ItemPropertyValue>>();
            PropertyValueList.Clear();
            PropertyValueList.AddRange(data);
        }

        /// <summary>
        /// 查询前事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void QueryView_Querying(object sender, QueryEventArgs e)
        {
            var criteria = (sender as ConditionQueryLogicalView).Current as ItemPropertyValueCriteria;
            criteria.ItemId = (this.Context.CurrentObject as PropertyValueViewModel).ItemId;
            criteria.Definition = (this.Context.CurrentObject as PropertyValueViewModel).Definition;
            criteria.FilterId = null;
            if (SelectedValueList.Any())
                criteria.FilterId = SelectedValueList.Select(p => p.Id).ToArray(); //过滤选中ID
        }

        /// <summary>
        /// 选中属性值控件
        /// </summary>
        /// <returns>FrameworkElement</returns>
        FrameworkElement CreateSelectedListControl()
        {
            var uiTemplate = new ListUITemplate(typeof(ItemPropertyValue), ViewConfig.ListView);
            ////uiTemplate.ViewGroup = "SelectedView";
            ////uiTemplate.BlocksDefined += UiTemplate_BlocksDefined;
            var ui = uiTemplate.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            ////selectedView = listView;
            listView.CommandsContainer.Visibility = Visibility.Collapsed;
            listView.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;
            (listView.Control.View as TableView).ShowCheckBoxSelectorColumn = false;
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
                ////listView.UnSelectEntities(e.NewItem as ItemPropertyValue);
                ////SelectedValueList.Remove(e.NewItem);
                listView.RefreshControl();
                queryView.TryExecuteQuery();
            };
            return ui.Control;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindingData()
        {
            var proprety = this.Context.CurrentObject as PropertyValueViewModel;

            var savedValueList = RT.Service.Resolve<ProductBomController>().GetProductBomPropertyValues(proprety.ParentId, proprety.DefinitionId);
            savedValueList.ForEach(p =>
            {
                var value = RT.Service.Resolve<ItemController>().GetItemPropertyValue(proprety.DefinitionId, p.Value, proprety.ItemId);
                if (value != null)
                {
                    SelectedValueList.Add(value);
                }
            });
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
                TextWrapping = TextWrapping.Wrap,
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
    }
}
