using SIE.MES.WIP;
using SIE.Wpf.Common;
using SIE.Wpf.MES.BatchWIP;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// BatchLoadItemBarcodeControl.xaml 的交互逻辑
    /// </summary>
    public partial class BatchLoadItemBarcodeControl : UserControl
    {
        /// <summary>
        /// 聚焦事件
        /// </summary>
        EventHandler _focuse;

        #region 批次数据采集视图模型 ViewModel
        /// <summary>
        /// 批次数据采集视图模型
        /// </summary>
        public BatchDataCollectionViewModel<WipController> ViewModel
        {
            get { return (BatchDataCollectionViewModel<WipController>)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 批次数据采集视图模型
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(BatchDataCollectionViewModel<WipController>), typeof(BatchLoadItemBarcodeControl), new PropertyMetadata());
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchLoadItemBarcodeControl()
        {
            InitializeComponent();
            SetLoadItemControl();
            this.DataContextChanged += BatchBarcodeControl_DataContextChanged;
            _focuse = (x, y) =>
            {
                txtBarcode.Focus();
            };
        }

        /// <summary>
        /// 设置上料控件
        /// </summary>
        private void SetLoadItemControl()
        {
            var control = new SwitchControl("上料", "装配采集");
            control.SetBinding(SwitchControl.IsCheckedProperty, "IsLoadItem");
            foreach (var item in control.Children)
            {
                var btn = item as ToggleButton;
                if (btn == null) continue;
                btn.MinWidth = 120;
            }
            loadItemControl.Content = control;
        }

        /// <summary>
        /// 上下文变更事件
        /// </summary>
        /// <param name="sender">控件</param>
        /// <param name="e">参数</param>
        private void BatchBarcodeControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var old = e.OldValue as IFocusTrigger;
            if (old != null)
                old.Focused -= _focuse;
            var trigger = e.NewValue as IFocusTrigger;
            if (trigger != null)
                trigger.Focused += _focuse;
        }

        /// <summary>
        /// 入站条码回车事件
        /// </summary>
        /// <param name="sender">TextEdit</param>
        /// <param name="e">参数</param>
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            txtBarcode.Focus();
            e.Handled = true;
        }

        /// <summary>
        /// 出站条码回车事件
        /// </summary>
        /// <param name="sender">TextEdit</param>
        /// <param name="e">参数</param>
        private void TxtOutBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            txtOutBarcode.Focus();
            e.Handled = true;
        }
    }

    /// <summary>
    /// 上料是隐藏出站条码控件
    /// </summary>
    public class IsLoadItemToVisibilyConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换
        /// </summary>
        /// <param name="value">是否上料</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化</param>
        /// <returns>是否上料为true时隐藏出站条码控件</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isLoadItem = false;
            bool.TryParse(value?.ToString(), out isLoadItem);
            return isLoadItem ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// 反向转换
        /// </summary>
        /// <param name="value">是否上料</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化</param>
        /// <returns>未实现</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 是否上料控件跨列编辑器
    /// </summary>
    public class IsLoadItemToColumnSpanConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换
        /// </summary>
        /// <param name="value">是否上料</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化</param>
        /// <returns>是否上料为true是跨两列</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isLoadItem = false;
            bool.TryParse(value?.ToString(), out isLoadItem);
            return isLoadItem ? 2 : 1;
        }

        /// <summary>
        /// 反向转换
        /// </summary>
        /// <param name="value">是否上料</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化</param>
        /// <returns>未实现</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}