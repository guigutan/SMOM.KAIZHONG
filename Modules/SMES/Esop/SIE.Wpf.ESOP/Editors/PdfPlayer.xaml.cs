using DevExpress.Xpf.PdfViewer;
using SIE.Common.Utils;
using SIE.ESop.Documents;
using SIE.Wpf.ESOP.ESOPFactory;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.ESop.Editors
{
    /// <summary>
    /// PdfPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class PdfPlayer : UserControl,IPlayer
    {
        /// <summary>
        /// 记录当前文件路径
        /// </summary>
        public Uri CurrentUri { get; private set; }

        /// <summary>
        /// 记录当前文件MD5加密内容
        /// </summary>
        private string MD5 { get; set; } = string.Empty;

        /// <summary>
        ///是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PdfPlayer()
        {
            InitializeComponent();
            PdfPlayer owner = this;
            pdfViewerControl.DataContext = owner;
            pdfViewerControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            pdfViewerControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            pdfViewerControl.Visibility = Visibility.Visible;
            pdfViewerControl.SizeChanged += PdfViewerControl_SizeChanged;
            pdfViewerControl.Loaded += PdfViewerControl_Loaded;
            pdfViewerControl.IsVisibleChanged += PdfViewerControl_IsVisibleChanged;
            pdfViewerControl.OverridesDefaultStyle = true;

            pdfViewerControl.ContextMenuOpening += PdfViewerControl_ContextMenuOpening;
            pdfViewerControl.CommandBarStyle = DevExpress.Xpf.DocumentViewer.CommandBarStyle.None;

        }
        /// <summary>
        /// 带构造参数
        /// </summary>
        /// <param name="showControl"></param>

        public PdfPlayer(FactoryShowControl  showControl)
        {
            InitializeComponent();
            PdfPlayer owner = this;
            pdfViewerControl.DataContext = owner;
            pdfViewerControl.SetBinding(FrameworkElement.WidthProperty, "ActualWidth");
            pdfViewerControl.SetBinding(FrameworkElement.HeightProperty, "ActualHeight");
            pdfViewerControl.Visibility = Visibility.Visible;
            pdfViewerControl.SizeChanged += PdfViewerControl_SizeChanged;
            pdfViewerControl.Loaded += PdfViewerControl_Loaded;
            pdfViewerControl.IsVisibleChanged += PdfViewerControl_IsVisibleChanged;
            pdfViewerControl.OverridesDefaultStyle = true;

            pdfViewerControl.ContextMenuOpening += PdfViewerControl_ContextMenuOpening;
            pdfViewerControl.CommandBarStyle = DevExpress.Xpf.DocumentViewer.CommandBarStyle.None;
            this.DataContext = showControl;
            this.HideUI();
            foreach (var ctr in showControl.Children)
            {
                var child = ctr as PdfPlayer;
                if (child != null)
                {
                    return;
                }
            }
            showControl.Children.Add(owner);

        }
        


        private void PdfViewerControl_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            //// var pdfViewer = sender as PdfViewerControl;
        }

        /// <summary>
        /// 控件初始化加载数据
        /// </summary>
        /// <param name="sender">当前初始化控件</param>
        /// <param name="e">路由事件参数</param>
        private void PdfViewerControl_Loaded(object sender, RoutedEventArgs e)
        {
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 控件可视化变化时触发
        /// </summary>
        /// <param name="sender">当前控件</param>
        /// <param name="e">事件参数</param>
        private void PdfViewerControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MatchUseRangeSize();
            ZoomUseRange();
        }

        /// <summary>
        /// 控件尺寸改变时触发
        /// </summary>
        /// <param name="sender">改变的控件对象</param>
        /// <param name="e">尺寸大小改变参数</param>
        private void PdfViewerControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ZoomUseRange();
        }

        /// <summary>
        /// 根据路径加载文件
        /// </summary>
        /// <param name="uri">路径</param>
        public void LoadData(Uri uri)
        {
            var md5 = FileHelper.ComputeHash(new FileInfo(uri.OriginalString));
            if (uri.OriginalString == CurrentUri?.OriginalString && md5 == MD5) return;
            pdfViewerControl.OpenDocument(uri.OriginalString);
            CurrentUri = uri;
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
            pdfViewerControl.CursorMode = CursorModeType.SelectTool;
        }

        private void BtnHand_Click(object sender, RoutedEventArgs e)
        {
            pdfViewerControl.CursorMode = CursorModeType.HandTool;
        }

        /// <summary>
        /// 放大事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            pdfViewerControl.ActualCommandProvider.ZoomInCommand.Execute(null);
        }

        /// <summary>
        /// 缩小事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            pdfViewerControl.ActualCommandProvider.ZoomOutCommand.Execute(null);
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
        /// 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClockwiseRotate_Click(object sender, RoutedEventArgs e)
        {
            pdfViewerControl.ActualCommandProvider.ClockwiseRotateCommand.Execute(null);
        }

        /// <summary>
        /// 还原
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void SetActualSize()
        {
            pdfViewerControl.ZoomMode = DevExpress.Xpf.DocumentViewer.ZoomMode.ActualSize;// 设置为实际大小比缩放模式
            pdfViewerControl.ZoomFactor = 1.0f; // 设置缩放比例为100%
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public void UpdatePlayer(Document document)
        {
            this.ShowUI();
            this.LoadData(new Uri(document.FilePath));
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
