using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 事务原因 视图配置
    /// </summary>
    internal class ReasonViewConfig : WebViewConfig<Reason>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands();
            View.UseCommands(typeof(AddReasonCommand).FullName, WebCommandNames.Edit, WebCommandNames.Save);           
            View.ReplaceCommands(WebCommandNames.Delete, typeof(DeleteReasonCommand).FullName);
            View.UseCommands(typeof(InitReasonCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code).UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}编码", "事务原因编码生成规则", "事务原因"); }).ShowInList(width: 150);
            View.Property(p => p.Name);
            View.Property(p => p.Describe);
            View.Property(p => p.ReasonType);
            View.Property(p => p.State);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ReasonType);
            View.Property(p => p.State);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ReasonType);
        }
    }
}
