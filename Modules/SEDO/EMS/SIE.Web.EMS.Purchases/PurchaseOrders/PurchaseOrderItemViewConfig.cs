using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.MetaModel.View;
using SIE.Web.EMS.Purchases.PurchaseOrders.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单明细视图配置
    /// </summary>
    public class PurchaseOrderItemViewConfig : WebViewConfig<PurchaseOrderItem>
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
            //隐藏完成按钮 
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.Status).ShowInList(80);
            View.Property(p => p.PurchaseRequisitionItemId).HasLabel("采购申请").ShowInList(120);
            View.Property(p => p.ObjectCode).ShowInList(130);
            View.Property(p => p.ObjectName).ShowInList(100);
            View.Property(p => p.Specification).ShowInList(120);
            View.Property(p => p.Qty).ShowInList(80);
            View.Property(p => p.UnitName).ShowInList(80);
            View.Property(p => p.Price).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.TaxRate).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(70);
            View.Property(p => p.PriceNoTax).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130);
            View.Property(p => p.Amount).ShowInList(130);
            View.Property(p => p.ProjectCode).ShowInList(120);
            View.Property(p => p.ProjectName).ShowInList(200);
            View.Property(p => p.KeyItem).ShowInList(200);
            View.Property(p => p.DeliveryDate).UseDateEditor().ShowInList(150);
            View.Property(p => p.TotalReciveQty).ShowInList(100);
            View.Property(p => p.TotalAcceptanceQty).ShowInList(100);
            View.Property(p => p.RejectQty).ShowInList(80);
            View.Property(p => p.InboundQty).ShowInList(80);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.ReciveQty).ShowInList(80).Show(ShowInWhere.All);
            View.Property(p => p.AcceptanceQty).ShowInList(80).Show(ShowInWhere.All);
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
            View.Property(p => p.LineNo).ShowInList(50);
            View.Property(p => p.PurchaseObjectType);
            View.Property(p => p.ObjectCode).ShowInList(130);
            View.Property(p => p.ObjectName).ShowInList(100);
            View.Property(p => p.Specification).HasLabel("技术规格").ShowInList(120);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(PurchaseOrder));
            View.AddBehavior("SIE.Web.EMS.Purchases.PurchaseOrders.PurOrderItemBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.PurchaseOrders.Commands.AddPurOrderDetailCommand", typeof(DeletePurOrderDetailCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(50).Readonly();
                View.Property(p => p.PurchaseRequisitionItemId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var pur = source as PurchaseOrderItem;
                    if (pur == null)
                    {
                        return new EntityList<PurchaseRequisitionItem>();
                    }
                    return RT.Service.Resolve<PurchaseRequisitionController>().GetPagingPurDetailByPurId(pur.FactoryId, pur.DepartmentId, pur.PurchaseObjectType, pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.Description), nameof(e.PurchaseRequisitionItem.Description));
                    keyValues.Add(nameof(e.UnitName), nameof(e.PurchaseRequisitionItem.UnitName));
                    keyValues.Add(nameof(e.KeyItem), nameof(e.PurchaseRequisitionItem.KeyItemDescription));
                    keyValues.Add(nameof(e.Qty), nameof(e.PurchaseRequisitionItem.Qty));
                    keyValues.Add(nameof(e.Price), nameof(e.PurchaseRequisitionItem.Price));
                    keyValues.Add(nameof(e.ProjectCode), nameof(e.PurchaseRequisitionItem.ProjectCode));
                    keyValues.Add(nameof(e.ProjectName), nameof(e.PurchaseRequisitionItem.ProjectName));
                    keyValues.Add(nameof(e.ObjectCode), nameof(e.PurchaseRequisitionItem.ObjectCode));
                    keyValues.Add(nameof(e.ObjectName), nameof(e.PurchaseRequisitionItem.Description));
                    keyValues.Add(nameof(e.Specification), nameof(e.PurchaseRequisitionItem.Specification));
                    m.DicLinkField = keyValues;
                }).HasLabel("采购申请").ShowInList(120);
                View.Property(p => p.ObjectCode).ShowInList(130).Readonly();
                View.Property(p => p.ObjectName).ShowInList(100).Readonly();
                View.Property(p => p.Specification).ShowInList(120).Readonly();
                View.Property(p => p.Qty).UseSpinEditor(p => p.MinValue = 1).ShowInList(80);
                View.Property(p => p.UnitName).ShowInList(80).Readonly();
                View.Property(p => p.Price).UseSpinEditor(p =>
                {
                    p.DecimalPrecision = 2;
                    p.MinValue = 0.01;
                }).ShowInList(130);
                View.Property(p => p.TaxRate).UseSpinEditor(p =>
                {
                    p.DecimalPrecision = 2;
                    p.MinValue = 0;
                    p.MaxValue = 100;
                }).ShowInList(70);
                View.Property(p => p.PriceNoTax).UseSpinEditor(p => p.DecimalPrecision = 2).ShowInList(130).Readonly();
                View.Property(p => p.Amount).ShowInList(130).Readonly();
                View.Property(p => p.ProjectCode).ShowInList(120).Readonly();
                View.Property(p => p.ProjectName).ShowInList(200).Readonly();
                View.Property(p => p.KeyItem).ShowInList(200).Readonly();
                View.Property(p => p.DeliveryDate).UseDateEditor().ShowInList(150);
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}