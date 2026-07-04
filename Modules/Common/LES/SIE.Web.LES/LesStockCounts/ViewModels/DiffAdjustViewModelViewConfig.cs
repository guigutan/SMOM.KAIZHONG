using SIE.Domain;
using SIE.LES.LesStockCounts;
using SIE.LES.LesStockCounts.ViewModels;
using SIE.MetaModel.View;
using SIE.Security;

namespace SIE.Web.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 差异调账视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    [AllowAnonymous]
    public class DiffAdjustViewModelViewConfig : WebViewConfig<DiffAdjustViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LesStockCount));
        }

        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.LabelNo).ShowInList(150);
                View.Property(p => p.ItemCode).ShowInList(120);
                View.Property(p => p.ItemName).ShowInList(120);
                View.Property(p => p.ItemExtPropName).ShowInList(150);
                View.Property(p => p.Qty).ShowInList();
                View.Property(p => p.Factory).ShowInList();
                View.Property(p => p.WarehouseName).ShowInList();
                View.Property(p => p.StorageLocation).ShowInList();
                View.Property(p => p.AvaiableQty).ShowInList();
            }
            View.ChildrenProperty(p => p.AdjustWorkOrder).IsVisible(true);
            //View.AttachChildrenProperty(typeof(AdjustWorkOrderViewModel), c =>
            //{
            //    return new EntityList<AdjustWorkOrderViewModel>();
            //}, ListView).Show(ChildShowInWhere.All).HasLabel("物料投入工单");


        }
    }

    /// <summary>
    /// 差异调账视图
    /// </summary>
    [AllowAnonymous]
    public class AdjustWorkOrderViewModelViewConfig : WebViewConfig<AdjustWorkOrderViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LesStockCount), typeof(DiffAdjustViewModel));
        }

        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.WithoutPaging();
            View.InlineEdit();
            View.UseCommands("SIE.Web.LES.LesStockCounts.Commands.AddAdjustWorkOrderCommand", "SIE.Web.LES.LesStockCounts.Commands.EditAdjustWorkOrderCommand",
                "SIE.Web.LES.LesStockCounts.Commands.DeleteAdjustWorkOrderCommand");
            View.AddBehavior("SIE.Web.LES.LesStockCounts.Scripts.DiffAdjustBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderId).ShowInList(120).Readonly(p => p.IsAuto);
                View.Property(p => p.Qty).ShowInList().Readonly();
                View.Property(p => p.ActualyQty).UseSpinEditor(p => { p.MinValue = 0; p.XType = "AdjustQtyNumberfield"; })
                    .ShowInList();
                View.Property(p => p.DiffQty).ShowInList().Readonly();
            }
        }
    }
}
