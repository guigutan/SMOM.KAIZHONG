using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 关单ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("关单")]
    //[DisplayMember(nameof())]
    public class CompelCloseViewModel : ViewModel
    {

        #region 关单原因 CloseReason
        /// <summary>
        /// 关单原因
        /// </summary>
        [Label("关单原因")]
        public static readonly Property<string> CloseReasonProperty = P<CompelCloseViewModel>.Register(e => e.CloseReason);

        /// <summary>
        /// 关单原因
        /// </summary>
        public string CloseReason
        {
            get { return this.GetProperty(CloseReasonProperty); }
            set { this.SetProperty(CloseReasonProperty, value); }
        }
        #endregion

    }
}
