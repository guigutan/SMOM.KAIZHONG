using SIE.MES.TeamManagement.ScoreRecords;

namespace SIE.Web.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 申诉记录视图配置类
    /// </summary>
    internal class PetitionRecordViewConfig : WebViewConfig<PetitionRecord>
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
            View.UseChildrenAsHorizontal(true).UseLayoutSize(-6, -4);
            using (View.OrderProperties())
            {
                View.Property(p => p.PetitionDate);
                View.Property(p => p.PetitionRemark);
                View.Property(p => p.ProcessDate);
                View.Property(p => p.ProcessResult);
                View.Property(p => p.ProcessMode);
                View.Property(p => p.HandlerCode);
                View.Property(p => p.HandlerName);
                View.Property(p => p.PetitionerCode);
                View.Property(p => p.PetitionerName);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).OrderNo = 10; //子--附件列表
                View.ChildrenProperty(p => p.ProcessList).OrderNo = 20; //子--处理记录列表
            }
        }
    }
}
