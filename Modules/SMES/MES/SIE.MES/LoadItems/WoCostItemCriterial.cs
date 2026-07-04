using SIE.Domain;
using SIE.Items;
using SIE.MES.LoadItems.Enum;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 倒扣记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("倒扣记录查询实体")]
    public class WoCostItemCriterial : Criteria
    {
        #region 耗用单号 CostNo
        /// <summary>
        /// 耗用单号
        /// </summary>
        [Label("耗用单号")]
        public static readonly Property<string> CostNoProperty = P<WoCostItemCriterial>.Register(e => e.CostNo);

        /// <summary>
        /// 耗用单号
        /// </summary>
        public string CostNo
        {
            get { return this.GetProperty(CostNoProperty); }
            set { this.SetProperty(CostNoProperty, value); }
        }
        #endregion

        #region 单据类型 RecordType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<WoCostItemType?> RecordTypeProperty = P<WoCostItemCriterial>.Register(e => e.RecordType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public WoCostItemType? RecordType
        {
            get { return this.GetProperty(RecordTypeProperty); }
            set { this.SetProperty(RecordTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<WoCostItemState?> StateProperty = P<WoCostItemCriterial>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public WoCostItemState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoNoProperty = P<WoCostItemCriterial>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WoCostItemCriterial>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 耗用物料编码 CostItemCode
        /// <summary>
        /// 耗用物料编码
        /// </summary>
        [Label("耗用物料编码")]
        public static readonly Property<string> CostItemCodeProperty = P<WoCostItemCriterial>.Register(e => e.CostItemCode);

        /// <summary>
        /// 耗用物料编码
        /// </summary>
        public string CostItemCode
        {
            get { return this.GetProperty(CostItemCodeProperty); }
            set { this.SetProperty(CostItemCodeProperty, value); }
        }
        #endregion

        #region 耗用物料名称 CostItemName
        /// <summary>
        /// 耗用物料名称
        /// </summary>
        [Label("耗用物料名称")]
        public static readonly Property<string> CostItemNameProperty = P<WoCostItemCriterial>.Register(e => e.CostItemName);

        /// <summary>
        /// 耗用物料名称
        /// </summary>
        public string CostItemName
        {
            get { return this.GetProperty(CostItemNameProperty); }
            set { this.SetProperty(CostItemNameProperty, value); }
        }
        #endregion

        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelProperty = P<WoCostItemCriterial>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
            set { this.SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 批次号 Lot
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotProperty = P<WoCostItemCriterial>.Register(e => e.Lot);

        /// <summary>
        /// 批次号
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
            set { this.SetProperty(LotProperty, value); }
        }
        #endregion

        #region 产品条码 BarCode
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> BarCodeProperty = P<WoCostItemCriterial>.Register(e => e.BarCode);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string BarCode
        {
            get { return this.GetProperty(BarCodeProperty); }
            set { this.SetProperty(BarCodeProperty, value); }
        }
        #endregion

        #region 产品批次 BatchNo
        /// <summary>
        /// 产品批次
        /// </summary>
        [Label("产品批次")]
        public static readonly Property<string> BatchNoProperty = P<WoCostItemCriterial>.Register(e => e.BatchNo);

        /// <summary>
        /// 产品批次
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<WoCostItemCriterial>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<WoCostItemCriterial>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<WoCostItemCriterial>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<WoCostItemCriterial>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 提交人 Submiter
        /// <summary>
        /// 提交人Id
        /// </summary>
        [Label("提交人")]
        public static readonly IRefIdProperty SubmiterIdProperty =
            P<WoCostItemCriterial>.RegisterRefId(e => e.SubmiterId, ReferenceType.Normal);

        /// <summary>
        /// 提交人Id
        /// </summary>
        public double? SubmiterId
        {
            get { return (double?)this.GetRefNullableId(SubmiterIdProperty); }
            set { this.SetRefNullableId(SubmiterIdProperty, value); }
        }

        /// <summary>
        /// 提交人
        /// </summary>
        public static readonly RefEntityProperty<Employee> SubmiterProperty =
            P<WoCostItemCriterial>.RegisterRef(e => e.Submiter, SubmiterIdProperty);

        /// <summary>
        /// 提交人
        /// </summary>
        public Employee Submiter
        {
            get { return this.GetRefEntity(SubmiterProperty); }
            set { this.SetRefEntity(SubmiterProperty, value); }
        }
        #endregion

        #region 提交时间 SubmitTime
        /// <summary>
        /// 提交时间
        /// </summary>
        [Label("提交时间")]
        public static readonly Property<DateRange> SubmitTimeProperty = P<WoCostItemCriterial>.Register(e => e.SubmitTime);

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateRange SubmitTime
        {
            get { return this.GetProperty(SubmitTimeProperty); }
            set { this.SetProperty(SubmitTimeProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WoCostItemController>().CriterialDeductItem(this);
        }
    }
}
