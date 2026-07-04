using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// ImageSelectorControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageSelectorControl : UserControl
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ImageSelectorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">路由事件</param>
        private void BtnSelector_Click(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog
            {
                Filter = "png|*.png|gif|*.gif|bmp|*.bmp|jpg|*.jpg|tiff|*.tiff"
            }; //打开文件会话类，用于打开文件

            if (fd.ShowDialog() == true)
            {
                FileInfo info = new FileInfo(fd.FileName);
                if (info.Length > 2 * 1024 * 1024)
                {
                    CRT.MessageService.ShowMessage("图片大小不能超2M".L10N());
                }

                var bytes = File.ReadAllBytes(fd.FileName);
                this.SetCurrentValue(ImageBytesProperty, bytes);
            }
        }

        #region ImageBytes DependencyProperty
        /// <summary>
        /// ImageBytes
        /// </summary>
        public static readonly DependencyProperty ImageBytesProperty = DependencyProperty.Register("ImageBytes", typeof(byte[]), typeof(ImageSelectorControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) => (d as ImageSelectorControl).OnImageBytesChanged(e)));

        /// <summary>
        /// ImageBytes
        /// </summary>
        public byte[] ImageBytes
        {
            get { return (byte[])this.GetValue(ImageBytesProperty); }
            set { this.SetValue(ImageBytesProperty, value); }
        }

        /// <summary>
        /// ImageBytes变更事件
        /// </summary>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private void OnImageBytesChanged(DependencyPropertyChangedEventArgs e)
        {
            var value = (byte[])e.NewValue;
            if (value != null && value.Length > 0)
            {
                var source = new BitmapImage();
                source.BeginInit();
                source.StreamSource = new MemoryStream(value);
                source.EndInit();
                img.Source = source;
                BindingOperations.GetBinding(this, ImageBytesProperty);
                var exp = this.GetBindingExpression(ImageBytesProperty);
                exp.UpdateSource();
            }
        }
        #endregion
    }
}
