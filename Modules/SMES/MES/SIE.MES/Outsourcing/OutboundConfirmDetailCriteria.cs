using SIE.Domain;
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
    /// 发货确认明细查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("发货确认明细查询实体")]
    public class OutboundConfirmDetailCriteria : Criteria
    {
        #region 流程单号 FlowNo
        /// <summary>
        /// 流程单号
        /// </summary>
        [Label("流程单号")]
        public static readonly Property<string> FlowNoProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.FlowNo);

        /// <summary>
        /// 流程单号
        /// </summary>
        public string FlowNo
        {
            get { return this.GetProperty(FlowNoProperty); }
            set { this.SetProperty(FlowNoProperty, value); }
        }
        #endregion

        #region 发出工厂 InitiatorFactory
        /// <summary>
        /// 发出工厂
        /// </summary>
        [Label("发出工厂")]
        public static readonly Property<string> InitiatorFactoryProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.InitiatorFactory);

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
        public static readonly Property<string> OutFactoryProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.OutFactory);

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
        public static readonly Property<OutboundConfirmDetailState?> StateProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public OutboundConfirmDetailState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 发货时间 DeliveryDate
        /// <summary>
        /// 发货时间
        /// </summary>
        [Label("发货时间")]
        public static readonly Property<DateRange> DeliveryDateProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateRange DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 工序标签 Sn
        /// <summary>
        /// 工序标签
        /// </summary>
        [Label("工序标签")]
        public static readonly Property<string> SnProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 工序标签
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<OutboundConfirmDetailCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OutsourcingController>().CriteriaOutboundConfirmDetail(this);
        }
    }
}
