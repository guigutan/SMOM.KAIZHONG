using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.EMS.EarlierStage.Enums;
using SIE.Equipments.Enums;
using SIE.EMS.EarlierStage.Projects;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;

namespace SIE.Web.EMS.EarlierStage.Projects.Commands
{
    /// <summary>
    /// 导入项目计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.EarlierStage.Projects.Commands.ImportWorkItemCommand")]
    public class ImportWorkItemCommand : ImportExcelCommand
    {
        /// <summary>
        /// 行数据读取, 默认按视图配置把数据读取后，还可以进行自定义处理
        /// </summary>
        /// <param name="data">行数据</param>
        /// <param name="cache">缓存数据</param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var workItem = data.Entity as ProjectWorkItem;
            if (workItem.Project == null)
                throw new ValidationException("项目编码不能为空".L10N());
            var project = RT.Service.Resolve<ProjectController>().GetProjectById(workItem.ProjectId);
            if (project == null)
                throw new ValidationException("用户没有项目权限".L10N());
            if (project.ApprovalStatus != ApprovalStatus.Draft && project.ApprovalStatus != ApprovalStatus.Reject)
                throw new ValidationException("项目{0}状态不是【待提交】或【驳回】".L10nFormat(project.Code));
            if (workItem.PrincipalId <= 0)
                throw new ValidationException("员工不能为空".L10N());
            var principalIds = RT.Service.Resolve<ProjectController>().GetMemberEmployeeIds(project.Id);
            if (!principalIds.Contains(workItem.PrincipalId))
                throw new ValidationException("员工不存在项目成员中".L10N());
            workItem.WorkStatus = WorkStatus.NotStarted;
        }
    }
}
