using SIE.EMS.SpareParts;
using SIE.Warehouses;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Common.Prints.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 入库明细视图配置
    /// </summary>
    public class StoreDetailViewConfig : WebViewConfig<StoreDetail>
    {
        /// <summary>
        /// 入库明细添加视图
        /// </summary>
        public const string AddStoreDetailViewGroup = "AddStoreDetailViewGroup";

        /// <summary>
        /// 入库明细入库视图
        /// </summary>
        public const string StoreDetailViewGroup = "StoreDetailViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { AddStoreDetailViewGroup, StoreDetailViewGroup });

            if (ViewGroup == AddStoreDetailViewGroup)
            {
                ConfigAddStoreDetailView();
            }

            if (ViewGroup == StoreDetailViewGroup)
            {
                ConfigStoreDetailView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
				View.AddBehavior("SIE.Web.EMS.SpareParts.Behaviors.StoreDetailBehavior");
				View.UseCommand("SIE.Web.EMS.SpareParts.Commands.SearchStoreDetailCommand");
                View.UseCommand("SIE.Web.EMS.SpareParts.Commands.PrintStoreDetailLabelCommand");
				View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
				View.Property(p => p.LineNo).Readonly();
				View.Property(p => p.SparePartId).ShowInList(width: 120).Readonly();
				View.Property(p => p.SparePartName).Readonly();
				View.Property(p => p.Specification).Readonly();
				View.Property(p => p.SpartType).Readonly();
				View.Property(p => p.EquipModelName).Readonly();
				View.Property(p => p.ControlMethod).Readonly();
				View.Property(p => p.IsReplacement).Readonly();
				View.Property(p => p.IsOldPart).Readonly();
				View.Property(p => p.OutDepotLineNo).Readonly().ShowInList(width: 150);
                View.Property(p => p.QualityStatus).Readonly();
				View.Property(p => p.UnitPrice).UseSpinEditor(m => m.MinValue = 0).Readonly(p => p.InboundStatus == SIE.Equipments.Enums.InboundStatus.Done);
				View.Property(p => p.BatchNumber).ShowInList(width: 120).Readonly();
				View.Property(p => p.Sn).ShowInList(width: 120).Readonly();
				View.Property(p => p.InboundStatus).Readonly();
				View.Property(p => p.Number).Readonly();
				View.Property(p => p.UnitName).Readonly();
				View.Property(p => p.StorageLocationId).UseDataSource((e, c, r) =>
                {
                    var detail = e as StoreDetail;
                    var entity = detail.FindParentEntity() as SparePartStore;
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas(entity.WarehouseId, r, c);
                }).Readonly(p => p.InboundStatus == SIE.Equipments.Enums.InboundStatus.Done);
                View.Property(p => p.PurchaseOrderNo).Readonly().ShowInList(width: 120);
                View.Property(p => p.PurchaseOrderLineNo).Readonly();
            }
        }

        /// <summary>
        /// 入库明细添加视图
        /// </summary>
        protected void ConfigAddStoreDetailView()
        {
            View.DisableEditing();
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.StoreDetailDeleteCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.PrintStoreDetailLabelCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Show();
                View.Property(p => p.SparePartId).ShowInList(width: 120).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.ControlMethod).Show();
                View.Property(p => p.IsReplacement).Readonly().Show();
                View.Property(p => p.IsOldPart).Readonly().Show();
                View.Property(p => p.PartOutDepotDetailId).ShowInList(width: 150);
                View.Property(p => p.QualityStatus).Show();
                View.Property(p => p.UnitPrice).Show();
                View.Property(p => p.BatchNumber).ShowInList(width: 120).Show();
                View.Property(p => p.Sn).ShowInList(width: 120).Show();
                View.Property(p => p.Number).Show();
                View.Property(p => p.StorageLocationId).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 入库明细入库视图
        /// </summary>
        protected void ConfigStoreDetailView()
        {
            View.DisableEditing();
            View.UseCommand("SIE.Web.EMS.SpareParts.Commands.PrintStoreDetailLabelCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Show();
                View.Property(p => p.SparePartId).ShowInList(width: 120).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.ControlMethod).Show();
                View.Property(p => p.IsReplacement).Readonly().Show();
                View.Property(p => p.IsOldPart).Readonly().Show();
                View.Property(p => p.PartOutDepotDetailId).ShowInList(width: 150);
                View.Property(p => p.QualityStatus).Show();
                View.Property(p => p.UnitPrice).Show();
                View.Property(p => p.BatchNumber).ShowInList(width: 120).Show();
                View.Property(p => p.Sn).ShowInList(width: 120).Show();
                View.Property(p => p.Number).Show();
                View.Property(p => p.StorageLocationId).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}