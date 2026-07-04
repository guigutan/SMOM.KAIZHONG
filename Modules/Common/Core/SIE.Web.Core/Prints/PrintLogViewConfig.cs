using SIE.Core.Prints;
using SIE.MetaModel.View;
using SIE.Web.Core.Prints.Commands;

namespace SIE.Web.Core.Prints
{
    /// <summary>
    /// 打印日志
    /// </summary>
    public class PrintLogViewConfig : WebViewConfig<PrintLog>
    {
        /// <summary>
        /// 视图函数
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXlsAll, typeof(RePrintCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.DataKey).ShowInList(200);
                View.Property(p => p.DeviceCode).ShowInList(125);
                View.Property(p => p.DeviceName).ShowInList(125);
                View.Property(p => p.PrinterName).ShowInList(125);
                View.Property(p => p.PrintTemplate).ShowInList(125);
                View.Property(p => p.PrintState).Show();
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.InvOrgId).Show();
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.DataKey).Show();
                View.Property(p => p.DeviceCode).Show();
                View.Property(p => p.DeviceName).Show();
                View.Property(p => p.PrinterName).Show();
                View.Property(p => p.PrintState).Show();
                View.Property(p => p.InvOrgId).Show();
            }
        }
    }
}
