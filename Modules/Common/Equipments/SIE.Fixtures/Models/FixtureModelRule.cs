using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.Fixtures.Models
{

    /// <summary>
    /// 工治具型号实体验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具型号实体验证规则")]
    [System.ComponentModel.Description("配置的工治具型号,提交前校验")]
    public class FixtureModelRule : EntityRule<FixtureModel>
    {
        /// <summary>
        /// 验证配置的设备类型，必须填写位置列表至少一行
        /// </summary>
        /// <param name="entity">设备台帐</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var fixtureModel = entity as FixtureModel;
            if (fixtureModel != null && fixtureModel.IsFeeder && !fixtureModel.SlotType.HasValue)
            {
                e.BrokenDescription = "勾选Feeder必须填写槽位类型!".L10N();
            }
        }
    }

    /// <summary>
    /// 工治具型号设备列表实体验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具型号设备列表实体验证规则")]
    [System.ComponentModel.Description("配置的工治具型号设备列表,提交前校验")]
    public class FixtureModelEquipDetailRule : EntityRule<FixtureModelEquipDetail>
    {
        /// <summary>
        /// 验证配置的设备类型，必须填写位置列表至少一行
        /// </summary>
        /// <param name="entity">设备台帐</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var fixtureModelEquipDetail = entity as FixtureModelEquipDetail;
            if (fixtureModelEquipDetail != null)
            {
                if (RT.Service.Resolve<CoreFixtureController>().IsExsitedFixtureModelEquipDetail(fixtureModelEquipDetail))
                    e.BrokenDescription = "工治具型号【{0}】已存在设备型号【{1}】!".L10nFormat(fixtureModelEquipDetail.FixtureModel.Code, fixtureModelEquipDetail.EquipModel.Code);
            }
        }
    }


    /// <summary>
    /// 工治具型号保养项目实体验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具型号保养项目实体验证规则")]
    [System.ComponentModel.Description("配置的工治具保养项目列表,提交前校验")]
    public class FixtureModelMaintainProjectRule : EntityRule<FixtureModelMaintainProject>
    {
        /// <summary>
        /// 验证配置的设备类型，必须填写位置列表至少一行
        /// </summary>
        /// <param name="entity">设备台帐</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var fixtureModelMaintainProject = entity as FixtureModelMaintainProject;
            if (fixtureModelMaintainProject != null)
            {
                if (fixtureModelMaintainProject.CheckTag == Defects.InspectionItems.CheckTag.Quantitative && fixtureModelMaintainProject.MinValue == null && fixtureModelMaintainProject.MaxValue == null)
                    e.BrokenDescription = "检测标记为定量时，最大值和最小值至少一个有值".L10N();
                if (fixtureModelMaintainProject.MinValue > fixtureModelMaintainProject.MaxValue)
                    e.BrokenDescription = "工治具保养项目子页签中应检测合格小值≤检测合格大值".L10N();
                if (RT.Service.Resolve<CoreFixtureController>().IsExsitedFixtureModelMaintainProject(fixtureModelMaintainProject))
                    e.BrokenDescription = "工治具型号【{0}】已存在保养项目【{1}】!".L10nFormat(fixtureModelMaintainProject.FixtureModel.Code, fixtureModelMaintainProject.MaintainProject.Name);
            }
        }
    }


}
