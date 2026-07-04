using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 采购对象
    /// </summary>
    public enum PurchaseObjectType
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
        SparePart = 20,
        /// <summary>
        /// 辅料
        /// </summary>
        [Label("辅料")]
        Excipients = 30,
        /// <summary>
        /// 模具
        /// </summary>
        [Label("模具")]
        Mold = 40,
        /// <summary>
        /// 工治具
        /// </summary>
        [Label("工治具")]
        Fixture = 50,
        /// <summary>
        /// 工具
        /// </summary>
        [Label("工具")]
        Tool = 60,
        /// <summary>
        /// 委外维修
        /// </summary>
        [Label("委外维修")]
        OutsourcedRepair = 70,
        /// <summary>
        /// 委外保养
        /// </summary>
        [Label("委外保养")]
        OutsourcedMaintainance = 80,
        /// <summary>
        /// 委外定检
        /// </summary>
        [Label("委外定检")]
        OutsourcedRegularInspection = 90,
        /// <summary>
        /// 委外校准
        /// </summary>
        [Label("委外校准")]
        OutsourcedCalibration = 100,
        /// <summary>
        /// 委外安装
        /// </summary>
        [Label("委外安装")]
        OutsourcedInstall = 110,
        /// <summary>
        /// 工程
        /// </summary>
        [Label("工程")]
        Engineering = 120,
    }
}
