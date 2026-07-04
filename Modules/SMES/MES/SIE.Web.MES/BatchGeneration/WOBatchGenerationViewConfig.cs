using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.BatchGeneration;
using SIE.MES.BatchGeneration.Services;
using SIE.MES.WorkOrderArchives;
using SIE.MES.WorkOrderArchives.Services;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.BatchGeneration
{
    /// <summary>
    /// 批次生成并过站视图配置
    /// </summary>
    public class WOBatchGenerationViewConfig : WebViewConfig<WOBatchGeneration>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommand("SIE.Web.MES.BatchGeneration.Commands.BatchWoGenerateCommand");
            using (View.OrderProperties())
            {
                View.UseClientOrder();
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.ProCode).ShowInList(width: 150);
                View.Property(p => p.ProName).ShowInList(width: 150);
                View.Property(p => p.WoState).ShowInList(width: 150);
                View.Property(p => p.PlanQty).ShowInList(width: 150);
                View.Property(p => p.PrintedQty).ShowInList(width: 150);
                
                View.Property(p => p.FinishQty).ShowInList(width: 150);
                View.Property(p => p.GeneratedQty).ShowInList(width: 150);
                
                View.Property(p => p.ScrapQty).ShowInList(width: 150);
                View.Property(p => p.WoType).ShowInList(width: 150);
                View.Property(p => p.PlanBeginDate).ShowInList(width: 150);
                View.Property(p => p.PlanEndDate).ShowInList(width: 150);
                View.Property(p => p.ActuStartDate).ShowInList(width: 150);
                View.Property(p => p.ActuFinishDate).ShowInList(width: 150);
                View.Property(p => p.ItemExtPropName).HasLabel("物料扩展属性").ShowInList(width: 150);
                View.Property(p => p.RetrospectType).ShowInList(width: 150);
                View.Property(p => p.Factory).ShowInList(width: 150);
                View.Property(p => p.WorkShop).ShowInList(width: 150);
                View.Property(p => p.Resource).ShowInList(width: 150);
                View.Property(p => p.Version).ShowInList(width: 150);
                View.Property(p => p.CreateDate).ShowInList(width: 150);
                View.Property(p => p.CreateBy).ShowInList(width: 150);
                View.Property(p => p.UpdateDate).ShowInList(width: 150);
                View.Property(p => p.UpdateBy).ShowInList(width: 150);

                View.AttachChildrenProperty(typeof(WipBatchViewModel), (e) =>
                {
                    var woBatchGeneration = e.Parent as WOBatchGeneration;
                    var args = e as ChildPagingDataArgs;
                    if (woBatchGeneration == null)
                    {
                        return new EntityList<WipBatchViewModel>();
                    }
                    else
                    {
                        return RT.Service.Resolve<WOBatchGenerationService>().GetWipBatchsViewModelByWorkOrder(woBatchGeneration.Id, sortInfo: (List<OrderInfo>)args.SortInfo, pagingInfo: args.PagingInfo);
                    }
                }).HasLabel("批次信息").HasOrderNo(10);
            }
        }
    }
}
