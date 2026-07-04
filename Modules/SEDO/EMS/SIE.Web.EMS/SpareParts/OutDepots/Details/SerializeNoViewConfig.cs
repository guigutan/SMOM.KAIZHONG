using SIE.EMS.SpareParts.OutDepots.Details;

namespace SIE.Web.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 序列化视图配置
    /// </summary>
    public class SerializeNoViewConfig :WebViewConfig<SerializeNo>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(
                  "SIE.Web.EMS.SpareParts.OutDepots.Commands.AddSeriaCommand",
                  "SIE.Web.EMS.SpareParts.OutDepots.Commands.EditSeriaCommand",
                  "SIE.Web.EMS.SpareParts.OutDepots.Commands.DelSeriaCommand"
                  );
            View.UseImportCommands();
            View.Property(p=>p.Code);
            View.Property(p=>p.Count);
            
            View.Property(p=>p.State);
        }
    }
}
