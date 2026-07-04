using Microsoft.AspNetCore.Http;
using Resources.IconPacks;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.Editors;
using SIE.Wpf.Windows;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 打印机选择编辑器(扩展)
    /// </summary>
    public class PrinterExEditor : PrinterEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public new const string EditorName = "PrinterExEditor";

        /// <summary>
        /// CreateEditingElement
        /// </summary>
        /// <returns></returns>
        protected override FrameworkElement CreateEditingElement()
        {
            ComboBox cbx = new ComboBox();
            cbx.DropDownOpened += (s, e) =>
            {
                BindDatas(cbx);
            };
            BindDatas(cbx);

            DockPanel dockPanel = new DockPanel();
            if (base.Meta.GetPropertyOrDefault("HasLockButton", defaultValue: false))
            {
                ToggleButton btnLock = new ToggleButton();
                btnLock.SetResourceReference(FrameworkElement.StyleProperty, "ImageButtonBaseStyle");
                btnLock.Padding = new Thickness(0.0);
                Binding binding = new Binding("ActualHeight");
                binding.RelativeSource = RelativeSource.Self;
                btnLock.SetBinding(FrameworkElement.WidthProperty, binding);
                PackIcon packIcon = IconManager.GetPackIcon("Lock");
                packIcon.Width = 16.0;
                btnLock.Content = packIcon;
                btnLock.SetValue(DockPanel.DockProperty, Dock.Right);
                KeyboardNavigation.SetIsTabStop(btnLock, isTabStop: false);
                btnLock.IsChecked = true;
                dockPanel.Children.Add(btnLock);
                btnLock.Click += delegate
                {
                    if (btnLock.IsChecked.GetValueOrDefault())
                    {
                        btnLock.Content = IconManager.GetPackIcon("Lock");
                        cbx.IsEnabled = false;
                    }
                    else
                    {
                        btnLock.Content = IconManager.GetPackIcon("Unlock");
                        cbx.IsEnabled = true;
                    }
                };
                cbx.IsEnabled = false;
            }

            dockPanel.Children.Add(cbx);
            ResetBinding(cbx);
            return dockPanel;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="cbx"></param>
        void BindDatas(ComboBox cbx)
        {
            var selectValue = cbx.SelectedValue;
            cbx.Items.Clear();
            foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
            {
                cbx.Items.Add(installedPrinter);
            }
            cbx.SelectedValue = selectValue;
        }
    }
}
