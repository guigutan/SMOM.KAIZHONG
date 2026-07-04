using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次采集记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次采集记录")]
    public partial class BatchWipProductProcess : DataEntity
    {
        #region 入站时间 InputDate
        /// <summary>
        /// 入站时间
        /// </summary>
        [Label("入站时间")]
        public static readonly Property<DateTime> InputDateProperty = P<BatchWipProductProcess>.Register(e => e.InputDate);

        /// <summary>
        /// 入站时间
        /// </summary>
        public DateTime InputDate
        {
            get { return GetProperty(InputDateProperty); }
            set { SetProperty(InputDateProperty, value); }
        }
        #endregion

        #region 出站时间 OutputDate
        /// <summary>
        /// 出站时间
        /// </summary>
        [Label("出站时间")]
        public static readonly Property<DateTime?> OutputDateProperty = P<BatchWipProductProcess>.Register(e => e.OutputDate);

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? OutputDate
        {
            get { return GetProperty(OutputDateProperty); }
            set { SetProperty(OutputDateProperty, value); }
        }
        #endregion

        #region 入站数量 InputQty
        /// <summary>
        /// 入站数量
        /// </summary>
        [Label("入站数量")]
        public static readonly Property<decimal> InputQtyProperty = P<BatchWipProductProcess>.Register(e => e.InputQty);

        /// <summary>
        /// 入站数量
        /// </summary>
        public decimal InputQty
        {
            get { return GetProperty(InputQtyProperty); }
            set { SetProperty(InputQtyProperty, value); }
        }
        #endregion

        #region 出站数量 OutputQty
        /// <summary>
        /// 出站数量
        /// </summary>
        [Label("出站数量")]
        public static readonly Property<decimal> OutputQtyProperty = P<BatchWipProductProcess>.Register(e => e.OutputQty);

        /// <summary>
        /// 出站数量
        /// </summary>
        public decimal OutputQty
        {
            get { return GetProperty(OutputQtyProperty); }
            set { SetProperty(OutputQtyProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<BatchWipProductProcess>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<BatchWipProductProcess>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<BatchWipProductProcess>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<BatchWipProductProcess>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 版本 Version
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static readonly IRefIdProperty VersionIdProperty = P<BatchWipProductProcess>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 版本Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductVersion> VersionProperty = P<BatchWipProductProcess>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 版本
        /// </summary>
        public BatchWipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 明细列表 DetailList
        /// <summary>
        /// 批次采集工序明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductProcessDetail>> DetailListProperty = P<BatchWipProductProcess>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 批次采集工序明细列表
        /// </summary>
        public EntityList<BatchWipProductProcessDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 视图属性
        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<BatchWipProductProcess>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<BatchWipProductProcess>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 批次采集记录 实体配置
    /// </summary>
    internal class BatchWipProductProcessConfig : EntityConfig<BatchWipProductProcess>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_PROCESS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}