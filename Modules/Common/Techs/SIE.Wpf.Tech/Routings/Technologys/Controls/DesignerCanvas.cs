using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Tech.Routings.Technologys.Controls
{
    /// <summary>
    /// 设计画布
    /// </summary>
    public class DesignerCanvas : Canvas
    {
        /// <summary>
        /// 测量
        /// </summary>
        /// <param name="constraint">尺寸(参数)</param>
        /// <returns>尺寸(返回值)</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            foreach (UIElement element in this.InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;
                ////measure desired size for each child
                element.Measure(constraint);
                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            //// add margin 
            size.Width += 10;
            size.Height += 10;

            return size;
        }
    }
}
