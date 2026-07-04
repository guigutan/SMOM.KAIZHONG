using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// bool值按钮编辑器
    /// </summary>
    public class BoolButtonEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "BoolButtonEditor";

        /// <summary>
        /// 设置绑定属性
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return ToggleButton.IsCheckedProperty;
        }

        /// <summary>
        /// 创建编辑控件
        /// </summary>
        /// <returns>控件</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var control = CreateButton(Meta.PropertyMeta.Label, Meta.PropertyMeta.Name);

            this.ResetBinding(control);
            this.SetAutomationElement(control);
            this.AddReadOnlyComponent(control, ToggleButton.IsEnabledProperty, false);
            return control;
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="content">按钮内容</param>
        /// <param name="name">编辑器名称</param>
        /// <returns>按钮控件基类</returns>
        ToggleButton CreateButton(string content, string name)
        {
            var button = new ToggleButton();
            button.Content = content.L10N();
            button.Name = name;
            button.Margin = new Thickness(2);
            button.SetBinding(ToggleButton.BackgroundProperty, new Binding("IsChecked")
            {
                Mode = BindingMode.TwoWay,
                Converter = new BackgroundConverter(),
                RelativeSource = RelativeSource.Self
            });
            return button;
        }
    }
}
