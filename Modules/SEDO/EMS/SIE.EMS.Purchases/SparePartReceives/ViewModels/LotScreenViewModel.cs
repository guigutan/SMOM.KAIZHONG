using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartReceives.ViewModels
{
    /// <summary>
    /// 序列号筛选 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("序列号筛选")]
    public class LotScreenViewModel : ViewModel
    {
        #region 接收id SparePartReceiveId
        /// <summary>
        /// 接收id
        /// </summary>
        [Label("接收id")]
        public static readonly Property<double> SparePartReceiveIdProperty = P<LotScreenViewModel>.Register(e => e.SparePartReceiveId);

        /// <summary>
        /// 接收id
        /// </summary>
        public double SparePartReceiveId
        {
            get { return this.GetProperty(SparePartReceiveIdProperty); }
            set { this.SetProperty(SparePartReceiveIdProperty, value); }
        }
        #endregion

        #region 接收明细 SparePartReceiveDetail
        /// <summary>
        /// 接收明细Id
        /// </summary>
        [Label("接收明细")]
        public static readonly IRefIdProperty SparePartReceiveDetailIdProperty =
            P<LotScreenViewModel>.RegisterRefId(e => e.SparePartReceiveDetailId, ReferenceType.Normal);

        /// <summary>
        /// 接收明细Id
        /// </summary>
        public double SparePartReceiveDetailId
        {
            get { return (double)this.GetRefId(SparePartReceiveDetailIdProperty); }
            set { this.SetRefId(SparePartReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 接收明细
        /// </summary>
        public static readonly RefEntityProperty<SparePartReceiveDetail> SparePartReceiveDetailProperty =
            P<LotScreenViewModel>.RegisterRef(e => e.SparePartReceiveDetail, SparePartReceiveDetailIdProperty);

        /// <summary>
        /// 接收明细
        /// </summary>
        public SparePartReceiveDetail SparePartReceiveDetail
        {
            get { return this.GetRefEntity(SparePartReceiveDetailProperty); }
            set { this.SetRefEntity(SparePartReceiveDetailProperty, value); }
        }
        #endregion
    }
}
