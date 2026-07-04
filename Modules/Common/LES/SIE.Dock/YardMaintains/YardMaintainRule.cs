using SIE.Dock.YardZones;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardMaintains
{
    /// <summary>
    /// 园区维护被引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("园区维护引用验证规则")]
    [System.ComponentModel.Description("园区被园片区维护引用不允许删除")]
    public class UndeleteYardMaintainRule : NoReferencedRule<YardMaintain>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        public UndeleteYardMaintainRule()
        {
            Properties.Add(YardZone.YardMaintainIdProperty);
            MessageBuilder = (e, c) =>
            {
                var yard = e as YardMaintain;
                return "园区[{0}]被园片区维护引用了[{1}]次，不允许删除".L10nFormat(yard.Name, c);
            };
        }
    }
}
