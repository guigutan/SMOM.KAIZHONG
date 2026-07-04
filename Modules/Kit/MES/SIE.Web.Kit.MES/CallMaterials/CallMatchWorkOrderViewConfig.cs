using SIE.Kit.MES.CallMaterials;
using SIE.MetaModel.View;
using SIE.Web.Kit.MES.CallMaterials.Commands;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 工单匹配视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class CallMatchWorkOrderViewConfig : WebViewConfig<CallMatchWorkOrder>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CallMaterialWorkOrder));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands();
            View.UseCommands(WebCommandNames.Edit, typeof(CallMatchSaveItemCommand).FullName);
            View.Property(p => p.IsUse).Show(ShowInWhere.List).UseCheckEditor().ShowInList(width: 90);
            View.Property(p => p.WorkOrder).HasLabel("匹配工单号").Readonly();
            View.Property(p => p.ItemCode).HasLabel("物料编码").Readonly();
            View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
            View.Property(p => p.ItemUnit).HasLabel("单位").Readonly();
            View.Property(p => p.ProcessName).HasLabel("工序名称").Readonly();
            View.Property(p => p.AlternativeCode).Readonly();
            View.Property(p => p.AlternativeName).Readonly();
            View.Property(p => p.IsChange).Show(ShowInWhere.Hide);
        }
    }
}
