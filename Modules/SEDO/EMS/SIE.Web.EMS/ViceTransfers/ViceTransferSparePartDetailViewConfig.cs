using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.ViceTransfers;
using SIE.EMS.Warehouses;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.ViceTransfers
{
    /// <summary>
    /// 
    /// </summary>
    public class ViceTransferSparePartDetailViewConfig : WebViewConfig<ViceTransferSparePartDetail>
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
                View.Property(p => p.LineNo).ShowInList().Readonly();
                View.Property(p => p.SparePartCode).ShowInList(120).Readonly();
                View.Property(p => p.SparePartName).ShowInList(120).Readonly();
                View.Property(p => p.Specification).ShowInList(120).Readonly();
                View.Property(p => p.SparePartType).ShowInList(80).Readonly();
                View.Property(p => p.ControlMethod).ShowInList(80).Readonly();
                View.Property(p => p.UnitName).ShowInList().Readonly();

                View.Property(p => p.StoreSummaryLotId).ShowInList(120);
                View.Property(p => p.StoreSummaryDetailId).ShowInList(120);
                View.Property(p => p.TransferQty).ShowInList();
                View.Property(p => p.QualityStatus).ShowInList().Readonly();
                View.Property(p => p.StorageLocationId).ShowInList();
                View.Property(p => p.TargetId).ShowInList();

                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }



        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferSparePartDetailBehavior");
            using (View.OrderProperties())
            {
                View.UseGridSelectionModel(checkOnly: true);
                View.UseCommand("SIE.Web.EMS.ViceTransfers.Commands.ViceCopyTransferCommand");
                View.Property(p => p.LineNo).ShowInList().Readonly();
                View.Property(p => p.SparePartCode).ShowInList().Readonly();
                View.Property(p => p.SparePartName).ShowInList().Readonly();
                View.Property(p => p.Specification).ShowInList().Readonly();
                View.Property(p => p.SparePartType).ShowInList().Readonly();
                View.Property(p => p.ControlMethod).ShowInList().Readonly();
                View.Property(p => p.RemainingDemandQty).ShowInList().Readonly();
                View.Property(p => p.UnitName).ShowInList().Readonly();

                //选择来源仓库有库存的批次号
                View.Property(p => p.StoreSummaryLotId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as ViceTransferSparePartDetail;
                    if (entity == null)
                    {
                        return new EntityList<StoreSummaryLot>();
                    }
                    return RT.Service.Resolve<ViceTransferController>().GetStoreSummaryLotList(entity, keyword, pagingInfo);
                }).UsePagingLookUpEditor().ShowInList(120).Readonly(p=>p.ControlMethod!=ControlMethod.Batch);

               // 可选来源仓库下库存状态为【入库】且质量状态符合的序列号
                View.Property(p => p.StoreSummaryDetailId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as ViceTransferSparePartDetail;
                    if (entity == null)
                    {
                        return new EntityList<StoreSummaryDetail>();
                    }
                    return RT.Service.Resolve<ViceTransferController>().GetStoreSummaryDetailList(entity, keyword, pagingInfo);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("StorageLocationId_Display", nameof(e.StoreSummaryDetail.StorageLocationCode));
                    keyValues.Add(nameof(e.StorageLocationId), nameof(e.StoreSummaryDetail.StorageLocationId));
                    m.DicLinkField = keyValues;
                }).ShowInList(120).Readonly(p => p.ControlMethod != ControlMethod.Sn);
                View.Property(p => p.TransferQty).HasLabel("调拨数量".L10N() + "*").ShowInList().Readonly(m => m.ControlMethod == ControlMethod.Sn).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; });
                View.Property(p => p.QualityStatus).ShowInList().Readonly();
                View.Property(p => p.StorageLocationId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as ViceTransferSparePartDetail;
                    if (entity == null)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<SIE.EMS.Warehouses.WarehouseController>().GetStorageLocations(entity.WarehouseId, pagingInfo, keyword);
                }).HasLabel("来源库位").ShowInList().Readonly(m => m.ControlMethod == ControlMethod.Sn);
                View.Property(p => p.SourceInventoryQty).ShowInList(150).Readonly();
                View.Property(p => p.TargetId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as ViceTransferSparePartDetail;
                    if (entity == null)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<SIE.EMS.Warehouses.WarehouseController>().GetStorageLocations(entity.TargetWarehouseId,  pagingInfo, keyword);
                }).ShowInList();

                View.Property(p => p.UpdateBy).Show( ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
