using DevExpress.Xpf.PivotGrid;
using System;
using System.Windows;

namespace SIE.Wpf.MES.DashBoard.Reports
{
    /// <summary>
    /// 报表属性区域固定帮助类
    /// </summary>
    public class FieldAreaHelper : DependencyObject
    {
        /// <summary>
        /// 固定值扩展属性
        /// </summary>
        public static readonly DependencyProperty FixAreasProperty;

        /// <summary>
        /// 构造函数
        /// </summary>
        static FieldAreaHelper()
        {
            FixAreasProperty = DependencyProperty.RegisterAttached("FixAreas", typeof(bool), typeof(FieldAreaHelper), new PropertyMetadata(OnFixAreasPropertyChanged));
        }

        /// <summary>
        /// 获取固定值
        /// </summary>
        /// <param name="element">控件对象</param>
        /// <returns>固定值</returns>
        public static bool GetFixAreas(DependencyObject element)
        {
            if (element == null)
            {
                return false;
            }
            if (!(element is PivotGridControl))
            {
                throw new ArgumentNullException(nameof(element));
            }
            return (bool)((PivotGridControl)element).GetValue(FixAreasProperty);
        }

        /// <summary>
        /// 设置固定值
        /// </summary>
        /// <param name="element">控件对象</param>
        /// <param name="value">设置值</param>
        /// <returns>固定值</returns>
        public static void SetFixAreas(DependencyObject element, bool value)
        {
            if (element == null)
            {
                return;
            }
            if (!(element is PivotGridControl))
            {
                throw new ArgumentNullException(nameof(element));
            }
            element.SetValue(FixAreasProperty, value);
        }

        /// <summary>
        /// 固定值属性变更
        /// </summary>
        /// <param name="d">控件对象</param>
        /// <param name="e">事件参数</param>
        static void OnFixAreasPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PivotGridControl pivot = d as PivotGridControl;
            if (pivot == null)
            {
                return;
            }
            pivot.FieldAreaChanging += OnPivotFieldAreaChanging;
        }

        /// <summary>
        /// 控件属性固定区域变更
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">事件参数</param>
        static void OnPivotFieldAreaChanging(object sender, PivotFieldAreaChangingEventArgs e)
        {
            PivotGridField field = e.Field;
            if (field == null ||
                !(field.Parent is PivotGridControl) ||
                ((PivotGridControl)field.Parent).OlapConnectionString != null ||
                field.UnboundType != FieldUnboundColumnType.Bound)
            {
                return;
            }
            if (field.Area == FieldArea.DataArea)
            {
                if (e.NewArea != FieldArea.DataArea)
                {
                    e.Allow = false;
                }
            }
            else
            {
                if (e.NewArea == FieldArea.DataArea)
                {
                    e.Allow = false;
                }
            }
        }
    }
}
