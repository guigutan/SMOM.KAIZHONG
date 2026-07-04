using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 上架规则控制器
    /// </summary>
    public partial class RuleController
    {
        /// <summary>
        /// 获取上架规则
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>上架规则集合</returns>
        public virtual EntityList<OnShelvesRule> GetOnShelvesRule(OnShelvesRuleCriteria criteria)
        {
            var query = Query<OnShelvesRule>();
            if (!criteria.Code.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (!criteria.Name.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
            if (criteria.WarehouseId > 0)
                query.Where(p => p.WarehouseId == criteria.WarehouseId);

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取上架规则编号
        /// </summary>
        /// <returns>返回上架规则编号</returns>
        public virtual string GetOnShelvesRuleCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(OnShelvesRule));
            if (config != null && config.BacodeRule != null)
                return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
            return string.Empty;
        }

        /// <summary>
        /// 获取非当前上架策略数据
        /// </summary>
        /// <returns>上架策略数据</returns>
        public virtual EntityList<OnShelvesRule> GetNonCurrentOnShelvesRules(double onShelvesRuleId, double warehouseId)
        {
            return Query<OnShelvesRule>().Where(p => p.Id != onShelvesRuleId && p.WarehouseId == warehouseId).ToList();
        }

        /// <summary>
        /// 获取可用的上架规则
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>上架规则信息</returns>
        public virtual EntityList<OnShelvesRule> GetEnableOnShelvesRule(double warehouseId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<OnShelvesRule>().Where(p => p.WarehouseId == warehouseId && p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取可用的上架规则
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="isDefault">是否默认</param>
        /// <param name="elo">elo</param>
        /// <returns>上架规则信息</returns>
        public virtual OnShelvesRule GetEnableOnShelvesRuleData(double warehouseId, bool isDefault, EagerLoadOptions elo = null)
        {
            var query = Query<OnShelvesRule>().Where(p => p.WarehouseId == warehouseId && p.State == State.Enable && p.IsDefault == isDefault);
            if (elo == null)
            {
                elo = new EagerLoadOptions().LoadWithViewProperty();
            }
            return query.FirstOrDefault(elo);
        }

        /// <summary>
        /// 验证上架规则明细选择策略信息
        /// </summary>
        /// <param name="onShelvesRuleDetail">上架规则明细</param>
        /// <exception cref="ValidationException"></exception>
        private void ValidOnShelvesStrategy(OnShelvesRuleDetail onShelvesRuleDetail)
        {
            if (onShelvesRuleDetail.SceneType == SceneType.NotASRS && (onShelvesRuleDetail.Strategy == StrategyType.Strategy09 || onShelvesRuleDetail.Strategy == StrategyType.Strategy10))
            {
                throw new ValidationException("非立库场景，策略的选项只能选“01-08”策略".L10N());
            }
            if ((onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy01 || onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy02) && !onShelvesRuleDetail.FromLocationId.HasValue)
            {
                throw new ValidationException("来源库位不能为空".L10N());
            }

            if (onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy02 || onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy03)
            {
                if (!onShelvesRuleDetail.ToAreaId.HasValue)
                    throw new ValidationException("目标库区不能为空".L10N());
                if (onShelvesRuleDetail.SceneType == SceneType.ASRS && !onShelvesRuleDetail.ToArea.IsAutomatedArea)
                    throw new ValidationException("目标库区库位不是立库库位，与应用场景不符".L10N());
                else if (onShelvesRuleDetail.SceneType == SceneType.NotASRS && onShelvesRuleDetail.ToArea.IsAutomatedArea)
                    throw new ValidationException("目标库区库位是立库库位，与应用场景不符".L10N());
            }

            if (onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy01 || onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy04)
            {
                if (!onShelvesRuleDetail.ToLocationId.HasValue)
                    throw new ValidationException("目标库位不能为空".L10N());
                if (onShelvesRuleDetail.SceneType == SceneType.ASRS && !onShelvesRuleDetail.ToLocation.IsAutomatedStorage)
                    throw new ValidationException("目标库位不是立库库位，与应用场景不符".L10N());
                else if (onShelvesRuleDetail.SceneType == SceneType.NotASRS && onShelvesRuleDetail.ToLocation.IsAutomatedStorage)
                    throw new ValidationException("目标库位是立库库位，与应用场景不符".L10N());
            }

            if (onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy07 || onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy08)
            {
                if (!onShelvesRuleDetail.ToLogicAreaId.HasValue)
                    throw new ValidationException("目标逻辑分区不能为空".L10N());
                if (onShelvesRuleDetail.SceneType == SceneType.ASRS && !onShelvesRuleDetail.ToLogicArea.IsAutomatedArea)
                    throw new ValidationException("目标逻辑分区库位不是立库库位，与应用场景不符".L10N());
                else if (onShelvesRuleDetail.SceneType == SceneType.NotASRS && onShelvesRuleDetail.ToLogicArea.IsAutomatedArea)
                    throw new ValidationException("目标逻辑分区库位是立库库位，与应用场景不符".L10N());
            }

            if (onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy08)
            {
                if (!onShelvesRuleDetail.FromLogicAreaId.HasValue)
                    throw new ValidationException("来源逻辑分区不能为空".L10N());
            }
            if (onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy09)
            {
                if (!onShelvesRuleDetail.ToStationId.HasValue)
                    throw new ValidationException("目标站台不能为空".L10N());
            }

            if (onShelvesRuleDetail.Strategy.Value == StrategyType.Strategy10)
            {
                if (!onShelvesRuleDetail.ToStationGroupId.HasValue)
                    throw new ValidationException("目标站台组不能为空".L10N());
            }
        }

        /// <summary>
        /// 验证上架规则明细
        /// </summary>
        /// <param name="onShelvesRuleDetail">明细</param>
        public virtual void ValidOnShelvesRule(OnShelvesRuleDetail onShelvesRuleDetail)
        {
            if (onShelvesRuleDetail == null)
            {
                throw new ValidationException("上架规则明细不能为空".L10N());
            }
            if (onShelvesRuleDetail.Strategy == null)
                throw new ValidationException("策略不能为空".L10N());

            ////验证上架规则策略
            ValidOnShelvesStrategy(onShelvesRuleDetail);

            ////待上架物料批次属性限制验证
            if (onShelvesRuleDetail.FromLotAtt01.HasValue && onShelvesRuleDetail.FromLotAtt01Value.IsNullOrEmpty())
                throw new ValidationException("上架物料批次属性值1不能为空".L10N());
            if (onShelvesRuleDetail.FromLotAtt02.HasValue && onShelvesRuleDetail.FromLotAtt02Value.IsNullOrEmpty())
                throw new ValidationException("上架物料批次属性值2不能为空".L10N());
            if (onShelvesRuleDetail.FromLotAtt03.HasValue && onShelvesRuleDetail.FromLotAtt03Value.IsNullOrEmpty())
                throw new ValidationException("上架物料批次属性值3不能为空".L10N());
            if (onShelvesRuleDetail.FromLotAtt04.HasValue && onShelvesRuleDetail.FromLotAtt04Value.IsNullOrEmpty())
                throw new ValidationException("上架物料批次属性值4不能为空".L10N());
            if (!onShelvesRuleDetail.FromLotAtt01.HasValue && (onShelvesRuleDetail.FromLotAtt02.HasValue || onShelvesRuleDetail.FromLotAtt03.HasValue || onShelvesRuleDetail.FromLotAtt04.HasValue))
                throw new ValidationException("请按顺序维护待上架物料批次属性".L10N());
            else if (!onShelvesRuleDetail.FromLotAtt02.HasValue && (onShelvesRuleDetail.FromLotAtt03.HasValue || onShelvesRuleDetail.FromLotAtt04.HasValue))
                throw new ValidationException("请按顺序维护待上架物料批次属性".L10N());
            else if (!onShelvesRuleDetail.FromLotAtt03.HasValue && onShelvesRuleDetail.FromLotAtt04.HasValue)
                throw new ValidationException("请按顺序维护待上架物料批次属性".L10N());

            Dictionary<int, LotAttribute> dicOnShelvesLotAtt = new Dictionary<int, LotAttribute>();
            if (onShelvesRuleDetail.FromLotAtt01.HasValue)
                ValidatLotAtt(dicOnShelvesLotAtt, onShelvesRuleDetail.FromLotAtt01.Value);

            if (onShelvesRuleDetail.FromLotAtt02.HasValue)
                ValidatLotAtt(dicOnShelvesLotAtt, onShelvesRuleDetail.FromLotAtt02.Value);

            if (onShelvesRuleDetail.FromLotAtt03.HasValue)
                ValidatLotAtt(dicOnShelvesLotAtt, onShelvesRuleDetail.FromLotAtt03.Value);

            if (onShelvesRuleDetail.FromLotAtt04.HasValue)
                ValidatLotAtt(dicOnShelvesLotAtt, onShelvesRuleDetail.FromLotAtt04.Value);

            ////目标库位库存批次属性限制验证
            if (onShelvesRuleDetail.ToLotAtt01.HasValue && onShelvesRuleDetail.ToLotAtt01Value.IsNullOrEmpty())
                throw new ValidationException("目标库位库存批次属性值1不能为空".L10N());
            if (onShelvesRuleDetail.ToLotAtt02.HasValue && onShelvesRuleDetail.ToLotAtt02Value.IsNullOrEmpty())
                throw new ValidationException("目标库位库存批次属性值2不能为空".L10N());
            if (onShelvesRuleDetail.ToLotAtt03.HasValue && onShelvesRuleDetail.ToLotAtt03Value.IsNullOrEmpty())
                throw new ValidationException("目标库位库存批次属性值3不能为空".L10N());
            if (onShelvesRuleDetail.ToLotAtt04.HasValue && onShelvesRuleDetail.ToLotAtt04Value.IsNullOrEmpty())
                throw new ValidationException("目标库位库存批次属性值4不能为空".L10N());
            if (!onShelvesRuleDetail.ToLotAtt01.HasValue && (onShelvesRuleDetail.ToLotAtt02.HasValue || onShelvesRuleDetail.ToLotAtt03.HasValue || onShelvesRuleDetail.ToLotAtt04.HasValue))
                throw new ValidationException("请按顺序维护目标库位库存批次属性".L10N());
            else if (!onShelvesRuleDetail.ToLotAtt02.HasValue && (onShelvesRuleDetail.ToLotAtt03.HasValue || onShelvesRuleDetail.ToLotAtt04.HasValue))
                throw new ValidationException("请按顺序维护目标库位库存批次属性".L10N());
            else if (!onShelvesRuleDetail.ToLotAtt03.HasValue && onShelvesRuleDetail.ToLotAtt04.HasValue)
                throw new ValidationException("请按顺序维护目标库位库存批次属性".L10N());

            Dictionary<int, LotAttribute> dicLoctionLotAtt = new Dictionary<int, LotAttribute>();
            if (onShelvesRuleDetail.ToLotAtt01.HasValue)
                ValidatLotAtt(dicLoctionLotAtt, onShelvesRuleDetail.ToLotAtt01.Value);

            if (onShelvesRuleDetail.ToLotAtt02.HasValue)
                ValidatLotAtt(dicLoctionLotAtt, onShelvesRuleDetail.ToLotAtt02.Value);

            if (onShelvesRuleDetail.ToLotAtt03.HasValue)
                ValidatLotAtt(dicLoctionLotAtt, onShelvesRuleDetail.ToLotAtt03.Value);

            if (onShelvesRuleDetail.ToLotAtt04.HasValue)
                ValidatLotAtt(dicLoctionLotAtt, onShelvesRuleDetail.ToLotAtt04.Value);

            ////目标库位空间限制验证
            if (!onShelvesRuleDetail.SpaceLimit1.HasValue && (onShelvesRuleDetail.SpaceLimit2.HasValue || onShelvesRuleDetail.SpaceLimit3.HasValue || onShelvesRuleDetail.SpaceLimit4.HasValue))
                throw new ValidationException("请按顺序维护空间限制条件".L10N());
            else if (!onShelvesRuleDetail.SpaceLimit2.HasValue && (onShelvesRuleDetail.SpaceLimit3.HasValue || onShelvesRuleDetail.SpaceLimit4.HasValue))
                throw new ValidationException("请按顺序维护空间限制条件".L10N());
            else if (!onShelvesRuleDetail.SpaceLimit3.HasValue && onShelvesRuleDetail.SpaceLimit4.HasValue)
                throw new ValidationException("请按顺序维护空间限制条件".L10N());

            Dictionary<int, SpaceLimit> dicLoctionSpaceLimit = new Dictionary<int, SpaceLimit>();
            if (onShelvesRuleDetail.SpaceLimit1.HasValue)
                ValidatSpaceLimit(dicLoctionSpaceLimit, onShelvesRuleDetail.SpaceLimit1.Value);

            if (onShelvesRuleDetail.SpaceLimit2.HasValue)
                ValidatSpaceLimit(dicLoctionSpaceLimit, onShelvesRuleDetail.SpaceLimit2.Value);

            if (onShelvesRuleDetail.SpaceLimit3.HasValue)
                ValidatSpaceLimit(dicLoctionSpaceLimit, onShelvesRuleDetail.SpaceLimit3.Value);

            if (onShelvesRuleDetail.SpaceLimit4.HasValue)
                ValidatSpaceLimit(dicLoctionSpaceLimit, onShelvesRuleDetail.SpaceLimit4.Value);

            ////目标库位储存限制验证
            if (!onShelvesRuleDetail.StorageLimit1.HasValue && (onShelvesRuleDetail.StorageLimit2.HasValue || onShelvesRuleDetail.StorageLimit3.HasValue || onShelvesRuleDetail.StorageLimit4.HasValue))
                throw new ValidationException("请按顺序维护储存限制条件".L10N());
            else if (!onShelvesRuleDetail.StorageLimit2.HasValue && (onShelvesRuleDetail.StorageLimit3.HasValue || onShelvesRuleDetail.StorageLimit4.HasValue))
                throw new ValidationException("请按顺序维护储存限制条件".L10N());
            else if (!onShelvesRuleDetail.StorageLimit3.HasValue && onShelvesRuleDetail.StorageLimit4.HasValue)
                throw new ValidationException("请按顺序维护储存限制条件".L10N());

            Dictionary<int, StorageLimit> dicLoctionStorageLimit = new Dictionary<int, StorageLimit>();
            if (onShelvesRuleDetail.StorageLimit1.HasValue)
                ValidatStorageLimit(dicLoctionStorageLimit, onShelvesRuleDetail.StorageLimit1.Value);

            if (onShelvesRuleDetail.StorageLimit2.HasValue)
                ValidatStorageLimit(dicLoctionStorageLimit, onShelvesRuleDetail.StorageLimit2.Value);

            if (onShelvesRuleDetail.StorageLimit3.HasValue)
                ValidatStorageLimit(dicLoctionStorageLimit, onShelvesRuleDetail.StorageLimit3.Value);

            if (onShelvesRuleDetail.StorageLimit4.HasValue)
                ValidatStorageLimit(dicLoctionStorageLimit, onShelvesRuleDetail.StorageLimit4.Value);
        }

        /// <summary>
        /// 验证批次属性
        /// </summary>
        /// <param name="dicLotAtt">批次属性字典</param>
        /// <param name="lotAtt">批次属性</param>
        private void ValidatLotAtt(Dictionary<int, LotAttribute> dicLotAtt, LotAttribute lotAtt)
        {
            if (dicLotAtt == null)
                dicLotAtt = new Dictionary<int, LotAttribute>();

            if (!dicLotAtt.ContainsKey((int)lotAtt))
                dicLotAtt.Add((int)lotAtt, lotAtt);
            else
                throw new ValidationException("同一个:{0}只能维护一次".L10nFormat(lotAtt.ToLabel()));
        }

        /// <summary>
        /// 验证空间限制
        /// </summary>
        /// <param name="dicLoctionSpaceLimit"></param>
        /// <param name="spaceLimit"></param>
        private void ValidatSpaceLimit(Dictionary<int, SpaceLimit> dicLoctionSpaceLimit, SpaceLimit spaceLimit)
        {
            if (dicLoctionSpaceLimit == null)
                dicLoctionSpaceLimit = new Dictionary<int, SpaceLimit>();

            if (!dicLoctionSpaceLimit.ContainsKey((int)spaceLimit))
                dicLoctionSpaceLimit.Add((int)spaceLimit, spaceLimit);
            else
                throw new ValidationException("同一个:{0}条件只能维护一次".L10nFormat(spaceLimit.ToLabel()));
        }

        /// <summary>
        /// 验证存储空间
        /// </summary>
        /// <param name="dicLoctionStorageLimit"></param>
        /// <param name="storageLimit"></param>
        private void ValidatStorageLimit(Dictionary<int, StorageLimit> dicLoctionStorageLimit, StorageLimit storageLimit)
        {
            if (dicLoctionStorageLimit == null)
                dicLoctionStorageLimit = new Dictionary<int, StorageLimit>();

            if (!dicLoctionStorageLimit.ContainsKey((int)storageLimit))
                dicLoctionStorageLimit.Add((int)storageLimit, storageLimit);
            else
                throw new ValidationException("同一个:{0}条件只能维护一次".L10nFormat(storageLimit.ToLabel()));
        }

        /// <summary>
        /// 上架规则设置默认命令
        /// </summary>
        /// <param name="onShelvesRuleId">上架策略ID</param>
        public virtual void SetIsDefaultOnShelvesRuleData(double onShelvesRuleId)
        {
            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                OnShelvesRule onShelvesRule = RF.GetById<OnShelvesRule>(onShelvesRuleId);
                if (onShelvesRule == null)
                    throw new ValidationException("上架规则不存在".L10N());

                onShelvesRule.IsDefault = true;
                RF.Save(onShelvesRule);

                EntityList<OnShelvesRule> onShelvesRules = GetNonCurrentOnShelvesRules(onShelvesRuleId, onShelvesRule.WarehouseId);
                onShelvesRules.ForEach(p => p.IsDefault = false);
                RF.Save(onShelvesRules);

                tran.Complete();
            }
        }
    }
}