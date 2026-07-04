using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// SCADA设备信息
    /// </summary>
    [Serializable]
    public class EquipAccountData
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }

    }
}