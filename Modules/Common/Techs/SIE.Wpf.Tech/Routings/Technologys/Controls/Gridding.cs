using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SIE.Wpf.Tech.Routings.Technologys.Controls
{
    /// <summary>
    /// 画布
    /// </summary>
    public class Gridding : Canvas
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public Gridding(double width, double height)
        {
            DoubleCollection strokeDashArray = new DoubleCollection() { 20.0, 5.0 };
            this.SetGridLines(width, height, 0, 0, 40, Colors.Black, 0.2d, strokeDashArray);

            DoubleCollection strokeDashArray2 = new DoubleCollection() { 10.0, 5.0 };
            this.SetGridLines(width, height, 20, 20, 40, Colors.Gray, 0.2d, strokeDashArray2);
        }

        /// <summary>
        /// 根据画布大小更新网格线
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public void Update(double width, double height)
        {
            Children.Clear();

            DoubleCollection strokeDashArray = new DoubleCollection() { 20.0, 5.0 };
            this.SetGridLines(width, height, 0, 0, 40, Colors.Black, 0.2d, strokeDashArray);

            DoubleCollection strokeDashArray2 = new DoubleCollection() { 10.0, 5.0 };
            this.SetGridLines(width, height, 20, 20, 40, Colors.Gray, 0.2d, strokeDashArray2);
        }

        /// <summary>
        /// 设置网格线
        /// </summary>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="left">起点位置距离画布左侧距离</param>
        /// <param name="top">起点位置距离画布顶部距离</param>
        /// <param name="stepLength">表格长度</param>
        /// <param name="color">颜色</param>
        /// <param name="thickness">边框宽度</param>
        /// <param name="strokeDashArray">Double集合</param>
        private void SetGridLines(double width, double height, double left, double top, double stepLength, Color color, double thickness, DoubleCollection strokeDashArray)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = color;

            double x, y;
            x = left;
            y = 0;
            while (x < width + left)
            {
                Line line = new Line();
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x;
                line.Y2 = y + height;

                line.Stroke = brush;
                line.StrokeThickness = thickness;
                line.StrokeDashArray.Clear();
                for (int i = 0; i < strokeDashArray.Count; i++) line.StrokeDashArray.Add(strokeDashArray[i]);
                line.Stretch = Stretch.None;
                Children.Add(line);
                x += stepLength;
            }

            x = 0;
            y = top;
            while (y < height + top)
            {
                Line line = new Line();
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x + width;
                line.Y2 = y;

                line.Stroke = brush;
                line.Stretch = Stretch.None;
                line.StrokeThickness = thickness;
                line.StrokeDashArray.Clear();
                for (int i = 0; i < strokeDashArray.Count; i++) line.StrokeDashArray.Add(strokeDashArray[i]);
                Children.Add(line);
                y += stepLength;
            }
        }
    }
}
