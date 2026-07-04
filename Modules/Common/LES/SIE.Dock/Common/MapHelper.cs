using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Common
{
    /// <summary>
    /// 计算距离工具类 参考文章https://www.cnblogs.com/hugeboke/p/13348034.html
    /// </summary>
    public class MapHelper:DomainController
    {
        /// <summary>
        /// 地球半径
        /// </summary>
        private const double EarthRadius = 6378.137;

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return d * Math.PI / 180d;
        }

        /// <summary>
        /// 计算两个坐标点之间的距离
        /// </summary>
        /// <param name="firstLongitude">第一个坐标的经度</param>
        /// <param name="firstLatitude">第一个坐标的纬度</param>
        /// <param name="secondLongitude">第二个坐标的经度</param>
        /// <param name="secondLatitude">第二个坐标的纬度</param>
        /// <returns>返回两点之间的距离，单位：公里/千米</returns>
        public static double GetDistance(double firstLongitude, double firstLatitude, double secondLongitude,double secondLatitude)
        {
            var firstRadLat = Rad(firstLatitude);
            var firstRadLng = Rad(firstLongitude);
            var secondRadLat = Rad(secondLatitude);
            var secondRadLng = Rad(secondLongitude);
            var a = firstRadLat - secondRadLat;
            var b = firstRadLng - secondRadLng;
            var cal = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(firstRadLat)
                * Math.Cos(secondRadLat) * Math.Pow(Math.Sin(b / 2), 2))) * EarthRadius;
            var result = Math.Round(cal * 10000) / 10000;
            return result;
        }
    }
}
