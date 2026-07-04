using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.AssetRequisitions.Rules
{
    /// <summary>
    /// 工治具编码非重复验证规则
    /// </summary>
    [DisplayName("工治具编码非重复验证规则")]
    [Description("工治具编码非重复验证规则")]
    public class AssetRequisitionFixtureNotDuplicateRule : NotDuplicateRule<AssetRequisitionFixture>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetRequisitionFixtureNotDuplicateRule()
        {
            Properties.Add(AssetRequisitionFixture.AssetRequisitionIdProperty);
            Properties.Add(AssetRequisitionFixture.FixtureEncodeIdProperty);
            MessageBuilder = (e) => { return "工治具编码不能重复".L10N(); };
        }
    }
}
