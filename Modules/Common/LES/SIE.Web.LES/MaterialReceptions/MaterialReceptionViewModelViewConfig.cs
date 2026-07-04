using SIE.LES.MaterialReceptions;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.MaterialReceptions.ViewModels;
using SIE.MetaModel.View;
using SIE.Web.LES.MaterialReceptions.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.MaterialReceptions
{
    /// <summary>
    /// 按单或按明细添加视图
    /// </summary>
    public class MaterialReceptionViewModelViewConfig : WebViewConfig<MaterialReceptionAddViewModel>
    {
        /// <summary>
        /// 按明细接收
        /// </summary>
        public const string AddDetailPageView = "AddDetailPageView";

        /// <summary>
        /// 按单接收
        /// </summary>
        public const string AddOrderPageView = "AddOrderPageView";

        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddDetailPageView, AddOrderPageView);
            View.AssignAuthorize(typeof(MaterialReception));
            if (ViewGroup == AddDetailPageView)
            {
                View.WithoutPaging();
                View.AddBehavior("SIE.Web.LES.MaterialReceptions.Behaviors.AddPageBehavior");
                View.UseCommands(typeof(AddByDetailOrderCommand).FullName, WebCommandNames.Delete, typeof(SubmitByDetailCommand).FullName);
                using (View.OrderProperties())
                {
                    View.Property(p => p.StockOrderNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ItemCode).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ItemName).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ItemExtPropName).HasLabel("物料拓展属性").ShowInList(width: 100).Readonly();
                    View.Property(p => p.LabelNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.LotNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.Qty).ShowInList(width: 100);
                    View.Property(p => p.StayQty).ShowInList(width: 100).Readonly();
                    View.Property(p => p.WarehouseName).ShowInList(width: 100).Readonly();
                    View.Property(p => p.WorkOrderNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ResourceName).ShowInList(width: 100).Readonly();
                }
            }
            if (ViewGroup == AddOrderPageView)
            {
                View.WithoutPaging();
                View.AddBehavior("SIE.Web.LES.MaterialReceptions.Behaviors.AddPageBehavior");
                View.UseCommands(typeof(AddByDetailOrderCommand).FullName, WebCommandNames.Delete, typeof(SubmitByDetailCommand).FullName);
                using (View.OrderProperties())
                {
                    View.Property(p => p.StockOrderNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ItemCode).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ItemName).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ItemExtPropName).HasLabel("物料拓展属性").ShowInList(width: 100).Readonly();
                    View.Property(p => p.LabelNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.LotNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.Qty).ShowInList(width: 100);
                    View.Property(p => p.StayQty).ShowInList(width: 100).Readonly();
                    View.Property(p => p.WarehouseName).ShowInList(width: 100).Readonly();
                    View.Property(p => p.WorkOrderNo).ShowInList(width: 100).Readonly();
                    View.Property(p => p.ResourceName).ShowInList(width: 100).Readonly();
                }
            }
        }

    }
}
