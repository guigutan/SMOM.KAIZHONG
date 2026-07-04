using SIE.Domain;
using SIE.Domain.Validation;
using System;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 点检保养项目保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("点检保养项目保存前事件")]
    [System.ComponentModel.Description("点检保养项目保存前事件")]
    public class ProjectDetailSubmitting : OnSubmitting<ProjectDetail>
    {
        /// <summary>
        /// 点检保养项目保存前事件
        /// </summary>
        /// <param name="entity">客户地址</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(ProjectDetail entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert ||
                e.Action == SubmitAction.Update)
            {
                if (string.IsNullOrEmpty(entity.CycleTypeInfoId))
                {
                    entity.CycleType = null;
                }
                else
                {
                    entity.CycleType = (CycleType)(int.Parse(entity.CycleTypeInfoId));
                }

                if (!entity.CycleType.HasValue && (entity.ProjectType == ProjectType.Check
                    || entity.ProjectType == ProjectType.Maintain
                    || entity.ProjectType == ProjectType.Lubrication
                    || entity.ProjectType == ProjectType.Verify))
                {
                    throw new ValidationException("点检保养项目类型：【{0}】周期类型不能为空"
                        .L10nFormat(entity.ProjectType.ToLabel().L10N()));
                }
            }
        }
    }
}
