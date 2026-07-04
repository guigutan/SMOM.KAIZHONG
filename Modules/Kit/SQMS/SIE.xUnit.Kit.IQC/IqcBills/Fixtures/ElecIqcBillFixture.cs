using SIE.Domain;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.xUnit.IQC.IqcBills.Fixtures;
using SIE.xUnit.Packages;

namespace SIE.xUnit.Elec.IQC.IqcBills.Fixtures
{
    /// <summary>
    /// 电子-来料检验固件
    /// </summary>
    public class ElecIqcBillFixture : IqcBillFixture
    {
        /// <summary>
        /// 标签条码集合
        /// </summary>
        public EntityList<PackingLabel> FixPropLabelList { get; set; } = new EntityList<PackingLabel>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public ElecIqcBillFixture()
        {
            CreatePackingLabels(2);
        }

        /// <summary>
        /// 创建标签条码
        /// </summary>
        protected virtual void CreatePackingLabels(int count)
        {
            var rule = RT.Service.Resolve<PkgTestController>().CreatePackageRule(true);

            var itemRule = new ItemPackageRule()
            {
                PackageRuleId = rule.Id,
                ItemId = FixPropItem.Id,
                IsDefault = true
            };
            itemRule.GenerateId();
            itemRule.Code = $"Code{itemRule.Id}";
            itemRule.Name = $"Name{itemRule.Id}";
            itemRule.Description = $"Desc{itemRule.Id}";
            RF.Save(itemRule);

            var id = FixPropItem.Id;
            var batchNo = $"BAT{id}";
            var asnNo = $"Asn{id}";
            for (var i = 0; i < count; i++)
            {
                var reelId = $"RID{id}{i + 1}";
                RT.Service.Resolve<PackingLabelController>().CreateLabel(FixPropItem, batchNo, 50, reelId, "QMS");
                var label = RT.Service.Resolve<PackingLabelController>().GetLabel(reelId);
                label.AsnNo = asnNo;
                RF.Save(label);
                FixPropLabelList.Add(label);
            }
        }
    }
}
