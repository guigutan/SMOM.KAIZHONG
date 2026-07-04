using SIE.Core.Items;
using SIE.ManagedProperty;
using SIE.MES.WorkOrders;
using SIE.Web.MES.WorkOrders;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 工单工序BOM视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class WorkOrderProcessBomViewConfig : WebViewConfig<WorkOrderProcessBom>
    {
        /// <summary>
        /// 叫料单工序BOM视图
        /// </summary>
        public const string CallMaterialBomView = "CallMaterialBomView";


        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.DeclareExtendViewGroup(CallMaterialBomView);
            if (ViewGroup == CallMaterialBomView)
            {
                CallMaterialCondfigView();
            }
        }

        /// <summary>
        /// 叫料单工序BOM视图配置
        /// </summary>
        void CallMaterialCondfigView()
        {
            View.AssignAuthorize(typeof(SIE.Kit.MES.CallMaterials.CallMaterialWorkOrder));
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-6, -4);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor(p => p.DisplayField = Item.CodeProperty.Name).Show(ShowInWhere.All).HasLabel("物料编码").Readonly();
                View.Property(p => p.ItemName).Readonly().Show(ShowInWhere.All).HasLabel("物料名称");
                View.Property(p => p.ItemSpecificationModel).Readonly().Show(ShowInWhere.All).HasLabel("基本型号");
                View.Property(p => p.SingleQty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ItemUnitName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ProcessNameView).Readonly().Show(ShowInWhere.All).HasLabel("工序名称");
                View.Property(p => p.WorkStep).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.MainMaterial).Readonly().Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
                View.ChildrenProperty(p => p.AlternativeList).UseViewGroup(WorkOrderViewConfig.ReadonlyView);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
