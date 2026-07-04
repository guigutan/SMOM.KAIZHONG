using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 序列号筛选 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("序列号筛选")]
    public class ReceiveSnScreenViewModel : ViewModel
    {
        #region 接收id EquipmentReceiveId
        /// <summary>
        /// 接收id
        /// </summary>
        [Label("接收id")]
        public static readonly Property<double> EquipmentReceiveIdProperty = P<ReceiveSnScreenViewModel>.Register(e => e.EquipmentReceiveId);

        /// <summary>
        /// 接收id
        /// </summary>
        public double EquipmentReceiveId
        {
            get { return this.GetProperty(EquipmentReceiveIdProperty); }
            set { this.SetProperty(EquipmentReceiveIdProperty, value); }
        }
        #endregion

        #region 接收明细 EquipmentReceiveDetail
        /// <summary>
        /// 接收明细Id
        /// </summary>
        [Label("接收明细")]
        public static readonly IRefIdProperty EquipmentReceiveDetailIdProperty =
            P<ReceiveSnScreenViewModel>.RegisterRefId(e => e.EquipmentReceiveDetailId, ReferenceType.Normal);

        /// <summary>
        /// 接收明细Id
        /// </summary>
        public double EquipmentReceiveDetailId
        {
            get { return (double)this.GetRefId(EquipmentReceiveDetailIdProperty); }
            set { this.SetRefId(EquipmentReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 接收明细
        /// </summary>
        public static readonly RefEntityProperty<EquipmentReceiveDetail> EquipmentReceiveDetailProperty =
            P<ReceiveSnScreenViewModel>.RegisterRef(e => e.EquipmentReceiveDetail, EquipmentReceiveDetailIdProperty);

        /// <summary>
        /// 接收明细
        /// </summary>
        public EquipmentReceiveDetail EquipmentReceiveDetail
        {
            get { return this.GetRefEntity(EquipmentReceiveDetailProperty); }
            set { this.SetRefEntity(EquipmentReceiveDetailProperty, value); }
        }
        #endregion
    }
}
