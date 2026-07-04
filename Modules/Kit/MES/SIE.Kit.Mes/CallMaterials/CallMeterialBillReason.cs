using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 紧急叫料原因
    /// </summary>
    [ChildEntity, Serializable]
    [Label("紧急叫料原因")]
    [DisplayMember(nameof(Id))]
    public class UrgencyCallMeterialReason : DataEntity
    {
        #region 叫料单 Bill
        /// <summary>
        /// 叫料单Id
        /// </summary>
        [Label("叫料单")]
        public static readonly IRefIdProperty BillIdProperty =
            P<UrgencyCallMeterialReason>.RegisterRefId(e => e.BillId, ReferenceType.Parent);

        /// <summary>
        /// 叫料单Id
        /// </summary>
        public double BillId
        {
            get { return (double)this.GetRefId(BillIdProperty); }
            set { this.SetRefId(BillIdProperty, value); }
        }

        /// <summary>
        /// 叫料单
        /// </summary>
        public static readonly RefEntityProperty<CallMaterialBill> BillProperty =
            P<UrgencyCallMeterialReason>.RegisterRef(e => e.Bill, BillIdProperty);

        /// <summary>
        /// 叫料单
        /// </summary>
        public CallMaterialBill Bill
        {
            get { return this.GetRefEntity(BillProperty); }
            set { this.SetRefEntity(BillProperty, value); }
        }
        #endregion

        #region 叫料原因 Reason
        /// <summary>
        /// 叫料原因Id
        /// </summary>
        [Label("叫料原因")]
        public static readonly IRefIdProperty ReasonIdProperty =
            P<UrgencyCallMeterialReason>.RegisterRefId(e => e.ReasonId, ReferenceType.Normal);

        /// <summary>
        /// 叫料原因Id
        /// </summary>
        public double ReasonId
        {
            get { return (double)this.GetRefId(ReasonIdProperty); }
            set { this.SetRefId(ReasonIdProperty, value); }
        }

        /// <summary>
        /// 叫料原因
        /// </summary>
        public static readonly RefEntityProperty<CallMaterialReason> ReasonProperty =
            P<UrgencyCallMeterialReason>.RegisterRef(e => e.Reason, ReasonIdProperty);

        /// <summary>
        /// 叫料原因
        /// </summary>
        public CallMaterialReason Reason
        {
            get { return this.GetRefEntity(ReasonProperty); }
            set { this.SetRefEntity(ReasonProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 叫料单号 BillNo
        /// <summary>
        /// 叫料单号
        /// </summary>
        [Label("叫料单号")]
        public static readonly Property<string> BillNoProperty = P<UrgencyCallMeterialReason>.RegisterView(e => e.BillNo, p => p.Bill.No);

        /// <summary>
        /// 叫料单号
        /// </summary>
        public string BillNo
        {
            get { return this.GetProperty(BillNoProperty); }
        }
        #endregion

        #region 叫料原因 ReasonName
        /// <summary>
        /// 叫料原因
        /// </summary>
        [Label("叫料原因")]
        public static readonly Property<string> ReasonNameProperty = P<UrgencyCallMeterialReason>.RegisterView(e => e.ReasonName, p => p.Reason.Name);

        /// <summary>
        /// 叫料原因
        /// </summary>
        public string ReasonName
        {
            get { return this.GetProperty(ReasonNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 紧急叫料原因 实体配置
    /// </summary>
    internal class UrgencyCallMeterialReasonConfig : EntityConfig<UrgencyCallMeterialReason>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_URG_REASON").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}