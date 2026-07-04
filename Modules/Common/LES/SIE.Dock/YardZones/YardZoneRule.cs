using SIE.Core;
using SIE.Dock.DockMaintains;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardZones
{
    /// <summary>
    /// 园片区维护被引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("园片区维护引用验证规则")]
    [System.ComponentModel.Description("园片区被月台维护引用不允许删除")]
    public class UndeleteYardZoneRule : NoReferencedRule<YardZone>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        public UndeleteYardZoneRule()
        {
            Properties.Add(DockMaintain.YardZoneIdProperty);
            MessageBuilder = (e, c) =>
            {
                var yard = e as YardZone;
                return "园片区[{0}]被月台维护引用了[{1}]次，不允许删除".L10nFormat(yard.Name, c);
            };
        }
    }

    /// <summary>
    /// 月台装卸能力页签不能为空
    /// </summary>
    [System.ComponentModel.DisplayName("月台装卸能力页签不能为空")]
    [System.ComponentModel.Description("月台装卸能力页签不能为空")]
    public class DockHandlingNotNullRule : EntityRule<YardZone>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public DockHandlingNotNullRule()
        {
            Property = YardZone.IdProperty;
        }

        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var yardZone = entity as YardZone;
            if (yardZone.DockHandlingList.Count == 0)
            {
                e.BrokenDescription = "月台装卸能力页签不能为空".L10nFormat();
            }
        }
    }

    /// <summary>
    /// 月台装卸能力结束时间大于开始时间
    /// </summary>
    [System.ComponentModel.DisplayName("月台装卸能力时间验证规则")]
    [System.ComponentModel.Description("月台装卸能力结束时间大于开始时间")]
    public class DockHandlingTimeRule : EntityRule<DockHandling>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public DockHandlingTimeRule()
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
            var dockHandling = entity as DockHandling;
            if (!DateTime.TryParse(dockHandling.BeginTime, out DateTime d1))
                e.BrokenDescription = "请输入或选择正确的时间";
            if (!DateTime.TryParse(dockHandling.EndTime, out DateTime d2))
                e.BrokenDescription = "请输入或选择正确的时间";
            if (d1 >= d2)
            {
                e.BrokenDescription = "结束时间:[{0}]必须大于开始时间:[{1}]".L10nFormat(dockHandling.EndTime, dockHandling.BeginTime);
            }
            if (!(dockHandling.ShipAppoNum > 0 || dockHandling.ReceiveAppoNum > 0))
            {
                e.BrokenDescription = "可预约数至少有一个大于0".L10N();
            }
            if (d1.ToString("HHmm") != "2359")
            {
                var beginmm = d1.ToString("mm");
                if (int.TryParse(beginmm, out int begins) && begins % 30 != 0)
                {
                    e.BrokenDescription = "开始时间:[{0}]不能为选项外的数值".L10nFormat(dockHandling.BeginTime);
                }
            }
            if (d2.ToString("HHmm") != "2359")
            {
                var endmm = d2.ToString("mm");
                if (int.TryParse(endmm, out int ends) && ends % 30 != 0)
                {
                    e.BrokenDescription = "结束时间:[{0}]不能为选项外的数值".L10nFormat(dockHandling.EndTime);
                }
            }
        }
    }
}
