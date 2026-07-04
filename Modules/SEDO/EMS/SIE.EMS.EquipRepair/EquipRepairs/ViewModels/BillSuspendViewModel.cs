using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 单据暂停ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("单据暂停")]
    //[DisplayMember(nameof())]
    public class BillSuspendViewModel : ViewModel
    {

        #region 暂停原因 SuspendReason
        /// <summary>
        /// 暂停原因
        /// </summary>
        [Label("暂停原因")]
        public static readonly Property<string> SuspendReasonProperty = P<BillSuspendViewModel>.Register(e => e.SuspendReason);

        /// <summary>
        /// 暂停原因
        /// </summary>
        public string SuspendReason
        {
            get { return this.GetProperty(SuspendReasonProperty); }
            set { this.SetProperty(SuspendReasonProperty, value); }
        }
        #endregion

    }
}
