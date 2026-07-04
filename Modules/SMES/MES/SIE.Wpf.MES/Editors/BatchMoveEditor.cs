using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 批次采集转入转出编辑器
    /// </summary>
    public class BatchMoveEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "BatchMoveEditor";

        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <returns>是否选择</returns>
        protected override DependencyProperty BindingProperty()
        {
            return SwitchControl.IsCheckedProperty;
        }

        /// <summary>
        /// 创建编辑元素
        /// </summary>
        /// <returns>控件</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var control = new SwitchControl("转入", "转出")
            {
                Name = this.Meta.Name
            };

            this.ResetBinding(control);

            this.SetAutomationElement(control);

            this.AddReadOnlyComponent(control, SwitchControl.IsEnabledProperty, false);

            return control;
        }
    }
}