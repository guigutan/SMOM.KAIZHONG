namespace SIE.Packages.Packings.Strategys
{
    /// <summary>
    /// 扫描方式
    /// </summary>
    public abstract class ScanStrategy : IScanStrategy
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        protected ScanStrategy()
        {
        }

        /// <summary>
        /// 内部编码
        /// </summary>
        public virtual string InsideBarcode { get; protected set; }

        /// <summary>
        /// 输出包装关系
        /// </summary>
        public virtual PackingRelation OuterPackingRelation { get; set; }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="barcode">编码</param>
        public abstract void Read(string barcode);

        /// <summary>
        /// 重置
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 获取提示
        /// </summary>
        /// <returns>提示</returns>
        public abstract string GetTips();

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="o">对象</param>
        public abstract void SetConfig(object o);
    }
}
