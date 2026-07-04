namespace SIE.Wpf.ESop.DocumentTransform
{
    /// <summary>
    /// 文档转换接口
    /// </summary>
    public interface IDocumentConvert
    {
        /// <summary>
        /// 转换方法
        /// </summary>
        void Convert();

        /// <summary>
        /// 停止方法
        /// </summary>
        void Stop();
    }
}
