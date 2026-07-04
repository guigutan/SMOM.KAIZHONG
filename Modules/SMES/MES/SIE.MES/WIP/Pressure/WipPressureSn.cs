using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压测试SN
    /// </summary>
    [RootEntity, Serializable]
    [Label("耐压测试SN")]
    [DisplayMember(nameof(Sn))]
    public partial class WipPressureSn : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipPressureSn()
        {
        }

        #region 耐压测试批次 WipPressure
        /// <summary>
        /// 耐压测试批次Id
        /// </summary>
        [Label("耐压测试批次")]
        public static readonly IRefIdProperty WipPressureIdProperty =
            P<WipPressureSn>.RegisterRefId(e => e.WipPressureId, ReferenceType.Parent);

        /// <summary>
        /// 耐压测试批次Id
        /// </summary>
        public double WipPressureId
        {
            get { return (double)this.GetRefId(WipPressureIdProperty); }
            set { this.SetRefId(WipPressureIdProperty, value); }
        }

        /// <summary>
        /// 耐压测试批次
        /// </summary>
        public static readonly RefEntityProperty<WipPressure> WipPressureProperty =
            P<WipPressureSn>.RegisterRef(e => e.WipPressure, WipPressureIdProperty);

        /// <summary>
        /// 耐压测试批次
        /// </summary>
        public WipPressure WipPressure
        {
            get { return this.GetRefEntity(WipPressureProperty); }
            set { this.SetRefEntity(WipPressureProperty, value); }
        }
        #endregion

        #region SN Sn
        /// <summary>
        /// SN
        /// </summary>
        [Label("SN")]
        public static readonly Property<string> SnProperty = P<WipPressureSn>.Register(e => e.Sn);

        /// <summary>
        /// SN
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region SN(暗码) Sn2
        /// <summary>
        /// SN(暗码)
        /// </summary>
        [Label("SN(暗码)")]
        public static readonly Property<string> Sn2Property = P<WipPressureSn>.Register(e => e.Sn2);

        /// <summary>
        /// SN(暗码)
        /// </summary>
        public string Sn2
        {
            get { return this.GetProperty(Sn2Property); }
            set { this.SetProperty(Sn2Property, value); }
        }
        #endregion

        #region 测试结果 TestResult
        /// <summary>
        /// 测试结果
        /// </summary>
        [Label("测试结果")]
        public static readonly Property<TestResult?> TestResultProperty = P<WipPressureSn>.Register(e => e.TestResult);

        /// <summary>
        /// 测试结果
        /// </summary>
        public TestResult? TestResult
        {
            get { return this.GetProperty(TestResultProperty); }
            set { this.SetProperty(TestResultProperty, value); }
        }
        #endregion

        #region 测试时间 TestTime
        /// <summary>
        /// 测试时间
        /// </summary>
        [Label("测试时间")]
        public static readonly Property<DateTime?> TestTimeProperty = P<WipPressureSn>.Register(e => e.TestTime);

        /// <summary>
        /// 测试时间
        /// </summary>
        public DateTime? TestTime
        {
            get { return this.GetProperty(TestTimeProperty); }
            set { this.SetProperty(TestTimeProperty, value); }
        }
        #endregion

        #region 测试数据 RawData
        /// <summary>
        /// 测试数据
        /// </summary>
        [Label("测试数据")]
        public static readonly Property<string> RawDataProperty = P<WipPressureSn>.Register(e => e.RawData);

        /// <summary>
        /// 测试数据
        /// </summary>
        public string RawData
        {
            get { return this.GetProperty(RawDataProperty); }
            set { this.SetProperty(RawDataProperty, value); }
        }
        #endregion

        #region 测试值 TestValueList
        /// <summary>
        /// 测试值
        /// </summary>
        [Label("测试值")]
        public static readonly ListProperty<EntityList<WipPressureTestValue>> TestValueListProperty = P<WipPressureSn>.RegisterList(e => e.TestValueList);

        /// <summary>
        /// 测试值
        /// </summary>
        public EntityList<WipPressureTestValue> TestValueList
        {
            get { return this.GetLazyList(TestValueListProperty); }
        }
        #endregion

        #region 是否超打 IsOver
        /// <summary>
        /// 是否超打
        /// </summary>
        [Label("是否超打")]
        public static readonly Property<bool> IsOverProperty = P<WipPressureSn>.Register(e => e.IsOver);

        /// <summary>
        /// 是否超打
        /// </summary>
        public bool IsOver
        {
            get { return this.GetProperty(IsOverProperty); }
            set { this.SetProperty(IsOverProperty, value); }
        }
        #endregion

        #region 是否已使用 Isuse
        /// <summary>
        /// 是否已使用
        /// </summary>
        [Label("是否已使用")]
        public static readonly Property<bool> IsuseProperty = P<WipPressureSn>.Register(e => e.Isuse);

        /// <summary>
        /// 是否已使用
        /// </summary>
        public bool Isuse
        {
            get { return this.GetProperty(IsuseProperty); }
            set { this.SetProperty(IsuseProperty, value); }
        }
        #endregion

        #region 是否可疑品 IsSuspectProduct
        /// <summary>
        /// 是否可疑品
        /// </summary>
        [Label("是否可疑品")]
        public static readonly Property<bool> IsSuspectProductProperty = P<WipPressureSn>.Register(e => e.IsSuspectProduct);

        /// <summary>
        /// 是否可疑品
        /// </summary>
        public bool IsSuspectProduct
        {
            get { return this.GetProperty(IsSuspectProductProperty); }
            set { this.SetProperty(IsSuspectProductProperty, value); }
        }
        #endregion

        #region 是否返工 IsRework
        /// <summary>
        /// 是否返工
        /// </summary>
        [Label("是否返工")]
        public static readonly Property<bool> IsReworkProperty = P<WipPressureSn>.Register(e => e.IsRework);

        /// <summary>
        /// 是否返工
        /// </summary>
        public bool IsRework
        {
            get { return this.GetProperty(IsReworkProperty); }
            set { this.SetProperty(IsReworkProperty, value); }
        }
        #endregion

        #region 是否报废 IsScraped
        /// <summary>
        /// 是否报废
        /// </summary>
        [Label("是否报废")]
        public static readonly Property<bool> IsScrapedProperty = P<WipPressureSn>.Register(e => e.IsScraped);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScraped
        {
            get { return GetProperty(IsScrapedProperty); }
            set { SetProperty(IsScrapedProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工序标签 BatchNo
        /// <summary>
        /// 工序标签
        /// </summary>
        [Label("工序标签")]
        public static readonly Property<string> BatchNoProperty = P<WipPressureSn>.RegisterView(e => e.BatchNo, p => p.WipPressure.BatchNo);

        /// <summary>
        /// 工序标签
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
        }

        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<WipPressureSn>.RegisterView(e => e.WorkOrderId, p => p.WipPressure.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipPressureSn>.RegisterView(e => e.WorkOrderNo, p => p.WipPressure.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipPressureSn>.RegisterView(e => e.ProductCode, p => p.WipPressure.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WipPressureSn>.RegisterView(e => e.ProductName, p => p.WipPressure.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<WipPressureSn>.RegisterView(e => e.ResourceCode, e => e.WipPressure.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return GetProperty(ResourceCodeProperty); }
        }
        #endregion   

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WipPressureSn>.RegisterView(e => e.ResourceName, e => e.WipPressure.Resource.Name);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
        }
        #endregion
        #endregion

    }

    /// <summary>
    /// 生产批次 实体配置
    /// </summary>
    internal class WipPressureSnEntityConfig : EntityConfig<WipPressureSn>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PRESSURE_SN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}