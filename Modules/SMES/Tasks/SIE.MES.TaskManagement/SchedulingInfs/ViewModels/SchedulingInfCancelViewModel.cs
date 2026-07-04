using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs.ViewModels
{
    [RootEntity, Serializable]
    public class SchedulingInfCancelViewModel : ViewModel
    {
        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        public static readonly Property<string> ReasonProperty = P<SchedulingInfCancelViewModel>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region Ids Ids
        /// <summary>
        /// Ids
        /// </summary>
        [Label("Ids")]
        public static readonly Property<string> IdsProperty = P<SchedulingInfCancelViewModel>.Register(e => e.Ids);

        /// <summary>
        /// Ids
        /// </summary>
        public string Ids
        {
            get { return this.GetProperty(IdsProperty); }
            set { this.SetProperty(IdsProperty, value); }
        }
        #endregion

    }
}
