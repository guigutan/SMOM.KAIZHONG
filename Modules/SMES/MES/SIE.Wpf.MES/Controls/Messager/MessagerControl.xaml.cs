using Stimulsoft.Report;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Controls.Messager
{
    /// <summary>
    /// Message.xaml 的交互逻辑
    /// </summary>
    public partial class MessagerControl : UserControl, INotifyPropertyChanged
    {
        const int MESSAGE_COUNT = 80;//容纳消息记录数

        /// <summary>
        /// 
        /// </summary>
        public MessagerControl()
        {
            this.DataContext = this;

            InitializeComponent();
        }

        #region 命令
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessageList = new ObservableCollection<MessageModel>();
        }

        #endregion

        #region ErrorMessageList 历史的消息
        ObservableCollection<MessageModel> errorMessageList;

        /// <summary>
        /// 消息
        /// </summary>
        public ObservableCollection<MessageModel> ErrorMessageList
        {
            get
            {
                if (errorMessageList == null)
                {
                    errorMessageList = new ObservableCollection<MessageModel>();
                }
                return errorMessageList;
            }
            set
            {
                errorMessageList = value;
                OnPropertyChanged("ErrorMessageList");
            }
        }


        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="messsagetype"></param>
        public void AddMessage(string messageText, MessageType messsagetype)
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                ErrorMessageList.Insert(0, new MessageModel()
                {
                    Body = string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), messageText),
                    Type = messsagetype,
                });

                if (MESSAGE_COUNT < ErrorMessageList.Count)
                {
                    ErrorMessageList.RemoveAt(ErrorMessageList.Count - 1);
                }
            }), null);
        }
        #endregion

        #region INotifyPropertyChanged Members  
        /// <summary>
        /// 属性变更
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性变更通知
        /// </summary>
        /// <param name="txt"></param>
        public void OnPropertyChanged(string txt)
        {
            PropertyChangedEventHandler handle = PropertyChanged;
            if (handle != null)
            {
                handle(this, new PropertyChangedEventArgs(txt));
            }
        }
        #endregion

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            string combinedString = string.Join(Environment.NewLine, errorMessageList.Select(x => x.Body));

            Clipboard.SetText(combinedString);
        }
    }
}
