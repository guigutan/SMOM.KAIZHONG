using SIE.MetaModel.View;
using SIE.Tech.Processs;

namespace SIE.Web.Tech.Processs
{
    /// <summary>
    /// 工序技能
    /// </summary>
    public class ProcessSkillViewConfig : WebViewConfig<ProcessSkill>
    {
        /// <summary>
        /// 视图函数
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == "ReadOnlyView")
                ReadOnlyView();
            else
            {
                base.ConfigView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessSkillCommand", WebCommandNames.Delete);
                View.Property(p => p.SkillCode).Readonly(true).Show(ShowInWhere.All).HasOrderNo(1);
                View.Property(p => p.SkillName).Readonly(true).Show(ShowInWhere.All).HasOrderNo(2);
                View.Property(p => p.SkillCategory).Readonly(true).Show(ShowInWhere.All).HasOrderNo(3);
                View.Property(p => p.SkillDesc).Readonly(true).Show(ShowInWhere.All).HasOrderNo(4);
                View.Property(p => p.IsCheck).Readonly(false).Show(ShowInWhere.All).HasOrderNo(5);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 只读列表视图
        /// </summary>
        protected void ReadOnlyView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands("SIE.Web.Tech.Processs.Commands.ProcessSkillCommand", WebCommandNames.Delete);
                View.Property(p => p.SkillCode).Readonly(true).Show(ShowInWhere.All).HasOrderNo(1);
                View.Property(p => p.SkillName).Readonly(true).Show(ShowInWhere.All).HasOrderNo(2);
                View.Property(p => p.SkillCategory).Readonly(true).Show(ShowInWhere.All).HasOrderNo(3);
                View.Property(p => p.SkillDesc).Readonly(true).Show(ShowInWhere.All).HasOrderNo(4);
                View.Property(p => p.IsCheck).Readonly(false).Show(ShowInWhere.All).HasOrderNo(5);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.SkillCode);
            View.Property(p => p.SkillName);
        }
    }
}
