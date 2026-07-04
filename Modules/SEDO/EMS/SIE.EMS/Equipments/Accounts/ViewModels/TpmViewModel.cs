using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Equipments.Accounts.ViewModels
{
    /// <summary>
    /// TPM信息VM
    /// </summary>
    [RootEntity, Serializable]
    [Label("TPM信息")]
    public class TpmViewModel : ViewModel
    {
        #region 上次执行时间 LastExecuteTime
        /// <summary>
        /// 上次执行时间
        /// </summary>
        [Label("上次执行时间")]
        public static readonly Property<DateTime?> LastExecuteTimeProperty = P<TpmViewModel>.Register(e => e.LastExecuteTime);

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? LastExecuteTime
        {
            get { return this.GetProperty(LastExecuteTimeProperty); }
            set { this.SetProperty(LastExecuteTimeProperty, value); }
        }
        #endregion

        #region 当前待执行时间 CurrentToBeExecuteTime
        /// <summary>
        /// 当前待执行时间
        /// </summary>
        [Label("当前待执行时间")]
        public static readonly Property<DateTime?> CurrentToBeExecuteTimeProperty = P<TpmViewModel>.Register(e => e.CurrentToBeExecuteTime);

        /// <summary>
        /// 当前待执行时间
        /// </summary>
        public DateTime? CurrentToBeExecuteTime
        {
            get { return this.GetProperty(CurrentToBeExecuteTimeProperty); }
            set { this.SetProperty(CurrentToBeExecuteTimeProperty, value); }
        }
        #endregion

        #region 下次待执行时间 NextToBeExecuteTime
        /// <summary>
        /// 下次待执行时间
        /// </summary>
        [Label("下次待执行时间")]
        public static readonly Property<DateTime?> NextToBeExecuteTimeProperty = P<TpmViewModel>.Register(e => e.NextToBeExecuteTime);

        /// <summary>
        /// 下次待执行时间
        /// </summary>
        public DateTime? NextToBeExecuteTime
        {
            get { return this.GetProperty(NextToBeExecuteTimeProperty); }
            set { this.SetProperty(NextToBeExecuteTimeProperty, value); }
        }
        #endregion
    }
}
