using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.LES.Distributions
{
    [DisplayName("产线仓库非重验证规则")]
    class NotDuplicateDistributionSetting : EntityRule<DistributionSetting>
    {
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var distributionSetting = entity as DistributionSetting;
            if (distributionSetting != null)
            {
                var count = RT.Service.Resolve<DistributionController>().GetDistributionSettings(distributionSetting.Id, distributionSetting.WarehouseId,
                    distributionSetting.ProductLineId, distributionSetting.State);
                if (count > 0)
                    e.BrokenDescription = "已经存在相同的目标产线和来源仓库！".L10N();
            }
        }
    }
}
