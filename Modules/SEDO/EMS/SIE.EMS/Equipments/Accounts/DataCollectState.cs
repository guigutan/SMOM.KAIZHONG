using SIE.ObjectModel;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 数采状态
    /// </summary>
    public enum DataCollectState
    {
        /// <summary>
        /// 开机
        /// </summary>
        [Label("开机")]
        On = 0,
        /// <summary>
        /// 停机
        /// </summary>
        [Label("停机")]
        Off = 1,
    }
}