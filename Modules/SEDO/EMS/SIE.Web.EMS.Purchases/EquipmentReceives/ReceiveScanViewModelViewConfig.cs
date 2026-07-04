using SIE.Domain;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收界面
    /// </summary>
    internal class ReceiveScanViewModelViewConfig : WebViewConfig<ReceiveScanViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(EquipmentReceive));
            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentReceives.ReceiveScanBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentReceives.Commands.SaveReceiveScanCommand",
                "SIE.Web.EMS.Purchases.EquipmentReceives.Commands.DetermineCommand",
               "SIE.Web.EMS.Purchases.EquipmentReceives.Commands.DeterminePrintCommand");
            View.HasDetailColumnsCount(4);
            View.Property(p => p.ReceiveNo).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.FactoryName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.DepartmentName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.ReceiveType).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.EquipmentReceiveDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as ReceiveScanViewModel;
                if (entity == null)
                {
                    return new EntityList<EquipmentReceiveDetail>();
                }
                return RT.Service.Resolve<EquipmentReceiveController>().GetDetailsByReceiveId(entity.EquipmentReceiveId, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("EquipModelId_Display", nameof(e.EquipmentReceiveDetail.EquipModelCode));
                keyValues.Add(nameof(e.EquipModelId), nameof(e.EquipmentReceiveDetail.EquipModelId));
                keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipmentReceiveDetail.EquipModelName));
                keyValues.Add(nameof(e.Qty), nameof(e.EquipmentReceiveDetail.Qty));
                keyValues.Add(nameof(e.OldRecivedQty), nameof(e.EquipmentReceiveDetail.RecivedQty));
                keyValues.Add(nameof(e.ReceiveLineNo), nameof(e.EquipmentReceiveDetail.LineNo));
                m.DicLinkField = keyValues;
            }).ShowInDetail(columnSpan: 1).HasLabel("明细行号");
            View.Property(p => p.EquipModelId).HasLabel("型号编码").ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.EquipModelName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.Qty).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.RecivedQty).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.CurrentQty).Readonly(p => p.ReceiveType == ReceiveType.Outsourced).UseSpinEditor(p => p.MinValue = 1).ShowInDetail(columnSpan: 1);
            View.Property(p => p.EquipAccountId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as ReceiveScanViewModel;
                if (entity == null)
                {
                    return new EntityList<EquipAccount>();
                }
                return RT.Service.Resolve<EquipAccountController>().GetRepairAccounts(pagingInfo, keyword);
            }).Readonly(p => p.ReceiveType != ReceiveType.Outsourced || p.CurrentQty > 1).ShowInDetail(columnSpan: 1);
            View.Property(p => p.ScanEquip).ShowInDetail(columnSpan: 1).Readonly(p => p.ScanEquip);
            View.Property(p => p.Message).UseDisplayEditor(p => p.XType = "MessageTextEditor").Readonly().ShowInDetail(columnSpan: 3);
            View.Property(p => p.ScanSn).ShowInDetail(columnSpan: 1).Readonly(p => p.ScanSn);
            View.Property(p => p.Sn).UseDisplayEditor(p => p.XType = "EquipmentReceivesSnEditor").ShowInDetail(columnSpan: 3);
#pragma warning disable S1125 // Boolean literals should not be redundant
            View.Property(p => p.ScanEquipAndSn).ShowInDetail(columnSpan: 1).Readonly(p => p.ReceiveType == ReceiveType.Outsourced || p.ScanEquipAndSn == true);
#pragma warning restore S1125 // Boolean literals should not be redundant
            View.AttachChildrenProperty(typeof(ReceiveScanSnViewModel), e => new EntityList<ReceiveScanSnViewModel>()).Show(ChildShowInWhere.All).HasLabel("序列号");
        }
    }
}