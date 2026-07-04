using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Kit.MES.Storages;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.Tech.Stations;
using System;
using System.ComponentModel;

namespace SIE.Kit.MES.StationStorages
{
    /// <summary>
    /// 工位不能重复验证规则
    /// </summary>
    [DisplayName("工位不能重复验证规则")]
    [Description("工位库存工位不能重复验证规则")]
    public class StationNotDuplicateRule : NotDuplicateRule<StationStorage>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationNotDuplicateRule()
        {
            Properties.Add(StationStorage.StationIdProperty);
            MessageBuilder = (e) =>
            {
                var storage = e as StationStorage;
                return "已存在工位[{0}]的工位库存".L10nFormat(storage.Station?.Name);
            };
        }
    }

    /// <summary>
    /// 工单工位库存不能重复验证规则
    /// </summary>
    [DisplayName("工单工位库存不能重复验证规则")]
    [Description("同个工位不能存在相同的工单工位库存")]
    public class WoStorageNotDuplicateRule : NotDuplicateRule<WoStationStorage>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WoStorageNotDuplicateRule()
        {
            Properties.Add(WoStationStorage.StationStorageIdProperty);
            Properties.Add(WoStationStorage.WorkOrderIdProperty);
            MessageBuilder = (e) =>
            {
                var woStorage = e as WoStationStorage;
                return "已存在工位[{0}]、工单[{1}]的工位库存".L10nFormat(woStorage.StationStorage?.Station?.Name, woStorage.WorkOrder?.No);
            };
        }
    }


    /// <summary>
    /// 工位物料库存验证规则
    /// </summary>
    [DisplayName("工位物料库存验证规则")]
    [Description("同个工位，同个工单，不能存在相同的物料库存")]
    public class StationItemStorageEntityRule : EntityRule<StationItemStorage>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationItemStorageEntityRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证工位物料库存
        /// </summary>
        /// <param name="entity">工位物料库存</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var itemStorage = entity as StationItemStorage;
            var woStorage = itemStorage.WoStorage;
            var stationStorage = woStorage?.StationStorage;
            if (woStorage == null || stationStorage == null)
                return;
            if (RT.Service.Resolve<StationStorageController>().IsExistSameItemStorage(itemStorage.Id, stationStorage.StationId, woStorage.WorkOrderId, itemStorage.ItemId))
            {
                e.BrokenDescription = "工位[{0}]已存在工单[{1}]物料[{2}]的库存".L10nFormat(stationStorage.Station?.Name, woStorage.WorkOrder?.No, itemStorage.Item?.Name);
            }
        }
    }


    /// <summary>
    /// 工位删除验证规则
    /// </summary>
    [DisplayName("工位删除验证规则")]
    [Description("工位被工位库存引用不能删除验证规则")]
    public class StationRefStorageRule : NoReferencedRule<Station>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationRefStorageRule()
        {
            Properties.Add(StationStorage.StationIdProperty);
        }
    }

    /// <summary>
    /// 工单删除验证规则
    /// </summary>
    [DisplayName("工单删除验证规则")]
    [Description("工单被工位工单库存引用不能删除验证规则")]
    public class WorkOrderRefWoStorageRule : NoReferencedRule<WorkOrder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderRefWoStorageRule()
        {
            Properties.Add(WoStationStorage.WorkOrderIdProperty);
        }
    }

    /// <summary>
    /// 工位删除验证规则
    /// </summary>
    [DisplayName("工位删除验证规则")]
    [Description("工位被产线工位货区引用不能删除验证规则")]
    public class StationRefStorageAreaRule : NoReferencedRule<Station>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationRefStorageAreaRule()
        {
            Properties.Add(StationStorageArea.StationIdProperty);
        }
    }
}