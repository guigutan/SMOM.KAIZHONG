using SIE.Barcodes.WipBatchs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.Web.Barcodes.WipBatchs
{
    /// <summary>
    /// 批次工单视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BatchWorkOrderViewConfig : WebViewConfig<BatchWorkOrder>
    {
        /// <summary>
        /// 批次工单视图
        /// </summary>
        public const string BatchWorkOrderView = "BatchWorkOrder";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BatchWorkOrderView);
            if (ViewGroup == BatchWorkOrderView)
                BatchWoView();
        }

        /// <summary>
        /// 批次生成视图
        /// </summary>
        protected override void ConfigListView()
        {
            //暂未实现
        }

        /// <summary>
        /// 批次工单视图
        /// </summary>
        void BatchWoView()
        {
            View.AssignAuthorize(typeof(BatchWorkOrder));
            View.UseCommands("SIE.Web.Barcodes.WipBatchs.BatchWoGenerateCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(150).Readonly();
                View.Property(p => p.ProductCode).HasLabel("产品编码").ShowInList(120).Readonly();
                View.Property(p => p.ProductName).HasLabel("产品名称").ShowInList(120).Readonly();
                View.Property(p => p.PlanQty).HasLabel("计划数量").ShowInList().Readonly();
                View.Property(p => p.GeneratedQty).HasLabel("已生成批次数量").ShowInList(120).Readonly();
                View.Property(p => p.PrintedQty).HasLabel("已打印数量").ShowInList().Readonly();
                View.Property(p => p.PlanBeginDate).HasLabel("计划开始日期").ShowInList(150).Readonly();
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide); //隐藏BOM列表
                View.AttachChildrenProperty(typeof(WipBatch), (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var workorder = args.Parent.CastTo<WorkOrder>();
                    if (workorder == null) return new EntityList<WipBatch>();
                    return RT.Service.Resolve<WipBatchController>().GetWipBatchsByWorkOrder(workorder.Id, sortInfo: (List<OrderInfo>)args.SortInfo, pagingInfo: args.PagingInfo);
                }).HasLabel("批次信息").Show(ChildShowInWhere.All); //自定义的界面、按钮 要用ShowInWhere
            }
        }
    }
}