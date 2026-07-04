using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    #region 派工任务明细
    /// <summary>
    /// 对象类型和对象名称不重复验证
    /// </summary>
    [DisplayName("派工任务明细验证规则")]
    [Description("当前派工任务明细中对象类型和对象名称不能重复")]
    class DispatchTaskDetailNotDuplicateRule : NotDuplicateRule<DispatchTaskDetail>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public DispatchTaskDetailNotDuplicateRule()
        {
            Properties.Add(DispatchTaskDetail.DispatchTaskIdProperty);
            Properties.Add(DispatchTaskDetail.AdoIdProperty);
            Properties.Add(DispatchTaskDetail.AdoNameProperty);
            Properties.Add(DispatchTaskDetail.AdoTypeProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as DispatchTaskDetail;
                return "当前派工任务明细中已经存在[名称]{0}和[类型]{1}的对象".L10nFormat(r.AdoName, r.AdoType);
            };
        }
    }
    #endregion
}
