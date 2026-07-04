using SIE.Items.ProductModels;

namespace SIE.Web.Items.ProductModels
{
    /// <summary>
    /// 班组缺编统计机型技能
    /// </summary>
    public class ModelSkillViewConfig : WebViewConfig<ModelSkillViewModel>
    {
        /// <summary>
        /// 班组缺编查看工单视图ViewGroup
        /// </summary>
        public const string WorkGroupView = "WorkGroupView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { WorkGroupView });
            if (ViewGroup == WorkGroupView)
            {
                WorkGroupConfigListView();
            }
        }

        /// <summary>
        /// 默认工单视图
        /// </summary>
        void WorkGroupConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SkillCode).ShowInList().Readonly();
                View.Property(p => p.SkillName).ShowInList().Readonly();
                View.Property(p => p.DemandQty).ShowInList().Readonly();
                View.Property(p => p.ActualQty).ShowInList().Readonly();
                View.Property(p => p.LackQty).ShowInList().Readonly();
            }
        }
    }
}
