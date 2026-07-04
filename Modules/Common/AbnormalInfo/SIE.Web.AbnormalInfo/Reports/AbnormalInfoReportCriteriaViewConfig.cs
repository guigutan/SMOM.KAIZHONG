using SIE.AbnormalInfo.AbnormalInfos;
using SIE.AbnormalInfo.Reports;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.Reports
{
    /// <summary>
    /// 来料批次合格率报表查询实体实体
    /// </summary>
    public class AbnormalInfoReportCriteriaViewConfig : WebViewConfig<AbnormalInfoReportCriteria>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.AbnormalInfo.Reports.AbnormalInfoReportQueryCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.AbnormalCategoryId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AbnormalCategortyName), nameof(AbnormalInfoCategory.Desc));
                    m.DicLinkField = keyValues;
                }).Show();
                View.Property(p => p.AbnormalCategortyName).Show().Readonly();
                View.Property(p => p.AbnormalDefinitionId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AbnormalDefinitionDesc), nameof(AbnormalInfoDefinition.Desc));
                    m.DicLinkField = keyValues;
                }).Show();
                View.Property(p => p.AbnormalDefinitionDesc).Show().Readonly();
                View.Property(p => p.CreateDate).Show().UseDateRangeEditor(d => d.DateRangeType = DateRangeType.Week).Show();
            }
        }
    }
}
