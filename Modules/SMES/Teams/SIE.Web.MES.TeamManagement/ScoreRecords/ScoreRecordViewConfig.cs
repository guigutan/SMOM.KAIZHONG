using SIE.MES.TeamManagement.ScoreRecords;

namespace SIE.Web.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录视图配置类
    /// </summary>
    internal class ScoreRecordViewConfig : WebViewConfig<ScoreRecord>
    {
        /// <summary>
        /// 配置通用视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            ////View.UseCommands(typeof(AchieveLevelSetCommand).FullName);
            View.UseCommands("SIE.Web.MES.TeamManagement.ScoreRecords.AchieveLevelSetCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.InitiateDate).ShowInList(width: 150);
                View.Property(p => p.OccurDate).ShowInList(width: 150);
                View.Property(p => p.Score);
                View.Property(p => p.Remark);
                View.Property(p => p.EmployeeCode);
                View.Property(p => p.EmployeeName);
                View.Property(p => p.InitiatorCode);
                View.Property(p => p.InitiaorName);
                View.Property(p => p.RatedItemCode);
                View.Property(p => p.RatedItemName);
                View.Property(p => p.ScoreState);
                View.Property(p => p.IsEffective);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).OrderNo = 10; //子--附件列表
                View.ChildrenProperty(p => p.PetitionList).OrderNo = 20; //子--申诉列表
            }
        }

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
                View.Property(p => p.InitiatorCode);
                View.Property(p => p.RatedItemCode);
                View.Property(p => p.ScoreState);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InitiateDate);
                View.Property(p => p.OccurDate);
                View.Property(p => p.EmployeeName);
                View.Property(p => p.InitiaorName);
                View.Property(p => p.RatedItemName);
                View.Property(p => p.ScoreState);
            }
        }
    }
}
