using DevExpress.Xpf.Editors;
using SIE.Wpf.Editors;
using SIE.Wpf.MES.WIP;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 工单明细弹框编辑器
    /// </summary>
    public class WorkOrderEditor : TextEditor
    {
        /// <summary>
        /// 工单明细编辑器名称
        /// </summary>
        public const string EditorName = "WorkOrderEditor";

        /// <summary>
        /// 返回本编辑器中控件所需要使用绑定的控件属性。
        /// </summary>
        /// <returns>返回依赖属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return TextEdit.TextProperty;
        }

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
            var text = base.CreateEditingElement();
            var panel = new DockPanel();
            var button = new Button();
            button.Content = "...";
            button.Click += (s, e) =>
            {
                var m = Context.CurrentObject as DataCollectionViewModel;
                var template = new DetailsUITemplate(typeof(WorkOrderDetailViewModel));
                var model = new WorkOrderDetailViewModel();
                model.WorkOrder = m.WorkOrder;
                var ui = template.CreateUI();
                ui.MainView.Data = model;
                CRT.Workbench.ShowDialog(ui, w =>
                {
                    w.Title = "工单信息".L10N();
                    w.MinWidth = 400;
                    w.MinHeight = 200;
                    w.Width = 550;
                    w.Height = 350;
                });
            };
            DockPanel.SetDock(button, Dock.Right);
            panel.Children.Add(button);
            panel.Children.Add(text);
            return panel;
        }
    }
}
