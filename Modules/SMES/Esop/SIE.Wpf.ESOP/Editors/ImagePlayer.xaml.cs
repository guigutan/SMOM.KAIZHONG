using SIE.Common.Utils;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SIE.Wpf.ESop.Common;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Map;
using SIE.Wpf.ESOP.ESOPFactory;
using SIE.ESop.Documents;

namespace SIE.Wpf.ESop.Editors
{
    /// <summary>
    /// PdfPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class ImagePlayer : UserControl, IPlayer
    {
        /// <summary>
        /// 记录当前文件路径
        /// </summary>
        public Uri CurrentUri { get; private set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 默认缩放比
        /// </summary>
        private TransformGroup defaultTransformGroup;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImagePlayer()
        {
            InitializeComponent();
            ImagePlayer owner = this;
            imageControl.DataContext = owner;
            imageControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            imageControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            imageControl.Visibility = Visibility.Visible;
            imageControl.SizeChanged += ImageControl_SizeChanged;
            imageControl.Loaded += ImageControl_Loaded;
            imageControl.IsVisibleChanged += ImageControl_IsVisibleChanged;
            imageControl.OverridesDefaultStyle = true;

        }
        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="showControl"></param>
        public ImagePlayer(FactoryShowControl showControl)
        {
            InitializeComponent();
            ImagePlayer owner = this;
            imageControl.DataContext = owner;
            imageControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            imageControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            imageControl.Visibility = Visibility.Visible;
            imageControl.SizeChanged += ImageControl_SizeChanged;
            imageControl.Loaded += ImageControl_Loaded;
            imageControl.IsVisibleChanged += ImageControl_IsVisibleChanged;
            imageControl.OverridesDefaultStyle = true;
            this.DataContext = showControl;
            this.HideUI();
            foreach (var ctr in showControl.Children)
            {
                var child = ctr as ImagePlayer;
                if (child != null)
                {
                    return;
                }
            }
            showControl.Children.Add(owner);

        }

        /// <summary>
        /// 控件初始化加载数据
        /// </summary>
        /// <param name="sender">当前初始化控件</param>
        /// <param name="e">路由事件参数</param>
        private void ImageControl_Loaded(object sender, RoutedEventArgs e)
        {
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 控件可视化变化时触发
        /// </summary>
        /// <param name="sender">当前控件</param>
        /// <param name="e">事件参数</param>
        private void ImageControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 控件尺寸改变时触发
        /// </summary>
        /// <param name="sender">改变的控件对象</param>
        /// <param name="e">尺寸大小改变参数</param>
        private void ImageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ZoomUseRange();
        }

        /// <summary>
        /// 根据路径加载文件
        /// </summary>
        /// <param name="filePath">路径</param>
        public void LoadData(string filePath)
        {
            BitmapImage bim = new BitmapImage();
            bim.BeginInit();
            bim.StreamSource = filePath.UirFileToMemoryStream();
            bim.EndInit();
            imageControl.Source = bim;
            GC.Collect(); //强制回收资源 
        }

        /// <summary>
        /// 显示指定页签
        /// </summary>
        /// <param name="sheetName">页签名称</param>
        public void Show(string sheetName)
        {
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 计算用户范围大小
        /// </summary>
        private void MatchUseRangeSize()
        {
            // 计算用户范围大小
        }

        /// <summary>
        /// 焦点根据用户范围和控件大小进行调整
        /// </summary>
        private void ZoomUseRange()
        {
            // 焦点根据用户范围和控件大小进行调整
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            //// pdfViewerControl.ActualCommandProvider.ZoomInCommand.Execute(null);
            TransformGroup tg = imageControl.RenderTransform as TransformGroup;
            var tgnew = tg.CloneCurrentValue();
            if (defaultTransformGroup == null)
            {
                defaultTransformGroup = tgnew.CloneCurrentValue();
            }
            if (tgnew != null)
            {
                ScaleTransform st = tgnew.Children[1] as ScaleTransform;
                imageControl.RenderTransformOrigin = new Point(0.25, 0.25);
                if (st.ScaleX > 0 && st.ScaleX <= 5.0)
                {
                    st.ScaleX += 0.25;
                    st.ScaleY += 0.25;
                }
                else if (st.ScaleX < 0 && st.ScaleX >= -5.0)
                {
                    st.ScaleX -= 0.25;
                    st.ScaleY += 0.25;
                }
                else
                {
                    //
                }
            }

            // 重新给图像赋值Transform变换属性
            imageControl.RenderTransform = tgnew;
        }

        /// <summary>
        /// 还原尺寸
        /// </summary>
        public void SetActualSize()
        {
            if (defaultTransformGroup != null)
            {
                imageControl.RenderTransform = defaultTransformGroup;
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            //// pdfViewerControl.ActualCommandProvider.ZoomOutCommand.Execute(null);
            TransformGroup tg = imageControl.RenderTransform as TransformGroup;
            var tgnew = tg.CloneCurrentValue();
            if (defaultTransformGroup == null)
            {
                defaultTransformGroup = tgnew.CloneCurrentValue();
            }
            if (tgnew != null)
            {
                ScaleTransform st = tgnew.Children[1] as ScaleTransform;
                imageControl.RenderTransformOrigin = new Point(0.25, 0.25);
                if (st.ScaleX >= 0.3)
                {
                    st.ScaleX -= 0.25;
                    st.ScaleY -= 0.25;
                }
                else if (st.ScaleX <= -0.3)
                {
                    st.ScaleX += 0.25;
                    st.ScaleY -= 0.25;
                }
                else
                {
                    //
                }
            }

            // 重新给图像赋值Transform变换属性
            imageControl.RenderTransform = tgnew;

        }

        /// <summary>
        /// 放大
        /// </summary>
        public void MagnifyAdd()
        {
            BtnZoomIn_Click(null, null);
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void MagnifyMinus()
        {
            BtnZoomOut_Click(null, null);

        }


        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClockwiseRotate_Click(object sender, RoutedEventArgs e)
        {
            TransformGroup tg = imageControl.RenderTransform as TransformGroup;
            var tgnew = tg.CloneCurrentValue();
            if (tgnew != null)
            {
                RotateTransform rt = tgnew.Children[2] as RotateTransform;
                imageControl.RenderTransformOrigin = new Point(0.5, 0.5);
                rt.Angle += 90;
            }

            // 重新给图像赋值Transform变换属性
            imageControl.RenderTransform = tgnew;

        }

        private Image movingObject;  // 记录当前被拖拽移动的图片
        private Point StartPosition; // 本次移动开始时的坐标点位置
        private Point EndPosition;   // 本次移动结束时的坐标点位置

        /// <summary>
        /// 按下鼠标左键，准备开始拖动图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDownCommand(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;

            movingObject = img;
            StartPosition = e.GetPosition(img);
        }

        /// <summary>
        /// 按住鼠标左键，拖动图片平移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMoveCommand(object sender, MouseEventArgs e)
        {
            Image img = sender as Image;

            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                EndPosition = e.GetPosition(img);

                TransformGroup tg = img.RenderTransform as TransformGroup;
                var tgnew = tg.CloneCurrentValue();
                if (tgnew != null)
                {
                    TranslateTransform tt = tgnew.Children[0] as TranslateTransform;

                    var X = EndPosition.X - StartPosition.X;
                    var Y = EndPosition.Y - StartPosition.Y;
                    tt.X += X;
                    tt.Y += Y;
                }

                // 重新给图像赋值Transform变换属性
                img.RenderTransform = tgnew;
            }
        }

        /// <summary>
        /// 鼠标左键弹起，结束图片的拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonUpCommand(object sender, MouseButtonEventArgs e)
        {
            movingObject = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public void UpdatePlayer(Document document)
        {
            this.ShowUI();
            this.LoadData(document.FilePath);
            this.Show(document.FileName);
        }

        public void Play()
        {
            // 播放图片
            // ...
        }

        public void Stop()
        {
            // 停止播放图片
            // ...
        }

        /// <summary>
        /// 
        /// </summary>
        public void ActualSize()
        {
            this.SetActualSize();
        }
        /// <summary>
        ///  添加显示界面的方法
        /// </summary>
        public void ShowUI()
        {
            this.Visibility = Visibility.Visible;
            IsShow = true;

        }

        /// <summary>
        /// 添加隐藏界面的方法
        /// </summary>
        public void HideUI()
        {
            this.Visibility = Visibility.Collapsed;
            IsShow = false;
        }
    }
}
