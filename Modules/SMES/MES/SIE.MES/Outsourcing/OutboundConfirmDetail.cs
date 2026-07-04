using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Anomalymonitors;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 发货确认明细
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(OutboundConfirmDetailCriteria))]
    [Label("发货确认明细")]
    public class OutboundConfirmDetail : DataEntity
    {
        #region 流程单号 FlowNo
        /// <summary>
        /// 流程单号
        /// </summary>
        [Label("流程单号")]
        public static readonly Property<string> FlowNoProperty = P<OutboundConfirmDetail>.Register(e => e.FlowNo);

        /// <summary>
        /// 流程单号
        /// </summary>
        public string FlowNo
        {
            get { return this.GetProperty(FlowNoProperty); }
            set { this.SetProperty(FlowNoProperty, value); }
        }
        #endregion

        #region 发货人 Outer
        /// <summary>
        /// 发货人Id
        /// </summary>
        [Label("发货人")]
        public static readonly IRefIdProperty OuterIdProperty =
            P<OutboundConfirmDetail>.RegisterRefId(e => e.OuterId, ReferenceType.Normal);

        /// <summary>
        /// 发货人Id
        /// </summary>
        public double OuterId
        {
            get { return (double)this.GetRefId(OuterIdProperty); }
            set { this.SetRefId(OuterIdProperty, value); }
        }

        /// <summary>
        /// 发货人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OuterProperty =
            P<OutboundConfirmDetail>.RegisterRef(e => e.Outer, OuterIdProperty);

        /// <summary>
        /// 发货人
        /// </summary>
        public Employee Outer
        {
            get { return this.GetRefEntity(OuterProperty); }
            set { this.SetRefEntity(OuterProperty, value); }
        }
        #endregion

        #region 发出工厂 InitiatorFactory
        /// <summary>
        /// 发出工厂
        /// </summary>
        [Label("发出工厂")]
        public static readonly Property<string> InitiatorFactoryProperty = P<OutboundConfirmDetail>.Register(e => e.InitiatorFactory);

        /// <summary>
        /// 发出工厂
        /// </summary>
        public string InitiatorFactory
        {
            get { return this.GetProperty(InitiatorFactoryProperty); }
            set { this.SetProperty(InitiatorFactoryProperty, value); }
        }
        #endregion

        #region 接收工厂 OutFactory
        /// <summary>
        /// 接收工厂
        /// </summary>
        [Label("接收工厂")]
        public static readonly Property<string> OutFactoryProperty = P<OutboundConfirmDetail>.Register(e => e.OutFactory);

        /// <summary>
        /// 接收工厂
        /// </summary>
        public string OutFactory
        {
            get { return this.GetProperty(OutFactoryProperty); }
            set { this.SetProperty(OutFactoryProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OutboundConfirmDetailState> StateProperty = P<OutboundConfirmDetail>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public OutboundConfirmDetailState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 框数 Qty
        /// <summary>
        /// 框数
        /// </summary>
        [Label("框数")]
        public static readonly Property<decimal> QtyProperty = P<OutboundConfirmDetail>.Register(e => e.Qty);

        /// <summary>
        /// 框数
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region OA返回信息 OaMsg
        /// <summary>
        /// OA返回信息
        /// </summary>
        [Label("OA返回信息")]
        public static readonly Property<string> OaMsgProperty = P<OutboundConfirmDetail>.Register(e => e.OaMsg);

        /// <summary>
        /// OA返回信息
        /// </summary>
        public string OaMsg
        {
            get { return this.GetProperty(OaMsgProperty); }
            set { this.SetProperty(OaMsgProperty, value); }
        }
        #endregion

        #region 工序标签明细 OutboundConfirmSnDetailList
        /// <summary>
        /// 工序标签明细
        /// </summary>
        [Label("工序标签明细")]
        public static readonly ListProperty<EntityList<OutboundConfirmSnDetail>> OutboundConfirmSnDetailListProperty = P<OutboundConfirmDetail>.RegisterList(e => e.OutboundConfirmSnDetailList);

        /// <summary>
        /// 工序标签明细
        /// </summary>
        public EntityList<OutboundConfirmSnDetail> OutboundConfirmSnDetailList
        {
            get { return this.GetLazyList(OutboundConfirmSnDetailListProperty); }
        }
        #endregion

        #region 唯一码 Zuid
        /// <summary>
        /// 唯一码
        /// </summary>
        [Label("唯一码")]
        public static readonly Property<string> ZuidProperty = P<OutboundConfirmDetail>.Register(e => e.Zuid);

        /// <summary>
        /// 唯一码
        /// </summary>
        public string Zuid
        {
            get { return this.GetProperty(ZuidProperty); }
            set { this.SetProperty(ZuidProperty, value); }
        }
        #endregion

        #region 是否上传事务 IsUpload
        /// <summary>
        /// 是否上传事务
        /// </summary>
        [Label("是否上传事务")]
        public static readonly Property<bool?> IsUploadProperty = P<OutboundConfirmDetail>.Register(e => e.IsUpload);

        /// <summary>
        /// 是否上传事务
        /// </summary>
        public bool? IsUpload
        {
            get { return this.GetProperty(IsUploadProperty); }
            set { this.SetProperty(IsUploadProperty, value); }
        }
        #endregion

    }

    internal class OutboundConfirmDetailConfig : EntityConfig<OutboundConfirmDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("OUTBOUND_CONFIRM_DTL").MapAllProperties();
            Meta.Property(OutboundConfirmDetail.OaMsgProperty).ColumnMeta.HasLength("MAX");
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
