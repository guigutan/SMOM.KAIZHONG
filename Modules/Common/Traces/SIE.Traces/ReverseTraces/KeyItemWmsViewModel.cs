using SIE.Domain;
using SIE.ObjectModel;
using SIE.Traces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Traces.ReverseTraces
{
    /// <summary>
	/// 反向追溯-工序采集-关键件-入库明细
	/// </summary>
	[RootEntity, Serializable]
    [Label("入库明细")]
    public class KeyItemWmsViewModel : ViewModel
    {
        #region Asn明细Id AsnDetailId
        /// <summary>
        /// Asn明细Id
        /// </summary>
        [Label("Asn明细Id")]
        public static readonly Property<double> AsnDetailIdProperty = P<KeyItemWmsViewModel>.Register(e => e.AsnDetailId);
        /// <summary>
        /// Asn明细Id
        /// </summary>
        public double AsnDetailId
        {
            get { return GetProperty(AsnDetailIdProperty); }
            set { SetProperty(AsnDetailIdProperty, value); }
        }
        #endregion

        #region Asn单号 AsnNo
        /// <summary>
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public static readonly Property<string> AsnNoProperty = P<KeyItemWmsViewModel>.Register(e => e.AsnNo);
        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo
        {
            get { return GetProperty(AsnNoProperty); }
            set { SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 供应商 SupplierName
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        public static readonly Property<string> SupplierNameProperty = P<KeyItemWmsViewModel>.Register(e => e.SupplierName);
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName
        {
            get { return GetProperty(SupplierNameProperty); }
            set { SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 生产批次 ProductionLot
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> ProductionLotProperty = P<KeyItemWmsViewModel>.Register(e => e.ProductionLot);
        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductionLot
        {
            get { return GetProperty(ProductionLotProperty); }
            set { SetProperty(ProductionLotProperty, value); }
        }
        #endregion

        #region 物料批次 ItemLot
        /// <summary>
        /// 物料批次
        /// </summary>
        [Label("物料批次")]
        public static readonly Property<string> ItemLotProperty = P<KeyItemWmsViewModel>.Register(e => e.ItemLot);
        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemLot
        {
            get { return GetProperty(ItemLotProperty); }
            set { SetProperty(ItemLotProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<KeyItemWmsViewModel>.Register(e => e.ProductionDate);
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return GetProperty(ProductionDateProperty); }
            set { SetProperty(ProductionDateProperty, value); }
        }
        #endregion

        #region 收货日期 CollectDate
        /// <summary>
        /// 收货日期
        /// </summary>
        [Label("收货日期")]
        public static readonly Property<DateTime?> CollectDateProperty = P<KeyItemWmsViewModel>.Register(e => e.CollectDate);
        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime? CollectDate
        {
            get { return GetProperty(CollectDateProperty); }
            set { SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 检验单号 InspectionNo
        /// <summary>
        /// 检验单号
        /// </summary>
        [Label("检验单号")]
        public static readonly Property<string> InspectionNoProperty = P<KeyItemWmsViewModel>.Register(e => e.InspectionNo);

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionNo
        {
            get { return GetProperty(InspectionNoProperty); }
            set { SetProperty(InspectionNoProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<string> InspectionResultProperty = P<KeyItemWmsViewModel>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public string InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 不合格处理方式 FailedAuditResult
        /// <summary>
        /// 不合格处理方式
        /// </summary>
        [Label("不合格处理方式")]
        public static readonly Property<string> FailedAuditResultProperty = P<KeyItemWmsViewModel>.Register(e => e.FailedAuditResult);

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public string FailedAuditResult
        {
            get { return GetProperty(FailedAuditResultProperty); }
            set { SetProperty(FailedAuditResultProperty, value); }
        }
        #endregion

        #region 缺陷记录 DefectRecord
        /// <summary>
        /// 缺陷记录
        /// </summary>
        [Label("缺陷记录")]
        public static readonly Property<string> DefectRecordProperty = P<KeyItemWmsViewModel>.Register(e => e.DefectRecord);

        /// <summary>
        /// 缺陷记录
        /// </summary>
        public string DefectRecord
        {
            get { return GetProperty(DefectRecordProperty); }
            set { SetProperty(DefectRecordProperty, value); }
        }
        #endregion

        #region 不合格审核流程编码 FailedAuditWorkflowCode
        /// <summary>
        /// 不合格审核流程编码
        /// </summary>
        [Label("不合格审核流程编码")]
        public static readonly Property<string> FailedAuditWorkflowCodeProperty = P<KeyItemWmsViewModel>.Register(e => e.FailedAuditWorkflowCode);

        /// <summary>
        /// 不合格审核流程编码
        /// </summary>
        public string FailedAuditWorkflowCode
        {
            get { return GetProperty(FailedAuditWorkflowCodeProperty); }
            set { SetProperty(FailedAuditWorkflowCodeProperty, value); }
        }
        #endregion

        #region 质量改进流程编码 QualityWorkflowCode
        /// <summary>
        /// 质量改进流程编码
        /// </summary>
        [Label("质量改进流程编码")]
        public static readonly Property<string> QualityWorkflowCodeProperty = P<KeyItemWmsViewModel>.Register(e => e.QualityWorkflowCode);

        /// <summary>
        /// 质量改进流程编码
        /// </summary>
        public string QualityWorkflowCode
        {
            get { return GetProperty(QualityWorkflowCodeProperty); }
            set { SetProperty(QualityWorkflowCodeProperty, value); }
        }
        #endregion

        #region 检验员名称 InspectionBy
        /// <summary>
        /// 检验员名称
        /// </summary>
        [Label("检验员名称")]
        public static readonly Property<string> InspectionByProperty = P<KeyItemWmsViewModel>.Register(e => e.InspectionBy);

        /// <summary>
        /// 检验员名称
        /// </summary>
        public string InspectionBy
        {
            get { return GetProperty(InspectionByProperty); }
            set { SetProperty(InspectionByProperty, value); }
        }
        #endregion

        #region 检验时间 InspectionTime
        /// <summary>
        /// 检验时间
        /// </summary>
        [Label("检验时间")]
        public static readonly Property<DateTime?> InspectionTimeProperty = P<KeyItemWmsViewModel>.Register(e => e.InspectionTime);

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectionTime
        {
            get { return GetProperty(InspectionTimeProperty); }
            set { SetProperty(InspectionTimeProperty, value); }
        }
        #endregion
    }
}
