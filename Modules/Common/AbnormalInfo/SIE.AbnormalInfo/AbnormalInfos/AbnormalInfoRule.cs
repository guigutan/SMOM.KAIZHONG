using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 同个异常信息分类所使用的“单位”必须一致
    /// </summary>
    [System.ComponentModel.DisplayName("同个异常信息分类所使用的“单位”必须一致")]
    [System.ComponentModel.Description("同个异常信息分类所使用的“单位”必须一致")]
    public class AbnormalInfoCategoryRule : EntityRule<AbnormalInfoCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalInfoCategoryRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">事件参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var abnormalInfoCategory = entity as AbnormalInfoCategory;
            List<string> disUnitTypeList = new List<string>();
            List<double> sendUpgradeSetIds = new List<double>();
            foreach (var sendUpgradeSet in abnormalInfoCategory.SendUpgradeSetList)
            {
                disUnitTypeList.Add(sendUpgradeSet.UnitType.ToString());
                sendUpgradeSetIds.Add(sendUpgradeSet.Id);
                if (sendUpgradeSet.TimeType <= 0)
                    e.BrokenDescription = "【分类编码为{0}】下的时间必须大于0！".L10nFormat(abnormalInfoCategory.Code);
            }

            var disUnitTypeData = RT.Service.Resolve<AbnormalInfoController>().GetDisUnitTypeData(abnormalInfoCategory.Id, sendUpgradeSetIds);
            foreach (var disUnitType in disUnitTypeData)
            {
                disUnitTypeList.Add(disUnitType.UnitType.ToString());
            }

            HashSet<string> disUnitTypes = new HashSet<string>(disUnitTypeList);
            if (disUnitTypes.Count > 1)
                e.BrokenDescription = "同个异常信息分类【分类编码为{0}】所使用的“单位”必须一致！".L10nFormat(abnormalInfoCategory.Code);
        }
    }

    /// <summary>
    /// 推送升级非重复数据验证
    /// </summary>
    [System.ComponentModel.DisplayName("同一条异常分类，推送升级设置数据不能重复")]
    [System.ComponentModel.Description("同一条异常分类，推送升级设置数据不能重复")]
    public class SenderUpgradeSettingsNotDuplicateRule : PropertyRule<AbnormalInfoCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SenderUpgradeSettingsNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = e =>
            {
                var entity = e as SenderUpgradeSettings;
                return string.Format("同一个异常分类【分类描述为{0}】，推送升级设置数据不能重复！", entity.AbnormalInfoCategory.Desc).L10N();
            };
        }

        /// <summary>
        /// 托管属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return AbnormalInfoCategory.SendUpgradeSetListProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var category = entity as AbnormalInfoCategory;
            if (category != null && category.SendUpgradeSetList?.Count > 0)
            {
                var setIds = category.SendUpgradeSetList.Select(p => p.Id).ToList();
                if (category.SendUpgradeSetList.DeletedList?.Count > 0)
                    setIds.AddRange(category.SendUpgradeSetList.DeletedList.Select(p => (double)p.GetId()).ToList());

                var srcSettings = RT.Service.Resolve<AbnormalInfoController>().GetSenderUpgradeSettingsExceptIds(category.Id, setIds);

                foreach (var set in category.SendUpgradeSetList)
                {
                    if (srcSettings.Any(p => p.Id != set.Id && p.ConditionType == set.ConditionType && p.TimeType == set.TimeType && p.UnitType == set.UnitType)
                        || category.SendUpgradeSetList.Any(p => p.Id != set.Id && p.ConditionType == set.ConditionType && p.TimeType == set.TimeType && p.UnitType == set.UnitType))
                    {
                        e.BrokenDescription = e.BrokenDescription ?? "" + MessageBuilder(set);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 同个异常信息定义所使用的“单位”必须一致
    /// </summary>
    [System.ComponentModel.DisplayName("同个异常信息定义所使用的“单位”必须一致")]
    [System.ComponentModel.Description("同个异常信息定义所使用的“单位”必须一致")]
    public class AbnormalInfoDefinitionRule : EntityRule<AbnormalInfoDefinition>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalInfoDefinitionRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">事件参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var abnormalController = RT.Service.Resolve<AbnormalInfoController>();
            var abnormalInfoDefinition = entity as AbnormalInfoDefinition;
            if (abnormalInfoDefinition == null)
            {
                throw new ValidationException("异常信息定义不存在".L10N());
            }
            if (abnormalInfoDefinition.AbnormalSource == AbnormalSource.Alert)
            {
                if (!abnormalInfoDefinition.AlerterId.HasValue)
                {
                    e.BrokenDescription = "异常来源为预警平台时，异常编码不可为空！".L10N();
                    return;
                }
                var definitionsCount = abnormalController.GetAlertAbnormalDefinitions(AbnormalSource.Alert, abnormalInfoDefinition.AlerterId.Value, abnormalInfoDefinition.AbnormalCategoryId).Count(p => p.Id != abnormalInfoDefinition.Id);
                if (definitionsCount > 0)
                {
                    e.BrokenDescription = "异常来源为预警平台时，同一个异常编码【异常编码为{0}】，同一个异常分类【分类描述为{1}】，异常信息描述不可重复！".L10nFormat(abnormalInfoDefinition.Code, abnormalInfoDefinition.AbnormalCategory.Code);
                    return;
                }

            }

            if (abnormalInfoDefinition.AbnormalSource == AbnormalSource.FirstInspection)
            {
                var definitionsCount = abnormalController.GetAbnormalDefinitions(AbnormalSource.FirstInspection).Count(p => p.Id != abnormalInfoDefinition.Id);
                if (definitionsCount > 0)
                {
                    e.BrokenDescription = "已经存异常来源为首检过程整改异常信息".L10N();
                    return;
                }
            }

            if (abnormalInfoDefinition.AbnormalSource == AbnormalSource.PatrolInspBill)
            {
                var definitionsCount = abnormalController.GetAbnormalDefinitions(AbnormalSource.PatrolInspBill).Count(p => p.Id != abnormalInfoDefinition.Id);
                if (definitionsCount > 0)
                {
                    e.BrokenDescription = "已经存异常来源为巡检过程整改异常信息".L10N();
                    return;
                }
            }

            if (abnormalInfoDefinition.AbnormalSource == AbnormalSource.EsdPatrolInspTask)
            {
                var definitionsCount = abnormalController.GetAbnormalDefinitions(AbnormalSource.EsdPatrolInspTask).Count(p => p.Id != abnormalInfoDefinition.Id);
                if (definitionsCount > 0)
                {
                    e.BrokenDescription = "已经存异常来源为ESD异常整改信息".L10N();
                    return;
                }
            }

            if (abnormalInfoDefinition.AbnormalSource == AbnormalSource.EquipCheck)
            {
                var definitionsCount = abnormalController.GetAbnormalDefinitions(AbnormalSource.EquipCheck).Count(p => p.Id != abnormalInfoDefinition.Id);
                if (definitionsCount > 0)
                {
                    e.BrokenDescription = "已经存异常来源为设备点检信息".L10N();
                    return;
                }
            }

            List<string> disUnitTypeList = new List<string>();
            List<double> sendUpgradeSetIds = new List<double>();
            foreach (var sendUpgradeSet in abnormalInfoDefinition.SendUpgradeSetList)
            {
                disUnitTypeList.Add(sendUpgradeSet.UnitType.ToString());
                sendUpgradeSetIds.Add(sendUpgradeSet.Id);
                if (sendUpgradeSet.TimeType <= 0)
                {
                    e.BrokenDescription = "【异常分类编码为{0}】下的时间必须大于0！".L10nFormat(abnormalInfoDefinition.AbnormalCategory.Code);
                    return;
                }
            }

            var disUnitTypeData = abnormalController.GetDefDisUnitTypeData(abnormalInfoDefinition.Id, sendUpgradeSetIds);
            foreach (var disUnitType in disUnitTypeData)
            {
                disUnitTypeList.Add(disUnitType.UnitType.ToString());
            }

            HashSet<string> disUnitTypes = new HashSet<string>(disUnitTypeList);
            if (disUnitTypes.Count > 1)
                e.BrokenDescription = "同个异常信息分类【分类编码为{0}】所使用的“单位”必须一致！".L10nFormat(abnormalInfoDefinition.AbnormalCategory.Code);
        }
    }

    /// <summary>
    /// 推送升级非重复数据验证
    /// </summary>
    [System.ComponentModel.DisplayName("同一条异常信息定义，推送升级设置数据不能重复")]
    [System.ComponentModel.Description("同一条异常信息定义，推送升级设置数据不能重复")]
    public class DefinitionSenderSettingsNotDuplicateRule : PropertyRule<AbnormalInfoDefinition>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefinitionSenderSettingsNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = e =>
            {
                var entity = e as DefinitionSenderSettings;
                var a = "";
                if (entity.AbnormalDefinition == null)
                {
                    a =  "同一个异常定义，推送升级设置数据不能重复！".L10N();

                }else if (entity.AbnormalDefinition != null)
                {
                    a = string.Format("同一个异常定义【异常编码为{0},分类描述为{1}】，推送升级设置数据不能重复！", entity.AbnormalDefinition.Code, entity.AbnormalDefinition.Desc).L10N();
                }
                return a;
            };
        }

        /// <summary>
        /// 托管属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return AbnormalInfoDefinition.SendUpgradeSetListProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var def = entity as AbnormalInfoDefinition;
            if (def != null && def.SendUpgradeSetList?.Count > 0)
            {
                var setIds = def.SendUpgradeSetList.Select(p => p.Id).ToList();
                if (def.SendUpgradeSetList.DeletedList?.Count > 0)
                    setIds.AddRange(def.SendUpgradeSetList.DeletedList.Select(p => (double)p.GetId()).ToList());

                var srcSettings = RT.Service.Resolve<AbnormalInfoController>().GetDefinitionSenderSettingsExceptIds(def.Id, setIds);

                foreach (var set in def.SendUpgradeSetList)
                {
                    if (srcSettings.Any(p => p.Id != set.Id && p.ConditionType == set.ConditionType && p.TimeType == set.TimeType && p.UnitType == set.UnitType)
                        || def.SendUpgradeSetList.Any(p => p.Id != set.Id && p.ConditionType == set.ConditionType && p.TimeType == set.TimeType && p.UnitType == set.UnitType))
                    {
                        e.BrokenDescription = e.BrokenDescription ?? "" + MessageBuilder(set);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 原因分析必填
    /// </summary>
    [System.ComponentModel.DisplayName("原因分析必填")]
    [System.ComponentModel.Description("原因分析必填")]
    public class ReasonAnalysisRull : PropertyRule<AbnormalInfor>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReasonAnalysisRull()
        {
            MessageBuilder = (e) =>
            {
                return "原因分析未填写。".L10N();
            };
        }

        /// <summary>
        /// 托管属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return AbnormalInfor.ReasonAnalysisProperty;
            }
        }

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var bill = entity as AbnormalInfor;
            if (bill.AbnormalStatus != AbnormalStatus.Close) return;
            if (bill.ReasonAnalysis.IsNullOrEmpty())
                e.BrokenDescription = MessageBuilder(bill);
        }

        /// <summary>
        /// 改善对策必填
        /// </summary>
        [System.ComponentModel.DisplayName("改善对策必填")]
        [System.ComponentModel.Description("改善对策必填")]
        public class MeasureRull : PropertyRule<AbnormalInfor>
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public MeasureRull()
            {
                MessageBuilder = (e) =>
                {
                    return "改善对策未填写。".L10N();
                };
            }

            /// <summary>
            /// 托管属性
            /// </summary>
            protected override IManagedProperty Property
            {
                get
                {
                    return AbnormalInfor.MeasureProperty;
                }
            }

            /// <summary>
            /// 验证逻辑
            /// </summary>
            /// <param name="entity">实体</param>
            /// <param name="e">规则参数</param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var bill = entity as AbnormalInfor;
                if (bill.AbnormalStatus != AbnormalStatus.Close) return;
                if (bill.Measure.IsNullOrEmpty())
                    e.BrokenDescription = MessageBuilder(bill);
            }
        }
    }
}
