using SIE.ESop.Documents;

namespace SIE.Wpf.ESOP.ESOPFactory
{
    /// <summary>
    /// 文档播放器接口
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 更新播放内容
        /// </summary>
        /// <param name="document"></param>
        void UpdatePlayer(Document document);
        /// <summary>
        /// 播放
        /// </summary>
        void Play();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 放大
        /// </summary>
        void MagnifyAdd();

        /// <summary>
        /// 缩小
        /// </summary>
        void MagnifyMinus();

        /// <summary>
        /// 还原
        /// </summary>
        void ActualSize();

        /// <summary>
        ///  添加显示界面的方法
        /// </summary>
        void ShowUI();

        /// <summary>
        /// 添加隐藏界面的方法
        /// </summary>
        void HideUI();
    }
}
