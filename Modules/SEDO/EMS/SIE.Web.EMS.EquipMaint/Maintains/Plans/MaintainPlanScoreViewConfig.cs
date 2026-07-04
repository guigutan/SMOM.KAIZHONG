using SIE.EMS.Maintains.Plans;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养计划维护视图配置
    /// </summary>
    internal class MaintainPlanScoreViewConfig : WebViewConfig<MaintainScore>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.ProjectName).Readonly();
            View.Property(p => p.CheckStandard).UseTextEditor(p => p.MaxLength = 1000).ShowInList(width: 200).Readonly();
            View.Property(p => p.Remark).Readonly();
            View.Property(p => p.ExistProblem);
            View.Property(p => p.Score).HasLabel("得分(0—10)").UseSpinEditor(p =>
            {
                p.AllowNegative = false;
                p.AllowDecimals = true;
                p.AllowBlank = false;
                p.DecimalPrecision = 1;
                p.Step = 0.1;
                p.MinValue = 0;
                p.MaxValue = 10;
            });
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
