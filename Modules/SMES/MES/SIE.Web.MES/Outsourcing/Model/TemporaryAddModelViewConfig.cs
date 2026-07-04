using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.MES.Outsourcing.Model;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Web.MES.Outsourcing.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Outsourcing.Model
{
    public class TemporaryAddModelViewConfig : WebViewConfig<TemporaryAddModel>
    {
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrder).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.Product), nameof(e.WorkOrder.ProductName));
                    dic.Add(nameof(e.Qty), nameof(e.WorkOrder.PlanQty));
                    m.DicLinkField = dic;
                }).Show(ShowInWhere.All).HasOrderNo(1);
                View.Property(p => p.Product).HasOrderNo(5).Readonly();
                View.Property(p => p.BeginRoutingProcess).UseDataSource((X, Y, Z) =>
                {
                    var entity = X as TemporaryAddModel;
                    if (entity != null && entity.WorkOrderId != 0)
                    {
                        return RT.Service.Resolve<WorkOrderController>().GetRoutingProcess(entity.WorkOrderId);
                    }
                    return new EntityList<WorkOrderRoutingProcess>();

                }).Show(ShowInWhere.All).HasOrderNo(15);
                View.Property(p => p.EndRoutingProcess).UseDataSource((X, Y, Z) =>
                {
                    var entity = X as TemporaryAddModel;
                    if (entity != null && entity.WorkOrderId != 0)
                    {
                        return RT.Service.Resolve<WorkOrderController>().GetRoutingProcess(entity.WorkOrderId);
                    }
                    return new EntityList<WorkOrderRoutingProcess>();

                }).Show(ShowInWhere.All).HasOrderNo(25);

                View.Property(p => p.Qty).HasOrderNo(30);
                View.Property(p => p.Supplier).HasOrderNo(35);
            }
        }
    }
}
