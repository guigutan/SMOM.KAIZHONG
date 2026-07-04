using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 历史订单查询界面
    /// </summary>
    internal class HistoryOrderViewModelCriteriaViewConfig : WebViewConfig<HistoryOrderViewModelCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExecuteQuery, "SIE.Web.EMS.Purchases.PurchaseOrders.Commands.HistoryOrderQueryCommand");
            View.AddBehavior("SIE.Web.EMS.Purchases.PurchaseOrders.HistoryOrderBehavior");
            View.Property(p => p.ObjectCodeInfo);
            View.Property(p => p.ObjectName).UseTextEditor(p => p.AllowBlank = true);
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            }).UsePagingLookUpEditor().HasLabel("供应商编码");
            View.Property(p => p.SupplierName).UseTextEditor(p => p.AllowBlank = true);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.Custom;
                p.StartDate = DateTime.Today.AddYears(-3);
                p.EndDate = DateTime.Today;
            });
        }
    }
}
