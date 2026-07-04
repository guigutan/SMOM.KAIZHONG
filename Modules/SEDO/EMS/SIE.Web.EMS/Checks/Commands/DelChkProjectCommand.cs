using SIE.Domain;
using SIE.EMS.Checks.Projects;
using SIE.Web.Command;

namespace SIE.Web.EMS.Checks.Commands
{
    /// <summary>
    /// 删除点检计划的项目 命令
    /// </summary>
    [JsCommand(CommandName)]
    public class DelChkProjectCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 删除点检计划的项目名称
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.Checks.Commands.DelChkProjectCommand";

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var mix = args.Data.ToJsonObject<DelCheckProject>();
            var delCheckProjects = mix.DelChkProjects;
            if (delCheckProjects.Count > 0)
            {
                foreach (var item in delCheckProjects)
                {
                    item.PersistenceStatus = PersistenceStatus.Deleted;
                }
                RF.Save(delCheckProjects);
            }

            return true;
        }

        /// <summary>
        /// 删除的点检项目
        /// </summary>
        public class DelCheckProject
        {
            /// <summary>
            /// 点检项目列表
            /// </summary>
            public EntityList<CheckProject> DelChkProjects { get; set; }
        }
    }
}
