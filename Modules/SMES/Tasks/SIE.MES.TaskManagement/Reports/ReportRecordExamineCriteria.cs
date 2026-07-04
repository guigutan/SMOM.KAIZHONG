using SIE.Common;
using SIE.Domain;
using SIE.MES.TaskManagement.Reports.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceType = SIE.MES.TaskManagement.Reports.Enums.SourceType;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录审核查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("报工记录审核查询实体")]
    public class ReportRecordExamineCriteria : Criteria
    {
        #region 任务单 No
        /// <summary>
        /// 任务单
        /// </summary>
        [Label("任务单")]
        public static readonly Property<string> NoProperty = P<ReportRecordExamineCriteria>.Register(e => e.No);

        /// <summary>
        /// 任务单
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 工单 Wo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoProperty = P<ReportRecordExamineCriteria>.Register(e => e.Wo);

        /// <summary>
        /// 工单
        /// </summary>
        public string Wo
        {
            get { return this.GetProperty(WoProperty); }
            set { this.SetProperty(WoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ReportRecordExamineCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ReportRecordExamineCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<ReportRecordExamineCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<ReportRecordExamineCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
            P<ReportRecordExamineCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<ReportRecordExamineCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<ReportRecordExamineCriteria>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        #region 检验状态 InspectionStatus
        /// <summary>
        /// 检验状态
        /// </summary>
        [Label("检验状态")]
        public static readonly Property<InspectionStatus?> InspectionStatusProperty = P<ReportRecordExamineCriteria>.Register(e => e.InspectionStatus);

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
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<ReportRecordExamineCriteria>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return this.GetProperty(InspectionResultProperty); }
            set { this.SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 审核状态 ExamineState
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ReportRecordExamineState?> ExamineStateProperty = P<ReportRecordExamineCriteria>.Register(e => e.ExamineState);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ReportRecordExamineState? ExamineState
        {
            get { return this.GetProperty(ExamineStateProperty); }
            set { this.SetProperty(ExamineStateProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ReportRecordExamineCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<ReportRecordExamineCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 报工时间 ReportTime
        /// <summary>
        /// 报工时间
        /// </summary>
        [Label("报工时间")]
        public static readonly Property<DateRange> ReportTimeProperty = P<ReportRecordExamineCriteria>.Register(e => e.ReportTime);

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateRange ReportTime
        {
            get { return this.GetProperty(ReportTimeProperty); }
            set { this.SetProperty(ReportTimeProperty, value); }
        }
        #endregion

        #region 批次标签 WipBatchNos
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> WipBatchNosProperty = P<ReportRecordExamineCriteria>.Register(e => e.WipBatchNos);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string WipBatchNos
        {
            get { return this.GetProperty(WipBatchNosProperty); }
            set { this.SetProperty(WipBatchNosProperty, value); }
        }
        #endregion

        #region 报工方式 SourceType
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<SourceType?> SourceTypeProperty = P<ReportRecordExamineCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 报工方式
        /// </summary>
        public SourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ReportRecordExamineCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 供应商批次 Licha
        /// <summary>
        /// 供应商批次
        /// </summary>
        [Label("供应商批次")]
        public static readonly Property<string> LichaProperty = P<ReportRecordExamineCriteria>.Register(e => e.Licha);

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string Licha
        {
            get { return this.GetProperty(LichaProperty); }
            set { this.SetProperty(LichaProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ReportController>().QueryReportRecordExamine(this);
        }
    }
}
