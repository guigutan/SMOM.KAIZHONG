using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances.ViewModels
{
    /// <summary>
    /// 备件验收批次明细
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(LotNo))]
    public class SparePartAcceptanceLotViewModel : Entity<double>
    {
        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<SparePartAcceptanceLotViewModel>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion
    }
}
