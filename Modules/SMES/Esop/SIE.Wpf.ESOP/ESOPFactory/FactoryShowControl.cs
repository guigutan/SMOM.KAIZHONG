using SIE.ESop.Documents;
using SIE.Wpf.ESop.Displays;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SIE.Wpf.ESOP.ESOPFactory
{
    /// <summary>
    /// ShowControl布局控件
    /// </summary>
    public class FactoryShowControl : Grid, IDisposable
    {
        /// <summary>
        ///图片播放器
        /// </summary>
        private readonly IPlayer imagePlayer;

        /// <summary>
        /// 媒体播放器
        /// </summary>
        private readonly IPlayer mediaPlayer;

        /// <summary>
        /// 表格文档展示
        /// </summary>
        private readonly IPlayer excelPlayer;

        /// <summary>
        /// PDF文档展示
        /// </summary>
        private readonly IPlayer pdfPlayer;

        /// <summary>
        /// word文档展示
        /// </summary>
        private readonly IPlayer wordPlayer;

        /// <summary>
        /// 播放编辑器
        /// </summary>
        public ESopViewModel PlayEditor { get; set; }

        /// <summary>
        /// 记录离上一次间隔多少秒
        /// </summary>
        private int Tick { get; set; }

        /// <summary>
        /// 定时器
        /// </summary>
        private DispatcherTimer _playTimer = new DispatcherTimer();

        /// <summary>
        /// 文档播放器集合
        /// </summary>
        private readonly List<IPlayer> playerList = new List<IPlayer>();

        /// <summary>
        /// 当前播放文档集
        /// </summary>
        public static readonly DependencyProperty CurrentPlayDocumentProperty = DependencyProperty.Register("CurrentPlayDocument", typeof(Document), typeof(FactoryShowControl), new FrameworkPropertyMetadata()
        {
            PropertyChangedCallback = (d, e) => (d as FactoryShowControl).CurrentPlayDocumentChanged(),
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
        /// 是否已经释放数据
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// 构造函数（初始化simpleImage、mediaElement、excelPlayer）
        /// </summary>
        public FactoryShowControl()
        {
            imagePlayer = PlayerFactory.CreatePlayer(this, DocumentType.Img);
            mediaPlayer = PlayerFactory.CreatePlayer(this, DocumentType.Video);
            excelPlayer = PlayerFactory.CreatePlayer(this, DocumentType.Document, DocumentExtension.xlsx);
            pdfPlayer = PlayerFactory.CreatePlayer(this, DocumentType.Document, DocumentExtension.pdf);
            wordPlayer = PlayerFactory.CreatePlayer(this, DocumentType.Document, DocumentExtension.docx);
            playerList = new List<IPlayer>() { imagePlayer, mediaPlayer, excelPlayer, pdfPlayer, wordPlayer };
            _playTimer.Tick += (x, y) =>
            {
                if (CurrentPlayDocument == null || this.PlayEditor == null)
                {
                    return;
                }

                Tick++;
                if (Tick >= this.PlayEditor.Interval)
                {
                    Tick = 0;
                    if (CurrentPlayDocument.DocumentType == DocumentType.Video)
                    {
                        mediaPlayer.UpdatePlayer(CurrentPlayDocument);
                        return;
                    }

                    Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.PlayEditor.PlayNextDocument();
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
            mediaPlayer.Stop();
            Tick = 0;
        }

        /// <summary>
        /// 上一个
        /// </summary>
        public void Previous()
        {
            Tick = 0;
            this.PlayEditor.PlayPreviousDocument();
        }

        /// <summary>
        /// 下一个
        /// </summary>
        public void Next()
        {
            Tick = 0;
            this.PlayEditor.PlayNextDocument();
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void MagnifyAdd()
        {
            try
            {
                foreach (var item in playerList)
                {
                    if (item.IsShow)
                    {
                        item.MagnifyAdd();
                    }
                }
            }catch (Exception ex) {
            
            //to do
            }

        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void MagnifyMinus()
        {
            try
            {
                foreach (var item in playerList)
                {
                    if (item.IsShow)
                    {
                        item.MagnifyMinus();
                    }
                }
            }
            catch (Exception ex) {
                //to do
            }
        }

        /// <summary>
        /// 还原初始大小
        /// </summary>
        public void ActualSize()
        {
            try
            {
                foreach (var item in playerList)
                {
                    item.ActualSize();
                }
            }
            catch (Exception ex) {
                //to do
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

            switch (CurrentPlayDocument.DocumentType)
            {
                case DocumentType.Img:
                    ShowCurrentPlayer(imagePlayer);
                    imagePlayer.UpdatePlayer(CurrentPlayDocument);
                    break;
                case DocumentType.Video:
                    ShowCurrentPlayer(mediaPlayer);
                    mediaPlayer.UpdatePlayer(CurrentPlayDocument);
                    break;
                case DocumentType.Document:
                    var fileExtension = CurrentPlayDocument.FileExtension.ToLower();
                    if (fileExtension.Contains("xlsx") || fileExtension.Contains("xls"))
                    {
                        UpdateShowPlayerDocument(excelPlayer);
                    }
                    if (fileExtension.Contains("pdf"))
                    {
                        UpdateShowPlayerDocument(pdfPlayer);
                    }
                    if (fileExtension.Contains("docx"))
                    {
                        UpdateShowPlayerDocument(wordPlayer);
                    }
                    break;
            }
        }

        /// <summary>
        /// 更新显示的文档
        /// </summary>
        private void UpdateShowPlayerDocument(IPlayer player)
        {
            ShowCurrentPlayer(player);
            player.UpdatePlayer(CurrentPlayDocument);

        }

        /// <summary>
        /// 显示当前播放器
        /// </summary>
        /// <param name="player"></param>
        void ShowCurrentPlayer(IPlayer player)
        {
            player.ShowUI();
            foreach (var item in playerList)
            {
                if (item != player)
                {
                    item.HideUI();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Tick = 0;
            mediaPlayer.Stop();
            var data = this.PlayEditor;
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
    }
}
