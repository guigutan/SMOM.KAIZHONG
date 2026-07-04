using SIE.ObjectModel;

namespace SIE.Wpf.MES.BatchWIP.Assemblys
{
    /// <summary>
    /// 扫描方式
    /// </summary>
    public enum ScanMode
    {
        /// <summary>
        /// 转入
        /// </summary>
        [Label("转入")]
        Input,

        /// <summary>
        /// 转出
        /// </summary>
        [Label("转出")]
        Output,

        /// <summary>
        /// 上料
        /// </summary>
        [Label("上料")]
        LoadItem,
    }
}
