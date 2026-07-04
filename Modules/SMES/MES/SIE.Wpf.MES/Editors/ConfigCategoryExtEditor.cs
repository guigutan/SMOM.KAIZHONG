using DevExpress.Xpf.Grid;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.View.Workbench;
using SIE.Wpf.Common.Configs;
using SIE.Wpf.Common.Templates;
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
    /// 配置项分类编辑器(扩展)
    /// </summary>
    public class ConfigCategoryExtEditor : ConfigCategoryEditor
    {
        /// <summary>
        /// 名称
        /// </summary>
        public new const string EditorName = "ConfigCategoryExtEditor";

        /// <summary>
        /// 创建编辑控件
        /// </summary>
        /// <returns></returns>
        protected override FrameworkElement CreateEditingElement()
        {
            DockPanel dockPanel = new DockPanel();
            Button button = new Button();
            button.SetResourceReference(FrameworkElement.StyleProperty, "ImageButtonBaseStyle");
            button.Padding = new Thickness(0.0);
            button.SetBinding(binding: new Binding("ActualHeight")
            {
                RelativeSource = RelativeSource.Self
            }, dp: FrameworkElement.WidthProperty);
            button.Content = IconManager.GetPackIcon("Search", 16, 16);
            button.SetValue(DockPanel.DockProperty, Dock.Right);
            button.Click += delegate
            {
                PopupSelection();
            };
            KeyboardNavigation.SetIsTabStop(button, isTabStop: false);
            dockPanel.Children.Add(button);
            TextBox textBox = new TextBox
            {
                AcceptsReturn = false,
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Name = base.Meta.Name,
                IsReadOnly = true
            };
            textBox.MouseDoubleClick += delegate
            {
                PopupSelection();
            };
            ResetBinding(textBox);
            dockPanel.Children.Add(textBox);
            SetAutomationElement(textBox);
            base.RaisePropertyChangeEvents = true;
            return dockPanel;
        }

        /// <summary>
        /// 弹出选择框
        /// </summary>
        public new virtual void PopupSelection()
        {
            ConfigDetailViewModel entity = base.Context.CurrentObject as ConfigDetailViewModel;
            if (entity == null)
            {
                return;
            }

            Type categoryType = entity.ConfigViewModel.CategoryType;
            if (!categoryType.IsSubclassOf(typeof(Entity)))
            {
                return;
            }

            ControlResult controlResult = new RootUITemplate(categoryType, "ListView").CreateUI();
            ListLogicalView listView = controlResult.MainView as ListLogicalView;
            listView.Control.SelectionMode = MultiSelectMode.Row;
            QueryLogicalView queryLogicalView = controlResult.MainView.Relations.Select((RelationView p) => p.View).OfType<QueryLogicalView>().FirstOrDefault();
            if (queryLogicalView != null)
            {
                queryLogicalView.TryExecuteQuery();
            }
            else
            {
                listView.DataLoader.LoadDataAsync();
            }

            CRT.Workbench.ShowDialog(controlResult, delegate (IDialogContent w)
            {
                w.Title = "选择".L10N() + base.Meta.Label;
                w.Width = 800;
                w.Height = 600;
                w.Closed += delegate
                {
                    if (w.Result == 0)
                    {
                        Entity entity2 = listView.SelectedEntities.FirstOrDefault();
                        if (entity2 != null)
                        {
                            entity.CategoryKey = entity2.GetId()?.ToString();
                        }
                        else
                        {
                            entity.CategoryKey = null;
                        }
                    }
                };
            });
        }
    }
}
