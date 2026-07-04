using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 导入项目成员
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectMemberImportCommand")]
    public class ProjectMemberImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 行数据读取, 默认按视图配置把数据读取后，还可以进行自定义处理
        /// </summary>
        /// <param name="data">行数据</param>
        /// <param name="cache">缓存数据</param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var member = data.Entity as ProjectMember;
            if (member.Project == null)
                throw new ValidationException("第{0}行项目编码不能为空".L10nFormat(data.RowIndex));
            var project = RT.Service.Resolve<ProjectController>().GetProjectById(member.ProjectId);
            if (project == null)
                throw new ValidationException("第{0}行用户没有项目权限".L10nFormat(data.RowIndex));
            if (project.ApprovalStatus != ApprovalStatus.Draft && project.ApprovalStatus != ApprovalStatus.Reject)
                throw new ValidationException("第{0}行项目{1}状态不是【待提交】或【驳回】".L10nFormat(data.RowIndex, project.Code));
        }
    }
}
