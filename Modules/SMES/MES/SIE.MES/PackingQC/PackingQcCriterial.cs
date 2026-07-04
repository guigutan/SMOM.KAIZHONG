using SIE.Domain;
using SIE.MES.ItemLine;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC
{
    /// <summary>
    /// 包装采集查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("包装采集查询实体")]
    public class PackingQcCriterial:Criteria
    {
        #region 蓝标 BlueLabel
        /// <summary>
        /// 蓝标
        /// </summary>
        [Required]
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<PackingQcCriterial>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 装箱标识 PackIdent
        /// <summary>
        /// 装箱标识
        /// </summary>
        [Label("装箱标识")]
        public static readonly Property<PackIdentEnum?> PackIdentProperty = P<PackingQcCriterial>.Register(e => e.PackIdent);

        /// <summary>
        /// 装箱标识
        /// </summary>
        public PackIdentEnum? PackIdent
        {
            get { return this.GetProperty(PackIdentProperty); }
            set { this.SetProperty(PackIdentProperty, value); }
        }
        #endregion

        #region (QC)是否确认 Confirm
        /// <summary>
        /// (QC)是否确认
        /// </summary>
        [Label("(QC)是否确认")]
        public static readonly Property<ConfirmEnum?> ConfirmProperty = P<PackingQcCriterial>.Register(e => e.Confirm);

        /// <summary>
        /// (QC)是否确认
        /// </summary>
        public ConfirmEnum? Confirm
        {
            get { return this.GetProperty(ConfirmProperty); }
            set { this.SetProperty(ConfirmProperty, value); }
        }
        #endregion

        #region 工序标签 ProductLabel
        /// <summary>
        /// 工序标签
        /// </summary>
        [Label("标签号/SN")]
        public static readonly Property<string> ProductLabelProperty = P<PackingQcCriterial>.Register(e => e.ProductLabel);

        /// <summary>
        /// 工序标签
        /// </summary>
        public string ProductLabel
        {
            get { return this.GetProperty(ProductLabelProperty); }
            set { this.SetProperty(ProductLabelProperty, value); }
        }
        #endregion

        #region 批次标签 BatchLabel
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchLabelProperty = P<PackingQcCriterial>.Register(e => e.BatchLabel);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchLabel
        {
            get { return this.GetProperty(BatchLabelProperty); }
            set { this.SetProperty(BatchLabelProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PackingQcCriterial>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 报工类型 ReportsType
        /// <summary>
        /// 报工类型
        /// </summary>
        [Label("报工类型")]
        public static readonly Property<ReportsTypeEnum?> ReportsTypeProperty = P<PackingQcCriterial>.Register(e => e.ReportsType);

        /// <summary>
        /// 报工类型
        /// </summary>
        public ReportsTypeEnum? ReportsType
        {
            get { return this.GetProperty(ReportsTypeProperty); }
            set { this.SetProperty(ReportsTypeProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<PackingQcCriterial>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<PackingQcCriterial>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PackingQcCriterial>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<PackingQcCriterial>.Register(e => e.CreateDate);

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
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PackingQcController>().CriterialPackingQc(this);
        }
    }
}
