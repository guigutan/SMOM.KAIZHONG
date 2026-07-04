using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.MES.OnOffDutyB
{
    /// <summary>
    /// B上下岗时间设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("B上下岗时间设置")]
    public class OnOffDutyBTimeSettingViewModel : ViewModel
    {
        #region 上岗补录时间 OnDutyTime
        /// <summary>
        /// 上岗补录时间
        /// </summary>
        [Label("上岗补录时间")]
        [Required]
        public static readonly Property<DateTime?> OnDutyTimeProperty = P<OnOffDutyBTimeSettingViewModel>.Register(e => e.OnDutyTime);

        /// <summary>
        /// 上岗补录时间
        /// </summary>
        public DateTime? OnDutyTime
        {
            get { return this.GetProperty(OnDutyTimeProperty); }
            set { this.SetProperty(OnDutyTimeProperty, value); }
        }
        #endregion

        #region 下岗补录时间 OffDutyTime
        /// <summary>
        /// 下岗补录时间
        /// </summary>
        [Label("下岗补录时间")]
        [Required]
        public static readonly Property<DateTime?> OffDutyTimeProperty = P<OnOffDutyBTimeSettingViewModel>.Register(e => e.OffDutyTime);

        /// <summary>
        /// 下岗补录时间
        /// </summary>
        public DateTime? OffDutyTime
        {
            get { return this.GetProperty(OffDutyTimeProperty); }
            set { this.SetProperty(OffDutyTimeProperty, value); }
        }
        #endregion
    }
}
