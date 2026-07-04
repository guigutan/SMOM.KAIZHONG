using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using tech = SIE.Tech.Routings.Technologys;

namespace SIE.Wpf.Tech.Routings.Technologys.Controls
{
    /// <summary>
    /// 箭头画布
    /// </summary>
    public class Arrowhead : Canvas
    {
        #region Stroke
        /// <summary>
        /// 画笔颜色
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// 画笔颜色
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(Arrowhead), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) => (d as Arrowhead).OnStrokeChanged(e)));

        /// <summary>
        /// 画笔颜色变更事件
        /// </summary>
        /// <param name="e">参数</param>
        private void OnStrokeChanged(DependencyPropertyChangedEventArgs e)
        {
            lineRight.Stroke = (Brush)e.NewValue;
            lineLeft.Stroke = (Brush)e.NewValue;
            lineCenter.Stroke = (Brush)e.NewValue;
        }
        #endregion

        #region Point
        /// <summary>
        /// 开始位置
        /// </summary>
        public tech.Point BeginPoint
        {
            get { return (tech.Point)GetValue(BeginPointProperty); }
            set { SetValue(BeginPointProperty, value); }
        }

        /// <summary>
        /// 开始位置
        /// </summary>
        public static readonly DependencyProperty BeginPointProperty =
            DependencyProperty.Register("BeginPoint", typeof(tech.Point), typeof(Arrowhead), new FrameworkPropertyMetadata(new tech.Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) => (d as Arrowhead).OnPointChanged(e)));

        /// <summary>
        /// 结束位置
        /// </summary>
        public tech.Point EndPoint
        {
            get { return (tech.Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        /// <summary>
        /// 结束位置
        /// </summary>
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(tech.Point), typeof(Arrowhead), new FrameworkPropertyMetadata(new tech.Point(0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) => (d as Arrowhead).OnPointChanged(e)));

        /// <summary>
        /// 开始结束位置变更事件
        /// </summary>
        /// <param name="e">参数</param>
        private void OnPointChanged(DependencyPropertyChangedEventArgs e)
        {
            SetAngleByPoint(BeginPoint, EndPoint);
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public Arrowhead()
        {
            lineLeft = new Line();
            lineRight = new Line();
            lineCenter = new Line();
            this.Children.Add(lineLeft);
            this.Children.Add(lineRight);
            this.Children.Add(lineCenter);
            lineCenter.Opacity = 0.1;

            lineLeft.X1 = 0;
            lineLeft.Y1 = 0;
            lineRight.X1 = 0;
            lineRight.Y1 = 0;
            lineCenter.X1 = 0;
            lineCenter.Y1 = 0;
        }

        /// <summary>
        /// 箭头的长度
        /// </summary>
        readonly int arrowLenght = 12;

        /// <summary>
        /// 箭头的与直线的夹角
        /// </summary>
        readonly int arrowAngle = 30;

        /// <summary>
        /// 左线
        /// </summary>
        readonly Line lineLeft;

        /// <summary>
        /// 右线
        /// </summary>
        readonly Line lineRight;

        /// <summary>
        /// 中线
        /// </summary>
        readonly Line lineCenter;

        /// <summary>
        /// 透明度
        /// </summary>
        public new double Opacity
        {
            get
            {
                return lineRight.Opacity;
            }

            set
            {
                lineRight.Opacity = value;
                lineLeft.Opacity = value;
            }
        }

        /// <summary>
        /// 箭头宽度
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return lineLeft.StrokeThickness;
            }

            set
            {
                lineRight.StrokeThickness = value;
                lineLeft.StrokeThickness = value;
                lineCenter.StrokeThickness = value;
            }
        }

        /// <summary>
        /// 设置箭头的旋转角度
        /// </summary>
        /// <param name="degreeLeft">左角度</param>
        /// <param name="degreeRight">右角度</param>
        void SetAngleByDegree(double degreeLeft, double degreeRight)
        {
            if (double.IsNaN(degreeLeft) || double.IsNaN(degreeRight))
                return;
            double x = Math.Sin(Math.PI * degreeLeft / 180.0);
            double y = Math.Sin(Math.PI * (90 - degreeLeft) / 180.0);

            lineLeft.X2 = -arrowLenght * x;
            lineLeft.Y2 = -arrowLenght * y;
            x = Math.Sin(Math.PI * degreeRight / 180.0);
            y = Math.Sin(Math.PI * (90 - degreeRight) / 180.0);
            lineRight.X2 = arrowLenght * x;
            lineRight.Y2 = -arrowLenght * y;

            lineCenter.X2 = (lineRight.X2 + lineLeft.X2) / 2;
            lineCenter.Y2 = (lineRight.Y2 + lineLeft.Y2) / 2;
        }

        /// <summary>
        /// 根据直线的起始点和结束点的坐标设置箭头的旋转角度
        /// </summary>
        /// <param name="beginPoint">开始位置</param>
        /// <param name="endPoint">结束位置</param>
        void SetAngleByPoint(SIE.Tech.Routings.Technologys.Point beginPoint, SIE.Tech.Routings.Technologys.Point endPoint)
        {
            double x = endPoint.X - beginPoint.X;
            double y = endPoint.Y - beginPoint.Y;
            double angle = 0;
            if (y == 0)
            {
                if (x > 0)
                    angle = -90;
                else
                    angle = 90;
            }
            else
                angle = Math.Atan(x / y) * 180 / Math.PI;

            if (endPoint.Y <= beginPoint.Y)
                SetAngleByDegree((arrowAngle + angle) - 180, (arrowAngle - angle) - 180);
            else
                SetAngleByDegree(arrowAngle + angle, arrowAngle - angle);
        }
    }
}
