using Resources.IconPacks;
using SIE.Wpf.Editors;
using SIE.Wpf.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 实体弹框选择编辑器，可以锁定信息
    /// </summary>
    public class LockableLookUpEditor : PagingLookUpEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "LockableLookUpEditor";

        /// <summary>
        /// 是否带锁扩展属性
        /// </summary>
        public const string HasLock = "HasLock";

        /// <summary>
        /// 子类在这个方法里面生成一个用于Edit的WPF Control
        /// 注意，此方法中可能需要：
        /// 1.调用 SetAutomationControl 方法：
        ///     由于返回的控件可能是一个大的窗口，所以应该在最终生成的控件上调用 SetAutomationControl 方法。
        /// 2.调用 BindElementReadOnly 方法：
        ///     调用此方法以支持只读属性的绑定。
        /// </summary>
        /// <returns>返回创建的UI</returns>   
        protected override FrameworkElement CreateEditingElement()
        {
            FrameworkElement editor = base.CreateEditingElement();
            FrameworkElement result = editor;
            bool haslock = base.Config.GetPropertyOrDefault(HasLock, false);
            if (haslock)
            {
                var panel = new DockPanel();
                ////开关按钮
                var btnLock = new ToggleButton();
                btnLock.SetResourceReference(Button.StyleProperty, "ImageButtonBaseStyle");
                btnLock.Padding = new Thickness(0);
                var bindingLock = new Binding("ActualHeight");
                bindingLock.RelativeSource = RelativeSource.Self;
                btnLock.SetBinding(Button.WidthProperty, bindingLock);
                PackIcon packIcon = IconManager.GetPackIcon("Lock", 16, 16);
                packIcon.Width = 16;
                btnLock.Content = packIcon;
                btnLock.SetValue(DockPanel.DockProperty, Dock.Right);
                KeyboardNavigation.SetIsTabStop(btnLock, false);
                btnLock.IsChecked = true;
                editor.IsEnabled = false;
                btnLock.Click += (s, e) =>
                {
                    if (btnLock.IsChecked == true)
                    {
                        btnLock.Content = IconManager.GetPackIcon("Lock", 16, 16);
                        editor.IsEnabled = false;
                    }
                    else
                    {
                        btnLock.Content = IconManager.GetPackIcon("Unlock", 16, 16);
                        editor.IsEnabled = true;
                    }
                };
                panel.Children.Add(btnLock);
                panel.Children.Add(editor);
                result = panel;
            }

            return result;
        }
    }
}
