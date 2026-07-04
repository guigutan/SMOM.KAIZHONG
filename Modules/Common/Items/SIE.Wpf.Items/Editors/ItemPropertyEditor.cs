using SIE.Common;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.MetaModel.View;
using SIE.View.Workbench;
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
    /// 物料属性编辑器
    /// </summary>
    class ItemPropertyEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemPropertyEditor";

        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <returns>依赖属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return TextBox.TextProperty;
        }

        /// <summary>
        /// 创建编辑元素
        /// </summary>
        /// <returns>元素</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var panel = new DockPanel();
            Button element = new Button();
            element.SetResourceReference(Button.StyleProperty, "ImageButtonBaseStyle");
            element.Padding = new Thickness(0);
            var binding = new Binding("ActualHeight")
            {
                RelativeSource = RelativeSource.Self
            };
            element.SetBinding(Button.WidthProperty, binding);
            element.Content = IconManager.GetPackIcon("Search", 16, 16);
            element.SetValue(DockPanel.DockProperty, Dock.Right);
            element.Click += (s, e) => Selection();
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

            txt.MouseDoubleClick += (o, e) => Selection();
            ResetBinding(txt);
            panel.Children.Add(txt);
            SetAutomationElement(txt);

            RaisePropertyChangeEvents = true;
            return panel;
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        private void Selection()
        {
            var ui = CreatUI();
            CRT.Workbench.ShowDialog(ui, w =>
             {
                 w.Closing += (s, e) => Selection_Closing(ui, w);
                 w.Width = 680;
                 w.Height = 380;
             });
        }

        /// <summary>
        /// 选择界面关闭事件
        /// </summary>
        /// <param name="ui">ui</param>
        /// <param name="w">视图对话框</param>
        private void Selection_Closing(ControlResult ui, IDialogContent w)
        {
            var parent = this.Context.CurrentObject as ProductBomDetail;
            var children = (ui.MainView as ListLogicalView).SelectedEntities.OfType<ProductBomDetailPropertyValue>().AsEntityList<ProductBomDetailPropertyValue>();// as EntityList<ProductBomDetailPropertyValue>;
            
            if (w.Result == -1 && children.IsDirty) 
            {
                CRT.MessageService.AskQuestion("数据未保存,是否继续退出?".L10N());
            }

            if (w.Result == 0) 
            {
                parent.PropertyValueList.Clear();
                parent.PropertyValueList.AddRange(children);
            }
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <returns>UI</returns>
        private ControlResult CreatUI()
        {
            var template = new ListUITemplate(typeof(ProductBomDetailPropertyValue))
            {
                ViewGroup = ProductBomDetailPropertyValueViewConfig.BomPropertyLookupView
            };
            var ui = template.CreateUI();
            var bomDetail = this.Context.CurrentObject as ProductBomDetail; 
            var allValueList = RT.Service.Resolve<ProductBomController>().GetProdBomDetailPropertyValuesByItemId(bomDetail.ItemId); 

            var listView = (ui.MainView as ListLogicalView);
            (listView.Control.View as DevExpress.Xpf.Grid.TableView).ShowCheckBoxSelectorColumn = true;
            listView.Control.SelectionMode = DevExpress.Xpf.Grid.MultiSelectMode.MultipleRow;
            ui.MainView.Data = allValueList;
            listView.SelectEntities(allValueList.Where(p => bomDetail.PropertyValueList.Any(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value)).ToArray());
            return ui;
        }
    }
}
