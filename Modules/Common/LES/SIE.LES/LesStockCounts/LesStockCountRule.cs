using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 实盘数量非空验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("实盘数量非空验证规则")]
    [System.ComponentModel.Description("实盘数量必须大于0")]
    public class ActualCountQtyRequire : PropertyRule<LesStockCountDetail>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return LesStockCountDetail.ActualCountQtyProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as LesStockCountDetail;
            if (dtl != null && dtl.IsNewInventory && (!dtl.ActualCountQty.HasValue || dtl.ActualCountQty <= 0))
            {
                e.BrokenDescription = "实盘数量必须大于0".L10N();
            }
        }
    }

    /// <summary>
    /// 标签非空验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("标签非空验证规则")]
    [System.ComponentModel.Description("标签非空验证规则")]
    public class LabelNoRequire : PropertyRule<LesStockCountDetail>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return LesStockCountDetail.LabelNoProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as LesStockCountDetail;
            if (dtl != null && dtl.IsNewInventory && (dtl.CountDimension== CountDimension.Label || dtl.CountDimension == CountDimension.Location))
            {
                if (dtl.LabelNo.IsNullOrEmpty())
                {
                    e.BrokenDescription = "标签号不能为空！".L10N();
                }
                else
                {
                    RT.Service.Resolve<LesStockCountController>().GetItemLabel(dtl.LabelNo, dtl.ItemId, dtl.WarehouseId,dtl.ItemExtProp);
                }
            }
        }
    }

    /// <summary>
    /// 库位非空验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("库位非空验证规则")]
    [System.ComponentModel.Description("库位非空验证规则")]
    public class StorageLocationRequire : PropertyRule<LesStockCountDetail>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return LesStockCountDetail.StorageLocationIdProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as LesStockCountDetail;
            if (dtl != null && dtl.IsNewInventory && dtl.CountDimension == CountDimension.Location && !dtl.StorageLocationId.HasValue)
            {
                e.BrokenDescription = "库位不能为空！".L10N();
            }
        }
    }

    /// <summary>
    /// 库存状态非空验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("库存状态非空验证规则")]
    [System.ComponentModel.Description("库存状态非空验证规则")]
    public class OnhandStateRequire : PropertyRule<LesStockCountDetail>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return LesStockCountDetail.OnhandStateProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as LesStockCountDetail;
            if (dtl != null && dtl.IsNewInventory && !dtl.OnhandState.HasValue)
            {
                e.BrokenDescription = "库存状态不能为空！".L10N();
            }
        }
    }


    /// <summary>
    /// 批次非空验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("批次非空验证规则")]
    [System.ComponentModel.Description("批次非空验证规则")]
    public class LotRequire : PropertyRule<LesStockCountDetail>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return LesStockCountDetail.LotIdProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as LesStockCountDetail;
            if (dtl != null && dtl.IsNewInventory && dtl.CountDimension == CountDimension.Lot && !dtl.LotId.HasValue)
            {
                e.BrokenDescription = "批次不能为空！".L10N();
            }
        }
    }

    /// <summary>
    /// 工厂非空验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工厂非空验证规则")]
    [System.ComponentModel.Description("工厂非空验证规则")]
    public class FactoryRequire : PropertyRule<LesStockCountDetail>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return LesStockCountDetail.FactoryIdProperty;
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as LesStockCountDetail;
            if (dtl != null && dtl.IsNewInventory && dtl.CountDimension == CountDimension.Location && !dtl.FactoryId.HasValue)
            {
                e.BrokenDescription = "工厂不能为空！".L10N();
            }
        }
    }
}
