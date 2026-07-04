using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 物料检验特性-物料周期检完整性验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("物料周期检完整性验证规则")]
    [System.ComponentModel.Description("勾选物料周期检时，间隔周期、周期类型必填验证规则")]
    public class ItemInspCharactRecurInspRules : EntityRule<ItemInspCharacteristics>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemInspCharactRecurInspRules()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var inspCharacteristics = entity as ItemInspCharacteristics;
            if (inspCharacteristics.RecurringInspection && (!inspCharacteristics.PeriodType.HasValue || !inspCharacteristics.IntervalPeriod.HasValue))
                e.BrokenDescription = "物料{0}勾选 物料周期检 时，周期类型与间隔周期不可空".L10nFormat(inspCharacteristics.Item.Code);
        }
    }

    /// <summary>
    /// 物料检验特性-供方状态为禁用,不允许设置物料特性
    /// </summary>
    [System.ComponentModel.DisplayName("供方状态是否禁用验证规则")]
    [System.ComponentModel.Description("供方状态为禁用,不允许设置物料特性规则")]
    public class ItemInspCharactDisableRules : EntityRule<ItemInspCharacteristics>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemInspCharactDisableRules()
        {
            Scope = EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var inspCharacteristics = entity as ItemInspCharacteristics;
            //修改供方状态时，忽略该规则
            var source = RF.Find<ItemInspCharacteristics>().GetById(entity.Id) as ItemInspCharacteristics;
            if (source != null && source.SupplierState != inspCharacteristics.SupplierState) return;

            if (inspCharacteristics.SupplierState == State.Disable)
                e.BrokenDescription = "物料{0}供方状态为禁用,不允许设置物料特性规则。".L10nFormat(inspCharacteristics.Item.Code);
        }
    }

    /// <summary>
    /// 物料检验特性-供方状态为禁用,不允许设置物料特性
    /// </summary>
    [System.ComponentModel.DisplayName("物料周期检和确认检规则")]
    [System.ComponentModel.Description("物料周期检和确认检只能选择其一规则")]
    public class ItemInspCharactSingleRules : EntityRule<ItemInspCharacteristics>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemInspCharactSingleRules()
        {
            Scope = EntityStatusScopes.Update | EntityStatusScopes.Add;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var inspCharacteristics = entity as ItemInspCharacteristics;

            if (inspCharacteristics.RecurringInspection && inspCharacteristics.ConfirmInspection)
                e.BrokenDescription = "物料{0}周期检和确认检只能选择其一。".L10nFormat(inspCharacteristics.Item.Code);
        }
    }

    /// <summary>
    /// 失效时间验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("免检失效时间验证")]
    [System.ComponentModel.Description("免检失效时间不能小于免检生效时间")]
    public class EffectiveEndTimeRule : EntityRule<ItemInspCharacteristics>
    {
       

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var vl = entity as ItemInspCharacteristics;
            if (vl.EffectiveEndTime.HasValue && vl.EffectiveEndTime < vl.EffectiveStartTime)
            {
                e.BrokenDescription = "免检失效时间不能小于免检生效时间".L10N();
            }
        }
    }

    /// <summary>
    /// 失效时间验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("免检失效时间验证")]
    [System.ComponentModel.Description("免检生效时间不能为空")]
    public class EffectiveStartTimeRule : EntityRule<ItemInspCharacteristics>
    {

        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var vl = entity as ItemInspCharacteristics;
            if (vl.InspectionFree && !vl.EffectiveStartTime.HasValue)
            {
                e.BrokenDescription = "免检生效时间不能为空".L10N();
            }
        }
    }
}
