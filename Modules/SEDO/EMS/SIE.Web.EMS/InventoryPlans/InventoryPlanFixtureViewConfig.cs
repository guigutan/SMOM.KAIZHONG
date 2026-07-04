using SIE.Core.Equipments;
using SIE.EMS.InventoryPlans;
using SIE.Fixtures;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Models;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.InventoryPlans
{
   /// <summary>
   /// 工治具盘点范围
   /// </summary>
    public class InventoryPlanFixtureViewConfig : WebViewConfig<InventoryPlanFixture>
    {

        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DisableEditing();
            View.UseDetail(3);
            View.Property(p => p.FixtureTypes).ShowInDetail(columnSpan: 4); 
            View.Property(p => p.FixtureModels).ShowInDetail(columnSpan: 4); 
            View.Property(p => p.FixtureEncodes).ShowInDetail(columnSpan: 4); 
            View.Property(p => p.ManageMode).Cascade(p=> p.IsFixAsset,null);
            View.Property(p => p.IsFixAsset);
            View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = SIE.Core.Equipments.EquipType.EquipTypeCatalogType; p.ReloadDataOnPopping = true; });
            View.Property(p => p.AssetOwnerId);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(InventoryPlan));
            View.UseDetail(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.FixtureTypes).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(FixtureType).FullName;
                    p.LinkField = InventoryPlanFixture.FixtureTypeIdsProperty.Name;
                    p.DisplayField = FixtureType.CodeProperty.Name;
                    p.XType = "FixtureTypesComboPopup";
                    p.Editable = false;
                    p.Separator = ",";
                }).ShowInDetail(columnSpan: 4);
                View.Property(p => p.FixtureModels).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(FixtureModel).FullName;
                    p.LinkField = InventoryPlanFixture.FixtureModelIdsProperty.Name;
                    p.DisplayField = FixtureModel.NameProperty.Name;
                    p.XType = "FixtureModelsComboPopup";
                    p.Editable = false;
                    p.Separator = ",";
                }).ShowInDetail(columnSpan: 4);

                View.Property(p => p.FixtureEncodes).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(FixtureEncode).FullName;
                    p.LinkField = InventoryPlanFixture.FixtureEncodeIdsProperty.Name;
                    p.DisplayField = FixtureEncode.CodeProperty.Name;
                    p.XType = "FixturesEncodesComboPopup";
                    p.Editable = false;
                    p.Separator = ",";
                }).ShowInDetail(columnSpan: 4);
                View.Property(p => p.ManageMode).Cascade(p => p.IsFixAsset, null).ShowInDetail();
                View.Property(p => p.IsFixAsset).Cascade(p => p.AssetsCategory, null).Cascade(p => p.AssetOwnerId, null).Show().Readonly(p => p.ManageMode != ManageMode.Number);
                View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.ReloadDataOnPopping = true; })
                    .Readonly(p => p.IsFixAsset != YesNo.Yes).Show();
                View.Property(p => p.AssetOwnerId).Readonly(p => p.IsFixAsset != YesNo.Yes).Show();
               
            }
        }
    }
}
