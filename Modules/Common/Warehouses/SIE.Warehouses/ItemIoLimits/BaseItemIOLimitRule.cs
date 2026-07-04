using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;

namespace SIE.Warehouses.ItemIoLimits
{

    /// <summary>
    /// 最高库存量验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("库存量验证规则")]
    [System.ComponentModel.Description("最高库存量需大于最低库存量")]
    public class StockQtyRule : PropertyRule<BaseItemIoLimit>
    {
        /// <summary>
        /// 属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return BaseItemIoLimit.MaxStockQtyProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">e</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e != null)
            {
                var data = entity as BaseItemIoLimit;
                if (data != null && data.MaxStockQty.HasValue && data.MinStockQty.HasValue &&
                    data.MaxStockQty.Value <= data.MinStockQty.Value)
                {
                    e.BrokenDescription = "最大库存量[{0}]需大于最低库存量[{1}]".L10nFormat(data.MaxStockQty.Value, data.MinStockQty.Value);
                }
            }
        }
    }

    /// <summary>
    /// 库存量验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("安全库存验证规则")]
    [System.ComponentModel.Description("安全库存量需小于等于最高库存量")]
    public class SafetyStockQtyRule : PropertyRule<BaseItemIoLimit>
    {
        /// <summary>
        /// 属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return BaseItemIoLimit.SafetyStockQtyProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">e</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e != null)
            {
                var data = entity as BaseItemIoLimit;
                if (data != null && data.MaxStockQty.HasValue && data.SafetyStockQty.HasValue &&
                    data.SafetyStockQty.Value > data.MaxStockQty.Value)
                {
                    e.BrokenDescription = "安全库存量[{0}]需小于等于最高库存量[{1}]".L10nFormat(data.SafetyStockQty.Value, data.MaxStockQty.Value);
                }
            }
        }
    }

    /// <summary>
    /// 收发控制仓库不重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("收发控制仓库不重复验证")]
    [System.ComponentModel.Description("收发控制仓库不能重复")]
    public class NotDuplicateWarehouseRule : NotDuplicateRule<BaseItemIoLimit>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateWarehouseRule()
        {
            Properties.Add(BaseItemIoLimit.ItemIdProperty);
            Properties.Add(BaseItemIoLimit.ItemExtPropProperty);
            Properties.Add(BaseItemIoLimit.WarehouseIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = (e) as BaseItemIoLimit;
                return "仓库[{0}]已存在物料收发控制中，不允许重复".L10nFormat(entity.Warehouse.Code);
            };
        }
    }

    /// <summary>
    /// 收发控制验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("收发控制验证规则")]
    [System.ComponentModel.Description("收发控制验证规则")]
    public class ItemIOLimitRule : EntityRule<BaseItemIoLimit>
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">e</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            //if (e != null)
            //{
            //    var data = entity as BaseItemIoLimit;
            //    if (data != null && !data.InUpLimitMultiple.HasValue)
            //        e.BrokenDescription = "收发控制的超收比例上限不能为空".L10N();
            //    if (data != null && !data.MaxInUpLimit.HasValue)
            //        e.BrokenDescription = "收发控制的超收数量上限不能为空".L10N();
            //}
        }
    }
}
