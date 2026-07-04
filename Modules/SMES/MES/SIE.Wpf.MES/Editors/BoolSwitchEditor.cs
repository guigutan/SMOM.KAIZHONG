using Resources.IconPacks;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using SIE.Wpf.Themes;
using SIE.Wpf.Windows;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 布尔切换按钮编辑器
    /// </summary>
    public class BoolSwitchEditor : PropertyEditor<BoolSwitchEditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "BoolSwitchEditor";

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
            string[] displayName = Config.DisplayName;
            var control = new SwitchControl(displayName[0], displayName[1])
            {
                Name = this.Meta.Name
            };

            this.ResetBinding(control);

            this.SetAutomationElement(control);

            this.AddReadOnlyComponent(control, SwitchControl.IsEnabledProperty, false);

            return control;
        }
    }

    /// <summary>
    /// 布尔切换按钮编辑器配置
    /// </summary>
    public class BoolSwitchEditorConfig : EditorConfig
    {
        /// <summary>
        /// 按钮显示名称
        /// </summary>
        [DisplayName("按钮显示名称")]
        public string[] DisplayName { get; set; }
    }

    /// <summary>
    /// 上料切换的控件
    /// </summary>
    public class SwitchControl : Grid
    {
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private const string IS_CHECKED = "IsChecked";

        /// <summary>
        /// 是否选择
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(IS_CHECKED, typeof(bool), typeof(SwitchControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 上料开关控件
        /// </summary>
        /// <param name="checkName">选择名称</param>
        /// <param name="uncheckName">未选择名称</param>
        public SwitchControl(string checkName, string uncheckName)
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            MaxWidth = 400;
            var checkButton = CreateButton(checkName, new Binding(IS_CHECKED) { Mode = BindingMode.TwoWay });
            var uncheckButton = CreateButton(uncheckName, new Binding(IS_CHECKED) { Mode = BindingMode.TwoWay, Converter = new IsCheckedConverter() });
            SetColumn(uncheckButton, 1);
            Children.Add(checkButton);
            Children.Add(uncheckButton);
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="checkBinding">绑定</param>
        /// <returns>按钮控件基类</returns>
        ToggleButton CreateButton(string name, Binding checkBinding)
        {
            var button = new ToggleButton();
            //button.Content = name.L10N();

            Grid btnGrid = new Grid();
            ColumnDefinition columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(1, GridUnitType.Star);
            btnGrid.ColumnDefinitions.Add(columnDefinition);

            ColumnDefinition columnDefinition1 = new ColumnDefinition();
            columnDefinition1.Width = new GridLength(1, GridUnitType.Auto);
            btnGrid.ColumnDefinitions.Add(columnDefinition1);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = name.L10N();
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            btnGrid.Children.Add(textBlock);

            PackIcon packIcon = IconManager.GetIcon("Check", 20, 20) as PackIcon;
            packIcon.SetResourceReference(PackIcon.ForegroundProperty,
                new ColorBrushesKeyExtension
                {
                    ResourceKey = ColorBrushesKeys.GreenBrush
                });
            Grid.SetColumn(packIcon, 1);
            packIcon.VerticalAlignment = VerticalAlignment.Center;
            packIcon.SetBinding(PackIcon.VisibilityProperty, new Binding(IS_CHECKED)
            {
                Mode = BindingMode.TwoWay,
                Converter = new BooleanToVisibilityConverter(),
                RelativeSource = new RelativeSource
                {
                    AncestorType = typeof(ToggleButton)
                }
            });
            btnGrid.Children.Add(packIcon);

            button.Content = btnGrid;

            button.Name = name;
            button.DataContext = this;
            button.Margin = new Thickness(2);
            button.SetBinding(ToggleButton.IsCheckedProperty, checkBinding);
            button.SetBinding(ToggleButton.BackgroundProperty, new Binding(IS_CHECKED)
            {
                Mode = BindingMode.TwoWay,
                Converter = new BackgroundConverter(),
                RelativeSource = RelativeSource.Self
            });
            return button;
        }
    }

    /// <summary>
    /// "是否选择" 转换器
    /// </summary>
    public class IsCheckedConverter : IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">提供有关特定区域性的信息</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return false;
            return true;
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">提供有关特定区域性的信息</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    /// <summary>
    /// 背景转换器
    /// </summary>
    public class BackgroundConverter : IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">提供有关特定区域性的信息</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) { 
                return new SolidColorBrush(Colors.LightGreen);
            }
            return new SolidColorBrush(Colors.LightGray);
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">提供有关特定区域性的信息</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}