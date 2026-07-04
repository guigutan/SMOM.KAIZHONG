using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances.ViewModels
{
    /// <summary>
    /// 备件验收批次明细
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Sn))]
    public class SparePartAcceptanceSnViewModel : Entity<double>
    {
        #region 序列号编码 Sn
        /// <summary>
        /// 序列号编码
        /// </summary>
        [Label("序列号编码")]
        public static readonly Property<string> SnProperty = P<SparePartAcceptanceSnViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 序列号编码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion
    }
}
