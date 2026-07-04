using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 维修开始ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("维修开始")]
    //[DisplayMember(nameof())]
    public class RepairStartViewModel :ViewModel
    {
        #region 是否停机维修 IsStopMachineRepair
        /// <summary>
        /// 是否停机维修
        /// </summary>
        [Label("是否停机维修")]
        public static readonly Property<bool> IsStopMachineRepairProperty = P<RepairStartViewModel>.Register(e => e.IsStopMachineRepair);

        /// <summary>
        /// 是否停机维修
        /// </summary>
        public bool IsStopMachineRepair
        {
            get { return this.GetProperty(IsStopMachineRepairProperty); }
            set { this.SetProperty(IsStopMachineRepairProperty, value); }
        }
        #endregion

    }
}
