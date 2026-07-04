using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.AssetRequisitions
{
	/// <summary>
	/// 审核记录
	/// </summary>
	[ChildEntity, Serializable]
	[Label("审核记录")]
	public partial class AssetRequisitionAuditRecord : DataEntity
	{
        #region 审核人 AuditEmployee
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Label("审核人")]
        public static readonly IRefIdProperty AuditEmployeeIdProperty =
            P<AssetRequisitionAuditRecord>.RegisterRefId(e => e.AuditEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double AuditEmployeeId
        {
            get { return (double)this.GetRefId(AuditEmployeeIdProperty); }
            set { this.SetRefId(AuditEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AuditEmployeeProperty =
            P<AssetRequisitionAuditRecord>.RegisterRef(e => e.AuditEmployee, AuditEmployeeIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee AuditEmployee
        {
            get { return this.GetRefEntity(AuditEmployeeProperty); }
            set { this.SetRefEntity(AuditEmployeeProperty, value); }
        }
        #endregion

        #region 审核结果 AuditResult
        /// <summary>
        /// 审核结果
        /// </summary>
        [Label("审核结果")]
        public static readonly Property<AuditResult> AuditResultProperty = P<AssetRequisitionAuditRecord>.Register(e => e.AuditResult);

        /// <summary>
        /// 审核结果
        /// </summary>
        public AuditResult AuditResult
        {
            get { return this.GetProperty(AuditResultProperty); }
            set { this.SetProperty(AuditResultProperty, value); }
        }
        #endregion

        #region 审核时间 AuditDate
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime> AuditDateProperty = P<AssetRequisitionAuditRecord>.Register(e => e.AuditDate);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditDate
        {
            get { return this.GetProperty(AuditDateProperty); }
            set { this.SetProperty(AuditDateProperty, value); }
        }
        #endregion

        #region 审核意见 AuditComment
        /// <summary>
        /// 审核意见
        /// </summary>
        [Label("审核意见")]
        public static readonly Property<string> AuditCommentProperty = P<AssetRequisitionAuditRecord>.Register(e => e.AuditComment);

        /// <summary>
        /// 审核意见
        /// </summary>
        public string AuditComment
        {
            get { return this.GetProperty(AuditCommentProperty); }
            set { this.SetProperty(AuditCommentProperty, value); }
        }
        #endregion

        #region 资产领用 AssetRequisition
        /// <summary>
        /// 资产领用Id
        /// </summary>
        [Label("资产领用")]
        public static readonly IRefIdProperty AssetRequisitionIdProperty = P<AssetRequisitionAuditRecord>.RegisterRefId(e => e.AssetRequisitionId, ReferenceType.Parent);

        /// <summary>
        /// 资产领用Id
        /// </summary>
        public double AssetRequisitionId
        {
            get { return (double)GetRefId(AssetRequisitionIdProperty); }
            set { SetRefId(AssetRequisitionIdProperty, value); }
        }

        /// <summary>
        /// 资产领用
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisition> AssetRequisitionProperty = P<AssetRequisitionAuditRecord>.RegisterRef(e => e.AssetRequisition, AssetRequisitionIdProperty);

        /// <summary>
        /// 资产领用
        /// </summary>
        public AssetRequisition AssetRequisition
        {
            get { return GetRefEntity(AssetRequisitionProperty); }
            set { SetRefEntity(AssetRequisitionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 审核记录 实体配置
    /// </summary>
    internal class AssetRequisitionAuditRecordConfig : EntityConfig<AssetRequisitionAuditRecord>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_REQ_REC").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}
