using SIE.EMS.Enums;
using SIE.EMS.Lubrications;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.MetaModel.View;
using SIE.Web.EMS.Lubrications.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Lubrications
{
    /// <summary>
    /// 润滑备件更换视图配置
    /// </summary>
    public class LubricationSparePartViewConfig : WebViewConfig<LubricationSparePart>
	{
		/// <summary>
		/// 查看记录
		/// </summary>
		public const string SeeView = "SeeView";

		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(SeeView);
			if (ViewGroup == SeeView)
			{
				ConfigSeeView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommands(typeof(SelEquipBomCommand).FullName, typeof(SelSparePartCommand).FullName);
			View.UseCommands(typeof(ExeChangeSparePartCommand).FullName);
			View.UseCommands(WebCommandNames.Edit, typeof(DeleteLubricationSparePartCommand).FullName);

			using (View.OrderProperties())
			{
				View.Property(p => p.SparePartCodeView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
				View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.PartOutDepotDetailId)
					.UsePagingLookUpEditor((m, e) =>
					{
						Dictionary<string, string> dic = new Dictionary<string, string>();
						
						dic.Add(nameof(e.RemainingQty), nameof(e.PartOutDepotDetail.RemainingQty));
						dic.Add(nameof(e.BatchNoView), nameof(e.PartOutDepotDetail.BatchNoView));
						dic.Add(nameof(e.SeriaNoView), nameof(e.PartOutDepotDetail.SeriaNoView));
						m.DicLinkField = dic;
						m.DisplayField = LubricationSparePart.OutDepotNoViewProperty.Name;
						m.BindDisplayField = LubricationSparePart.OutDepotNoViewProperty.Name;
					}).UseDataSource((s, p, k) =>
					{
						var entity = s as LubricationSparePart;
						var lubrication = entity.Lubrication;
						var outDtl = RT.Service.Resolve<OutDepotController>()
							.GetPartOutDepotDtls(entity.SparePartId, lubrication.EquipAccountId, lubrication.EquipAccount?.EquipModelId??0,
							lubrication.LubricationNo, p, k);
						return outDtl;
					}).ShowInList(150).Readonly(p => p.State == ChangeSparePartState.Finished);
				View.Property(p => p.RemainingQty).Show(ShowInWhere.All).Readonly();
				
				View.Property(p => p.State).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.ChangeQty).Show(ShowInWhere.All)
					.UseSpinEditor(m => m.MinValue = 1)
					.Readonly(p => p.State == ChangeSparePartState.Finished || p.ControlMethod == SIE.EMS.SpareParts.Enums.ControlMethod.Sn);
				View.Property(p => p.ControlMethod).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.BatchNoView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.SeriaNoView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.OldSequence).Show(ShowInWhere.All).Readonly(p => p.State == ChangeSparePartState.Finished);
				View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly(p => p.State == ChangeSparePartState.Finished);

				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}

		/// <summary>
		/// 查看视图
		/// </summary>
		public void ConfigSeeView() {

			View.FormEdit();
			using (View.OrderProperties())
			{
				View.Property(p => p.SparePartCodeView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
				View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.PartOutDepotDetailId).UsePagingLookUpEditor((m, e) =>
					{
						m.DisplayField = LubricationSparePart.OutDepotNoViewProperty.Name;
						m.BindDisplayField = LubricationSparePart.OutDepotNoViewProperty.Name;
					}).ShowInList(150).Readonly();
				View.Property(p => p.RemainingQty).Show(ShowInWhere.All).Readonly();
				
				View.Property(p => p.State).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.ChangeQty).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.ControlMethod).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.BatchNoView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.SeriaNoView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.OldSequence).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly();
				View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
				View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
			}
		}
	}
}