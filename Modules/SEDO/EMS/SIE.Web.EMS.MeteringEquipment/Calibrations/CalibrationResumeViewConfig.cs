using SIE.EMS.MeteringEquipment.Calibrations; 

namespace SIE.Web.EMS.MeteringEquipment.Calibrations
{
	/// <summary>
	/// 计量设备定检操作记录视图配置
	/// </summary>
	public class CalibrationResumeViewConfig : WebViewConfig<CalibrationResume>
	{
		
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			using (View.OrderProperties())
			{
				View.Property(p => p.OperationType);
				View.Property(p => p.OperationDateTime);
				View.Property(p => p.InspectionResult);
				View.Property(p => p.OperatorId).HasLabel("操作人");
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}

	}
}