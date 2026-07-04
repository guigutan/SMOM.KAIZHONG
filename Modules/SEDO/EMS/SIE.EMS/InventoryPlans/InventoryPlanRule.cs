using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.InventoryTasks;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.InventoryPlans
{
    #region 盘点人
    /// <summary>
    /// 盘点人非重复验证规则
    /// </summary>
    [DisplayName("盘点人非重复验证规则")]
    [Description("盘点人非重复验证规则")]
    public class InventoryCounterNotDuplicateRule : NotDuplicateRule<InventoryCounter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryCounterNotDuplicateRule()
        {
            Properties.Add(InventoryCounter.InventoryPlanIdProperty);
            Properties.Add(InventoryCounter.EmployeeIdProperty);
            MessageBuilder = (e) => { return "盘点人不能重复".L10N(); };
        }
    }
    /// <summary>
    /// 盘点人验证规则
    /// </summary>
    [DisplayName("盘点人验证规则")]
    [Description("盘点人验证规则-初盘和复盘必须至少勾选一个")]
    public class InventoryCounterRule : EntityRule<InventoryCounter>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var counter = entity as InventoryCounter;
            if (!counter.First && !counter.Second)
            {
                e.BrokenDescription = "初盘和复盘必须至少勾选一个".L10N();
            }
        }
    }
    #endregion


    #region 盘点人
    /// <summary>
    /// 盘点人非重复验证规则
    /// </summary>
    [DisplayName("盘点人非重复验证规则")]
    [Description("盘点人非重复验证规则")]
    public class InventoryPlanFixtureCounterNotDuplicateRule : NotDuplicateRule<InventoryFixtureCounter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryPlanFixtureCounterNotDuplicateRule()
        {
            Properties.Add(InventoryFixtureCounter.InventoryPlanIdProperty);
            Properties.Add(InventoryFixtureCounter.EmployeeIdProperty);
            MessageBuilder = (e) => { return "盘点人不能重复".L10N(); };
        }
    }
    /// <summary>
    /// 盘点人验证规则
    /// </summary>
    [DisplayName("盘点人验证规则")]
    [Description("盘点人验证规则-初盘和复盘必须至少勾选一个")]
    public class InventoryPlanFixtureCounterRule : EntityRule<InventoryFixtureCounter>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var counter = entity as InventoryFixtureCounter;
            if (!counter.First && !counter.Second)
            {
                e.BrokenDescription = "初盘和复盘必须至少勾选一个".L10N();
            }
        }
    }
    #endregion


    #region 盘点人
    /// <summary>
    /// 盘点人非重复验证规则
    /// </summary>
    [DisplayName("盘点人非重复验证规则")]
    [Description("盘点人非重复验证规则")]
    public class InventoryTaskSparePartCounterNotDuplicateRule : NotDuplicateRule<InventoryTaskSparePartCounter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryTaskSparePartCounterNotDuplicateRule()
        {
            Properties.Add(InventoryTaskSparePartCounter.InventoryTaskIdProperty);
            Properties.Add(InventoryTaskSparePartCounter.EmployeeIdProperty);
            MessageBuilder = (e) => { return "盘点人不能重复".L10N(); };
        }
    }
    /// <summary>
    /// 盘点人验证规则
    /// </summary>
    [DisplayName("盘点人验证规则")]
    [Description("盘点人验证规则-初盘和复盘必须至少勾选一个")]
    public class InventoryTaskSparePartCounterRule : EntityRule<InventoryTaskSparePartCounter>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var counter = entity as InventoryTaskSparePartCounter;
            if (!counter.First && !counter.Second)
            {
                e.BrokenDescription = "初盘和复盘必须至少勾选一个".L10N();
            }
        }
    }
    #endregion
}
