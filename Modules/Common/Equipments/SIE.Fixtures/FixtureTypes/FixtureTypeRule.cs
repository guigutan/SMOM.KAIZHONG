using SIE.Domain.Validation;
using SIE.Fixtures.Models;
using System;
using System.ComponentModel;

namespace SIE.Fixtures.FixtureTypes
{
    /// <summary>
    /// 被引用的工段，不允许删除
    /// </summary>
    [DisplayName("被引用的工治具类型，不允许删除")]
    [Description("被引用的工治具类型，不允许删除")]
    public class FixtureTypeRule : NoReferencedRule<FixtureType>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FixtureTypeRule()
        {
            Properties.Add(FixtureModel.FixtureTypeIdProperty); 
            MessageBuilder = (o, e) =>
            {
                var ps = o as FixtureType;
                return "工治具类型[{0}]被工治具型号引用，不能删除".L10nFormat(ps.Code);
            };
        }
    }
}
