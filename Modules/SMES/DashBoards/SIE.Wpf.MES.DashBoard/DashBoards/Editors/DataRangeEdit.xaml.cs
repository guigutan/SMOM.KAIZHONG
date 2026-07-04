using SIE.ObjectModel;
using SIE.Wpf.Common.Diagram;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.DashBoard.DashBoards.Editors
{
    /// <summary>
    /// DataRangeEdit.xaml 的交互逻辑
    /// </summary>
    public partial class DataRangeEdit : UserControl
    {
        /// <summary>
        /// 数据区间
        /// </summary>
        public DataRange DataRange
        {
            get { return (DataRange)GetValue(DataRangeProperty); }
            set { SetValue(DataRangeProperty, value); }
        }

        /// <summary>
        /// 依赖属性
        /// </summary>
        // Using a DependencyProperty as the backing store for DataRange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataRangeProperty =
            DependencyProperty.Register("DataRange", typeof(DataRange), typeof(DataRangeEdit), new PropertyMetadata(new DataRange()));

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataRangeEdit()
        {
            InitializeComponent();
            this.Loaded += (o, e) =>
            {
                DataRangeEdit owner = this;
                var path = PropertyEditorBinder.GetPropertyName(owner);
                SetBinding(DataRangeProperty, new Binding(path) { Mode = BindingMode.TwoWay });
                sp.DataContext = this.DataRange;
                DataRange.PropertyChanged += DataRange_PropertyChanged;
            };
        }
        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件包含的值</param>
        private void DataRange_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var dataRange = sender as DataRange;
            ////if (dataRange.LowerLimitValue == 0 && dataRange.UpperLimitValue == 0)
            ////{
            ////}
            if (dataRange.LowerLimitValue < 0 || dataRange.LowerLimitValue > 100)
            {
                dataRange.LowerLimitValue = 0;
            }
            else if (dataRange.UpperLimitValue < 0 || dataRange.UpperLimitValue > 100)
            {
                dataRange.UpperLimitValue = 0;
            }
            else if (dataRange.LowerLimitValue > dataRange.UpperLimitValue)
            {
                dataRange.LowerLimitValue = 0;
            }
            else
            {
                //
            }
        }
    }

    /// <summary>
    /// 数据区间
    /// </summary>
    [Serializable]
    public class DataRange : ObservableObject
    {
        /// <summary>
        /// 下限值
        /// </summary>
        public double LowerLimitValue
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 上限值
        /// </summary>
        public double UpperLimitValue
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
    }
}
