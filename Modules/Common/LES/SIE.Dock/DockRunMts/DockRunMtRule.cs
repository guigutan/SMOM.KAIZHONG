using SIE.Core;
using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockRunMts
{
    /// <summary>
    /// 月台运行维护的月台编码不允许重复
    /// </summary>
    [System.ComponentModel.DisplayName("月台编码非重验证")]
    [System.ComponentModel.Description("月台运行维护的月台编码不允许重复")]
    public class DockRunMtNotDuplicateRule : NotDuplicateRule<DockRunMt>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        public DockRunMtNotDuplicateRule()
        {
            Properties.Add(DockRunMt.DockMaintainIdProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = (e) =>
            {
                var entity = e as DockRunMt;
                return "月台运行维护已存在月台编码[{0}],不允许重复".L10nFormat(entity.DockMaintain?.Code);
            };
        }
    }

    /// <summary>
    /// 工作时段结束时间必须大于开始时间
    /// </summary>
    [System.ComponentModel.DisplayName("工作时段数据验证")]
    [System.ComponentModel.Description("工作时段结束时间必须大于开始时间")]
    public class DockRunMtWorkTimeRule : EntityRule<WorkTime>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public DockRunMtWorkTimeRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var workTime = entity as WorkTime;
            if (workTime.BeginTime.TimeOfDay >= workTime.EndTime.TimeOfDay)
            {
                e.BrokenDescription = "工作时段中结束时间:[{0}]必须大于开始时间:[{1}]".L10nFormat(workTime.EndTime.ToString(DateTimeFormat.HHmm), workTime.BeginTime.ToString(DateTimeFormat.HHmm));
            }

            var beginmm = workTime.BeginTime.ToString("mm");
            if (int.TryParse(beginmm, out int begins) && begins % 30 != 0)
            {
                e.BrokenDescription = "工作时段中开始时间:[{0}]不能为选项外的数值".L10nFormat(workTime.BeginTime.ToString(DateTimeFormat.HHmm));
            }

            var endmm = workTime.EndTime.ToString("mm");
            if (int.TryParse(endmm, out int ends) && ends % 30 != 0)
            {
                e.BrokenDescription = "工作时段中结束时间:[{0}]不能为选项外的数值".L10nFormat(workTime.EndTime.ToString(DateTimeFormat.HHmm));
            }
        }
    }

    /// <summary>
    /// 例外时段结束时间必须大于开始时间
    /// </summary>
    [System.ComponentModel.DisplayName("例外时段数据验证")]
    [System.ComponentModel.Description("例外时段结束时间必须大于开始时间")]
    public class DockRunMtExcepTimeRule : EntityRule<ExcepTime>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public DockRunMtExcepTimeRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var excepTime = entity as ExcepTime;
            if (excepTime.BeginTime >= excepTime.EndTime)
            {
                e.BrokenDescription = "例外时段中结束时间:[{0}]必须大于开始时间:[{1}]".L10nFormat(excepTime.EndTime, excepTime.BeginTime);
            }

            var beginmm = excepTime.BeginTime.ToString("mm");
            if (int.TryParse(beginmm, out int begins) && begins % 30 != 0)
            {
                e.BrokenDescription = "例外时段中开始时间:[{0}]不能为时间选项外的数值".L10nFormat(excepTime.BeginTime);
            }

            var endmm = excepTime.EndTime.ToString("mm");
            if (int.TryParse(endmm, out int ends) && ends % 30 != 0)
            {
                e.BrokenDescription = "例外时段中结束时间:[{0}]不能为时间选项外的数值".L10nFormat(excepTime.EndTime);
            }
        }
    }
}
