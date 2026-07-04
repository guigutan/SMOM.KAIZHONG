using SIE.Wpf.Editors;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 批次上料条码编辑器
    /// 多上料，装配按钮
    /// </summary>
    public class BatchLoadItemBarcodeEditor : PropertyEditor<BaseEditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "BatchLoadItemBarcodeEditor";

        /// <summary>
        /// 返回本编辑器中控件所需要使用绑定的控件属性。
        /// </summary>
        /// <returns>返回依赖属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return BatchLoadItemBarcodeControl.ViewModelProperty;
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
            BatchLoadItemBarcodeControl control = new BatchLoadItemBarcodeControl();
            control.Name = this.Meta.Name;
            this.ResetBinding(control);
            this.SetAutomationElement(control);
            return control;
        }
    }
}