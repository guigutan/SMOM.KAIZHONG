using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentAcceptances.ViewModels
{
    /// <summary>
    /// 设备验收设备明细视图
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备验收设备明细视图")]
    [DisplayMember(nameof(EquipmentCode))]
    public class EquipmentAcceptanceDetailViewModel : ViewModel
    {
        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty
            = P<EquipmentAcceptanceDetailViewModel>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return this.GetProperty(EquipmentCodeProperty); }
            set { this.SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

    }
}
