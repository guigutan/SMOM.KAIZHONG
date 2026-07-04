using SIE.Dock.DockRunMts;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockMaintains
{
    /// <summary>
    /// 月台维护被引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("月台维护引用验证规则")]
    [System.ComponentModel.Description("月台维护被月台运行维护引用不允许删除")]
    public class UndeleteDockMaintainRule : NoReferencedRule<DockMaintain>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        public UndeleteDockMaintainRule()
        {
            Properties.Add(DockRunMt.DockMaintainIdProperty);
            MessageBuilder = (e, c) =>
            {
                var maintain = e as DockMaintain;
                return "月台维护[{0}]被月台运行维护引用了[{1}]次，不允许删除".L10nFormat(maintain.Name, c);
            };
        }
    }
}
