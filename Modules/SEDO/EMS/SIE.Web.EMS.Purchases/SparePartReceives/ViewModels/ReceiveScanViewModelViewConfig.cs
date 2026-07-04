using SIE.Domain;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.EMS.Purchases.SparePartReceives.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.SparePartReceives.ViewModels
{
    /// <summary>
    /// 备件接收界面
    /// </summary>
    internal class ReceiveScanViewModelViewConfig : WebViewConfig<ReceiveScanViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(SparePartReceive));
            View.AddBehavior("SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveScanBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.SparePartReceives.Commands.SaveReceiveScanCommand", "SIE.Web.EMS.Purchases.SparePartReceives.Commands.DetermineCommand",
                "SIE.Web.EMS.Purchases.SparePartReceives.Commands.DeterminePrintCommand");
            View.HasDetailColumnsCount(4);
            View.Property(p => p.ReceiveNo).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.FactoryName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.DepartmentName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.ReceiveType).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.SparePartReceiveDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as ReceiveScanViewModel;
                if (entity == null)
                {
                    return new EntityList<SparePartReceiveDetail>();
                }
                return RT.Service.Resolve<SparePartReceiveController>().GetDetailsByReceiveId(entity.SparePartReceiveId, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("SparePartId_Display", nameof(e.SparePartReceiveDetail.SparePartCode));
                keyValues.Add(nameof(e.SparePartId), nameof(e.SparePartReceiveDetail.SparePartId));
                keyValues.Add(nameof(e.SparePartName), nameof(e.SparePartReceiveDetail.SparePartName));
                keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePartReceiveDetail.ControlMethod));
                keyValues.Add(nameof(e.Qty), nameof(e.SparePartReceiveDetail.Qty));
                keyValues.Add(nameof(e.UnitName), nameof(e.SparePartReceiveDetail.UnitName));
                keyValues.Add(nameof(e.PartOutDepotDetailId), nameof(e.SparePartReceiveDetail.PartOutDepotDetailId));
                keyValues.Add(nameof(e.PartOutDepotDetailLineNo), nameof(e.SparePartReceiveDetail.OutDepotNo));
                keyValues.Add(nameof(e.SnPartOutDepotDetailLineNo), nameof(e.SparePartReceiveDetail.OutDepotNo));
                m.DicLinkField = keyValues;
            }).ShowInDetail(columnSpan: 1);
            View.Property(p => p.SparePartId).Readonly().HasLabel("备件编码").ShowInDetail(columnSpan: 1);
            View.Property(p => p.SparePartName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.PurchaseOrderLineNo).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.ControlMethod).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.Qty).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.RecivedQty).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.UnitName).Readonly().ShowInDetail(columnSpan: 1);

            //不是批次管控时显示此字段
            View.Property(p => p.CurrentQty).UseSpinEditor(p => p.MinValue = 0).ShowInDetail(columnSpan: 1).Visibility(p => p.ControlMethod != ControlMethod.Batch);
            //序列号管控时显示此字段\不是委外返厂时只读
            View.Property(p => p.StoreSummaryDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as ReceiveScanViewModel;
                if (entity == null)
                {
                    return new EntityList<StoreSummaryDetail>();
                }
                return RT.Service.Resolve<SparePartReceiveScanController>().GetStoreSummaryDetails(entity.PartOutDepotDetailId, pagingInfo, keyword);
            }).Visibility(p => p.ControlMethod == ControlMethod.Sn).Readonly(p => p.ReceiveType != ReceiveType.Outsourced);
            //批次管控时显示此字段
            View.Property(p => p.LotCount).Visibility(p => p.ControlMethod == ControlMethod.Batch).UseSpinEditor(p => p.MinValue = 1);
            //批次管控时显示此字段
            View.Property(p => p.LotQty).Visibility(p => p.ControlMethod == ControlMethod.Batch).UseSpinEditor(p => p.MinValue = 0);
            //物料管控时显示此字段
            View.Property(p => p.PartOutDepotDetailLineNo).Visibility(p => p.ControlMethod == ControlMethod.ItemCode || p.ControlMethod == null).Readonly();
            //批次管控和序列号管控时显示此字段
            View.Property(p => p.SnPartOutDepotDetailLineNo).Visibility(p => p.ControlMethod == ControlMethod.Batch || p.ControlMethod == ControlMethod.Sn)
                .Readonly().ShowInDetail(columnSpan: 2, width: "50%");

            //不是物料管控时显示此字段
            View.Property(p => p.Message).Visibility(p => p.ControlMethod != ControlMethod.ItemCode)
                .Readonly().ShowInDetail(columnSpan: 4, width: "100%");
            //批次管控时显示此字段
            View.Property(p => p.LotSn)
                .UseDisplayEditor(p => p.XType = "SparePartReceiveLotEditor").ShowInDetail(columnSpan: 2);
            //批次管控时显示此字段
            View.Property(p => p.FixedQty);

            //序列号管控时显示此字段
            View.Property(p => p.Sn).UseDisplayEditor(p => p.XType = "SparePartReceiveSnEditor");
            //序列号管控时显示此字段
            View.Property(p => p.ScanEquip).Visibility(p => p.ControlMethod == ControlMethod.Sn || p.ControlMethod == null).Readonly(p => p.ScanEquip);
            //序列号管控时显示此字段
            View.Property(p => p.ScanSn).Visibility(p => p.ControlMethod == ControlMethod.Sn || p.ControlMethod == null).Readonly(p => p.ScanSn);
            //序列号管控时显示此字段
            View.Property(p => p.ScanEquipAndSn).Visibility(p => p.ControlMethod == ControlMethod.Sn || p.ControlMethod == null)
#pragma warning disable S1125 // Boolean literals should not be redundant
                .Readonly(p => p.ReceiveType == ReceiveType.Outsourced || p.ScanEquipAndSn == true);
#pragma warning restore S1125 // Boolean literals should not be redundant

            View.AttachChildrenProperty(typeof(SparePartReceiveDetail), w => new EntityList<SparePartReceiveDetail>()).HasLabel("备件明细").HasOrderNo(1).UseViewGroup(SparePartReceiveDetailViewConfig.ScanView).Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(SparePartReceiveLot), w => new EntityList<SparePartReceiveLot>()).HasLabel("批次明细").HasOrderNo(2).UseViewGroup(SparePartReceiveLotViewConfig.ScanView).Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(SparePartReceiveSn), w => new EntityList<SparePartReceiveSn>()).HasLabel("序列号明细").HasOrderNo(3).UseViewGroup(SparePartReceiveSnViewConfig.ScanView).Show(ChildShowInWhere.All);
        }
    }
}
