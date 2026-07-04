using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using System.Windows;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// 图片编辑器
    /// </summary>
    class ImagePropertyEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ImagePropertyEditor";

        /// <summary>
        /// 依赖属性：绑定属性，绑定WPF控件
        /// </summary>
        /// <returns>DependencyProperty</returns>
        protected override DependencyProperty BindingProperty()
        {
            return ImageSelectorControl.ImageBytesProperty;
        }

        /// <summary>
        /// 框架元素：创建编辑元素
        /// </summary>
        /// <returns>FrameworkElement</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var selector = new ImageSelectorControl();
            this.ResetBinding(selector);
            this.AddReadOnlyComponent(selector);
            return selector;
        }
    }
}
