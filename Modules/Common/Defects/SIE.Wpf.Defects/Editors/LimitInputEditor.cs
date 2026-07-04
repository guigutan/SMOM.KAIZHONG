using DevExpress.Xpf.Editors;
using SIE.Defects.InspectionItems;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Wpf.Editors;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.Defects.Editors
{
    /// <summary>
    /// 规格下限编辑器
    /// </summary>
    public class LimitLowEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// LimitLowEditor
        /// </summary>
        public const string EditorName = "LimitLowEditor";

        /// <summary>
        /// 下拉列表选择控件
        /// </summary>
        ComboBox cbx;

        /// <summary>
        /// SpinEdit
        /// </summary>
        SpinEdit tbk;

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return SpinEdit.TextProperty;
        }

        /// <summary>
        /// 创建编辑器样式
        /// </summary>
        /// <returns>FrameworkElement</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var panel = new Grid();
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            cbx = new ComboBox();
            cbx.SelectedIndex = 0;
            var source = Enum.GetValues(typeof(CompareType)).Cast<Enum>().Select(p => new
            {
                (Attribute.GetCustomAttribute(p.GetType().GetField(p.ToString()), typeof(LabelAttribute)) as LabelAttribute).Label,
                (Attribute.GetCustomAttribute(p.GetType().GetField(p.ToString()), typeof(CategoryAttribute)) as CategoryAttribute).Category,
                p
            }).Where(p => p.Category == "Greater").OrderBy(item => item.p).ToList();
            source.Insert(0, new { Label = string.Empty, Category = string.Empty, p = (Enum)null });
            cbx.ItemsSource = source;
            var comboBoxBinding = new Binding("LimitLowCompare");
            comboBoxBinding.Mode = BindingMode.TwoWay;
            cbx.SetBinding(ComboBox.SelectedValueProperty, comboBoxBinding);
            cbx.DisplayMemberPath = "Label";
            cbx.SelectedValuePath = "p";
            cbx.SelectionChanged += Cbx_SelectionChanged;

            tbk = new SpinEdit();
            tbk.AllowNullInput = true;
            var spinEditBinding = new Binding("LimitLow");
            spinEditBinding.Mode = BindingMode.TwoWay;
            spinEditBinding.Converter = new DecimalValueConverter();
            tbk.SetBinding(SpinEdit.TextProperty, spinEditBinding);
            tbk.EditValueChanged += Tbk_EditValueChanged;
            Grid.SetColumn(cbx, 0);
            Grid.SetColumn(tbk, 1);
            panel.Children.Add(cbx);
            panel.Children.Add(tbk);
            return panel;
        }

        /// <summary>
        /// 清空规格下限符号的同时清空规范下限的值
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">选择变更事件参数</param>
        private void Cbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.SelectedValue == null)
                tbk.EditValue = null;
        }

        /// <summary>
        /// 清空规格下限值的同时清空规范下限符号
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">编辑值变更事件参数</param>
        private void Tbk_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var spinEdit = sender as SpinEdit;
            if (spinEdit.EditValue == null)
                cbx.SelectedValue = null;
        }
    }

    /// <summary>
    /// 规格上限编辑器
    /// </summary>
    public class LimitMaxEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// LimitMaxEditor
        /// </summary>
        public const string EditorName = "LimitMaxEditor";

        /// <summary>
        /// 下拉列表选择控件
        /// </summary>
        ComboBox cbx;

        /// <summary>
        /// SpinEdit
        /// </summary>
        SpinEdit tbk;

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return SpinEdit.TextProperty;
        }

        /// <summary>
        /// 创建编辑器样式
        /// </summary>
        /// <returns>FrameworkElement</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var panel = new Grid();
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            cbx = new ComboBox();
            cbx.IsEditable = true;
            cbx.SelectedIndex = 0;
            var source = Enum.GetValues(typeof(CompareType)).Cast<Enum>().Select(p => new
            {
                (Attribute.GetCustomAttribute(p.GetType().GetField(p.ToString()), typeof(LabelAttribute)) as LabelAttribute).Label,
                (Attribute.GetCustomAttribute(p.GetType().GetField(p.ToString()), typeof(CategoryAttribute)) as CategoryAttribute).Category,
                p
            }).Where(p => p.Category == "Less").OrderBy(item => item.p).ToList();
            source.Insert(0, new { Label = string.Empty, Category = string.Empty, p = (Enum)null });
            cbx.ItemsSource = source;

            var comboBoxBinding = new Binding("LimitMaxCompare");
            comboBoxBinding.Mode = BindingMode.TwoWay;
            cbx.SetBinding(ComboBox.SelectedValueProperty, comboBoxBinding);
            cbx.DisplayMemberPath = "Label";
            cbx.SelectedValuePath = "p";
            cbx.SelectionChanged += Cbx_SelectionChanged;

            tbk = new SpinEdit();
            tbk.AllowNullInput = true;
            var spinEditBinding = new Binding("LimitMax");
            spinEditBinding.Mode = BindingMode.TwoWay;
            spinEditBinding.Converter = new DecimalValueConverter();
            tbk.SetBinding(SpinEdit.TextProperty, spinEditBinding);
            tbk.EditValueChanged += Tbk_EditValueChanged;
            Grid.SetColumn(cbx, 0);
            Grid.SetColumn(tbk, 1);
            panel.Children.Add(cbx);
            panel.Children.Add(tbk);
            return panel;
        }

        /// <summary>
        /// 清空规格上限值的同时清空规格上限符号
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">编辑值变更事件参数</param>
        private void Tbk_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var spinEdit = sender as SpinEdit;
            if (spinEdit.EditValue == null)
                cbx.SelectedValue = null;
        }

        /// <summary>
        /// 清空规格上限符号的同时清空规格上限的值
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">选择变更事件参数</param>
        private void Cbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.SelectedValue == null)
                tbk.EditValue = null;
        }
    }

    /// <summary>
    /// SpinEdit数值转换器
    /// </summary>
    class DecimalValueConverter : IValueConverter
    {
        /// <summary>
        /// 将源值转换为绑定源的值。数据绑定引擎在将值从绑定源传播给绑定目标时，调用此方法。
        /// </summary>
        /// <param name="value">源绑定生成的值的数组</param>
        /// <param name="targetType">绑定目标属性的类型</param>
        /// <param name="parameter">要使用的转换器参数</param>
        /// <param name="culture">要用在转换器中的区域性</param>
        /// <returns> 转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将绑定目标值转换为源绑定值
        /// </summary>
        /// <param name="value">绑定目标生成的值</param>
        /// <param name="targetType"> 要转换到的类型数组</param>
        /// <param name="parameter">要使用的转换器参数</param>
        /// <param name="culture">要用在转换器中的区域性</param>
        /// <returns>从目标值转换回源值的值的数组</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((string)value) != string.Empty)
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
