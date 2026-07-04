using SIE.MetaModel.View;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.Editors;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 布尔开关编辑器
    /// </summary>
    public class BoolToggleButtonEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "BoolToggleButtonEditor";

        /// <summary>
        /// 字体尺寸属性名称
        /// </summary>
        public const string FontSizePropertyName = "FontSize";

        /// <summary>
        /// 最小高度属性名称
        /// </summary>
        public const string MinHeightPropertyName = "MinHeight";

        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <returns>依赖属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return ToggleButton.IsCheckedProperty;
        }

        /// <summary>
        /// 创建编辑元素
        /// </summary>
        /// <returns>元素</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var btn = new ToggleButton
            {
                MinHeight = Meta.GetPropertyOrDefault<double>(BoolToggleButtonEditor.MinHeightPropertyName, 30),
                Margin = new Thickness(1)
            };
            btn.SetResourceBinding(ToggleButton.ContentProperty, Meta.Label);
            btn.SetBinding(ToggleButton.BackgroundProperty, new Binding("IsChecked") { Mode = BindingMode.TwoWay, Converter = new IsCheckedBackgroundConverter(), RelativeSource = RelativeSource.Self });
            this.ResetBinding(btn);
            this.SetAutomationElement(btn);
            return btn;
        }
    }
}
