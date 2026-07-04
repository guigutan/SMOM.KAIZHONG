using SIE.Domain;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.MetaModel.View;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases.EquipmentReceives.Commands;
using SIE.Web.Resources;
using System;

namespace SIE.Web.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收视图配置
    /// </summary>
    internal class EquipmentReceiveViewConfig : WebViewConfig<EquipmentReceive>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentReceives.Commands.AddEquipmentReceiveCommand", "SIE.Web.EMS.Purchases.EquipmentReceives.Commands.EditEquipmentReceiveCommand",
                typeof(DeleteEquipmentReceiveCommand).FullName, typeof(SubmitEquipmentReceiveCommand).FullName, "SIE.Web.EMS.Purchases.EquipmentReceives.Commands.ReceiveScanCommand",
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.ReceiveNo).ShowInList(130);
            View.Property(p => p.ReceiveBillStatus).ShowInList(60);
            View.Property(p => p.ReceiveType).ShowInList(80);
            View.Property(p => p.AcceptanceType).ShowInList(80);
            View.Property(p => p.VarietyQuantity).ShowInList(60);
            View.Property(p => p.TotalQty).ShowInList(60);
            View.Property(p => p.ReceiverId);
            View.Property(p => p.ReceiveDateTime).ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).HasLabel("接收明细").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(EquipmentReceiveSn), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipmentReceive>();
                if (parent == null)
                {
                    return new EntityList<EquipmentReceiveSn>();
                }
                return RT.Service.Resolve<EquipmentReceiveSnController>().GetReceiveSnInfo(parent.Id, args.SortInfo, args.PagingInfo);
            }).HasLabel("序列号明细").HasOrderNo(2);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SaveEquipmentReceiveCommand).FullName);
            View.UseDetail(4);
            View.Property(p => p.ReceiveNo).Readonly();
            View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.DepartmentId, null);
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.ReceiveType);
            View.Property(p => p.VarietyQuantity).Readonly();
            View.Property(p => p.TotalQty).Readonly();
            View.Property(p => p.AcceptanceType);
            View.ChildrenProperty(p => p.DetailList).UseViewGroup(EquipmentReceiveDetailViewConfig.EditView).HasLabel("接收明细").HasOrderNo(1);
        }
    }
}