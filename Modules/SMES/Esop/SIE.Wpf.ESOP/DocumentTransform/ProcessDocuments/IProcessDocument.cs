namespace SIE.Wpf.ESop.DocumentTransform.ProcessDocuments
{
    /// <summary>
    /// 文档处理接口
    /// </summary>
    public interface IProcessDocument
    {
        /// <summary>
        /// 处理文档方法
        /// </summary>
        void Process();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
    }
}
