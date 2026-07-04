using SIE.Domain;
using SIE.Items;
using SIE.MES.PackingQC;
using SIE.MetaModel;
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
    /// 包装QC确认明细报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PackingDetailReportCriteria))]
    [Label("包装QC确认明细报表")]    
    public class PackingDetailReport : ViewModel
    {
        
        # region 工单号
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PackingDetailReport>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 蓝标
        /// <summary>
        /// 蓝标
        /// </summary>
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<PackingDetailReport>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 原蓝标
        /// <summary>
        /// 原蓝标
        /// </summary>
        [Label("原蓝标")]
        public static readonly Property<string> OldBlueLabelProperty = P<PackingDetailReport>.Register(e => e.OldBlueLabel);

        /// <summary>
        /// 原蓝标
        /// </summary>
        public string OldBlueLabel
        {
            get { return this.GetProperty(OldBlueLabelProperty); }
            set { this.SetProperty(OldBlueLabelProperty, value); }
        }
        #endregion

        #region 蓝标可装箱数量
        /// <summary>
        /// 蓝标可装箱数量
        /// </summary>
        [Label("蓝标可装箱数量")]
        public static readonly Property<int> BlueLableNumProperty = P<PackingDetailReport>.Register(e => e.BlueLableNum);

        /// <summary>
        /// 蓝标可装箱数量
        /// </summary>
        public int BlueLableNum
        {
            get { return this.GetProperty(BlueLableNumProperty); }
            set { this.SetProperty(BlueLableNumProperty, value); }
        }
        #endregion

        #region 蓝标已装箱数量
        /// <summary>
        /// 蓝标已装箱数量
        /// </summary>
        [Label("蓝标已装箱数量")]
        public static readonly Property<int> BlueLablePackingNumProperty = P<PackingDetailReport>.Register(e => e.BlueLablePackingNum);

        /// <summary>
        /// 蓝标已装箱数量
        /// </summary>
        public int BlueLablePackingNum
        {
            get { return this.GetProperty(BlueLablePackingNumProperty); }
            set { this.SetProperty(BlueLablePackingNumProperty, value); }
        }
        #endregion

        #region 蓝标未装箱数量
        /// <summary>
        /// 蓝标未装箱数量
        /// </summary>
        [Label("蓝标未装箱数量")]
        public static readonly Property<int> UnboxedQtyProperty = P<PackingDetailReport>.Register(e => e.UnboxedQty);

        /// <summary>
        /// 蓝标未装箱数量
        /// </summary>
        public int UnboxedQty
        {
            get { return this.GetProperty(UnboxedQtyProperty); }
            set { this.SetProperty(UnboxedQtyProperty, value); }
        }
        #endregion

        #region 装箱标识
        /// <summary>
        /// 装箱标识
        /// </summary>
        [Label("装箱标识")]
        public static readonly Property<PackIdentEnum> PackIdentProperty = P<PackingDetailReport>.Register(e => e.PackIdent);

        /// <summary>
        /// 装箱标识
        /// </summary>
        public PackIdentEnum PackIdent
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
        public static readonly Property<ConfirmEnum> ConfirmProperty = P<PackingDetailReport>.Register(e => e.Confirm);

        /// <summary>
        /// QC是否已确认
        /// </summary>
        public ConfirmEnum Confirm
        {
            get { return this.GetProperty(ConfirmProperty); }
            set { this.SetProperty(ConfirmProperty, value); }
        }
        #endregion
      

        #region 物料编码
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PackingDetailReport>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<PackingDetailReport>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion
             
      

        #region 生产资源编码
        /// <summary>
        /// 生产资源编码
        /// </summary>
        [Label("生产资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<PackingDetailReport>.Register(e => e.ResourceCode);

        /// <summary>
        /// 生产资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 生产资源名称
        /// <summary>
        /// 生产资源名称
        /// </summary>
        [Label("生产资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<PackingDetailReport>.Register(e => e.ResourceName);

        /// <summary>
        /// 生产资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion



        #region 是否已上传SAP
        /// <summary>
        /// 是否已上传SAP
        /// </summary>
        [Label("是否已上传SAP")]
        public static readonly Property<bool> IsUploadSapProperty = P<PackingDetailReport>.Register(e => e.IsUploadSap);

        /// <summary>
        /// 是否已上传SAP
        /// </summary>
        public bool IsUploadSap
        {
            get { return this.GetProperty(IsUploadSapProperty); }
            set { this.SetProperty(IsUploadSapProperty, value); }
        }
        #endregion

        #region 上传结果
        /// <summary>
        /// 上传结果
        /// </summary>
        [Label("上传结果")]
        public static readonly Property<string> UploadResultProperty = P<PackingDetailReport>.Register(e => e.UploadResult);

        /// <summary>
        /// 上传结果
        /// </summary>
        public string UploadResult
        {
            get { return this.GetProperty(UploadResultProperty); }
            set { this.SetProperty(UploadResultProperty, value); }
        }
        #endregion




        # region 标签号/SN码
        /// <summary>
        /// 标签号/SN码
        /// </summary>
        [Label("标签号/SN码")]
        public static readonly Property<string> ProductLabelProperty = P<PackingDetailReport>.Register(e => e.ProductLabel);

        /// <summary>
        /// 标签号/SN码
        /// </summary>
        public string ProductLabel
        {
            get { return this.GetProperty(ProductLabelProperty); }
            set { this.SetProperty(ProductLabelProperty, value); }
        }
        #endregion

        #region 是否已报工
        /// <summary>
        /// 是否已报工
        /// </summary>
        [Label("是否已报工")]
        public static readonly Property<ReportsTypeEnum> ReportsTypeProperty = P<PackingDetailReport>.Register(e => e.ReportsType);

        /// <summary>
        /// 是否已报工
        /// </summary>
        public ReportsTypeEnum ReportsType
        {
            get { return this.GetProperty(ReportsTypeProperty); }
            set { this.SetProperty(ReportsTypeProperty, value); }
        }
        #endregion

        #region 标签类型
        /// <summary>
        /// 标签类型
        /// </summary>
        [Label("标签类型")]
        public static readonly Property<LabelTypeEnum> LabelTypeProperty = P<PackingDetailReport>.Register(e => e.LabelType);

        /// <summary>
        /// 标签类型
        /// </summary>
        public LabelTypeEnum LabelType
        {
            get { return this.GetProperty(LabelTypeProperty); }
            set { this.SetProperty(LabelTypeProperty, value); }
        }
        #endregion

        #region 标签/SN数量  
        /// <summary>
        /// 标签/SN数量 
        /// </summary>
        [Label("标签/SN数量 ")]
        public static readonly Property<int> PackingNumProperty = P<PackingDetailReport>.Register(e => e.PackingNum);

        /// <summary>
        /// 标签/SN数量 
        /// </summary>
        public int PackingNum
        {
            get { return this.GetProperty(PackingNumProperty); }
            set { this.SetProperty(PackingNumProperty, value); }
        }
        #endregion

        #region 标签号
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchLabelProperty = P<PackingDetailReport>.Register(e => e.BatchLabel);

        /// <summary>
        /// 标签号
        /// </summary>
        public string BatchLabel
        {
            get { return this.GetProperty(BatchLabelProperty); }
            set { this.SetProperty(BatchLabelProperty, value); }
        }
        #endregion

        #region 测试值
        /// <summary>
        /// 测试值
        /// </summary>
        [Label("测试值")]
        public static readonly Property<string> TestValueProperty = P<PackingDetailReport>.Register(e => e.TestValue);

        /// <summary>
        /// 测试值
        /// </summary>
        public string TestValue
        {
            get { return this.GetProperty(TestValueProperty); }
            set { this.SetProperty(TestValueProperty, value); }
        }
        #endregion              

        #region 创建人
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly Property<string> CreateByNameProperty = P<PackingDetailReport>.Register(e => e.CreateByName);

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateByName
        {
            get { return this.GetProperty(CreateByNameProperty); }
            set { this.SetProperty(CreateByNameProperty, value); }
        }
        #endregion

        #region 创建时间
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateTime> CreateDateProperty = P<PackingDetailReport>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 修改人
        /// <summary>
        /// 修改人
        /// </summary>
        [Label("修改人")]
        public static readonly Property<string> UpdateByNameProperty = P<PackingDetailReport>.Register(e => e.UpdateByName);

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateByName
        {
            get { return this.GetProperty(UpdateByNameProperty); }
            set { this.SetProperty(UpdateByNameProperty, value); }
        }
        #endregion

        #region 修改时间
        /// <summary>
        /// 修改时间
        /// </summary>
        [Label("修改时间")]
        public static readonly Property<DateTime> UpdateDateProperty = P<PackingDetailReport>.Register(e => e.UpdateDate);

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { this.SetProperty(UpdateDateProperty, value); }
        }
        #endregion




    }






}
