using Resources.IconPacks;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Themes;
using SIE.Wpf.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 检查结果布尔值按钮编辑器
    /// </summary>
    public class CheckResultBoolEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "CheckResultBoolEditor";

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
            var control = CreateButton(Meta.PropertyMeta.Name);

            this.ResetBinding(control);
            this.SetAutomationElement(control);
            this.AddReadOnlyComponent(control, ToggleButton.IsEnabledProperty, false);
            return control;
        }

        /// <summary>
        /// 创建按钮
        /// </summary>        
        /// <param name="name">编辑器名称</param>
        /// <returns>按钮控件基类</returns>
        ToggleButton CreateButton(string name)
        {
            const string controlName = "txtContent";
            var button = new ToggleButton();

            button.Name = name;

            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.Name = controlName;
            textBlock.SetValue(TextBlock.MarginProperty, new Thickness(3));
            textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);

            var border = new FrameworkElementFactory(typeof(Border));
            border.AppendChild(textBlock);
            border.Name = "Border";
            border.SetBinding(Border.BackgroundProperty, new Binding("BackgroundColor")
            {
                RelativeSource = RelativeSource.TemplatedParent
            });

            //值为False时，将文字改成不合格，背景变成亮灰色
            Trigger triggerWhenFalse = new Trigger();
            triggerWhenFalse.Property = ToggleButton.IsCheckedProperty;
            triggerWhenFalse.Value = false;

            Setter setterBackgroundWhenFalse = new Setter();
            setterBackgroundWhenFalse.TargetName = "Border";
            setterBackgroundWhenFalse.Property = Border.BackgroundProperty;
            setterBackgroundWhenFalse.Value = new SolidColorBrush(Colors.Red);

            Setter setterTextWhenFalse = new Setter();
            setterTextWhenFalse.TargetName = controlName;
            setterTextWhenFalse.Property = TextBlock.TextProperty;
            setterTextWhenFalse.Value = "不合格".L10N();

            Setter setterForegrondWhenFalse = new Setter();
            setterForegrondWhenFalse.TargetName = controlName;
            setterForegrondWhenFalse.Property = TextBlock.ForegroundProperty;
            setterForegrondWhenFalse.Value = new SolidColorBrush(Colors.White);

            triggerWhenFalse.Setters.Add(setterBackgroundWhenFalse);
            triggerWhenFalse.Setters.Add(setterTextWhenFalse);
            triggerWhenFalse.Setters.Add(setterForegrondWhenFalse);

            //值为True时，将文字改成合格，背景变成亮绿色
            Trigger triggerWhenTrue = new Trigger();
            triggerWhenTrue.Property = ToggleButton.IsCheckedProperty;
            triggerWhenTrue.Value = true;

            Setter setterBackgroundWhenTrue = new Setter();
            setterBackgroundWhenTrue.TargetName = "Border";
            setterBackgroundWhenTrue.Property = Border.BackgroundProperty;
            setterBackgroundWhenTrue.Value = new SolidColorBrush(Colors.LightGreen);

            Setter setterTextWhenTrue = new Setter();
            setterTextWhenTrue.TargetName = controlName;
            setterTextWhenTrue.Property = TextBlock.TextProperty;
            setterTextWhenTrue.Value = "合格".L10N();

            triggerWhenTrue.Setters.Add(setterBackgroundWhenTrue);
            triggerWhenTrue.Setters.Add(setterTextWhenTrue);


            ControlTemplate controlTemplate = new ControlTemplate();
            controlTemplate.TargetType = typeof(ToggleButton);
            controlTemplate.VisualTree = border;

            controlTemplate.Triggers.Add(triggerWhenTrue);
            controlTemplate.Triggers.Add(triggerWhenFalse);

            button.Template = controlTemplate;
            button.Margin = new Thickness(2);
            return button;
        }
    }
}
