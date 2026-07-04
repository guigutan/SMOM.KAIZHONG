using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.MES.Workbench.KeyPerformances
{
    [DisplayName("产线目标值设置类型验证")]
    [Description("产线目标值设置类型必须和其对应车间的设置类型一致")]
    public class LineTargetSetTypeRule : PropertyRule<LineTargetSetting>
    {
        public LineTargetSetTypeRule()
        {
            ConnectToDataSource = true;
        }
        protected override IManagedProperty Property
        {
            get
            {
                return LineTargetSetting.TargetSettingTypeProperty;
            }
        }

        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var set = entity as LineTargetSetting;
            if (set.TargetSettingType != set.ShopPlanRPSetting.TargetSettingType)
                e.BrokenDescription = "产线目标值设置类型必须和其对应车间的设置类型一致".L10N();
        }
    }
}
