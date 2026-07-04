using DevExpress.Xpf.Editors;
using SIE.Defects;
using SIE.Domain;
using SIE.Wpf.Editors;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.TaskManagement.Reports.Editors
{
    /// <summary>
    /// 缺陷录入弹框编辑器
    /// </summary>
    public class ReportDefectEditor : TextEditor
    {
        /// <summary>
        /// 缺陷录入弹框编辑器名称
        /// </summary>
        public const string EditorName = "ReportDefectEditor";

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
                var editor = DefectControlFactory.CreateControl();
                editor.AllowMultiple = true;
                editor.AllowQty = false;
                editor.Defects.AddRange(RF.GetAll<Defect>());
                editor.SelectedValue.Clear();

                var vm = Context.CurrentObject as TaskReportViewModel;
                //设置当前报工记录已选的缺陷信息
                vm.DefectList.ForEach(defect =>
                {
                    var item = new DefectItem() { Defect = defect };
                    editor.SelectedValue.Add(item);
                });

                CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
                {
                    w.Title = "缺陷录入".L10N();
                    w.Height = 500;
                    w.Width = 750;
                    w.MinHeight = 350;
                    w.MinWidth = 400;
                    w.Closing += (a, b) =>
                    {
                        if (w.Result == 0)
                        {
                            var defects = editor.SelectedValue.Select(p => p.Defect).ToList();
                            vm.DefectList.Clear();
                            vm.DefectList.AddRange(defects);
                            vm.DefectNames = string.Join(";", defects.Select(p => p.Description)); 
                        }
                    };
                });
            };
            DockPanel.SetDock(button, Dock.Right);
            panel.Children.Add(button);
            panel.Children.Add(text);
            return panel;
        }
    }
}
