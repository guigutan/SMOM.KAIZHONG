using SIE.ObjectModel;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区类型
    /// </summary>
    public enum StorageAreaType
    {
        /// <summary>
        /// 投入
        /// </summary>
        [Label("投入")]
        Input,

        /// <summary>
        /// 产出
        /// </summary>
        [Label("产出")]
        Output,
    }
}