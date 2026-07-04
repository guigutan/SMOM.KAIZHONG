using SIE.EMS.AssetReturns;
using SIE.Web.Equipments.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetReturns
{
	/// <summary>
	/// 归还设备清单视图配置
	/// </summary>
	public class AssetReturnEquipmentViewConfig : WebViewConfig<AssetReturnEquipment>
	{
        /// <summary>
        /// 设备清单编辑视图
        /// </summary>
        public const string EditAssetReturnEquipmentViewGroup = "EditAssetReturnEquipmentViewGroup";

        /// <summary>
        /// 设备清单归还视图
        /// </summary>
        public const string ExistAssetReturnEquipmentViewGroup = "ExistAssetReturnEquipmentViewGroup";

        /// <summary>
        /// 配置视图属性
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { EditAssetReturnEquipmentViewGroup, ExistAssetReturnEquipmentViewGroup });

            if (ViewGroup == EditAssetReturnEquipmentViewGroup)
            {
                ConfigEditAssetReturnEquipmentView();
            }

            if (ViewGroup == ExistAssetReturnEquipmentViewGroup)
            {
                ConfigExistAssetReturnEquipmentView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.LineNo);
			View.Property(p => p.ReturnType);
			View.Property(p => p.EquipAccountCode);
			View.Property(p => p.EquipAccountName);
			View.Property(p => p.Alias);
			View.Property(p => p.EquipModelCode);
			View.Property(p => p.EquipModelName);
			View.Property(p => p.Specifications);
			View.Property(p => p.WorkshopId);
			View.Property(p => p.ResourceId);
			View.Property(p => p.Location);
			View.Property(p => p.DepositaryId);
		}

        ///<summary>
        /// 配置设备清单编辑视图
        /// </summary>
        protected void ConfigEditAssetReturnEquipmentView()
        {
            View.AddBehavior("SIE.Web.EMS.AssetReturns.Behaviors.AssetReturnEquipmentBehavior");
            using (View.OrderProperties())
            {
                View.WithoutPaging();
                View.UseGridSelectionModel(checkOnly: true);
                View.Property(p => p.LineNo).Readonly().DisableSort().Show();
                View.Property(p => p.ReturnType)
                    .Cascade(p => p.WorkshopId, null)
                    .Cascade(p => p.ResourceId, null)
                    .Cascade(p => p.Location, null)
                    .Cascade(p => p.DepositaryId, null).DisableSort().Show();
                View.Property(p => p.EquipAccountCode).Readonly().DisableSort().Show();
                View.Property(p => p.EquipAccountName).Readonly().DisableSort().Show();
                View.Property(p => p.Alias).Readonly().DisableSort().Show();
                View.Property(p => p.EquipModelCode).Readonly().DisableSort().Show();
                View.Property(p => p.EquipModelName).Readonly().DisableSort().Show();
                View.Property(p => p.Specifications).Readonly().DisableSort().Show();
                View.Property(p => p.WorkshopId).UseFactoryWorkshopEditor()
                    .Readonly(p => p.ReturnType != SIE.EMS.Enums.ReturnType.Real).DisableSort().Show();
                View.Property(p => p.ResourceId).UseWorkShopResourceEditor()
                    .Readonly(p => p.ReturnType != SIE.EMS.Enums.ReturnType.Real).DisableSort().Show();
                View.Property(p => p.Location).Readonly(p => p.ReturnType != SIE.EMS.Enums.ReturnType.Real).DisableSort().Show();
                View.Property(p => p.DepositaryId).Readonly(p => p.ReturnType != SIE.EMS.Enums.ReturnType.Real).DisableSort().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        ///<summary>
        /// 配置设备清单归还视图
        /// </summary>
        protected void ConfigExistAssetReturnEquipmentView()
        {
            using (View.OrderProperties())
            {
                View.WithoutPaging();
                View.DisableEditing();
                View.Property(p => p.ReturnNo).DisableSort().ShowInList(width:120).Show();
                View.Property(p => p.ApprovalStatus).DisableSort().Show();
                View.Property(p => p.LineNo).DisableSort().Show();
                View.Property(p => p.ReturnType).DisableSort().Show();
                View.Property(p => p.EquipAccountCode).DisableSort().Show();
                View.Property(p => p.EquipAccountName).DisableSort().Show();
                View.Property(p => p.Alias).DisableSort().Show();
                View.Property(p => p.EquipModelCode).DisableSort().Show();
                View.Property(p => p.EquipModelName).DisableSort().Show();
                View.Property(p => p.Specifications).DisableSort().Show();
                View.Property(p => p.WorkshopId).DisableSort().Show();
                View.Property(p => p.ResourceId).DisableSort().Show();
                View.Property(p => p.Location).DisableSort().Show();
                View.Property(p => p.DepositaryId).DisableSort().Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
