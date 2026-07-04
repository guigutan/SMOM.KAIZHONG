
using SIE.Inventory.Transactions;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Transactions.ViewModels;

namespace SIE.Web.Inventory.Transactions
{
    /// <summary>
    /// 选择订单类型模型
    /// </summary>
    public class FunctionModelViewConfig : WebViewConfig<FunctionModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Function));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 查询实体配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.Inventory.Transactions.Commands.InitExcuteQueryCommand");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}
