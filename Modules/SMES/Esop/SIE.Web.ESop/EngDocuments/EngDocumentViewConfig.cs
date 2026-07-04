using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ESop.EngDocuments;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.ESop.EngDocuments.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件视图配置
    /// </summary>
    public class EngDocumentViewConfig : WebViewConfig<EngDocument>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, "SIE.Web.ESop.EngDocuments.Commands.EngDocDeleteCommand", typeof(EngDocSaveCommand).FullName);
            View.UseCommands(typeof(EngDocUseTypeCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Type).Cascade(p => p.WorkOrderId, null).Cascade(p => p.ProductId, null).Cascade(p => p.ProductName, null).ShowInList();
                View.Property(p => p.WorkOrder).UseDataSource((s, p, k) =>
                {
                    var workList = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(k, p);
                    if (workList != null)
                    {
                        return workList;
                    }
                    else
                    {
                        return new EntityList<WorkOrder>();
                    }
                }).Readonly(p => p.Type == SIE.ESop.EngDocuments.Enums.EngDocType.Product).ShowInList(width: 150);
                View.Property(p => p.Product).HasLabel("产品编码").UsePagingLookUpEditor((m, e) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    m.DicLinkField = keyValues;
                }).UseDataSource((s,p,k) =>
                {
                    var productList = RT.Service.Resolve<ItemController>().GetProductItems(k, p);
                    if (productList != null)
                    {
                        return productList;
                    }
                    else
                    {
                        return new EntityList<Item>();
                    }
                }).Readonly(p => p.Type == SIE.ESop.EngDocuments.Enums.EngDocType.WorkOrder).ShowInList(width: 150);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 150);
                View.ChildrenProperty(p => p.EngDocumentDetailList);
            }
        }
    }
}
