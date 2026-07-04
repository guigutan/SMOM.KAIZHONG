using SIE.EMS.AssetRequisitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetRequisitions
{
	/// <summary>
	/// 审核结果视图配置
	/// </summary>
	internal class AssetRequisitionAuditRecordViewConfig : WebViewConfig<AssetRequisitionAuditRecord>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.AuditEmployeeId);
			View.Property(p => p.AuditResult);
			View.Property(p => p.AuditDate);
			View.Property(p => p.AuditComment);
		}
	}
}
