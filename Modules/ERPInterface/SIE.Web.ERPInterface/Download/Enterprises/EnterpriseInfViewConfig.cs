using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Enterprises
{
    /// <summary>
    /// 企业模型中间表视图配置
    /// </summary>
    internal class EnterpriseInfViewConfig : WebViewConfig<EnterpriseInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ErpOrgId);
            View.Property(p => p.IsResource);
            View.Property(p => p.Level);
            View.Property(p => p.EnterpriseLevelNum);
            View.Property(p => p.ParentCode);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}