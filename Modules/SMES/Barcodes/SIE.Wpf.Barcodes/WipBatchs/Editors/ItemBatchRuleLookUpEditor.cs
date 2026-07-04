using SIE.Core.Items;
using SIE.Wpf.Barcodes.WipBatchs.ViewModels;
using SIE.Wpf.Editors;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.Barcodes.BatchBarcodes.Editors
{
    /// <summary>
    /// 批次规则下拉编辑器
    /// </summary>
    public class ItemBatchRuleLookUpEditor : EnumEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ItemBatchRuleLookUpEditor";

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <returns>控件</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var lookupEditor = base.CreateEditingElement();
            var label = new TextBox() { BorderThickness = new Thickness(1), Margin = new Thickness(2, 0, 0, 0) };
            var binding = new Binding(nameof(BatchGeneratingViewModel.BatchQty)) { Mode = BindingMode.TwoWay };
            label.SetBinding(TextBox.TextProperty, binding);
            var readonlyBinding = new Binding(nameof(BatchGeneratingViewModel.BatchRule)) { Mode = BindingMode.OneWay, Converter = new BatchRuleToLabelReadonlyConverter() };
            label.SetBinding(TextBox.IsReadOnlyProperty, readonlyBinding);
            Grid.SetColumn(lookupEditor, 0);
            Grid.SetColumn(label, 1);
            var grid = new Grid() { Margin = new Thickness(0) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(lookupEditor);
            grid.Children.Add(label);
            return grid;
        }
    }

    /// <summary>
    /// 批次数量只读转换器
    /// </summary>
    public class BatchRuleToLabelReadonlyConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BatchRule rule)
            {
                return rule != BatchRule.FixedValue;
            }

            return true;
        }

        /// <summary>
        /// 逆向转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}