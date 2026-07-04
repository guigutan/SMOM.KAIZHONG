using SIE.Domain;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.MetaModel.View;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases.SparePartReceives.Commands;
using SIE.Web.Resources;
using System;

namespace SIE.Web.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收视图配置
    /// </summary>
    internal class SparePartReceiveViewConfig : WebViewConfig<SparePartReceive>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands("SIE.Web.EMS.Purchases.SparePartReceives.Commands.AddSparePartReceiveCommand",
                "SIE.Web.EMS.Purchases.SparePartReceives.Commands.EditSparePartReceiveCommand", typeof(DeleteSparePartReceiveCommand).FullName
                , "SIE.Web.EMS.Purchases.SparePartReceives.Commands.ReceiveScanCommand", typeof(SubmitSparePartReceiveCommand).FullName,
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.ReceiveNo).ShowInList(130);
            View.Property(p => p.ReceiveBillStatus).HasLabel("状态").ShowInList(60);
            View.Property(p => p.ReceiveType).ShowInList(80);
            View.Property(p => p.VarietyQuantity).ShowInList(60);
            View.Property(p => p.TotalQty).ShowInList(60);
            View.Property(p => p.ReceiverId);
            View.Property(p => p.ReceiveDateTime).ShowInList(150);
            View.ChildrenProperty(p => p.DetailList).HasLabel("备件明细").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(SparePartReceiveLot), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<SparePartReceive>();
                if (parent == null)
                {
                    return new EntityList<SparePartReceiveLot>();
                }
                return RT.Service.Resolve<SparePartReceiveController>().GetReceiveLotInfo(parent.Id, args.PagingInfo);
            }).HasLabel("批次明细").HasOrderNo(2);
            View.AttachChildrenProperty(typeof(SparePartReceiveSn), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<SparePartReceive>();
                if (parent == null)
                {
                    return new EntityList<SparePartReceiveSn>();
                }
                return RT.Service.Resolve<SparePartReceiveController>().GetReceiveSnInfo(parent.Id, args.PagingInfo);
            }).HasLabel("序列号明细").HasOrderNo(3);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SaveSparePartReceiveCommand).FullName);
            View.UseDetail(4);
            View.Property(p => p.ReceiveNo).Readonly();
            View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.DepartmentId, null);
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.ReceiveType).UseEnumEditor(p => p.FilterCategoery = "SparePartReceive");
            View.Property(p => p.VarietyQuantity).Readonly();
            View.Property(p => p.TotalQty).Readonly();
            View.ChildrenProperty(p => p.DetailList).UseViewGroup(SparePartReceiveDetailViewConfig.EditView).HasLabel("备件明细").HasOrderNo(1);
        }
    }
}