using System;

namespace SIE.EMS.SMDC.Equipments.Infos
{
    /// <summary>
    /// 信息
    /// </summary>
    [Serializable]
    public class EquipParaInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public String AssetsCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String MesModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String MesDeviceName { get; set; }
    }
}
