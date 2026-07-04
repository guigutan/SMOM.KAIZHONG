using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.InventoryTasks
{
    #region 盘点人
    /// <summary>
    /// 盘点人非重复验证规则
    /// </summary>
    [DisplayName("盘点人非重复验证规则")]
    [Description("盘点人非重复验证规则")]
    public class InventoryTaskCounterNotDuplicateRule : NotDuplicateRule<InventoryTaskCounter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryTaskCounterNotDuplicateRule()
        {
            Properties.Add(InventoryTaskCounter.InventoryTaskIdProperty);
            Properties.Add(InventoryTaskCounter.EmployeeIdProperty);
            MessageBuilder = (e) => { return "盘点人不能重复".L10N(); };
        }
    }
    /// <summary>
    /// 盘点人验证规则
    /// </summary>
    [DisplayName("盘点人验证规则")]
    [Description("盘点人验证规则-初盘和复盘必须至少勾选一个")]
    public class InventoryTaskCounterRule : EntityRule<InventoryTaskCounter>
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
            var counter = entity as InventoryTaskCounter;
            if (!counter.First && !counter.Second)
            {
                e.BrokenDescription = "初盘和复盘必须至少勾选一个".L10N();
            }
        }
    }
    #endregion
}
