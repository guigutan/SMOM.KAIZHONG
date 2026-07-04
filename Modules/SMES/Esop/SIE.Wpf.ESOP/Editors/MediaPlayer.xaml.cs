using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using SIE.ESop.Documents;
using SIE.Wpf.ESOP.ESOPFactory;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.ESop.Editors
{
    /// <summary>
    /// PdfPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class MediaPlayer : UserControl, IPlayer
    {
        /// <summary>
        /// 记录当前文件路径
        /// </summary>
        public Uri CurrentUri { get; private set; }


        /// <summary>
        /// 默认尺寸
        /// </summary>
        private Size defaultSize = new Size(0, 0);

        /// <summary>
        /// 记录当前文件MD5加密内容
        /// </summary>
        private string MD5 { get; set; } = string.Empty;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        private FactoryShowControl ShowControl { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MediaPlayer()
        {
            InitializeComponent();
            MediaPlayer owner = this;
            mediaViewerControl.DataContext = owner;
            mediaViewerControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            mediaViewerControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            mediaViewerControl.Visibility = Visibility.Visible;
            mediaViewerControl.SizeChanged += MediaViewerControl_SizeChanged;
            mediaViewerControl.Loaded += MediaViewerControl_Loaded;
            mediaViewerControl.IsVisibleChanged += MediaViewerControl_IsVisibleChanged;
            mediaViewerControl.OverridesDefaultStyle = true;
        }

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="showControl"></param>
        public MediaPlayer(FactoryShowControl showControl)
        {
            InitializeComponent();
            MediaPlayer owner = this;
            mediaViewerControl.DataContext = owner;
            mediaViewerControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            mediaViewerControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            mediaViewerControl.Visibility = Visibility.Visible;
            mediaViewerControl.SizeChanged += MediaViewerControl_SizeChanged;
            mediaViewerControl.Loaded += MediaViewerControl_Loaded;
            mediaViewerControl.IsVisibleChanged += MediaViewerControl_IsVisibleChanged;
            mediaViewerControl.OverridesDefaultStyle = true;
            this.ShowControl = showControl;
            this.DataContext = showControl;
            foreach (var ctr in showControl.Children)
            {
                var child = ctr as MediaPlayer;
                if (child != null)
                {
                    return;
                }
            }
            showControl.Children.Add(owner);
            MediaEnded();
        }

        /// <summary>
        /// 控件初始化加载数据
        /// </summary>
        /// <param name="sender">当前初始化控件</param>
        /// <param name="e">路由事件参数</param>
        private void MediaViewerControl_Loaded(object sender, RoutedEventArgs e)
        {
            MatchUseRangeSize();
            ZoomUseRange();
            this.mediaViewerControl.Stretch = System.Windows.Media.Stretch.Uniform;
            this.mediaViewerControl.LoadedBehavior = MediaState.Manual;
        }

        /// <summary>
        /// 控件可视化变化时触发
        /// </summary>
        /// <param name="sender">当前控件</param>
        /// <param name="e">事件参数</param>
        private void MediaViewerControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 控件尺寸改变时触发
        /// </summary>
        /// <param name="sender">改变的控件对象</param>
        /// <param name="e">尺寸大小改变参数</param>
        private void MediaViewerControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ZoomUseRange();
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

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (this.mediaViewerControl.HasVideo || this.mediaViewerControl.HasAudio)
            {
                this.Play();
            }

        }

        private void BtnHand_Click(object sender, RoutedEventArgs e)
        {
            if (this.mediaViewerControl.HasVideo || this.mediaViewerControl.HasAudio)
            {
                this.Pause();
            }
        }

        /// <summary>
        /// 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (this.viewBox.Width.IsNaN())
            {
                defaultSize = new Size(this.viewBox.ActualWidth, this.viewBox.ActualHeight);
                this.viewBox.SetSize(new Size(this.viewBox.ActualWidth + 50, this.viewBox.ActualHeight + 50));
            }
            this.viewBox.Width += 50;
            this.viewBox.Height += 25;

        }

        /// <summary>
        /// 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (this.viewBox.Width.IsNaN())
            {
                defaultSize = new Size(this.viewBox.ActualWidth, this.viewBox.ActualHeight);
                this.viewBox.SetSize(new Size(this.viewBox.ActualWidth - 50, this.viewBox.ActualHeight - 50));
            }
            if (this.viewBox.Width >= 400 && this.viewBox.Height >= 300)//避免出现0或负数
            {
                this.viewBox.Width -= 50;
                this.viewBox.Height -= 25;
            }
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
        /// 还原
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void SetActualSize()
        {
            if (defaultSize.Width != 0 && defaultSize.Height != 0)
            {
                this.viewBox.Width = defaultSize.Width;
                this.viewBox.Height = defaultSize.Height;
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            this.mediaViewerControl.Play();

        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            this.mediaViewerControl.Pause();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            this.mediaViewerControl.Stop();
        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="uri"></param>
        internal void SetSource(Uri uri)
        {
            this.mediaViewerControl.Source = uri;
        }

        /// <summary>
        /// 播放结束事件
        /// </summary>
        internal Action<object, RoutedEventArgs> MediaElement_MediaEnded;
        internal void MediaEnded()
        {
            this.mediaViewerControl.MediaEnded -= MediaViewerControl_MediaEnded;
            this.mediaViewerControl.MediaEnded += MediaViewerControl_MediaEnded;
        }

        private void MediaViewerControl_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (MediaElement_MediaEnded != null)
                MediaElement_MediaEnded(sender, e);
        }
        /// <summary>
        /// 还原
        /// </summary>
        public void ActualSize()
        {
            if (IsShow)
            {
                this.SetActualSize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public void UpdatePlayer(Document document)
        {
            this.ShowUI();
            this.MediaElement_MediaEnded = this.ShowControl.MediaElement_MediaEnded;
            this.SetSource(new Uri(document.FilePath));
            this.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowUI()
        {
            this.Visibility = Visibility.Visible;
            IsShow = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideUI()
        {
            this.Visibility = Visibility.Collapsed;
            IsShow = false;
        }

        void IPlayer.Play()
        {
            this.Play();
        }

        void IPlayer.Stop()
        {
            this.Stop();
        }

        private void btnFast_Click(object sender, RoutedEventArgs e)
        {
            if (this.mediaViewerControl.SpeedRatio <= 4)
                this.mediaViewerControl.SpeedRatio = this.mediaViewerControl.SpeedRatio * 2;
        }

        private void btnSlow_Click(object sender, RoutedEventArgs e)
        {
            if (this.mediaViewerControl.SpeedRatio >= 0.125)
                this.mediaViewerControl.SpeedRatio = this.mediaViewerControl.SpeedRatio / 2;
        }

        private void btnOriginal_Click(object sender, RoutedEventArgs e)
        {
            this.mediaViewerControl.SpeedRatio = 1d;
        }
    }
}
