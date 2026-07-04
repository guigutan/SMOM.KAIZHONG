using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.MES.WIP;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 
    /// </summary>
    public class ChangeWorkStationEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ChangeWorkStationEditor";

        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <returns>是否选择</returns>
        protected override DependencyProperty BindingProperty()
        {
            return Button.NameProperty;
        }

        /// <summary>
        /// 创建编辑元素
        /// </summary>
        /// <returns>控件</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var control = new Button();
            control.Content = "切换工作单元".L10N();

            this.ResetBinding(control);

            this.SetAutomationElement(control);

            control.Click += (s, e) =>
            {
                var _btn = s as Button;
                var vm = ((_btn as Control).Parent as Grid).DataContext as Workstation;
                if (vm != null)
                {
                    if (vm.ChangeWorkstationEvent() && vm.WorkCellViewModel != null)
                    {
                        vm.WorkCellViewModel.Reset(ResetType.ChangeWorkStation);
                        vm.WorkCellViewModel.SaveWorkstation();
                    }
                }
                else
                {
                    var vm1 = ((_btn as Control).Parent as Grid).DataContext as KZWorkstation;
                    if (vm1 != null && vm1.ChangeWorkstationEvent() && vm1.WorkCellViewModel != null)
                    {
                        vm1.WorkCellViewModel.Reset(ResetType.ChangeWorkStation);
                        vm1.WorkCellViewModel.SaveWorkstation();
                    }
                }
            };

            return control;
        }


    }
}
