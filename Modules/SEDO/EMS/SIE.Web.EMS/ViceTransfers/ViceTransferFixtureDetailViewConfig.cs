using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.ViceTransfers;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.ViceTransfers
{
    /// <summary>
    /// 工治具调拨明细
    /// </summary>
    public class ViceTransferFixtureDetailViewConfig : WebViewConfig<ViceTransferFixtureDetail>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string ReadOnlyListView = "ReadOnlyListView";


        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadOnlyListView);
            if (ViewGroup == ReadOnlyListView)
            {
                GetReadOnlyListView();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void GetReadOnlyListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(80).Readonly();
                View.Property(p => p.FixtureEncodeCode).ShowInList(120).Readonly();
                View.Property(p => p.ModelCode).ShowInList().Readonly();
                View.Property(p => p.ModelName).ShowInList().Readonly();
                View.Property(p => p.FixtureTypeCode).ShowInList().Readonly();
                View.Property(p => p.ManageMode).ShowInList().Readonly();
                View.Property(p => p.FixtureStatus).ShowInList(100);
                View.Property(p => p.FixtureIDAccountId).ShowInList(140);
                View.Property(p => p.TransferQty).ShowInList(80);
                View.Property(p => p.UintName).ShowInList().Readonly();
                View.Property(p => p.FixtureQualityState).ShowInList().Readonly();
                View.Property(p => p.StorageLocationId).ShowInList(120);
                View.Property(p => p.TargetId).ShowInList(160);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferFixtureDetailBehavior");
            using (View.OrderProperties())
            {
                View.UseGridSelectionModel(checkOnly: true);
                View.UseCommand("SIE.Web.EMS.ViceTransfers.Commands.ViceCopyTransferCommand");
                View.Property(p => p.LineNo).ShowInList(80).Readonly();

                View.Property(p => p.FixtureEncodeCode).ShowInList(120).Readonly();
                View.Property(p => p.ModelCode).ShowInList().Readonly();
                View.Property(p => p.ModelName).ShowInList().Readonly();
                View.Property(p => p.FixtureTypeCode).ShowInList().Readonly();
                View.Property(p => p.ManageMode).ShowInList().Readonly();
                View.Property(p => p.RemainingDemandQty).ShowInList(120).Readonly();
                View.Property(p => p.FixtureStatus).ShowInList(100);
                View.Property(p => p.FixtureIDAccountId).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as ViceTransferFixtureDetail;
                        if (entity == null)
                        {
                            return new EntityList<FixtureIDAccount>();
                        }
                        return RT.Service.Resolve<ViceTransferController>().GetFixtureIDAccounts(entity, pagingInfo, keyword);
                    }).ShowInList(140).Readonly(p => p.ManageMode == ManageMode.Code);
                View.Property(p => p.TransferQty).HasLabel("调拨数量".L10N()+"*").ShowInList().Readonly(p => p.ManageMode == ManageMode.Number)
                    .UseSpinEditor(p => { p.MinValue = 1; p.AllowDecimals = false; });
                View.Property(p => p.UintName).ShowInList(60).Readonly();
                View.Property(p => p.FixtureQualityState).ShowInList().Readonly();
                View.Property(p => p.StorageLocationId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as ViceTransferFixtureDetail;
                    if (entity == null)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<SIE.EMS.Warehouses.WarehouseController>().GetStorageLocations(entity.WarehouseId, pagingInfo, keyword);
                }).HasLabel("来源库位*").ShowInList(180).Readonly(m => m.ManageMode == ManageMode.Number || (m.ManageMode == ManageMode.Code && m.FixtureStatus == FixtureStatus.OnLine));
                View.Property(p => p.SourceInventoryQty).ShowInList(130).Readonly();
                View.Property(p => p.OnlineQty).ShowInList(60).Readonly();
                View.Property(p => p.TargetId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as ViceTransferFixtureDetail;
                    if (entity == null)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<SIE.EMS.Warehouses.WarehouseController>().GetStorageLocations(entity.TargetWarehouseId, pagingInfo, keyword);
                }).ShowInList(120);
                View.Property(p => p.WorkShop).Readonly().ShowInList(80);
                View.Property(p => p.Resoures).Readonly().ShowInList(80);

                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
