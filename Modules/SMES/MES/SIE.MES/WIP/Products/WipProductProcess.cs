using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 生产采集记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("生产采集记录")]
    public partial class WipProductProcess : DataEntity
    {
        #region 操作时间 OperateTime
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime?> OperateTimeProperty = P<WipProductProcess>.Register(e => e.OperateTime);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperateTime
        {
            get { return GetProperty(OperateTimeProperty); }
            set { SetProperty(OperateTimeProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<WipProductProcess>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty = P<WipProductProcess>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion 

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<WipProductProcess>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WipProductProcess>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<WipProductProcess>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)GetRefId(ShiftIdProperty); }
            set { SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<WipProductProcess>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 采集结果 Result
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly Property<ResultType> ResultProperty = P<WipProductProcess>.Register(e => e.Result);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 操作人 OperateBy
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperateByIdProperty = P<WipProductProcess>.RegisterRefId(e => e.OperateById, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperateById
        {
            get { return (double)GetRefId(OperateByIdProperty); }
            set { SetRefId(OperateByIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperateByProperty = P<WipProductProcess>.RegisterRef(e => e.OperateBy, OperateByIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee OperateBy
        {
            get { return GetRefEntity(OperateByProperty); }
            set { SetRefEntity(OperateByProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductProcess>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WipProductProcess>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 采集记录 Version
        /// <summary>
        /// 采集记录Id
        /// </summary>
        [Label("采集记录")]
        public static readonly IRefIdProperty VersionIdProperty = P<WipProductProcess>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 采集记录Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 采集记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductVersion> VersionProperty = P<WipProductProcess>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 采集记录
        /// </summary>
        public WipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<WipProductProcessState> StateProperty = P<WipProductProcess>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public WipProductProcessState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion  

        #region 工序采集步骤最后一个条码 Barcode
        /// <summary>
        /// 工序采集步骤最后一个条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<WipProductProcess>.Register(e => e.Barcode);

        /// <summary>
        /// 工序采集步骤最后一个条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion 

        #region 测试结果 TestResultList
        /// <summary>
        /// 测试结果
        /// </summary>
        [Label("测试结果")]
        public static readonly ListProperty<EntityList<WipProductTestResult>> TestResultListProperty = P<WipProductProcess>.RegisterList(e => e.TestResultList);

        /// <summary>
        /// 测试结果
        /// </summary>
        public EntityList<WipProductTestResult> TestResultList
        {
            get { return this.GetLazyList(TestResultListProperty); }
        }
        #endregion

        #region 关键件 KeyItemList
        /// <summary>
        /// 关键件
        /// </summary>
        [Label("关键件")]
        public static readonly ListProperty<EntityList<WipProductProcessKeyItem>> KeyItemListProperty = P<WipProductProcess>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 关键件
        /// </summary>
        public EntityList<WipProductProcessKeyItem> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion

        #region 员工信息 EmployeeList
        /// <summary>
        /// 员工信息
        /// </summary>
        [Label("员工信息")]
        public static readonly ListProperty<EntityList<WipProProcessEmployee>> EmployeeListProperty = P<WipProductProcess>.RegisterList(e => e.EmployeeList);

        /// <summary>
        /// 员工信息
        /// </summary>
        public EntityList<WipProProcessEmployee> EmployeeList
        {
            get { return this.GetLazyList(EmployeeListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工位名称 StationCode
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationCodeProperty = P<WipProductProcess>.RegisterView(e => e.StationCode, p => p.Station.Code);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationCode
        {
            get { return this.GetProperty(StationCodeProperty); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<WipProductProcess>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<WipProductProcess>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType?> ProcessTypeProperty = P<WipProductProcess>.RegisterView(e => e.ProcessType, p => p.Process.Type);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType? ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WipProductProcess>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 操作人名称 EmployeeName
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Label("操作人")]
        public static readonly Property<string> EmployeeNameProperty = P<WipProductProcess>.RegisterView(e => e.EmployeeName, p => p.OperateBy.Name);

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 车间名称 WorkShopNameV
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameVProperty = P<WipProductProcess>.RegisterView(e => e.WorkShopNameV, p => p.Version.WorkOrder.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopNameV
        {
            get { return this.GetProperty(WorkShopNameVProperty); }
        }
        #endregion

        #endregion

        #region 上料采集单体条码列表 SinglelabelList
        /// <summary>
        /// 上料采集单体条码列表
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductProcessSinglelabel>> SinglelabelListProperty = P<WipProductProcess>.RegisterList(e => e.SinglelabelList);
        /// <summary>
        /// 上料采集单体条码列表
        /// </summary>
        public EntityList<WipProductProcessSinglelabel> SinglelabelList
        {
            get { return this.GetLazyList(SinglelabelListProperty); }
        }
        #endregion

        #region 胜制局中 InInning
        /// <summary>
        /// 胜制局中
        /// </summary>
        [Label("胜制局中")]
        public static readonly Property<bool?> InInningProperty = P<WipProductProcess>.Register(e => e.InInning);

        /// <summary>
        /// 胜制局中
        /// </summary>
        public bool? InInning
        {
            get { return this.GetProperty(InInningProperty); }
            set { this.SetProperty(InInningProperty, value); }
        }
        #endregion  
    }

    /// <summary>
    /// 生产采集记录 实体配置
    /// </summary>
    internal class WipProductProcessConfig : EntityConfig<WipProductProcess>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_PROCESS").MapAllProperties();
            Meta.Property(WipProductProcess.VersionIdProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(WipProductProcess.ResourceIdProperty, WipProductProcess.ProcessIdProperty);
            Meta.IndexGroupOnProperties(WipProductProcess.ProcessIdProperty, WipProductProcess.BarcodeProperty);
            Meta.EnablePhantoms();
        }
    }
}