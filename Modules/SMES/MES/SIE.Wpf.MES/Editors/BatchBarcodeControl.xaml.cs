using SIE.MES.WIP;
using SIE.Wpf.Common;
using SIE.Wpf.MES.BatchWIP;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// BarchBarcodeControl.xaml 的交互逻辑
    /// </summary>
    public partial class BatchBarcodeControl : UserControl
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
            DependencyProperty.Register("ViewModel", typeof(BatchDataCollectionViewModel<WipController>), typeof(BatchBarcodeControl), new PropertyMetadata());
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchBarcodeControl()
        {
            InitializeComponent();
            this.DataContextChanged += BatchBarcodeControl_DataContextChanged;
            _focuse = (x, y) =>
            {
                txtBarcode.Focus();
            };
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
    /// 条码编辑器是否可见转换器
    /// </summary>
    public class TextToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换
        /// </summary>
        /// <param name="value">条码值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化</param>
        /// <returns>条码值不为空可见，为空隐藏</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            return value.ToString().IsNullOrEmpty() ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// 反向转换
        /// </summary>
        /// <param name="value">条码值</param>
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