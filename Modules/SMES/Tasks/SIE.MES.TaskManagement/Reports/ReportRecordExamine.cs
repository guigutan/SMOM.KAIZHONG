using DocumentFormat.OpenXml.Spreadsheet;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.EventMessages;
using SIE.Items;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.TaskManagement.Reports.Enums;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Security.Principal;
using Item = SIE.Items.Item;
using SourceType = SIE.MES.TaskManagement.Reports.Enums.SourceType;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录审核
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(ReportRecordUploadConfig))]
    [ConditionQueryType(typeof(ReportRecordExamineCriteria))]
    [Label("报工记录审核")]
    public class ReportRecordExamine : Entity<double>
    {
        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<ReportRecordExamine>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 关联工单 Wo
        /// <summary>
        /// 关联工单
        /// </summary>
        [Label("关联工单")]
        public static readonly Property<string> WoProperty = P<ReportRecordExamine>.Register(e => e.Wo);

        /// <summary>
        /// 关联工单
        /// </summary>
        public string Wo
        {
            get { return this.GetProperty(WoProperty); }
            set { this.SetProperty(WoProperty, value); }
        }
        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<ReportRecordExamine>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return this.GetProperty(DispatchQtyProperty); }
            set { this.SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 任务进度
        #region 报工数量 ReportQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<ReportRecordExamine>.Register(e => e.ReportQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
            set { this.SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 合格数量 OkQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<decimal> OkQtyProperty = P<ReportRecordExamine>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get { return this.GetProperty(OkQtyProperty); }
            set { this.SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<ReportRecordExamine>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)this.GetRefId(ProductIdProperty); }
            set { this.SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<ReportRecordExamine>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 责任人 Principal
        /// <summary>
        /// 责任人Id
        /// </summary>
        [Label("责任人")]
        public static readonly IRefIdProperty PrincipalIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

        /// <summary>
        /// 责任人Id
        /// </summary>
        public double PrincipalId
        {
            get { return (double)this.GetRefId(PrincipalIdProperty); }
            set { this.SetRefId(PrincipalIdProperty, value); }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrincipalProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.Principal, PrincipalIdProperty);

        /// <summary>
        /// 责任人
        /// </summary>
        public Employee Principal
        {
            get { return this.GetRefEntity(PrincipalProperty); }
            set { this.SetRefEntity(PrincipalProperty, value); }
        }
        #endregion

        #region 记录合格数 RecordOkQty
        /// <summary>
        /// 记录合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> RecordOkQtyProperty = P<ReportRecordExamine>.Register(e => e.RecordOkQty);

        /// <summary>
        /// 记录合格数
        /// </summary>
        public decimal RecordOkQty
        {
            get { return this.GetProperty(RecordOkQtyProperty); }
            set { this.SetProperty(RecordOkQtyProperty, value); }
        }
        #endregion

        #region 记录不合格数 RecordNgQty
        /// <summary>
        /// 记录不合格数
        /// </summary>
        [Label("不合格数")]
        public static readonly Property<decimal> RecordNgQtyProperty = P<ReportRecordExamine>.Register(e => e.RecordNgQty);

        /// <summary>
        /// 记录不合格数
        /// </summary>
        public decimal RecordNgQty
        {
            get { return this.GetProperty(RecordNgQtyProperty); }
            set { this.SetProperty(RecordNgQtyProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<ReportRecordExamine>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 返工数 ReworkQty
        /// <summary>
        /// 返工数
        /// </summary>
        [Label("返工数")]
        public static readonly Property<decimal> ReworkQtyProperty = P<ReportRecordExamine>.Register(e => e.ReworkQty);

        /// <summary>
        /// 返工数
        /// </summary>
        public decimal ReworkQty
        {
            get { return this.GetProperty(ReworkQtyProperty); }
            set { this.SetProperty(ReworkQtyProperty, value); }
        }
        #endregion

        #region 返工标识 IsRework
        /// <summary>
        /// 返工标识
        /// </summary>
        [Label("返工标识")]
        public static readonly Property<bool> IsReworkProperty = P<ReportRecordExamine>.Register(e => e.IsRework);

        /// <summary>
        /// 返工标识
        /// </summary>
        public bool IsRework
        {
            get { return this.GetProperty(IsReworkProperty); }
            set { this.SetProperty(IsReworkProperty, value); }
        }
        #endregion

        #region 检验状态 InspectionStatus
        /// <summary>
        /// 检验状态
        /// </summary>
        [Label("检验状态")]
        public static readonly Property<InspectionStatus?> InspectionStatusProperty = P<ReportRecordExamine>.Register(e => e.InspectionStatus);

        /// <summary>
        /// 检验状态
        /// </summary>
        public InspectionStatus? InspectionStatus
        {
            get { return this.GetProperty(InspectionStatusProperty); }
            set { this.SetProperty(InspectionStatusProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<ReportRecordExamine>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return this.GetProperty(InspectionResultProperty); }
            set { this.SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 处理方式 ProcessMode
        /// <summary>
        /// 处理方式
        /// </summary>
        [Label("处理方式")]
        public static readonly Property<string> ProcessModeProperty = P<ReportRecordExamine>.Register(e => e.ProcessMode);

        /// <summary>
        /// 处理方式
        /// </summary>
        public string ProcessMode
        {
            get { return this.GetProperty(ProcessModeProperty); }
            set { this.SetProperty(ProcessModeProperty, value); }
        }
        #endregion

        #region 审核状态 ExamineState
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ReportRecordExamineState> ExamineStateProperty = P<ReportRecordExamine>.Register(e => e.ExamineState);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ReportRecordExamineState ExamineState
        {
            get { return this.GetProperty(ExamineStateProperty); }
            set { this.SetProperty(ExamineStateProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double?)this.GetRefNullableId(WorkGroupIdProperty); }
            set { this.SetRefNullableId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return this.GetRefEntity(WorkGroupProperty); }
            set { this.SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty =
            P<ReportRecordExamine>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double? ShiftId
        {
            get { return (double?)this.GetRefNullableId(ShiftIdProperty); }
            set { this.SetRefNullableId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty =
            P<ReportRecordExamine>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return this.GetRefEntity(ShiftProperty); }
            set { this.SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 报工时间 ReportTime
        /// <summary>
        /// 报工时间
        /// </summary>
        [Label("报工时间")]
        public static readonly Property<DateTime?> ReportTimeProperty = P<ReportRecordExamine>.Register(e => e.ReportTime);

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? ReportTime
        {
            get { return this.GetProperty(ReportTimeProperty); }
            set { this.SetProperty(ReportTimeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ReportRecordExamine>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 创建时间 CreateTime
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateTime> CreateTimeProperty = P<ReportRecordExamine>.Register(e => e.CreateTime);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return this.GetProperty(CreateTimeProperty); }
            set { this.SetProperty(CreateTimeProperty, value); }
        }
        #endregion

        #region 工序流水码 Vornr
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> VornrProperty = P<ReportRecordExamine>.Register(e => e.Vornr);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
            set { this.SetProperty(VornrProperty, value); }
        }
        #endregion

        #region 控制码(工序控制码) Steus
        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        [Label("控制码(工序控制码)")]
        public static readonly Property<string> SteusProperty = P<ReportRecordExamine>.Register(e => e.Steus);

        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        public string Steus
        {
            get { return this.GetProperty(SteusProperty); }
            set { this.SetProperty(SteusProperty, value); }
        }
        #endregion

        #region 分单数量 Zcode
        /// <summary>
        /// 分单数量
        /// </summary>
        [Label("分单数量")]
        public static readonly Property<decimal> ZcodeProperty = P<ReportRecordExamine>.Register(e => e.Zcode);

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode
        {
            get { return this.GetProperty(ZcodeProperty); }
            set { this.SetProperty(ZcodeProperty, value); }
        }
        #endregion

        #region 是否已上传 UploadFlag
        /// <summary>
        /// 是否已上传
        /// </summary>
        [Label("是否已上传")]
        public static readonly Property<bool?> UploadFlagProperty = P<ReportRecordExamine>.Register(e => e.UploadFlag);

        /// <summary>
        /// 是否已上传
        /// </summary>
        public bool? UploadFlag
        {
            get { return this.GetProperty(UploadFlagProperty); }
            set { this.SetProperty(UploadFlagProperty, value); }
        }
        #endregion

        #region 上传结果 UploadResult
        /// <summary>
        /// 上传结果
        /// </summary>
        [Label("上传结果")]
        public static readonly Property<string> UploadResultProperty = P<ReportRecordExamine>.Register(e => e.UploadResult);

        /// <summary>
        /// 上传结果
        /// </summary>
        public string UploadResult
        {
            get { return this.GetProperty(UploadResultProperty); }
            set { this.SetProperty(UploadResultProperty, value); }
        }
        #endregion

        #region 批次标签 WipBatchNos
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> WipBatchNosProperty = P<ReportRecordExamine>.Register(e => e.WipBatchNos);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string WipBatchNos
        {
            get { return this.GetProperty(WipBatchNosProperty); }
            set { this.SetProperty(WipBatchNosProperty, value); }
        }
        #endregion

        #region 供应商批次 Lichas
        /// <summary>
        /// 供应商批次
        /// </summary>
        [Label("供应商批次")]
        public static readonly Property<string> LichasProperty = P<ReportRecordExamine>.Register(e => e.Lichas);

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string Lichas
        {
            get { return this.GetProperty(LichasProperty); }
            set { this.SetProperty(LichasProperty, value); }
        }
        #endregion

        #region 报工方式 SourceType
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<SourceType?> SourceTypeProperty = P<ReportRecordExamine>.Register(e => e.SourceType);

        /// <summary>
        /// 报工方式
        /// </summary>
        public SourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ReportRecordExamine>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ReportRecordExamine>.RegisterView(e => e.ProductCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<ReportRecordExamine>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<ReportRecordExamine>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<ReportRecordExamine>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ReportRecordExamine>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ReportRecordExamine>.RegisterView(e => e.ShortDescription, p => p.Product.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 数据源配置
    /// </summary>
    public class ReportRecordExamineConfig : EntityConfig<ReportRecordExamine>
    {
        /// <summary>
        /// 数据库
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<ReportRecord>("rr")
            .Join<ReportDispatchTask>("rdt", (rr, rdt) => rr.DispatchTaskId == rdt.Id && rdt.SQL<int>("rdt.IS_PHANTOM") == 0 && rdt.SQL<int?>("rdt.INV_ORG_ID") == RT.InvOrg)
            .Select<ReportDispatchTask>((rr, rdt) => new
            {
                Id = rr.Id,
                No = rdt.No,
                Wo = rdt.AssociatedWorkOrder,
                Dispatch_Qty = rdt.DispatchQty,
                Report_Qty = rr.ReportQty,
                Ok_Qty = rdt.OkQty,
                Ng_Qty = rdt.NgQty,
                Process_Id = rdt.ProcessId,
                Product_Id = rdt.ProductId,
                Resource_Id = rdt.ResourceId,
                Work_Shop_Id = rdt.WorkShopId,
                Batch_No = rr.BatchNo,
                Principal_Id = rr.PrincipalId,
                Record_Ok_Qty = rr.OkQty,
                Record_Ng_Qty = rr.NgQty,
                Suspect_Qty = rr.SuspectQty,
                Rework_Qty = rr.ReworkQty,
                Is_Rework = rr.IsRework,
                Inspection_Status = rr.InspectionStatus,
                Inspection_Result = rr.InspectionResult,
                Process_Mode = rr.ProcessMode,
                Examine_State = rr.ExamineState,
                Station_Id = rr.StationId,
                Work_Group_Id = rr.WorkGroupId,
                Shift_Id = rr.ShiftId,
                Report_Time = rr.ReportTime,
                Remark = rr.Remark,
                Create_Time = rr.CreateDate,
                Vornr = rr.Vornr,
                Steus = rr.Steus,
                Zcode = rr.Zcode,
                Upload_Flag = rr.UploadFlag,
                Upload_Result = rr.UploadResult,
                Source_Type = rr.SourceType,
            })
            .Where(rr => rr.SourceId == null && rr.SQL<int>("rr.IS_PHANTOM") == 0 && rr.SQL<int?>("rr.INV_ORG_ID") == RT.InvOrg)
            .ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.IsTreeEntity = false;
            Meta.DisablePhantoms();
            Meta.Property(ReportRecordExamine.WipBatchNosProperty).DontMapColumn();
            Meta.Property(ReportRecordExamine.LichasProperty).DontMapColumn();
        }
    }
}
