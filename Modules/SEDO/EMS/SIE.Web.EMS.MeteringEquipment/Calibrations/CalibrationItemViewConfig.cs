using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations
{
	/// <summary>
	/// 计量设备定检项目视图配置
	/// </summary>
	public class CalibrationItemViewConfig : WebViewConfig<CalibrationItem>
	{

		/// <summary>
		/// 只读视图
		/// </summary>
		public readonly static string ReadonlyView = "ReadonlyView";

		/// <summary>
		/// 字符显示宽度
		/// </summary>
		private readonly static int charWidth = 20;
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup( ReadonlyView);
			
			if (ViewGroup == ReadonlyView)
			{
				ConfigReadOnlyView();
			}

		}
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommands("SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.SelCalibrationItemCommand", WebCommandNames.Delete);
			using (View.OrderProperties())
			{
				View.Property(p => p.Name).HasLabel("项目名称").Readonly().Show(ShowInWhere.All);
				View.Property(p => p.Method).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.MinValue).HasLabel("最小值").ShowInList(width: (charWidth * 4));
				View.Property(p => p.MaxValue).HasLabel("最大值").ShowInList(width: (charWidth * 4));
				View.Property(p => p.Unit).Readonly().Show(ShowInWhere.All);
				View.Property(p => p.Part).Show(ShowInWhere.All);
				View.Property(p => p.Consumable).Show(ShowInWhere.All);
				View.Property(p => p.Method).Show(ShowInWhere.All);
				View.Property(p => p.Standard).Show(ShowInWhere.All);
				View.Property(p => p.UseTime).Show(ShowInWhere.All);
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}



		/// <summary>
		/// 只读视图
		/// </summary>
		protected void ConfigReadOnlyView()
		{
			using (View.OrderProperties())
			{
				View.Property(p => p.Name).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.MinValue).HasLabel("最小值").ShowInList(width: (charWidth * 4)).Readonly();
				View.Property(p => p.MaxValue).HasLabel("最大值").ShowInList(width: (charWidth * 4)).Readonly();
				View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Part).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Consumable).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Standard).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.UseTime).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}

		/// <summary>
		/// 下拉选择视图
		/// </summary>
        protected override void ConfigSelectionView()
        {
			using (View.OrderProperties())
			{
				View.Property(p => p.Name).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.MinValue).HasLabel("最小值").ShowInList(width: (charWidth * 4));
				View.Property(p => p.MaxValue).HasLabel("最大值").ShowInList(width: (charWidth * 4));
				View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Part).Show(ShowInWhere.All);
				View.Property(p => p.Consumable).Show(ShowInWhere.All);
				View.Property(p => p.Method).Show(ShowInWhere.All);
				View.Property(p => p.Standard).Show(ShowInWhere.All);
				View.Property(p => p.UseTime).Show(ShowInWhere.All);
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}
    }
}