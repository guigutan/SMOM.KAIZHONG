using DevExpress.Xpf.Grid;
using SIE.Defects;
using SIE.Defects.Defects;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SIE.Wpf.Andon.Editors
{
    /// <summary>
    /// 缺陷多选编辑器
    /// </summary>
    public class DefectMultiSelectEditor : PropertyEditor<DefectMultiSelectEditorConfig>
    {
        /// <summary>
        /// DefectMultiSelectEditor
        /// </summary>
        public const string EditorName = "DefectMultiSelectEditor";

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
        public EntityList<Defect> PropertyValueList { get; }

        /// <summary>
        /// 选中属性值列表
        /// </summary>
        public EntityList<Defect> SelectedValueList { get; }

        /// <summary>
        /// 查询面板
        /// </summary>
        private QueryLogicalView queryView;

        /// <summary>
        /// PropertyValueEditor
        /// </summary>
        public DefectMultiSelectEditor()
        {
            PropertyValueList = new EntityList<Defect>();
            SelectedValueList = new EntityList<Defect>();
        }

        /// <summary>
        /// PopupSelection
        /// </summary>
        protected void PopupSelection()
        {
            var grid = CreateControl();
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), grid, w =>
            {
                w.Title = "【缺陷代码】选择".L10N();
                w.Height = 600;
                w.Width = 1024;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        var ids = string.Join(this.Config.Separator, SelectedValueList.Select(x => x.Id).Distinct());

                        Type type = ClientEntities.Find(this.Config.Model).EntityType;
                        var entityRepo = RF.Find(type);
                        var displyProperty = entityRepo.EntityMeta.ManagedProperties.FindProperty(this.Config.DisplayField);
                        var displayString = string.Join(this.Config.Separator, SelectedValueList.Select(x => x.GetProperty(displyProperty)).Distinct());

                        var proprety = this.Context.CurrentObject;
                        var linkField = proprety?.PropertyContainer.FindProperty(this.Config.LinkField);
                        var bindingField = proprety?.PropertyContainer.FindProperty(this.Config.BindingField);
                        if (proprety != null)
                        {
                            proprety.SetProperty(linkField, ids);
                            proprety.SetProperty(bindingField, displayString);
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
            PropertyValueList.Clear();
            SelectedValueList.Clear();
            var grid = new Grid();
            grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(5, GridUnitType.Pixel) });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            var selectedView = CreateSelectedListControl();
            var listView = CreateListControl();
            var itemGroup = new GroupBox() { Header = "缺陷代码" };
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
            var template = new ListUITemplate(typeof(Defect), DefectViewConfig.ReadOnlyListView);
            template.BlocksDefined += ListTemplate_BlocksDefined;
            template.EntityType = typeof(Defect);
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;

            if (listView.CommandsContainer != null)
            {
                listView.CommandsContainer.Visibility = Visibility.Collapsed;
            }

            listView.IsReadOnly = MetaModel.ReadOnlyStatus.ReadOnly;
            (listView.Control.View as TableView).ShowCheckBoxSelectorColumn = false;
            listView.Control.SelectionMode = MultiSelectMode.Row;

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

                listView.SelectedEntities.ForEach(p => listView.Data.Remove(p));
                listView.SelectedEntities.ForEach(p => listView.UnSelectEntities(p));

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
            var conditionBlock = new ConditionBlock(typeof(DefectCriteria), ViewConfig.QueryView);
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
            var data = e.ResultView.Data.CastTo<EntityList<Defect>>();
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
            var criteria = (sender as ConditionQueryLogicalView).Current as DefectCriteria;

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
            var uiTemplate = new ListUITemplate(typeof(Defect), DefectViewConfig.ReadOnlyListView);
            uiTemplate.BlocksDefined += Template_BlocksDefined;
            var ui = uiTemplate.CreateUI();
            var listView = ui.MainView as ListLogicalView;
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
                listView.RefreshControl();
                //queryView.TryExecuteQuery();
            };
            return ui.Control;
        }

        /// <summary>
        /// 块定义
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        protected void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            //去掉查询条件
            e.Blocks.Surrounders.Clear();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindingData()
        {
            var proprety = this.Context.CurrentObject;
            var linkField = proprety?.PropertyContainer.FindProperty(this.Config.LinkField);
            if (proprety != null)
            {
                var idString = proprety.GetProperty(linkField) as string;
                List<double> ids;
                if (!idString.IsNullOrEmpty())
                {
                    ids = idString.Split(this.Config.Separator.ToCharArray()).Select(x => double.Parse(x)).ToList();

                    var savedValueList = RT.Service.Resolve<DefectController>().GetDefectList(ids);

                    SelectedValueList.AddRange(savedValueList);
                }
            }
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
            element.Content = IconManager.GetPackIcon("Search", 16, 16);
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
    }

    /// <summary>
    /// 缺陷代码多选编辑器Config
    /// </summary>
    public class DefectMultiSelectEditorConfig : EditorConfig
    {
        /// <summary>
        /// 缺陷代码多选编辑器Config
        /// </summary>
        public DefectMultiSelectEditorConfig()
        {
            Separator = ",";
        }

        /// <summary>
        /// 下拉列表中显示的实体名称。
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 关联字段
        /// </summary>
        public string LinkField { get; set; }

        /// <summary>
        /// 绑定字段
        /// </summary>
        public string BindingField { get; set; }

        /// <summary>
        /// 显示字段
        /// </summary>
        public string DisplayField { get; set; }

        /// <summary>
        /// 设置联动值分隔符
        /// </summary>
        public string Separator { get; set; }
    }
}
