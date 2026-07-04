using SIE.Core.Enums;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Resources.Enterprises;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ShipPlan
{
    /// <summary>
    /// 分配仓库规则控制器
    /// </summary>
    public partial class AssignWarehouseRuleController : DomainController
    {
        /// <summary>
        /// 获取分配仓库规则数据
        /// </summary>
        /// <param name="criteria">分配仓库规则查询实体</param>
        /// <returns>分配仓库规则数据</returns>
        public virtual EntityList<AssignWarehouseRule> GetAssignWarehouseRules(AssignWarehouseRuleCriteria criteria)
        {
            var query = Query<AssignWarehouseRule>();

            if (criteria.OrderType.HasValue)
            {
                query.Where(p => p.OrderType == criteria.OrderType.Value);
            }
            if (criteria.ItemType.HasValue)
            {
                query.Where(p => p.ItemType == criteria.ItemType.Value);
            }
            if (criteria.ItemCategoryId.HasValue)
            {
                query.Where(p => p.ItemCategoryId == criteria.ItemCategoryId.Value);
            }

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取分配仓库规则数据
        /// </summary>
        /// <param name="orderTypes">单据类型集合</param>
        /// <param name="itemTypes">基本类型集合</param>
        /// <returns>分配仓库规则数据</returns>
        public virtual EntityList<AssignWarehouseRule> GetAssignWarehouseRuleList(List<OrderType> orderTypes, List<ItemType> itemTypes)
        {
            var query = Query<AssignWarehouseRule>();
            if (orderTypes.Any())
                query.Where(p => orderTypes.Contains(p.OrderType));

            if (itemTypes.Any())
                query.Where(p => itemTypes.Contains(p.ItemType));

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取分配仓库规则
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="item">物料</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="enterpriseId">部门ID</param>
        /// <param name="ResourceId">资源ID</param>
        /// <returns>分配仓库规则</returns>
        public virtual AssignWarehouseRule GetAssignWarehouseRuleData(OrderType orderType, Item item, double? customerId, double? supplierId, double? enterpriseId, double? ResourceId = null)
        {
            var query = Query<AssignWarehouseRule>().Where(p => p.OrderType == orderType && p.ItemType == item.Type);
            if (customerId.HasValue)
            {
                query.Where(p => p.CustomerId == customerId.Value || p.CustomerId == null);
            }
            else
            {
                query.Where(p => p.CustomerId == null);
            }
            if (supplierId.HasValue)
            {
                query.Where(p => p.SupplierId == supplierId.Value || p.SupplierId == null);
            }
            else
            {
                query.Where(p => p.SupplierId == null);
            }

            var itemCategory = RT.Service.Resolve<ItemController>().GetItemCategoryRelationData(item.Id, CategoryType.Item)?.ItemCategory;
            if (itemCategory == null)
            {
                query.Where(p => p.ItemCategoryId == null);
            }
            else
            {
                EntityList<ItemCategory> itemCategories = new EntityList<ItemCategory>();
                if (itemCategory.TreePId == null)
                {
                    itemCategories.Add(itemCategory);
                }
                else
                {
                    itemCategories.Add(itemCategory);
                    GetItemCateGoryFuns(itemCategory.TreePId, itemCategories);
                }

                if (itemCategories.Any())
                {
                    var itemCategorieIds = itemCategories.Select(p => p.Id).Distinct().ToList();
                    query.Where(p => itemCategorieIds.Contains((double)p.ItemCategoryId) || p.ItemCategoryId == null);
                }
            }

            if (!enterpriseId.HasValue || enterpriseId == 0)
            {
                query.Where(p => p.EnterpriseId == null);
            }
            else
            {
                var enterprise = RF.GetById<Enterprise>(enterpriseId.Value);
                EntityList<Enterprise> enterprises = new EntityList<Enterprise>();
                if (enterprise.TreePId == null)
                {
                    enterprises.Add(enterprise);
                }
                else
                {
                    enterprises.Add(enterprise);
                    GetEnterpriseFuns(enterprise.TreePId, enterprises);
                }

                if (enterprises.Any())
                {
                    var enterpriseIds = enterprises.Select(p => p.Id).Distinct().ToList();
                    query.Where(p => enterpriseIds.Contains((double)p.EnterpriseId) || p.EnterpriseId == null);
                }
            }
            //增加资源过滤
            if (ResourceId.HasValue)
            {
                query.Where(p => p.ResourceId == ResourceId.Value || p.ResourceId == null);
            }
            AssignWarehouseRule assignWhRule = query.OrderBy(p => p.Priority).OrderBy(p => p.CreateDate).FirstOrDefault();

            return assignWhRule;
        }

        /// <summary>
        /// 获取物料分类递归方法
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="itemCategories">物料分类数据</param>
        private void GetItemCateGoryFuns(double? parentId, EntityList<ItemCategory> itemCategories)
        {
            if (parentId == null)
            {
                return;
            }
            var tempItemCategory = RT.Service.Resolve<ItemController>().GetItemCategory(parentId);
            if (tempItemCategory != null)
            {
                itemCategories.Add(tempItemCategory);
                GetItemCateGoryFuns(tempItemCategory.TreePId, itemCategories);
            }
        }

        /// <summary>
        /// 获取企业模型递归方法
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="enterprises">企业模型</param>
        private void GetEnterpriseFuns(double? parentId, EntityList<Enterprise> enterprises)
        {
            if (parentId == null)
            {
                return;
            }
            var tempEnterprise = RT.Service.Resolve<EnterpriseController>().GetEnterpriseById(parentId.Value);
            if (tempEnterprise != null)
            {
                enterprises.Add(tempEnterprise);
                GetEnterpriseFuns(tempEnterprise.TreePId, enterprises);
            }
        }

        /// <summary>
        /// 判断重复数据逻辑
        /// </summary>        
        public virtual bool GetSameRule(OrderType orderType, double curId, double? resourceId, ItemType itemType, double? itemCateId, double? cusId, double? supId, double? entId)
        {
            var query = Query<AssignWarehouseRule>().Where(p => p.OrderType == orderType && p.ItemType == itemType
             && p.ItemCategoryId == itemCateId && p.CustomerId == cusId && p.SupplierId == supId && p.EnterpriseId == entId && p.Id != curId
            );
            if (orderType == OrderType.WorkFeed && resourceId > 0)
            {
                query.Where(f => f.ResourceId == resourceId);
            }
            return query.Count() > 0;
        }
    }
}
