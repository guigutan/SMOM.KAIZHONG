using SIE.Domain;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.FixtureReceives
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
            View.AssignAuthorize(typeof(FixtureReceive));
            View.AddBehavior("SIE.Web.EMS.Purchases.FixtureReceives.ReceiveScanBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.FixtureReceives.Commands.SaveReceiveScanCommand",
                "SIE.Web.EMS.Purchases.FixtureReceives.Commands.DetermineCommand",
                "SIE.Web.EMS.Purchases.FixtureReceives.Commands.DeterminePrintCommand");
            View.HasDetailColumnsCount(4);
            View.Property(p => p.ReceiveNo).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.FactoryName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.DepartmentName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.ReceiveType).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.FixtureReceiveDetailId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as ReceiveScanViewModel;
                if (entity == null)
                {
                    return new EntityList<FixtureReceiveDetail>();
                }
                return RT.Service.Resolve<FixtureReceiveController>().GetDetailsByReceiveId(entity.FixtureReceiveId, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ModelCode), nameof(e.FixtureReceiveDetail.ModelCode));
                keyValues.Add(nameof(e.ModelName), nameof(e.FixtureReceiveDetail.ModelName));
                keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureReceiveDetail.ManageMode));
                keyValues.Add(nameof(e.Qty), nameof(e.FixtureReceiveDetail.Qty));
                keyValues.Add(nameof(e.RecivedQty), nameof(e.FixtureReceiveDetail.RecivedQty));
                keyValues.Add(nameof(e.UintName), nameof(e.FixtureReceiveDetail.UnitName));
                keyValues.Add(nameof(e.PuOrderLineNo), nameof(e.FixtureReceiveDetail.PuOrderLineNo));
                keyValues.Add(nameof(e.FixtureEncodeId), nameof(e.FixtureReceiveDetail.FixtureEncodeId));
                keyValues.Add("FixtureEncodeId_Display", nameof(e.FixtureReceiveDetail.FixtureEncodeCode));

                m.DicLinkField = keyValues;
            }).ShowInDetail(columnSpan: 1);
            View.Property(p => p.FixtureEncodeId).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ModelCode).HasLabel("型号编码").ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.PuOrderLineNo).HasLabel("采购订单").ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ManageMode).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.Qty).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.RecivedQty).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.UintName).Readonly().ShowInDetail(columnSpan: 1);
            View.Property(p => p.CurrentQty).UseSpinEditor(p => p.MinValue = 1).ShowInDetail(columnSpan: 1);

            View.Property(p => p.SnCode).UseDataSource((e, page, code) =>
            {
                return RT.Service.Resolve<FixtureReceiveScanController>().GetFixtureIDCode(page, code);
            }).Visibility(p => p.ManageMode == ManageMode.Number || p.ManageMode == null).ShowInDetail(columnSpan: 1)
            .Readonly(p => p.ReceiveType != ReceiveType.Outsourced);
            View.Property(p => p.ProductionDate).Visibility(p=>p.ManageMode == ManageMode.Number || p.ManageMode == null).Readonly(m=>m.ReceiveType== ReceiveType.Outsourced);
            View.Property(p => p.Maker).Visibility(p => p.ManageMode == ManageMode.Number || p.ManageMode == null).Readonly(m => m.ReceiveType == ReceiveType.Outsourced);
           
            View.Property(p => p.Message).Readonly().Visibility(p => p.ManageMode == ManageMode.Number || p.ManageMode == null).ShowInDetail(columnSpan: 4); 
            View.Property(p => p.Sn).UseDisplayEditor(p => p.XType = "FixturesReceivesSnEditor").Visibility(p => p.ManageMode== ManageMode.Number || p.ManageMode == null);
            View.Property(p => p.ScanSnCode).Readonly(p => p.ScanSnCode).Visibility(p => p.ManageMode ==ManageMode.Number || p.ManageMode == null);
            View.Property(p => p.ScanSn).Readonly(p => p.ScanSn).Visibility(p => p.ManageMode == ManageMode.Number || p.ManageMode == null);

#pragma warning disable S1125 // Boolean literals should not be redundant
            View.Property(p => p.ScanSnCodeAndSn).Readonly(p => p.ReceiveType == ReceiveType.Outsourced || p.ScanSnCodeAndSn == true)
                .Visibility(p => p.ManageMode == ManageMode.Number || p.ManageMode == null);
#pragma warning restore S1125 // Boolean literals should not be redundant

            View.AttachChildrenProperty(typeof(FixtureReceiveDetail),
               e => new EntityList<FixtureReceiveDetail>())
                .Show(ChildShowInWhere.All).HasLabel("工治具明细").ViewGroup = FixtureReceiveDetailViewConfig.ScanView;
            View.AttachChildrenProperty(typeof(FixtureReceiveSn),
                e => new EntityList<FixtureReceiveSn>()).Show(ChildShowInWhere.All).HasLabel("序列号").LazyLoad(false).ViewGroup= FixtureReceiveSnViewConfig.ScanView;
        }
    }
}