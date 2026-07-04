using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks
{
    #region 触发任务=8D或pdca时,验证缺陷代码必填


    /// <summary>
    ///  触发任务=8D或pdca时,验证缺陷代码必填
    /// </summary>
    [DisplayName("缺陷代码验证规则")]
    [Description("触发任务=8D或pdca时,验证缺陷代码必填")]
    public class AbnormalMonitorTaskDefectRule : EntityRule<AbnormalMonitorTask>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalMonitorTaskDefectRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">数据源配置</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var task = entity as AbnormalMonitorTask;
            if (task.PushMethord.HasValue && (task.PushMethord == Common.PushMethordEnum.EightD || task.PushMethord == Common.PushMethordEnum.PDCA) && task.JoinDefectCodes.IsNullOrEmpty())
                e.BrokenDescription = "触发任务为8D或pdca改善时，缺陷代码不能为空".L10N();
        }
    }

    #endregion

    #region 触发任务=8D或pdca时,任务处理人必填


    /// <summary>
    ///  触发任务=8D或pdca时,验证处理人必填
    /// </summary>
    [DisplayName("缺陷代码验证规则")]
    [Description("触发任务=8D或pdca时,验证处理人必填")]
    public class AbnormalMonitorTaskHandertRule : EntityRule<AbnormalMonitorTask>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalMonitorTaskHandertRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">数据源配置</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var task = entity as AbnormalMonitorTask;
            if (task.PushMethord.HasValue && (task.PushMethord == Common.PushMethordEnum.EightD || task.PushMethord == Common.PushMethordEnum.PDCA) && task.TaskHandlerId == null)
                e.BrokenDescription = "触发任务为8D或pdca改善时，任务处理人不能为空".L10N();
        }
    }

    #endregion
}
