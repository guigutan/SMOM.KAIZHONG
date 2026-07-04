using SIE.Core.ApiLogs;
using SIE.Web.Core.ApiLogs.Commands;

namespace SIE.Web.Core.ApiLogs
{
    /// <summary>
    /// API接口日志 视图配置
    /// </summary>
    internal class ApiLogViewConfig : WebViewConfig<ApiLog>
    {
        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(LookUpContextCommand).FullName);
            View.Property(p => p.ApiName).ShowInList(width: 180);
            View.Property(p => p.Controller).ShowInList(width: 120);
            View.Property(p => p.Method).ShowInList(width: 120);
            View.Property(p => p.Key1);
            View.Property(p => p.Key2);
            View.Property(p => p.Key3);
            View.Property(p => p.Key4);
            View.Property(p => p.Key5);
            //View.Property(p => p.Request);
            //View.Property(p => p.Response);
            View.Property(p => p.IsSuccess);
            View.Property(p => p.HasException);
            View.Property(p => p.StartTime).ShowInList(width: 180);
            View.Property(p => p.EndTime).ShowInList(width: 180);
            View.Property(p => p.TimeSpanMilliseconds);
            View.Property(p => p.InvOrgName);           
            View.Property(p => p.EmployeeName);

            View.Property(p => p.LogId).ShowInList(width: 150);
        }

        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DisableEditing();
            View.HasDetailColumnsCount(4);
            View.AddBehavior("SIE.Web.Core.ApiLogs.Scripts.ApiLogBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.ApiName).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Method).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Key1).ShowInDetail(columnSpan: 1);
                View.Property(p => p.Key2).ShowInDetail(columnSpan: 1);
                View.Property(p => p.Key3).ShowInDetail(columnSpan: 1);
                View.Property(p => p.Key4).ShowInDetail(columnSpan: 1);
                View.Property(p => p.Request).UseMemoEditor().ShowInDetail(columnSpan: 4, height: "300px");
                View.Property(p => p.Response).UseMemoEditor().ShowInDetail(columnSpan: 4, height: "300px");
            }
        }

        /// <summary>
        /// 配置查询
        /// </summary>
        protected override void ConfigQueryView()
        {           
        }
    }
}
