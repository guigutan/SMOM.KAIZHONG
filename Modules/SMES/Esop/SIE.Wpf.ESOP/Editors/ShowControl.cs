/*
 该类已过时。请使用ESOPFactory目录代码 2023/12/27
 */
using SIE.ESop.Documents;
using SIE.Wpf.ESop.Displays;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SIE.Wpf.ESop.Editors
{
    /// <summary>
    /// ShowControl布局控件
    /// </summary>
    public class ShowControl : Grid, IDisposable
    {
        /// <summary>
        /// 播放编辑器
        /// </summary>
        public ESopViewModel PlayEditor { get; set; }

        private const string ACTUAL_WIDTH = "ActualWidth";
        private const string ACTUAL_HEIGHT = "ActualHeight";

        /// <summary>
        /// 计时器
        /// </summary>
        private DispatcherTimer _playTimer = new DispatcherTimer();

        ///// <summary>
        ///// 图片对象
        ///// </summary>
        ////private Image simpleImage = new Image();

        /// <summary>
        /// 图片查看器
        /// </summary>
        private readonly ImagePlayer imagePlayer = new ImagePlayer();

        /// <summary>
        /// 多媒体对象
        /// </summary>
        private readonly MediaPlayer mediaElement = new MediaPlayer();

        /// <summary>
        /// EXCEL播放器
        /// </summary>
        private readonly ExcelPlayer excelPlayer = new ExcelPlayer() { IsEnabled = true };

        /// <summary>
        /// PDF对象
        /// </summary>
        private readonly PdfPlayer pdfPlayer = new PdfPlayer();

        /// <summary>
        /// word显示器
        /// </summary>
        private readonly WordPlayer wordPlayer = new WordPlayer();

        /// <summary>
        /// 是否已经释放数据
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// 记录离上一次间隔多少秒
        /// </summary>
        private int Tick { get; set; }

        /// <summary>
        /// 当前播放文档集
        /// </summary>
        public static readonly DependencyProperty CurrentPlayDocumentProperty = DependencyProperty.Register("CurrentPlayDocument", typeof(Document), typeof(ShowControl), new FrameworkPropertyMetadata()
        {
            PropertyChangedCallback = (d, e) => (d as ShowControl).CurrentPlayDocumentChanged(),
        });

        /// <summary>
        /// 当前播放文档集
        /// </summary>
        public Document CurrentPlayDocument
        {
            get { return (Document)GetValue(CurrentPlayDocumentProperty); }
            set { SetValue(CurrentPlayDocumentProperty, value); }
        }

        /// <summary>
        /// 构造函数（初始化simpleImage、mediaElement、excelPlayer）
        /// </summary>
        public ShowControl()
        {
            ShowControl owner = this;
            mediaElement.Visibility = Visibility.Collapsed;
            excelPlayer.Visibility = Visibility.Collapsed;
            pdfPlayer.Visibility = Visibility.Collapsed;
            wordPlayer.Visibility = Visibility.Collapsed;
            imagePlayer.Visibility = Visibility.Collapsed;

            mediaElement.DataContext = owner;
            mediaElement.SetBinding(FrameworkElement.WidthProperty, ACTUAL_WIDTH);
            mediaElement.SetBinding(FrameworkElement.HeightProperty, ACTUAL_HEIGHT);
            excelPlayer.DataContext = owner;
            excelPlayer.SetBinding(FrameworkElement.HeightProperty, ACTUAL_HEIGHT);
            excelPlayer.SetBinding(FrameworkElement.WidthProperty, ACTUAL_WIDTH);

            pdfPlayer.DataContext = owner;
            pdfPlayer.SetBinding(FrameworkElement.HeightProperty, ACTUAL_HEIGHT);
            pdfPlayer.SetBinding(FrameworkElement.WidthProperty, ACTUAL_WIDTH);

            wordPlayer.DataContext = owner;
            wordPlayer.SetBinding(FrameworkElement.HeightProperty, ACTUAL_HEIGHT);
            wordPlayer.SetBinding(FrameworkElement.WidthProperty, ACTUAL_WIDTH);

            imagePlayer.DataContext = owner;
            imagePlayer.SetBinding(FrameworkElement.HeightProperty, ACTUAL_HEIGHT);
            imagePlayer.SetBinding(FrameworkElement.WidthProperty, ACTUAL_WIDTH);
            Children.Add(mediaElement);
            Children.Add(excelPlayer);
            Children.Add(pdfPlayer);
            Children.Add(wordPlayer);
            Children.Add(imagePlayer);
           
            _playTimer.Tick += (x, y) =>
            {
                if (CurrentPlayDocument == null || PlayEditor == null)
                {
                    return;
                }

                Tick++;
                if (Tick >= PlayEditor.Interval)
                {
                    Tick = 0;
                    if (CurrentPlayDocument.DocumentType == DocumentType.Video)
                    {
                        mediaElement.Play();
                        return;
                    }

                    Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        PlayEditor.PlayNextDocument();
                    }));
                }
            };
        }

        /// <summary>
        /// 计时器开始，播放
        /// </summary>
        public void Play()
        {
            _playTimer.Interval = new TimeSpan(0, 0, 1);
            Tick = 0;
            _playTimer.Start();
        }

        /// <summary>
        /// 计时器停止，停止播放
        /// </summary>
        public void Stop()
        {
            _playTimer.Stop();
            mediaElement.Pause();
            Tick = 0;
        }

        /// <summary>
        /// 上一个
        /// </summary>
        public void Previous()
        {
            Tick = 0;
            PlayEditor.PlayPreviousDocument();
        }

        /// <summary>
        /// 下一个
        /// </summary>
        public void Next()
        {
            Tick = 0;
            PlayEditor.PlayNextDocument();
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void MagnifyAdd()
        {
            if (imagePlayer.Visibility == Visibility.Visible)
            {
                imagePlayer.BtnZoomIn_Click(null, null);
            }

            if (pdfPlayer.Visibility == Visibility.Visible)
            {
                pdfPlayer.BtnZoomIn_Click(null, null);
            }
            if (wordPlayer.Visibility == Visibility.Visible)
            {
                wordPlayer.BtnZoomIn_Click(null, null);
            }
            if (excelPlayer.Visibility == Visibility.Visible)
            {
                excelPlayer.BtnZoomIn_Click(null, null);
            }
            if (mediaElement.Visibility == Visibility.Visible)
            {
                mediaElement.BtnZoomIn_Click(null, null);
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void MagnifyMinus()
        {
            if (imagePlayer.Visibility == Visibility.Visible)
            {
                imagePlayer.BtnZoomOut_Click(null, null);
            }
            if (pdfPlayer.Visibility == Visibility.Visible)
            {
                pdfPlayer.BtnZoomOut_Click(null, null);
            }
            if (wordPlayer.Visibility == Visibility.Visible)
            {
                wordPlayer.BtnZoomOut_Click(null, null);
            }
            if (excelPlayer.Visibility == Visibility.Visible)
            {
                excelPlayer.BtnZoomOut_Click(null, null);
            }
            if (mediaElement.Visibility == Visibility.Visible)
            {
                mediaElement.BtnZoomOut_Click(null, null);
            }
        }

        /// <summary>
        /// 当前播放文档集变更时触发
        /// </summary>
        void CurrentPlayDocumentChanged()
        {
            if (CurrentPlayDocument == null)
            {
                return;
            }

            if (CurrentPlayDocument.DocumentType == DocumentType.Img)
            {
                ////ShowImg();
                ////simpleImage.Visibility = Visibility.Collapsed;
                mediaElement.Visibility = Visibility.Collapsed;
                excelPlayer.Visibility = Visibility.Collapsed;
                pdfPlayer.Visibility = Visibility.Collapsed;
                wordPlayer.Visibility = Visibility.Collapsed;
                imagePlayer.Visibility = Visibility.Visible;
                imagePlayer.LoadData(CurrentPlayDocument.FilePath);
                imagePlayer.Show(CurrentPlayDocument.FileName);
            }
            else if (CurrentPlayDocument.DocumentType == DocumentType.Video)
            {
                PlayVideo();
            }
            else if (CurrentPlayDocument.DocumentType == DocumentType.Document)
            {
                if (CurrentPlayDocument.FileExtension.ToLower().Contains("xlsx") || CurrentPlayDocument.FileExtension.ToLower().Contains("xls"))
                {
                    ////simpleImage.Visibility = Visibility.Collapsed;
                    mediaElement.Visibility = Visibility.Collapsed;
                    pdfPlayer.Visibility = Visibility.Collapsed;
                    imagePlayer.Visibility = Visibility.Collapsed;
                    wordPlayer.Visibility = Visibility.Collapsed;
                    excelPlayer.Visibility = Visibility.Visible;
                    excelPlayer.LoadData(new Uri(CurrentPlayDocument.FilePath));
                    excelPlayer.Show(CurrentPlayDocument.FileName);
                }
                if (CurrentPlayDocument.FileExtension.ToLower().Contains("pdf"))
                {
                    ////simpleImage.Visibility = Visibility.Collapsed;
                    mediaElement.Visibility = Visibility.Collapsed;
                    wordPlayer.Visibility = Visibility.Collapsed;
                    excelPlayer.Visibility = Visibility.Collapsed;
                    imagePlayer.Visibility = Visibility.Collapsed;
                    pdfPlayer.Visibility = Visibility.Visible;
                    pdfPlayer.LoadData(new Uri(CurrentPlayDocument.FilePath));
                    pdfPlayer.Show(CurrentPlayDocument.FileName);
                }
                if (CurrentPlayDocument.FileExtension.ToLower().Contains("docx"))
                {
                    ////simpleImage.Visibility = Visibility.Collapsed;
                    mediaElement.Visibility = Visibility.Collapsed;
                    excelPlayer.Visibility = Visibility.Collapsed;
                    imagePlayer.Visibility = Visibility.Collapsed;
                    pdfPlayer.Visibility = Visibility.Collapsed;
                    wordPlayer.Visibility  = Visibility.Visible;

                    wordPlayer.LoadData(new Uri(CurrentPlayDocument.FilePath));
                    wordPlayer.Show(CurrentPlayDocument.FileName);

                }
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        void PlayVideo()
        {
            wordPlayer.Visibility = Visibility.Collapsed;
            excelPlayer.Visibility = Visibility.Collapsed;
            imagePlayer.Visibility = Visibility.Collapsed;
            pdfPlayer.Visibility = Visibility.Collapsed;
            mediaElement.Visibility = Visibility.Visible;
            mediaElement.MediaElement_MediaEnded=MediaElement_MediaEnded;
            mediaElement.SetSource(new Uri(CurrentPlayDocument.FilePath));
            mediaElement.Play();
        }

        /// <summary>
        /// 视频结束时触发
        /// </summary>
        /// <param name="sender">当前多媒体控件</param>
        /// <param name="e">事件参数</param>
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Tick = 0;
            mediaElement.Stop();
            var data = PlayEditor;
            data.PlayNextDocument();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放计时器</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _playTimer.Stop();
                    _playTimer = null;
                }

                //// TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                //// TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        /// <summary>
        /// TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        /// ~ShowControl() {
        ///   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        ///   Dispose(false);
        /// }
        /// </summary>
        public void Dispose()
        {
            //// 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }

       /// <summary>
       /// 还原初始大小
       /// </summary>
        public void ActualSize()
        {
            if (imagePlayer.Visibility == Visibility.Visible)
                imagePlayer.SetActualSize();
            if (pdfPlayer.Visibility == Visibility.Visible)
                pdfPlayer.SetActualSize();
            if (wordPlayer.Visibility == Visibility.Visible)
              wordPlayer.SetActualSize();
            if (excelPlayer.Visibility == Visibility.Visible)
                excelPlayer.SetActualSize();
            if (mediaElement.Visibility == Visibility.Visible)
                mediaElement.SetActualSize();
            
        }
    }
}