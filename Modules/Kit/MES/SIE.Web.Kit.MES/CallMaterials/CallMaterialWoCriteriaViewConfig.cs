using SIE.Kit.MES.CallMaterials;
using SIE.Resources.WipResources;
using SIE.Web.Kit.MES._Extensions_;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料工单查询实体视图配置
    /// </summary>
    internal class CallMaterialWoCriteriaViewConfig : WebViewConfig<CallMaterialWoCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.Detail);
                View.Property(p => p.ProductCode).Show(ShowInWhere.Detail);
                View.Property(p => p.ProductName).Show(ShowInWhere.Detail);
                View.Property(p => p.WorkOrderState).UseEnumEditor("CallMaterial").Show(ShowInWhere.Detail);

                View.Property(p => p.ResourceName).Show(ShowInWhere.Detail).UseWipResourceCodeLookupEditor(p =>
                {
                    p.BindDisplayField = WipResource.NameProperty.Name;
                    p.DisplayField = WipResource.NameProperty.Name;
                    p.ValueField = WipResource.NameProperty.Name;
                    p.AllowBlank = false;
                });
            }
        }
    }
}
