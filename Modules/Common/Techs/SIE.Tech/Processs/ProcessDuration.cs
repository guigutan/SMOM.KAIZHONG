using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序加工时长
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProcessDurationCriteria))]
    [Label("工序加工时长")]
    public partial class ProcessDuration : DataEntity
    {
        #region 加工时长(小时) Durations
        /// <summary>
        /// 加工时长(小时)
        /// </summary>
        [Label("加工时长(小时)")]
        public static readonly Property<decimal> DurationsProperty = P<ProcessDuration>.Register(e => e.Durations);

        /// <summary>
        /// 加工时长(小时)
        /// </summary>
        public decimal Durations
        {
            get { return GetProperty(DurationsProperty); }
            set { SetProperty(DurationsProperty, value); }
        }
        #endregion


        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        public static readonly IRefIdProperty ProductIdProperty = P<ProcessDuration>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ProcessDuration>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessDuration>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessDuration>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProcessDuration>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工序加工时长 实体配置
    /// </summary>
    internal class ProcessDurationConfig : EntityConfig<ProcessDuration>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROC_DURATION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}