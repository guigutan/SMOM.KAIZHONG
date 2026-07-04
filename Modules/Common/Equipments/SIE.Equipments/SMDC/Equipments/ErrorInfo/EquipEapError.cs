using System;

namespace SIE.EMS.SMDC.Equipments.ErrorInfo
{
    /// <summary>
    /// 信息
    /// </summary>
    [Serializable]
    public class EquipEapError
    {  /// <summary>
       /// 信息
       /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object InnerError { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
    }
}
