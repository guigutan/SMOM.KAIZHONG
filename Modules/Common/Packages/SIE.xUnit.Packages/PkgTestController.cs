using SIE.Common.Sort;
using SIE.Domain;
using SIE.Packages;
using SIE.Packages.Packages;
using SIE.xUnit.Core;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Packages
{
    /// <summary>
    /// 包装规则测试控制器
    /// </summary>
    public partial class PkgTestController : DomainController
    {
        /// <summary>
        /// 创建主单位
        /// </summary>
        /// <returns></returns>
        public virtual PackingUnit CreateMasterUnit()
        {
            var ctl = RT.Service.Resolve<PackageController>();
            //判断是否已存在主单位
            var masterUnit = ctl.GetMasterUnit();
            if (masterUnit == null)
                masterUnit = ctl.AddMasterUnit();
            RF.Save(masterUnit);
            return masterUnit;
        }

        /// <summary>
        /// 创建包装单位
        /// </summary>
        /// <returns></returns>
        public virtual PackingUnit CreatePackingUnit()
        {
            PackingUnit unit = new PackingUnit();
            unit.GenerateId();
            unit.Code = "PackingUnitCode" + unit.Id;
            unit.Name = "PackingUnitName" + unit.Id;
            RF.Save(unit);
            return unit;
        }

        /// <summary>
        /// 创建包装规则
        /// </summary>
        /// <returns></returns>
        public virtual PackageRule CreatePackageRule(bool IsSerialNumber)
        {
            var baseCtl = RT.Service.Resolve<ContextControllerTest>();
            var numberRule = baseCtl.CreateNumberRule("包装条码号");

            var rule = new PackageRule();
            rule.GenerateId();
            rule.Code = "PackageRuleCode" + rule.Id;
            rule.Name = "PackageRuleName" + rule.Id;

            var ruleDtl = new PackageRuleDetail();
            ruleDtl.PackageUnit = CreateMasterUnit();
            ruleDtl.Qty = 1;
            ruleDtl.LevelQty = 1;
            ruleDtl.SetIndex(1);
            ruleDtl.NumberRule= numberRule;
            ruleDtl.IsSequence = IsSerialNumber;
            rule.PackageRuleDetailList.Add(ruleDtl);

            var ruleDtl1 = new PackageRuleDetail();
            ruleDtl1.PackageUnit = CreatePackingUnit();
            ruleDtl1.Qty = 2;
            ruleDtl1.LevelQty = 2;
            ruleDtl1.SetIndex(2);
            ruleDtl1.NumberRule = numberRule;
            rule.PackageRuleDetailList.Add(ruleDtl1);

            var ruleDtl2 = new PackageRuleDetail();
            ruleDtl2.PackageUnit = CreatePackingUnit();
            ruleDtl2.Qty = 4;
            ruleDtl2.LevelQty = 2;
            ruleDtl2.SetIndex(3);
            ruleDtl2.NumberRule = numberRule;
            rule.PackageRuleDetailList.Add(ruleDtl2);

            RF.Save(rule);
            return rule;
        }

        /// <summary>
        /// 创建物料包装规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual ItemPackageRule CreateItemPackageRule(double itemId,bool IsSerialNumber)
        {
            var ctl = RT.Service.Resolve<PackageController>();
            var packageRule = CreatePackageRule(IsSerialNumber);
            var itemPackageRule = ctl.CreateItemPackageRule(new List<PackageRule>() { packageRule }, itemId).FirstOrDefault();
            return itemPackageRule;
        }
    }
}
