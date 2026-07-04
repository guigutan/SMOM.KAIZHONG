using SIE.ESop.Documents;

namespace SIE.Wpf.ESop.Displays
{
    /// <summary>
    /// Esop播放接口
    /// </summary>
    public interface IEsopPlay
    {
        /// <summary>
        /// 当前播放文档集
        /// </summary>
        Document CurrentPlayDocument { get; }

        /// <summary>
        /// 间隔
        /// </summary>
        int Interval { get; }

        /// <summary>
        /// 播放下一文档集
        /// </summary>
        void PlayNextDocument();

        /// <summary>
        /// 播放前一文档集
        /// </summary>
        void PlayPreviousDocument();
    }
}