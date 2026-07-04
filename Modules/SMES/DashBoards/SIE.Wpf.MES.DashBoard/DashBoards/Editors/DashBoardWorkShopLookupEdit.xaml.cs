using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.DashBoard.DashBoards.Editors
{
    /// <summary>
    /// 车间下拉框
    /// </summary>
    public partial class DashBoardWorkShopLookupEdit : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DashBoardWorkShopLookupEdit()
        {
            InitializeComponent();

            var shopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(null, string.Empty);
            this.workShopLookupEdit.ItemsSource = shopList;
        }
    }

    /// <summary>
    /// 多选选择选择值是一个List对象
    /// 将其转换成string
    /// </summary>
    public class ListToStringConverter : IValueConverter
    {
        /// <summary>
        /// 字符串转换为doulbe类型集合
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>doulbe类型集合</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var valueStr = value as string;
            List<double> selectedIds = new List<double>();
            valueStr.Split(';').ForEach(p => selectedIds.Add(double.Parse(p)));
            return selectedIds;
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>以逗号隔开的字符串</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var valueList = (value as List<object>).OfType<double>().ToList();
            return string.Join(";", valueList);
        }
    }
}
