using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 盘点资产对象
    /// </summary>
    public enum InventoryAssetObject
    {
        /// <summary>
        /// 设备
        /// </summary>
        [Label("设备")]
        Equipment = 10,
        /// <summary>
        /// 备件
        /// </summary>
        [Label("备件")]
        Spare = 20,
        /// <summary>
        /// 工治具
        /// </summary>
        [Label("工治具")]
        Fixture = 30,        
    }
}