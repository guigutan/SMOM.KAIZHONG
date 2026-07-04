using SIE.ObjectModel;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 资产类型
    /// </summary>
    public enum AssetsType
    {
        /// <summary>
        /// 设备
        /// </summary>
        [Label("设备")]
        Equipment = 5,
        /// <summary>
        /// 备件
        /// </summary>
        [Label("备件")]
        SpareParts = 10,
        /// <summary>
        /// 工治具
        /// </summary>
        [Label("工治具")]
        ToolsFixtures = 15,
        /// <summary>
        /// 模具
        /// </summary>
        [Label("模具")]
        Molds = 20,

        /// <summary>
        /// IT类
        /// </summary>
        [Label("IT类")]
        IT = 25,

        /// <summary>
        /// 办公类
        /// </summary>
        [Label("办公类")]
        Office = 30,

        /// <summary>
        /// 其他
        /// </summary>
        [Label("其他")]
        Other
    }
}