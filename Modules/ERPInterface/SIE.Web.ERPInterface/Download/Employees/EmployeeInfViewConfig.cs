using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Employees
{
    /// <summary>
    /// 员工中间表视图配置
    /// </summary>
    internal class EmployeeInfViewConfig : WebViewConfig<EmployeeInf>
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
            View.Property(p => p.HireDate);
            View.Property(p => p.Phone);
            View.Property(p => p.Email);
            View.Property(p => p.Remark);
            View.Property(p => p.Sex);
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