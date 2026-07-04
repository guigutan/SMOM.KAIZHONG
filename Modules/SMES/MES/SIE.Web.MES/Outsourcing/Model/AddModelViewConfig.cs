using System.Collections.Generic;

namespace SIE.MES.Outsourcing.Model
{
    /// <summary>
    ///添加模型
    /// </summary>
    internal class AddModelViewConfig : WebViewConfig<AddModel>
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
                View.Property(p => p.Qty).HasOrderNo(10);
                View.Property(p => p.Supplier).HasOrderNo(15);
            }
        }

    }
}
