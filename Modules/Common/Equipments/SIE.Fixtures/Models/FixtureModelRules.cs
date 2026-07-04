using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures.Projects;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Fixtures.Models
{
    #region 保养项目存在工治具型号保养项目引用不能删除
    /// <summary>
    /// 保养项目存在工治具型号保养项目引用不能删除
    /// </summary>
    [DisplayName("NoReferencedRule验证规则")]
    [Description("保养项目存在工治具型号保养项目引用不能删除")]
    public class MaintainProjectReferencedRule : NoReferencedRule<MaintainProject>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaintainProjectReferencedRule()
        {
            Properties.Add(FixtureModelMaintainProject.MaintainProjectIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，保养项目被工治具型号保养项目引用".L10nFormat();
            };
        }
    }
    #endregion

    #region 设备型号存在工治具型号设备清单引用不能删除
    /// <summary>
    /// 设备型号存在工治具型号设备清单引用不能删除
    /// </summary>
    [DisplayName("NoReferencedRule验证规则")]
    [Description("设备型号存在工治具型号设备清单引用不能删除")]
    public class EquipDetailReferencedRule : NoReferencedRule<EquipModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipDetailReferencedRule()
        {
            Properties.Add(FixtureModelEquipDetail.EquipModelIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，设备型号被工治具型号设备清单引用".L10nFormat();
            };
        }
    }
    #endregion

    #region 工治具型号存在工治具编码引用不能删除
    /// <summary>
    /// 工治具型号存在工治具编码引用不能删除
    /// </summary>
    [DisplayName("NoReferencedRule验证规则")]
    [Description("工治具型号存在工治具编码引用不能删除")]
    public class FixtureModelReferencedRule : NoReferencedRule<FixtureModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FixtureModelReferencedRule()
        {
            Properties.Add(FixtureEncode.FixtureModelIdProperty);
            MessageBuilder = (o, e) =>
            {
                return "不能删除，工治具型号被工治具编码引用".L10nFormat();
            };
        }
    }
    #endregion

    #region 工治具型号-上线保养项目规则
    /// <summary>
    /// 工治具型号-上线保养项目规则
    /// </summary>
    [System.ComponentModel.DisplayName("工治具型号-上线保养项目规则")]
    [System.ComponentModel.Description("工治具型号中【上线定期保养标准】大于0时，保养项目至少维护一行上线定期保养项目数据方可保存")]
    public class FixtureModelOnlineProjectRule : EntityRule<FixtureModel>
    {
        /// <summary>
        /// 工治具型号-上线保养项目规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var fixtureModel = entity as FixtureModel;
            if (fixtureModel == null || fixtureModel.OnlineHour <= 0)
                return;
            if (fixtureModel.PersistenceStatus == PersistenceStatus.New)
            {
                if (!fixtureModel.MaintainProjectList.Any(p => p.OnlineMaintain))
                {
                    e.BrokenDescription = "上线定期保养的工治具，需存在勾选了上线定期保养的项目".L10N();
                }
            }
            else if (fixtureModel.PersistenceStatus == PersistenceStatus.Modified)
            {

                if (fixtureModel.MaintainProjectList.Any(p => p.OnlineMaintain))
                {
                    return;
                }

                var modifiedIds = fixtureModel.MaintainProjectList.Select(p => p.Id).ToList<double>();
                if (fixtureModel.MaintainProjectList.Deleted.Count > 0)
                {
                    var deleteds = fixtureModel.MaintainProjectList.Deleted;
                    foreach (FixtureModelMaintainProject deleted in deleteds)
                    {
                        modifiedIds.Add(deleted.Id);
                    }
                }
                var isExist = RT.Service.Resolve<CoreFixtureController>().IsExistModelOnlineMaintains(fixtureModel.Id, modifiedIds);
                if (!isExist)
                    e.BrokenDescription = "上线定期保养的工治具，需存在勾选了上线定期保养的项目".L10N();
            }
        }
    }
    #endregion	

    /// <summary>
    /// 槽位验证规则（型号的工治具类型为Feeder时，槽位必填；否则其他情况，则槽位不可填）
    /// </summary>
    [DisplayName("槽位验证规则")]
    [Description("工治具类型为Feeder槽位必填")]
    public class FixtureModelSlotTypeRule : EntityRule<FixtureModel>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public FixtureModelSlotTypeRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var model = entity as FixtureModel;
            if (model.IsFeeder)
            {
                if (!model.SlotType.HasValue)
                {
                    e.BrokenDescription = "工治具类型为Feeder，槽位必填".L10N();
                }
            }
            else
            {
                if (model.SlotType.HasValue)
                {
                    e.BrokenDescription = "工治具类型不为Feeder，槽位为空".L10N();
                }
            }
        }
    }
}
