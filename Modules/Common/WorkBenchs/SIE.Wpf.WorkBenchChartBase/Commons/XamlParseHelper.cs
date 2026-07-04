using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class XamlParseHelper
    {
        /// <summary>
        /// 根据字符串xaml构建控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xamlStr"></param>
        /// <returns></returns>
        public static T BuildViewContent<T>(string xamlStr)
        {
            return (T)XamlReader.Parse(xamlStr);
        }

        /// <summary>
        /// 从资源文件中加载控件
        /// </summary>
        /// <typeparam name="T">生成控件类型</typeparam>
        /// <param name="uriStr">Uri字符串</param>
        /// <param name="uriKind">Uri类型</param>
        /// <returns>控件</returns>
        public static T LoadEmbeddedXaml<T>(string uriStr, UriKind uriKind) where T : FrameworkElement
        {
            //Build Action = Resource,Do not Copy,无相应cs文件
            Uri uri = new Uri(uriStr, uriKind);
            Stream stream = Application.GetResourceStream(uri).Stream;
            T obj = (T)XamlReader.Load(stream);
            return obj;
        }
    }
}
