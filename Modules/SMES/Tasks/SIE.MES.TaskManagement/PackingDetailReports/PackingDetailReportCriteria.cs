using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingDetailReports
{
    /// <summary>
    /// 包装QC确认明细报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("包装QC确认明细报表查询实体")]
    public class PackingDetailReportCriteria : Criteria
    {

        #region 蓝标
        /// <summary>
        /// 蓝标
        /// </summary>
        [Required]
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<PackingDetailReportCriteria>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 装箱标识
        /// <summary>
        /// 装箱标识
        /// </summary>
        [Label("装箱标识")]
        public static readonly Property<PackIdentEnum?> PackIdentProperty = P<PackingDetailReportCriteria>.Register(e => e.PackIdent);

        /// <summary>
        /// 装箱标识
        /// </summary>
        public PackIdentEnum? PackIdent
        {
            get { return this.GetProperty(PackIdentProperty); }
            set { this.SetProperty(PackIdentProperty, value); }
        }
        #endregion

        #region QC是否已确认
        /// <summary>
        /// QC是否已确认
        /// </summary>
        [Label("QC是否已确认")]
        public static readonly Property<ConfirmEnum?> ConfirmProperty = P<PackingDetailReportCriteria>.Register(e => e.Confirm);

        /// <summary>
        /// QC是否已确认
        /// </summary>
        public ConfirmEnum? Confirm
        {
            get { return this.GetProperty(ConfirmProperty); }
            set { this.SetProperty(ConfirmProperty, value); }
        }
        #endregion

        #region 标签号/SN码
        /// <summary>
        /// 标签号/SN码
        /// </summary>
        [Label("标签号/SN码")]
        public static readonly Property<string> ProductLabelProperty = P<PackingDetailReportCriteria>.Register(e => e.ProductLabel);

        /// <summary>
        /// 标签号/SN码
        /// </summary>
        public string ProductLabel
        {
            get { return this.GetProperty(ProductLabelProperty); }
            set { this.SetProperty(ProductLabelProperty, value); }
        }
        #endregion

        #region 标签号
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchLabelProperty = P<PackingDetailReportCriteria>.Register(e => e.BatchLabel);

        /// <summary>
        /// 标签号
        /// </summary>
        public string BatchLabel
        {
            get { return this.GetProperty(BatchLabelProperty); }
            set { this.SetProperty(BatchLabelProperty, value); }
        }
        #endregion

        #region 工单号
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PackingDetailReportCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 是否已报工
        /// <summary>
        /// 是否已报工
        /// </summary>
        [Label("是否已报工")]
        public static readonly Property<ReportsTypeEnum?> ReportsTypeProperty = P<PackingDetailReportCriteria>.Register(e => e.ReportsType);

        /// <summary>
        /// 是否已报工
        /// </summary>
        public ReportsTypeEnum? ReportsType
        {
            get { return this.GetProperty(ReportsTypeProperty); }
            set { this.SetProperty(ReportsTypeProperty, value); }
        }
        #endregion

        #region 生产资源
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<PackingDetailReportCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<PackingDetailReportCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 物料编码
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PackingDetailReportCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 创建时间
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<PackingDetailReportCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PackingDetailReportController>().Fetch(this);
        }




    }
}
