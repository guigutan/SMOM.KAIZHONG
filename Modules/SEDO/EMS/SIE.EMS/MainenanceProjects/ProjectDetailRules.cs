using SIE.Domain.Validation;
using SIE.Utils;
using System;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 点检保养项目非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("点检保养项目验证规则")]
    [System.ComponentModel.Description("点检保养项目不能重复")]
    public class NotDuplicateProjectDetail : NotDuplicateRule<ProjectDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateProjectDetail()
        {
            Properties.Add(ProjectDetail.NameProperty);
            Properties.Add(ProjectDetail.ProjectTypeProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as ProjectDetail;
                return "已存在项目名称[{0}]和项目类型[{1}]相同的点检保养项目".L10nFormat(entity.Name, EnumViewModel.EnumToLabel(entity.ProjectType).L10N());
            };
        }
    }

    /// <summary>
    /// 点检保养项目校验验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("点检保养项目校验验证规则")]
    [System.ComponentModel.Description("点检保养项目校验验证规则")]
    public class ProjectDetailCheckRule : EntityRule<ProjectDetail>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ProjectDetailCheckRule()
        {
            Scope = MetaModel.EntityStatusScopes.Add | MetaModel.EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(Domain.IEntity entity, MetaModel.RuleArgs e)
        {
            var detail = entity as ProjectDetail;
            if (detail.ProjectType != ProjectType.PeriodicalInsp && detail.ProjectType == ProjectType.Verify && detail.CycleType==null)
            {
                e.BrokenDescription = "项目类型不为设备定检和计量校验时，【周期类型】不允许空".L10N();
            }
        }
    }


    /// <summary>
    /// 点检保养项目最大值最小值验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("点检保养项目最大值最小值验证规则")]
    [System.ComponentModel.Description("点检保养项目最大值不能小于等于最小值")]
    public class ProjectDetailMaxMinValueRule : EntityRule<ProjectDetail>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ProjectDetailMaxMinValueRule()
        {
            Scope = MetaModel.EntityStatusScopes.Add | MetaModel.EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(Domain.IEntity entity, MetaModel.RuleArgs e)
        {
            var detail = entity as ProjectDetail;
            if (detail.MaxValue.HasValue && detail.MinValue.HasValue && detail.MaxValue < detail.MinValue)
            {
                e.BrokenDescription = "最大值不能小于等于最小值".L10N();
            }
        }
    }
}
