using SIE.Common;
using SIE.Inventory.Transactions;
using SIE.MetaModel.View;

namespace SIE.Web.Inventory.Transactions
{
    /// <summary>
    /// MES功能对应ERP功能 视图
    /// </summary>
    internal class ErpFunctionFunctionViewConfig : WebViewConfig<ErpFunctionFunction>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(/*typeof(AddErpFunctionCommand),*/ WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.FunctionCode).HasLabel("功能代码");
                View.Property(p => p.FunctionId).HasLabel("功能名称");
                View.Property(p => p.ErpFunctionCode).HasLabel("ERP功能代码");
                View.Property(p => p.ErpFunctionId).HasLabel("ERP功能名称").UseDataSource((e, p, s) =>
                {
                    return RT.Service.Resolve<TransactionController>().GetFunctions(SourceType.External);
                }).UseListSetting(e => { e.HelpInfo = "显示数据来源为外部的单据大类"; });
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.FunctionCode).HasLabel("功能代码");
            View.Property(p => p.FunctionId).HasLabel("功能名称");
            View.Property(p => p.ErpFunctionCode).HasLabel("ERP功能代码");
            View.Property(p => p.ErpFunctionId).HasLabel("ERP功能名称").UseDataSource((e, p, s) =>
            {
                return RT.Service.Resolve<TransactionController>().GetFunctions(SourceType.External);
            }).UseListSetting(e => { e.HelpInfo = "显示数据来源为外部的单据大类"; });
        }
    }
}
