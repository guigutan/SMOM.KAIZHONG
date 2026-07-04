namespace SIE.Wpf.ESop.DocumentTransform
{
    /// <summary>
    /// 文档处理任务接口
    /// </summary>
    public interface IProcessTask
    {
        /// <summary>
        /// 继续
        /// </summary>
        void Continue();

        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();

        /// <summary>
        /// 开始
        /// </summary>
        void Star();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
    }
}
