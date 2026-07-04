using SIE.Domain;
using SIE.EMS.Projects;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.Items;
using SIE.Items.Units;
using SIE.MetaModel.View;
using SIE.Web.Core.Common;
using SIE.Web.EMS.Purchases._Extensions_;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请明细视图配置
    /// </summary>
    public class PurchaseRequisitionItemViewConfig : WebViewConfig<PurchaseRequisitionItem>
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

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.ObjectCode).ShowInList(120);
            View.Property(p => p.Description).ShowInList(200);
            View.Property(p => p.Specification).ShowInList(120);
            View.Property(p => p.ProjectKeyItemId).HasLabel("项目事项").ShowInList(120);
            View.Property(p => p.Qty).ShowInList(80);
            View.Property(p => p.ItemUnitId).HasLabel("单位").ShowInList(80);
            View.Property(p => p.Price).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.TotalAmount).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.SupplierId).UseSupplierEditor().ShowInList(120);
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.DemandDate).ShowInList(150).UseDateEditor();
            View.Property(p => p.PurchasedQty).ShowInList(100);
            View.Property(p => p.PurchasePrice).ShowInList(130);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PurchaseRequisitionNo).ShowInList(130);
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.ObjectCode).ShowInList(120).HasLabel("采购编码");
            View.Property(p => p.Description).ShowInList(200).HasLabel("采购描述");
            View.Property(p => p.Specification).ShowInList(120);
            View.Property(p => p.SupplierId).UseSupplierEditor().ShowInList(120);
            View.Property(p => p.SupplierName).ShowInList(200);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(PurchaseRequisition));
            View.AddBehavior("SIE.Web.EMS.Purchases.PurchaseRequisitions.PurDetailBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.AddPurDetailCommand", WebCommandNames.Delete,
                "SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.HistoryOrderCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly().ShowInList(50);
                View.Property(p => p.ObjectCodeInfoId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var purDetail = source as PurchaseRequisitionItem;
                    if (purDetail == null)
                    {
                        return new EntityList<ObjectCodeInfo>();
                    }
                    return RT.Service.Resolve<PurchaseRequisitionController>().GetObjectCodeInfos(purDetail.PurchaseObjectType, pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ObjectCode), nameof(e.ObjectCodeInfo.Value));
                    keyValues.Add(nameof(e.Description), nameof(e.ObjectCodeInfo.Name));
                    keyValues.Add(nameof(e.Specification), nameof(e.ObjectCodeInfo.Specification));
                    keyValues.Add(WebCommon.GetDisplayName(nameof(e.ItemUnitId)), nameof(e.ObjectCodeInfo.ItemUnitNmae));
                    keyValues.Add(nameof(e.ItemUnitId), nameof(e.ObjectCodeInfo.ItemUnitId));
                    m.DicLinkField = keyValues;
                }).HasLabel("采购对象编码".L10N()+"*").ShowInList(120);
                View.Property(p => p.Description).ShowInList(200).Readonly(p => p.ObjectCode != "");
                View.Property(p => p.Specification).ShowInList(120);
                View.Property(p => p.ProjectKeyItemId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var purDetail = source as PurchaseRequisitionItem;
                    if (purDetail == null)
                    {
                        return new EntityList<ProjectKeyItem>();
                    }
                    return RT.Service.Resolve<PurchaseRequisitionController>().GetPurKeyItemsByProId(purDetail.ProjectId, pagingInfo, keyword);
                }).HasLabel("项目事项").ShowInList(120).Readonly(p => p.PurchaseType == PurchaseType.NoneProject);
                View.Property(p => p.Qty).HasLabel("需求数量".L10N() + "*").UseSpinEditor(p => p.MinValue = 1).ShowInList(80);
                View.Property(p => p.ItemUnitId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ItemController>().GetUnits(keyword, pagingInfo);
                }).Readonly(p => p.PurchaseObjectType == PurchaseObjectType.OutsourcedRepair
                || p.PurchaseObjectType == PurchaseObjectType.OutsourcedMaintainance || p.PurchaseObjectType == PurchaseObjectType.OutsourcedRegularInspection || p.PurchaseObjectType == PurchaseObjectType.OutsourcedCalibration
                ).HasLabel("单位").ShowInList(80);
                View.Property(p => p.Price).UseSpinEditor(p =>
                {
                    p.DecimalPrecision = 2;
                    p.MinValue = 0.01;
                }).ShowInList(130);
                View.Property(p => p.TotalAmount).UseSpinEditor(p => { p.DecimalPrecision = 2; }).ShowInList(130);
                View.Property(p => p.SupplierId).UseSupplierEditor().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).ShowInList(120);
                View.Property(p => p.SupplierName).Readonly(p => p.SupplierId > 0).ShowInList(200);
                View.Property(p => p.DemandDate).ShowInList(150).UseDateEditor(p => p.MinValue = DateTime.Now.ToString());
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}