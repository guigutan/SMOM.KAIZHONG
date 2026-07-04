using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 采集步骤
    /// </summary>
    [ChildEntity, Serializable]
    [Label("采集步骤")]
    [DisplayMember(nameof(BarcodeType))]
    public partial class ProcessCollectStep : DataEntity
    {
        #region 是否解绑 IsUnbound
        /// <summary>
        /// 是否解绑
        /// </summary>
        [Label("是否解绑")]
        public static readonly Property<bool> IsUnboundProperty = P<ProcessCollectStep>.Register(e => e.IsUnbound);

        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool IsUnbound
        {
            get { return GetProperty(IsUnboundProperty); }
            set { SetProperty(IsUnboundProperty, value); }
        }
        #endregion

        #region 步骤 Step
        /// <summary>
        /// 步骤
        /// </summary>
        [Label("步骤")]
        public static readonly Property<int> StepProperty = P<ProcessCollectStep>.Register(e => e.Step);

        /// <summary>
        /// 步骤
        /// </summary>
        public int Step
        {
            get { return GetProperty(StepProperty); }
            set { SetProperty(StepProperty, value); }
        }
        #endregion

        #region 条码类型 BarcodeType
        /// <summary>
        /// 条码类型
        /// </summary>
        [Required]
        [Label("条码类型")]
        public static readonly Property<BarcodeType> BarcodeTypeProperty = P<ProcessCollectStep>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get { return GetProperty(BarcodeTypeProperty); }
            set { SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion

        #region 出入站类型 PlugType
        /// <summary>
        /// 出入站类型
        /// </summary>
        [Label("出入类型")]
        public static readonly Property<PlugType?> PlugTypeProperty = P<ProcessCollectStep>.Register(e => e.PlugType);

        /// <summary>
        /// 出入站类型
        /// </summary>
        public PlugType? PlugType
        {
            get { return this.GetProperty(PlugTypeProperty); }
            set { this.SetProperty(PlugTypeProperty, value); }
        }
        #endregion

        #region 是否生成批次 IsGenerateBatch
        /// <summary>
        /// 是否生成批次
        /// </summary>
        [Label("是否生成批次")]
        public static readonly Property<bool> IsGenerateBatchProperty = P<ProcessCollectStep>.Register(e => e.IsGenerateBatch);

        /// <summary>
        /// 是否生成批次
        /// </summary>
        public bool IsGenerateBatch
        {
            get { return this.GetProperty(IsGenerateBatchProperty); }
            set { this.SetProperty(IsGenerateBatchProperty, value); }
        }
        #endregion 

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessCollectStep>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessCollectStep>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 采集步骤 实体配置
    /// </summary>
    internal class ProcessCollectStepConfig : EntityConfig<ProcessCollectStep>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROC_STEP").MapAllProperties();
            Meta.EnableSort();
            Meta.EnablePhantoms();
            Meta.Property(ProcessCollectStep.ProcessIdProperty).ColumnMeta.HasIndex();
        }
    }
}