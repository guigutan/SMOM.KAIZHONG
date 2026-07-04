using SIE.AbnormalInfo.AbnormalMonitors;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule
{
    /// <summary>
    /// 异常来源视图配置
    /// </summary>
    internal class AbnormalSourceViewConfig : WebViewConfig<AbnormalSource>
    {

		protected override void ConfigListView()
		{
            View.UseDefaultCommands();
            View.Property(p => p.MonitorName);
            View.Property(p => p.AbnormalEntityMetadataId).UsePagingLookUpEditor(
                        (m, e) =>
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            //dic.Add(nameof(e.MonitorType), nameof(e.AbnormalEntityMetadata.Type));
                            dic.Add(nameof(e.TabName), nameof(e.AbnormalEntityMetadata.TableName));
                            m.DicLinkField = dic;
                            m.ReloadDataOnPopping = true;
                        }).HasLabel("功能模块");
            //View.Property(p => p.MonitorType).Readonly().ShowInList(225);
            View.Property(p => p.TabName).Readonly();
        }

		protected override void ConfigQueryView()
		{
            View.Property(p => p.MonitorName);
        }

		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
        {
            View.Property(p => p.MonitorName);
            View.Property(p => p.AbnormalEntityMetadataId).HasLabel("功能模块");
            View.Property(p => p.MonitorType).ShowInList(225);
            View.Property(p => p.TabName);
        }
    }
}