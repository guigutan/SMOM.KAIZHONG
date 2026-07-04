using System;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 位置
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// x坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>返回实例显示字符串</returns>
        public override string ToString()
        {
            return "{0},{1}".FormatArgs(X, Y);
        }
    }
}
