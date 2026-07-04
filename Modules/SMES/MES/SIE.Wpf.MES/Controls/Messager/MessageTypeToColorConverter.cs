using System;
using System.Windows.Data;

namespace SIE.Wpf.MES.Controls.Messager
{
    /// <summary>
    /// 网络状态转换成颜色
    /// </summary>
    public class MessageTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MessageType messageType = (MessageType)value;
            System.Windows.Media.Brush msgForeground = System.Windows.Media.Brushes.Black;
            switch (messageType)
            {
                case MessageType.Normal:
                    msgForeground = System.Windows.Media.Brushes.Black;
                    break;
                case MessageType.Success:
                    msgForeground = System.Windows.Media.Brushes.Blue;
                    break;
                case MessageType.Error:
                    msgForeground = System.Windows.Media.Brushes.Red;
                    break;

            }
            return msgForeground;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
