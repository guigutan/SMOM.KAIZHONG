using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 工序标签明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序标签明细")]
    public class OutboundConfirmSnDetail : DataEntity
    {
        #region 发货明细确认 OutboundConfirmDetail
        /// <summary>
        /// 发货明细确认Id
        /// </summary>
        [Label("发货明细确认")]
        public static readonly IRefIdProperty OutboundConfirmDetailIdProperty =
            P<OutboundConfirmSnDetail>.RegisterRefId(e => e.OutboundConfirmDetailId, ReferenceType.Parent);

        /// <summary>
        /// 发货明细确认Id
        /// </summary>
        public double OutboundConfirmDetailId
        {
            get { return (double)this.GetRefId(OutboundConfirmDetailIdProperty); }
            set { this.SetRefId(OutboundConfirmDetailIdProperty, value); }
        }

        /// <summary>
        /// 发货明细确认
        /// </summary>
        public static readonly RefEntityProperty<OutboundConfirmDetail> OutboundConfirmDetailProperty =
            P<OutboundConfirmSnDetail>.RegisterRef(e => e.OutboundConfirmDetail, OutboundConfirmDetailIdProperty);

        /// <summary>
        /// 发货明细确认
        /// </summary>
        public OutboundConfirmDetail OutboundConfirmDetail
        {
            get { return this.GetRefEntity(OutboundConfirmDetailProperty); }
            set { this.SetRefEntity(OutboundConfirmDetailProperty, value); }
        }
        #endregion

        #region 委外发货明细 ProcessingOutbound
        /// <summary>
        /// 委外发货明细Id
        /// </summary>
        [Label("委外发货明细")]
        public static readonly IRefIdProperty ProcessingOutboundIdProperty =
            P<OutboundConfirmSnDetail>.RegisterRefId(e => e.ProcessingOutboundId, ReferenceType.Normal);

        /// <summary>
        /// 委外发货明细Id
        /// </summary>
        public double ProcessingOutboundId
        {
            get { return (double)this.GetRefId(ProcessingOutboundIdProperty); }
            set { this.SetRefId(ProcessingOutboundIdProperty, value); }
        }

        /// <summary>
        /// 委外发货明细
        /// </summary>
        public static readonly RefEntityProperty<ProcessingOutbound> ProcessingOutboundProperty =
            P<OutboundConfirmSnDetail>.RegisterRef(e => e.ProcessingOutbound, ProcessingOutboundIdProperty);

        /// <summary>
        /// 委外发货明细
        /// </summary>
        public ProcessingOutbound ProcessingOutbound
        {
            get { return this.GetRefEntity(ProcessingOutboundProperty); }
            set { this.SetRefEntity(ProcessingOutboundProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<OutboundConfirmSnDetail>.RegisterView(e => e.ItemCode, p => p.ProcessingOutbound.OutsourcingRequest.WorkOrder.Product.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<OutboundConfirmSnDetail>.RegisterView(e => e.ItemName, p => p.ProcessingOutbound.OutsourcingRequest.WorkOrder.Product.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<OutboundConfirmSnDetail>.RegisterView(e => e.Qty,p=>p.ProcessingOutbound.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
        }
        #endregion

        #region 工序标签 Sn
        /// <summary>
        /// 工序标签
        /// </summary>
        [Label("工序标签")]
        public static readonly Property<string> SnProperty = P<OutboundConfirmSnDetail>.RegisterView(e => e.Sn, p => p.ProcessingOutbound.SN);

        /// <summary>
        /// 工序标签
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
        }
        #endregion

        #endregion
    }

    internal class OutboundConfirmSnDetailConfig : EntityConfig<OutboundConfirmSnDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("OUTBOUND_CONFIRM_SN_DTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
