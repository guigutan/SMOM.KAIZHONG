using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 序列号筛选 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("序列号筛选")]
    public class ReceiveSnScreenViewModel : ViewModel
    {
        #region 接收id FixtureReceiveId
        /// <summary>
        /// 接收id
        /// </summary>
        [Label("接收id")]
        public static readonly Property<double> FixtureReceiveIdProperty = P<ReceiveSnScreenViewModel>.Register(e => e.FixtureReceiveId);

        /// <summary>
        /// 接收id
        /// </summary>
        public double FixtureReceiveId
        {
            get { return this.GetProperty(FixtureReceiveIdProperty); }
            set { this.SetProperty(FixtureReceiveIdProperty, value); }
        }
        #endregion

        #region 接收明细 FixtureReceiveDetail
        /// <summary>
        /// 接收明细Id
        /// </summary>
        [Label("接收明细")]
        public static readonly IRefIdProperty FixtureReceiveDetailIdProperty =
            P<ReceiveSnScreenViewModel>.RegisterRefId(e => e.FixtureReceiveDetailId, ReferenceType.Normal);

        /// <summary>
        /// 接收明细Id
        /// </summary>
        public double FixtureReceiveDetailId
        {
            get { return (double)this.GetRefId(FixtureReceiveDetailIdProperty); }
            set { this.SetRefId(FixtureReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 接收明细
        /// </summary>
        public static readonly RefEntityProperty<FixtureReceiveDetail> FixtureReceiveDetailProperty =
            P<ReceiveSnScreenViewModel>.RegisterRef(e => e.FixtureReceiveDetail, FixtureReceiveDetailIdProperty);

        /// <summary>
        /// 接收明细
        /// </summary>
        public FixtureReceiveDetail FixtureReceiveDetail
        {
            get { return this.GetRefEntity(FixtureReceiveDetailProperty); }
            set { this.SetRefEntity(FixtureReceiveDetailProperty, value); }
        }
        #endregion
    }
}
