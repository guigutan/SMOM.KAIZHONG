using SIE.ObjectModel;

namespace SIE.WMS.Common
{
    /// <summary>
    /// 序列号管理模式
    /// </summary>
    public enum SerialModel
    {
        /// <summary>
        /// 位置跟踪模式
        /// </summary>
        [Label("位置跟踪模式")]
        Location = 0,

        /// <summary>
        /// 收发验证模式-2023-5-12取消收发验证模式
        /// </summary>
        //[Label("收发验证模式")]
        //Check = 1,
    }
}