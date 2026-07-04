using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 取消ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("取消")]
    //[DisplayMember(nameof())]
    public class BillCancelViewModel : ViewModel
    {

        #region 取消原因 CancelReason
        /// <summary>
        /// 取消原因
        /// </summary>
        [Label("取消原因")]
        public static readonly Property<string> CancelReasonProperty = P<BillCancelViewModel>.Register(e => e.CancelReason);

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason
        {
            get { return this.GetProperty(CancelReasonProperty); }
            set { this.SetProperty(CancelReasonProperty, value); }
        }
        #endregion

    }
}
