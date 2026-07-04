using SIE.Core.Items;
using SIE.Domain;
using SIE.Services;
using SIE.Warehouses.ItemStockData;
using System;

namespace SIE.MES.WorkOrders.Interfaces
{
    /// <summary>
    /// 获取工单打印设置接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultWorkOrderTemplate))]
    public interface IWorkOrderTemplate
    {
        /// <summary>
        /// 获取工单打印设置
        /// </summary>
        /// <param name="productId">物料Id</param>
        /// <param name="customerId">客户Id</param>
        /// <returns>打印设置</returns>
        LabelPrintTemplate GetWorkOrderTemplate(double productId, double? customerId);
    }

    /// <summary>
    /// 获取工单打印设置接口默认实现（默认取物料的设置）
    /// </summary>
    public class DefaultWorkOrderTemplate : IWorkOrderTemplate
    {
        /// <summary>
        /// 获取工单打印设置（默认取物料的设置）
        /// </summary>
        /// <param name="productId">物料Id</param>
        /// <param name="customerId">客户Id</param>
        /// <returns>打印设置</returns>
        public LabelPrintTemplate GetWorkOrderTemplate(double productId, double? customerId)
        {
            var item = RF.GetById<SIE.Items.Item>(productId);
            if (item == null) { return null; }
            var isBitch = RT.Service.Resolve<ItemStockBaseController>().CheckItemIsBatch(item.Id);

            var template = item.Template;
            if (template != null && template.LabelTemplate != null)
            {
                if (!isBitch && template.LabelTemplate.EntityType != typeof(Barcodes.Printables.BarcodePrintable).GetQualifiedName())//单体时过滤只能使用单体打印标签
                {
                    template.LabelTemplate = null;
                }
                if (isBitch && template.LabelTemplate.EntityType != typeof(Barcodes.Printables.WipBatchPrintable).GetQualifiedName())//是批次时只能使用批次类型打印标签
                {
                    template.LabelTemplate = null;
                }
            }
            if (template != null && template.PackingTemplate != null && template.PackingTemplate.EntityType != typeof(SIE.MES.WIP.Moves.BarcodePrintable).GetQualifiedName())
            {
                template.PackingTemplate = null;
            }
            return template;



        }
    }
}
