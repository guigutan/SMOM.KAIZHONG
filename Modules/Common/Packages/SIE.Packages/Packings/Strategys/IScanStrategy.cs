namespace SIE.Packages.Packings.Strategys
{
    /// <summary>
    /// 扫描类型
    /// </summary>
    public interface IScanStrategy
    {
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="barcode">编码</param>
        void Read(string barcode);

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="o">对象</param>
        void SetConfig(object o);
    }
}
