using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.Common
{
    /// <summary>
    /// 品质追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("质量信息追溯")]
	public partial class QmsTraceViewModel : ViewModel
    {
		#region 检验类型 InspectionType
		/// <summary>
		/// 检验类型
		/// </summary>
		[Label("检验类型")]
		public static readonly Property<string> InspectionTypeProperty = P<QmsTraceViewModel>.Register(e => e.InspectionType);

		/// <summary>
		/// 检验类型
		/// </summary>
		public string InspectionType
		{
			get { return GetProperty(InspectionTypeProperty); }
			set { SetProperty(InspectionTypeProperty, value); }
		}
		#endregion

		#region 检验单号 InspectionNo
		/// <summary>
		/// 检验单号
		/// </summary>
		[Label("检验单号")]
		public static readonly Property<string> InspectionNoProperty = P<QmsTraceViewModel>.Register(e => e.InspectionNo);

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
		public static readonly Property<string> InspectionResultProperty = P<QmsTraceViewModel>.Register(e => e.InspectionResult);

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
		public static readonly Property<string> FailedAuditResultProperty = P<QmsTraceViewModel>.Register(e => e.FailedAuditResult);

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
		public static readonly Property<string> DefectRecordProperty = P<QmsTraceViewModel>.Register(e => e.DefectRecord);

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
		public static readonly Property<string> FailedAuditWorkflowCodeProperty = P<QmsTraceViewModel>.Register(e => e.FailedAuditWorkflowCode);

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
		public static readonly Property<string> QualityWorkflowCodeProperty = P<QmsTraceViewModel>.Register(e => e.QualityWorkflowCode);

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
		public static readonly Property<string> InspectionByProperty = P<QmsTraceViewModel>.Register(e => e.InspectionBy);

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
		public static readonly Property<DateTime?> InspectionTimeProperty = P<QmsTraceViewModel>.Register(e => e.InspectionTime);

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