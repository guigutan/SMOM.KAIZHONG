using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.AssetDisposals.Rues
{
    /// <summary>
    /// 设备编码非重复验证规则
    /// </summary>
    [DisplayName("设备编码非重复验证规则")]
    [Description("设备编码非重复验证规则")]
    public class AssetDisposalEquipmentNotDuplicateRule : NotDuplicateRule<AssetDisposalEquipment>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetDisposalEquipmentNotDuplicateRule()
        {
            Properties.Add(AssetDisposalEquipment.EquipAccountIdProperty);
            MessageBuilder = (e) =>
            {
                var disposalEquip = e as AssetDisposalEquipment;

                if (disposalEquip != null)
                {
                    return "已存在【设备编码】是【{0}】的处置单,同一设备编码不能重复处置"
                        .L10nFormat(disposalEquip.EquipAccount.Code);
                }
                else
                {
                    return "处置清单中的【设备编码】已存在于其他处置单中,同一设备编码不能重复处置".L10N();
                }
            };
        }
    }

    /// <summary>
    /// 工治具序列号非重复验证规则
    /// </summary>
    [DisplayName("工治具序列号非重复验证规则")]
    [Description("工治具序列号非重复验证规则")]
    public class AssetDisposalFixtureNotDuplicateRule : NotDuplicateRule<AssetDisposalFixture>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssetDisposalFixtureNotDuplicateRule()
        {
            Properties.Add(AssetDisposalFixture.FixtureAccountIdProperty);
            MessageBuilder = (e) =>
            {
                var disposalFixture = e as AssetDisposalFixture;

                if (disposalFixture != null)
                {
                    return "已存在【序列号】是【{0}】的处置单,同一工治具序列号不能重复处置"
                        .L10nFormat(disposalFixture.FixtureAccount.Code);
                }
                else
                {
                    return "处置清单中的【序列号】已存在于其他处置单中,同一工治具序列号不能重复处置".L10N();
                }
            };
        }
    }
}
