using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Wpf.Editors;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 称重信息
    /// </summary>
    [RootEntity, Serializable]
    public class WeightInfo : ViewModel
    {
        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [Label("重量")]
        public static readonly Property<decimal> WeightProperty = P<WeightInfo>.Register(e => e.Weight);

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 上偏差 UpperLimit
        /// <summary>
        /// 上偏差
        /// </summary>
        [Label("上偏差")]
        public static readonly Property<decimal?> UpperLimitProperty = P<WeightInfo>.Register(e => e.UpperLimit);

        /// <summary>
        /// 上偏差
        /// </summary>
        public decimal? UpperLimit
        {
            get { return this.GetProperty(UpperLimitProperty); }
            set { this.SetProperty(UpperLimitProperty, value); }
        }
        #endregion

        #region 下偏差 LowerLimit
        /// <summary>
        /// 下偏差
        /// </summary>
        [Label("下偏差")]
        public static readonly Property<decimal?> LowerLimitProperty = P<WeightInfo>.Register(e => e.LowerLimit);

        /// <summary>
        /// 下偏差
        /// </summary>
        public decimal? LowerLimit
        {
            get { return this.GetProperty(LowerLimitProperty); }
            set { this.SetProperty(LowerLimitProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 称重编辑器
    /// </summary>
    public class WeighEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "WeighEditor";

        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <returns>重量值</returns>
        protected override DependencyProperty BindingProperty()
        {
            return WeighShowControl.ValueProperty;
        }

        /// <summary>
        /// 创建编辑元素
        /// </summary>
        /// <returns>编辑控件</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var control = new WeighShowControl()
            {
                Name = this.Meta.Name
            };

            this.ResetBinding(control);
            this.SetAutomationElement(control);
            return control;
        }
    }

    /// <summary>
    /// 重量显示控件
    /// </summary>
    class WeighShowControl : Grid
    {
        /// <summary>
        /// 重量值
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(WeightInfo), typeof(WeighShowControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 重量值
        /// </summary>
        public WeightInfo Value
        {
            get
            {
                return (WeightInfo)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public WeighShowControl()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { });
            TextBlock textBlock = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.Red),
                Background = new SolidColorBrush(Colors.Black),
                FontSize = 32,
                FontWeight = FontWeights.UltraLight,
                TextAlignment = TextAlignment.Right
            };
            textBlock.DataContext = this;
            textBlock.SetBinding(TextBlock.WidthProperty, "Width");
            var styles = new Style(typeof(TextBlock));

            var setter = new Setter(TextBlock.FontFamilyProperty, new FontFamily(
                new Uri("pack://application:,,,/", UriKind.Absolute), "./Resources;component/Fonts/#Quartz Regular"));

            styles.Setters.Add(setter);

            textBlock.Style = styles;

            //绑定文字
            var bindingTextBlockText = new Binding("Value.Weight");

            bindingTextBlockText.Mode = BindingMode.TwoWay;

            textBlock.SetBinding(TextBlock.TextProperty, bindingTextBlockText);

            //绑定颜色
            var bindingTextBlockBackground = new Binding("Value")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Converter = new BackgroundConverter()
            };

            bindingTextBlockBackground.Mode = BindingMode.TwoWay;

            textBlock.SetBinding(TextBlock.BackgroundProperty, bindingTextBlockBackground);
            SetColumn(textBlock, 0);
            Children.Add(textBlock);
        }

        /// <summary>
        /// 背景转换器
        /// </summary>
        class BackgroundConverter : IValueConverter
        {
            /// <summary>
            /// 转换
            /// </summary>
            /// <param name="value">重量</param>
            /// <param name="targetType">目标类型</param>
            /// <param name="parameter">参数</param>
            /// <param name="culture">提供有关特定区域性信息</param>
            /// <returns>重量(object类型)</returns>
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                SolidColorBrush solidColorBrush = Brushes.Black;

                WeightInfo weighInfo = value as WeightInfo;
                if (weighInfo == null)
                {
                    return solidColorBrush;
                }

                if (weighInfo.LowerLimit.HasValue || weighInfo.UpperLimit.HasValue)
                {
                    if (weighInfo.LowerLimit.HasValue && weighInfo.Weight < weighInfo.LowerLimit)
                    {
                        solidColorBrush = new SolidColorBrush(Colors.Yellow);
                    }

                    if (weighInfo.UpperLimit.HasValue && weighInfo.Weight > weighInfo.UpperLimit)
                    {
                        solidColorBrush = new SolidColorBrush(Colors.Yellow);
                    }
                }

                return solidColorBrush;
            }

            /// <summary>
            /// 回滚
            /// </summary>
            /// <param name="value">重量</param>
            /// <param name="targetType">目标类型</param>
            /// <param name="parameter">参数</param>
            /// <param name="culture">提供有关特定区域性信息</param>
            /// <returns>重量(object类型)</returns>
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
