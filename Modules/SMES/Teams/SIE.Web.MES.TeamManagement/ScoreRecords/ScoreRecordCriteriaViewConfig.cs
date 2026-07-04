using SIE.MES.TeamManagement.ScoreRecords;

namespace SIE.Web.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录查询视图配置类
    /// </summary>
    internal class ScoreRecordCriteriaViewConfig : WebViewConfig<ScoreRecordCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InitiateDate).UseDateEditor(p => p.Format = "Y/m/d");
                View.Property(p => p.OccurDate).UseDateEditor(p => p.Format = "Y/m/d");
                ////View.Property(p => p.LoseEfficacy);
                View.Property(p => p.EmployeeCode);
                View.Property(p => p.InitiaorCode);
                View.Property(p => p.RatedItemCode);
                View.Property(p => p.ScoreState);
            }
        }
    }
}
