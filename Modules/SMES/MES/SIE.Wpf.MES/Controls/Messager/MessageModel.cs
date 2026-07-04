namespace SIE.Wpf.MES.Controls.Messager
{
    /// <summary>
    /// 过站消息
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 消息类别
        /// </summary>
        public MessageType Type { get; set; }
    }
}
