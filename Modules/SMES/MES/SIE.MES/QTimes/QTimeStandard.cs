using SIE.Alert;
using SIE.Common.Configs;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.Items;
using SIE.MES.QTimes.Configs;
using SIE.MES.QTimes.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.QTimes
{
    /// <summary>
    /// QTime标准维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(QTimeStandardCriteria))]
    [EntityWithConfig(typeof(QTimeJobTimeRangeConfig))]
    [Label("QTime标准维护")]
    public class QTimeStandard : DataEntity
    {
        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<QTimeStandard>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<QTimeStandard>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<QTimeStandard>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
            P<QTimeStandard>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<QTimeStandard>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<QTimeStandard>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 开始工序 StartProcess
        /// <summary>
        /// 开始工序Id
        /// </summary>
        [Label("开始工序")]
        public static readonly IRefIdProperty StartProcessIdProperty =
            P<QTimeStandard>.RegisterRefId(e => e.StartProcessId, ReferenceType.Normal);

        /// <summary>
        /// 开始工序Id
        /// </summary>
        public double StartProcessId
        {
            get { return (double)this.GetRefId(StartProcessIdProperty); }
            set { this.SetRefId(StartProcessIdProperty, value); }
        }

        /// <summary>
        /// 开始工序
        /// </summary>
        public static readonly RefEntityProperty<Process> StartProcessProperty =
            P<QTimeStandard>.RegisterRef(e => e.StartProcess, StartProcessIdProperty);

        /// <summary>
        /// 开始工序
        /// </summary>
        public Process StartProcess
        {
            get { return this.GetRefEntity(StartProcessProperty); }
            set { this.SetRefEntity(StartProcessProperty, value); }
        }
        #endregion

        #region 结束工序 EndProcess
        /// <summary>
        /// 结束工序Id
        /// </summary>
        [Label("结束工序")]
        public static readonly IRefIdProperty EndProcessIdProperty =
            P<QTimeStandard>.RegisterRefId(e => e.EndProcessId, ReferenceType.Normal);

        /// <summary>
        /// 结束工序Id
        /// </summary>
        public double EndProcessId
        {
            get { return (double)this.GetRefId(EndProcessIdProperty); }
            set { this.SetRefId(EndProcessIdProperty, value); }
        }

        /// <summary>
        /// 结束工序
        /// </summary>
        public static readonly RefEntityProperty<Process> EndProcessProperty =
            P<QTimeStandard>.RegisterRef(e => e.EndProcess, EndProcessIdProperty);

        /// <summary>
        /// 结束工序
        /// </summary>
        public Process EndProcess
        {
            get { return this.GetRefEntity(EndProcessProperty); }
            set { this.SetRefEntity(EndProcessProperty, value); }
        }
        #endregion

        #region 开始状态 StartState
        /// <summary>
        /// 开始状态
        /// </summary>
        [Label("开始状态")]
        public static readonly Property<QTProcessState?> StartStateProperty = P<QTimeStandard>.Register(e => e.StartState);

        /// <summary>
        /// 开始状态
        /// </summary>
        public QTProcessState? StartState
        {
            get { return this.GetProperty(StartStateProperty); }
            set { this.SetProperty(StartStateProperty, value); }
        }
        #endregion

        #region 结束状态 EndState
        /// <summary>
        /// 结束状态
        /// </summary>
        [Label("结束状态")]
        public static readonly Property<QTProcessState?> EndStateProperty = P<QTimeStandard>.Register(e => e.EndState);

        /// <summary>
        /// 结束状态
        /// </summary>
        public QTProcessState? EndState
        {
            get { return this.GetProperty(EndStateProperty); }
            set { this.SetProperty(EndStateProperty, value); }
        }
        #endregion

        #region 时间值 Time
        /// <summary>
        /// 时间值
        /// </summary>
        [Label("时间值")]
        [MinValue(0)]
        public static readonly Property<decimal> TimeProperty = P<QTimeStandard>.Register(e => e.Time);

        /// <summary>
        /// 时间值
        /// </summary>
        public decimal Time
        {
            get { return this.GetProperty(TimeProperty); }
            set { this.SetProperty(TimeProperty, value); }
        }
        #endregion

        #region 是否预警 IsAlert
        /// <summary>
        /// 是否预警
        /// </summary>
        [Label("是否预警")]
        public static readonly Property<bool> IsAlertProperty = P<QTimeStandard>.Register(e => e.IsAlert);

        /// <summary>
        /// 是否预警
        /// </summary>
        public bool IsAlert
        {
            get { return this.GetProperty(IsAlertProperty); }
            set { this.SetProperty(IsAlertProperty, value); }
        }
        #endregion

        #region 推送方式 PushPlug
        /// <summary>
        /// 推送方式Id
        /// </summary>
        [Label("推送方式")]
        public static readonly IRefIdProperty PushPlugIdProperty =
            P<QTimeStandard>.RegisterRefId(e => e.PushPlugId, ReferenceType.Normal);

        /// <summary>
        /// 推送方式Id
        /// </summary>
        public double PushPlugId
        {
            get { return (double)this.GetRefId(PushPlugIdProperty); }
            set { this.SetRefId(PushPlugIdProperty, value); }
        }

        /// <summary>
        /// 推送方式
        /// </summary>
        public static readonly RefEntityProperty<PushPlug> PushPlugProperty =
            P<QTimeStandard>.RegisterRef(e => e.PushPlug, PushPlugIdProperty);

        /// <summary>
        /// 推送方式
        /// </summary>
        public PushPlug PushPlug
        {
            get { return this.GetRefEntity(PushPlugProperty); }
            set { this.SetRefEntity(PushPlugProperty, value); }
        }
        #endregion

        #region 消息模板 MessageTemplate
        /// <summary>
        /// 消息模板
        /// </summary>
        [MaxLength(4000)]
        [Label("消息模板")]
        public static readonly Property<string> MessageTemplateProperty = P<QTimeStandard>.Register(e => e.MessageTemplate);

        /// <summary>
        /// 消息模板
        /// </summary>
        public string MessageTemplate
        {
            get { return this.GetProperty(MessageTemplateProperty); }
            set { this.SetProperty(MessageTemplateProperty, value); }
        }
        #endregion

        #region 时间单位 TimeUnit
        /// <summary>
        /// 时间单位
        /// </summary>
        [Label("时间单位")]
        public static readonly Property<QTTimeUnit?> TimeUnitProperty = P<QTimeStandard>.Register(e => e.TimeUnit);

        /// <summary>
        /// 时间单位
        /// </summary>
        public QTTimeUnit? TimeUnit
        {
            get { return this.GetProperty(TimeUnitProperty); }
            set { this.SetProperty(TimeUnitProperty, value); }
        }
        #endregion

        #region 是否启用 State
        /// <summary>
        /// 是否启用
        /// </summary>
        [Label("是否启用")]
        public static readonly Property<bool> StateProperty = P<QTimeStandard>.Register(e => e.State);

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 开始工序类型 StartProcessType
        /// <summary>
        /// 开始工序类型
        /// </summary>
        [Label("开始工序类型")]
        public static readonly Property<ProcessType?> StartProcessTypeProperty = P<QTimeStandard>.Register(e => e.StartProcessType);

        /// <summary>
        /// 开始工序类型
        /// </summary>
        public ProcessType? StartProcessType
        {
            get { return this.GetProperty(StartProcessTypeProperty); }
            set { this.SetProperty(StartProcessTypeProperty, value); }
        }
        #endregion

        #region 结束工序类型 EndProcessType
        /// <summary>
        /// 结束工序类型
        /// </summary>
        [Label("结束工序类型")]
        public static readonly Property<ProcessType?> EndProcessTypeProperty = P<QTimeStandard>.Register(e => e.EndProcessType);

        /// <summary>
        /// 结束工序类型
        /// </summary>
        public ProcessType? EndProcessType
        {
            get { return this.GetProperty(EndProcessTypeProperty); }
            set { this.SetProperty(EndProcessTypeProperty, value); }
        }
        #endregion

        #region 推送对象列表 QTPushObjectList
        /// <summary>
        /// 推送对象列表
        /// </summary>
        [Label("推送对象列表")]
        public static readonly ListProperty<EntityList<QTPushObject>> QTPushObjectListProperty = P<QTimeStandard>.RegisterList(e => e.QTPushObjectList);

        /// <summary>
        /// 推送对象列表
        /// </summary>
        public EntityList<QTPushObject> QTPushObjectList
        {
            get { return this.GetLazyList(QTPushObjectListProperty); }
        }
        #endregion

        #region 视图属性
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<QTimeStandard>.RegisterView(e => e.ProductCode, p => p.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<QTimeStandard>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 产线编码 ResourceCode
        /// <summary>
        /// 产线编码
        /// </summary>
        [Label("产线编码")]
        public static readonly Property<string> ResourceCodeProperty = P<QTimeStandard>.RegisterView(e => e.ResourceCode, p => p.WipResource.Code);

        /// <summary>
        /// 产线编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 开始工序名称 StartProcessName
        /// <summary>
        /// 开始工序名称
        /// </summary>
        [Label("开始工序名称")]
        public static readonly Property<string> StartProcessNameProperty = P<QTimeStandard>.RegisterView(e => e.StartProcessName, p => p.StartProcess.Name);

        /// <summary>
        /// 开始工序名称
        /// </summary>
        public string StartProcessName
        {
            get { return this.GetProperty(StartProcessNameProperty); }
        }
        #endregion

        #region 结束工序名称 EndProcessName
        /// <summary>
        /// 结束工序名称
        /// </summary>
        [Label("结束工序名称")]
        public static readonly Property<string> EndProcessNameProperty = P<QTimeStandard>.RegisterView(e => e.EndProcessName, p => p.EndProcess.Name);

        /// <summary>
        /// 结束工序名称
        /// </summary>
        public string EndProcessName
        {
            get { return this.GetProperty(EndProcessNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class QTimeStandardConfig : EntityConfig<QTimeStandard>
    {
        /// <summary>
        /// 数据库
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QTIME_STANDARD").MapAllProperties();
            Meta.Property(QTimeStandard.MessageTemplateProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
