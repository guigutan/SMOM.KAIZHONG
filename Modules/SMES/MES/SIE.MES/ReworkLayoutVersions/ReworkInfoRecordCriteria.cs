using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions
{
    /// <summary>
    /// 返工信息查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("返工信息查询实体")]
    public class ReworkInfoRecordCriteria : Criteria
    {
        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReworkInfoRecordState?> StateProperty = P<ReworkInfoRecordCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReworkInfoRecordState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<ReworkInfoRecordCriteria>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ReworkInfoRecordCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ReworkInfoRecordCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 基本开始日期 BeginDateTime
        /// <summary>
        /// 基本开始日期
        /// </summary>
        [Label("基本开始日期")]
        public static readonly Property<DateRange> BeginDateTimeProperty = P<ReworkInfoRecordCriteria>.Register(e => e.BeginDateTime);

        /// <summary>
        /// 基本开始日期
        /// </summary>
        public DateRange BeginDateTime
        {
            get { return this.GetProperty(BeginDateTimeProperty); }
            set { this.SetProperty(BeginDateTimeProperty, value); }
        }
        #endregion

        #region 基本完成日期 EndDateTime
        /// <summary>
        /// 基本完成日期
        /// </summary>
        [Label("基本完成日期")]
        public static readonly Property<DateRange> EndDateTimeProperty = P<ReworkInfoRecordCriteria>.Register(e => e.EndDateTime);

        /// <summary>
        /// 基本完成日期
        /// </summary>
        public DateRange EndDateTime
        {
            get { return this.GetProperty(EndDateTimeProperty); }
            set { this.SetProperty(EndDateTimeProperty, value); }
        }
        #endregion

        #region 返工工艺路线版本 ReworkLayoutVersion
        /// <summary>
        /// 返工工艺路线版本Id
        /// </summary>
        [Label("返工工艺路线版本")]
        public static readonly IRefIdProperty ReworkLayoutVersionIdProperty =
            P<ReworkInfoRecordCriteria>.RegisterRefId(e => e.ReworkLayoutVersionId, ReferenceType.Normal);

        /// <summary>
        /// 返工工艺路线版本Id
        /// </summary>
        public double? ReworkLayoutVersionId
        {
            get { return (double?)this.GetRefNullableId(ReworkLayoutVersionIdProperty); }
            set { this.SetRefNullableId(ReworkLayoutVersionIdProperty, value); }
        }

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        public static readonly RefEntityProperty<ReworkLayoutVersion> ReworkLayoutVersionProperty =
            P<ReworkInfoRecordCriteria>.RegisterRef(e => e.ReworkLayoutVersion, ReworkLayoutVersionIdProperty);

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        public ReworkLayoutVersion ReworkLayoutVersion
        {
            get { return this.GetRefEntity(ReworkLayoutVersionProperty); }
            set { this.SetRefEntity(ReworkLayoutVersionProperty, value); }
        }
        #endregion

        #region 需求部门 Department
        /// <summary>
        /// 需求部门
        /// </summary>
        [Label("需求部门")]
        public static readonly Property<string> DepartmentProperty = P<ReworkInfoRecordCriteria>.Register(e => e.Department);

        /// <summary>
        /// 需求部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion

        #region 唯一码 UniqueCode
        /// <summary>
        /// 唯一码
        /// </summary>
        [Label("唯一码")]
        public static readonly Property<string> UniqueCodeProperty = P<ReworkInfoRecordCriteria>.Register(e => e.UniqueCode);

        /// <summary>
        /// 唯一码
        /// </summary>
        public string UniqueCode
        {
            get { return this.GetProperty(UniqueCodeProperty); }
            set { this.SetProperty(UniqueCodeProperty, value); }
        }
        #endregion

        #region 生产订单 ProductOrder
        /// <summary>
        /// 生产订单
        /// </summary>
        [Label("生产订单")]
        public static readonly Property<string> ProductOrderProperty = P<ReworkInfoRecordCriteria>.Register(e => e.ProductOrder);

        /// <summary>
        /// 生产订单
        /// </summary>
        public string ProductOrder
        {
            get { return this.GetProperty(ProductOrderProperty); }
            set { this.SetProperty(ProductOrderProperty, value); }
        }
        #endregion

        #region 反馈信息 Msg
        /// <summary>
        /// 反馈信息
        /// </summary>
        [Label("反馈信息")]
        public static readonly Property<string> MsgProperty = P<ReworkInfoRecordCriteria>.Register(e => e.Msg);

        /// <summary>
        /// 反馈信息
        /// </summary>
        public string Msg
        {
            get { return this.GetProperty(MsgProperty); }
            set { this.SetProperty(MsgProperty, value); }
        }
        #endregion

        #region 标签号 Sn
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> SnProperty = P<ReworkInfoRecordCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ReworkInfoRecordCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ReworkLayoutVersionController>().CriteriaReworkInfoRecord(this);
        }
    }
}
