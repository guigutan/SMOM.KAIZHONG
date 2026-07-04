using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SIE.Wpf.MES.DashBoard.DashBoards.Editors
{
    /// <summary>
    /// TitleImageEdit.xaml 的交互逻辑
    /// </summary>
    public partial class TitleImageEdit : UserControl
    {
        public TitleImageEdit()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// 图片与字符串转换器
    /// </summary>
    public class ImageToStringConverter : IValueConverter
    {
        /// <summary>
        /// 字符串转换为图片
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>转换后的结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string imageBase64 = value as string;
                var imageBytes = System.Convert.FromBase64String(imageBase64);
                BitmapImage bmp = null;
                if (imageBytes != null)
                {
                    bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = new MemoryStream(imageBytes);
                    bmp.EndInit();
                }

                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>转换后的结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapSource imageSource = value as BitmapSource;
            byte[] result = null;
            if (imageSource != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageSource));
                    encoder.Save(stream);
                    result = stream.ToArray();
                }
            }
            else
            {
                result = new byte[0];
            }

            return System.Convert.ToBase64String(result);
        }
    }
}
