using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Common;
using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Common.InvOrg;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Core.Items;
using SIE.Core.TemporaryObject;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Items;
using SIE.EventMessages.Items.Datas;
using SIE.Items.Items;
using SIE.Items.Items.Configs;
using SIE.Items.ProductBoms;
using SIE.Items.ProductBoms.Models;
using SIE.Items.Units;
using SIE.Resources;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Items
{
    /// <summary>
    /// 物料控制器
    /// </summary>
    public partial class ItemController : DomainController, IItem
    {

        #region 不校验工厂物料清单

        /// <summary>
        /// 根据物料编码获取物料清单
        /// </summary>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public virtual EntityList<UnValidFactoryItem> GetUnValidFactoryItemsByItemCodes(List<string> itemCodes)
        {
            var list = itemCodes.SplitContains(codes =>
            {
                return Query<UnValidFactoryItem>().Where(p => codes.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据物料编码获取物料清单
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public virtual UnValidFactoryItem GetUnValidFactoryItemByItemCode(string itemCode)
        {
            var first = Query<UnValidFactoryItem>().Where(p => p.Item.Code == itemCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }

        /// <summary>
        /// 根据物料ID获取物料清单
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<UnValidFactoryItem> GetUnValidFactoryItemsByItemIds(List<double> itemIds)
        {
            var list = itemIds.SplitContains(ids =>
            {
                return Query<UnValidFactoryItem>().Where(p => ids.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        #endregion

        #region 客户料码数据

        /// <summary>
        /// 随机获取一个客户料码数据
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        public virtual ItemCusotmerRelationData GetItemCusotmerRelationData(double ItemId)
        {
            CustomFeatureRel itemCusotmer = Query<CustomFeatureRel>().Where(p => p.ItemId == ItemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (itemCusotmer == null)
            {
                return null;
            }
            ItemCusotmerRelationData data = new ItemCusotmerRelationData();
            data.Attribute2 = itemCusotmer.Customer;
            data.Attribute1 = itemCusotmer.Zqttx;
            return data;
        }

        #endregion

        #region 父级物料

        /// <summary>
        /// 根据物料Id获取父级物料
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<ParentItem> GetParentItemsByItemIds(List<double> itemIds)
        {
            var list = itemIds.SplitContains(ids =>
            {
                return Query<ParentItem>().Where(p => ids.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据物料Id获取父级物料
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual ParentItem GetParentItemByItemId(double itemId)
        {
            var parentItem = Query<ParentItem>().Where(p => p.ItemId == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return parentItem;
        }

        /// <summary>
        /// 根据物料，获取父级物料信息(随机一个)
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public virtual ParentItemData GetParentItemData(string itemCode)
        {
            ParentItem parentItem = Query<ParentItem>().Where(p => p.Item.Code == itemCode).FirstOrDefault();
            if (parentItem == null)
                return null;
            var item = Query<Item>().Where(p => p.Code == parentItem.ParentItemCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (item == null)
                return null;
            ParentItemData parentItemData = new ParentItemData();
            parentItemData.ItemCode = item.Code;
            parentItemData.Weight = item.Weight;
            parentItemData.WeightUnit = item.WeightUnit;
            parentItemData.SpecificationModel = item.SpecificationModel;
            parentItemData.ShortDescription = item.ShortDescription;
            return parentItemData;
        }

        #endregion

        #region 分类层级&分类

        /// <summary>
        /// 获取物料分类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual EntityList<ItemCategory> GetAllItemCategorys(CategoryType type)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            var query = Query<ItemCategory>().Where(p => p.Type == type);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 是否存在子分类层级
        /// </summary>
        /// <param name="parentId">父分类层级ID</param>
        /// <returns>存在子层级返回true，否则返回false</returns>
        public virtual bool HasChildLevel(double parentId)
        {
            return Query<ItemCategoryLevel>().Where(p => p.TreePId == parentId).Count() > 0;
        }

        /// <summary>
        /// 是否存在子分类
        /// </summary>
        /// <param name="parentId">父分类ID</param>
        /// <returns>存在子分类返回true，否则返回false</returns>
        public virtual bool HasChildCategory(double parentId)
        {
            return Query<ItemCategory>().Where(p => p.TreePId == parentId).Count() > 0;
        }

        /// <summary>
        /// 根据分类类型获取相应的分类
        /// </summary>
        /// <param name="type">分类类型</param>
        /// <param name="itemTypeList">物料类型列表</param>
        /// <param name="info">分页信息</param>
        /// <param name="keyWord">模糊查询关键字</param>
        /// <returns>分类列表</returns>
        public virtual EntityList<ItemCategory> GetItemCategoryByType(CategoryType type, List<int> itemTypeList/*ItemType? itemType*/, PagingInfo info = null, string keyWord = "")
        {
            var query = Query<ItemCategory>().Where(p => p.Type == type);
            if (itemTypeList != null)
                query.Where(p => itemTypeList.Contains((int)p.ItemType));
            var source = query.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
            if (!string.IsNullOrEmpty(keyWord))
            {
                EntityList<ItemCategory> items = new EntityList<ItemCategory>();
                foreach (var item in source)
                {
                    if (item.Code.Contains(keyWord) || item.Name.Contains(keyWord))
                    {
                        items.Add(item);
                    }
                    else if (source.ToList().FindIndex(x => x.TreePId == item.Id && (x.Code.Contains(keyWord) || x.Name.Contains(keyWord))) >= 0)
                    {
                        items.Add(item);
                    }
                }
                return items;
            }
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemTypeValueList"></param>
        /// <param name="state"></param>
        /// <param name="productFamilyId"></param>
        /// <param name="keyword">关键字</param>
        /// <param name="pageinfo">分页属性</param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemsByProductFamilyId(List<int> itemTypeValueList, State? state,
            double productFamilyId, string keyword, PagingInfo pageinfo = null)
        {
            var query = Query<Item>()
                .Where(x => x.ProductFamilyId == productFamilyId);

            if (itemTypeValueList != null)
            {
                query = query.Where(p => itemTypeValueList.Contains((int)p.Type));
            }

            if (state != null)
            {
                query.Where(p => p.State == state);
            }

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }


            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 根据产品族分类Id获取物料
        /// </summary>
        /// <param name="itemTypeValueList"></param>
        /// <param name="state"></param>
        /// <param name="productFamilyCategoryId">产品族分类Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pageinfo">分页属性</param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemsByProductFamilyCategoryId(List<int> itemTypeValueList, State? state,
            double? productFamilyCategoryId, string keyword, PagingInfo pageinfo = null)
        {
            var query = Query<Item>().Join<ProductFamily>((a, b) => a.ProductFamilyId == b.Id);
            if (productFamilyCategoryId.HasValue)
            {
                query.Where<ProductFamily>((a, b) => b.CategoryId == productFamilyCategoryId);
            }

            if (itemTypeValueList != null)
            {
                query = query.Where(p => itemTypeValueList.Contains((int)p.Type));
            }

            if (state != null)
            {
                query.Where(p => p.State == state);
            }

            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }


            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料分类类型获取相应的分类维护数据
        /// </summary>
        /// <param name="type">基本分类</param>
        /// <param name="categoryType">分类类型</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<ItemCategory> GetItemCategoryByItemType(ItemType type, CategoryType categoryType, string keyword, PagingInfo info)
        {
            var q = Query<ItemCategory>().Where(p => p.ItemType == type && p.Type == categoryType);
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据分类编码获取相应的分类
        /// </summary>
        /// <param name="codes">编码</param>        
        /// <returns>分类列表</returns>
        public virtual EntityList<ItemCategory> GetItemCategoryByCodes(List<string> codes)
        {
            return codes.SplitContains(tempCodes =>
            {
                return Query<ItemCategory>().Where(p => tempCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>MO
        /// 分类层级是否被分类引用
        /// </summary>
        /// <param name="levelId">分类层级ID</param>
        /// <returns>返回true分类层级被引用，否则返回false</returns> 
        public virtual bool LevelHasCategory(double levelId)
        {
            return Query<ItemCategory>().Where(p => p.LevelId == levelId).Count() > 0;
        }

        /// <summary>
        /// 是否存在分类层级树
        /// </summary>
        /// <param name="type">层级类型</param>
        /// <returns>存在返回true，不存在返回false</returns>
        /// <exception cref="ArgumentNullException">类型为空</exception>
        public virtual bool IsExistLevelType(CategoryType type)
        {
            return Query<ItemCategoryLevel>().Where(p => p.Type == type && p.TreePId == null).Count() > 0;
        }

        /// <summary>
        /// 获取分类父节点
        /// </summary>
        /// <param name="parentId">分类父Id</param>
        /// <returns>分类</returns>
        public virtual ItemCategory GetItemCategory(double? parentId)
        {
            return Query<ItemCategory>().Where(p => p.Id == parentId).FirstOrDefault();
        }

        /// <summary>
        /// 根据ERP分类ID获取分类信息
        /// </summary>
        /// <param name="categoryIds">ERP分类ID集合</param>
        /// <returns></returns>
        public virtual EntityList<ItemCategory> GetItemCategoryByErpCategoryId(List<double> categoryIds)
        {

            return categoryIds.SplitContains((tmpCategoryIds) =>
            {
                return Query<ItemCategory>().Where(p => tmpCategoryIds.Contains((double)p.ErpCategoryId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 通过单位编码列表 获取单位列表(忽略库存组织）
        /// </summary>
        /// <param name="itemCodes">单位编码列表</param>
        /// <returns>单位列表</returns>
        public virtual EntityList<Item> GetItemInvOrgList(List<string> itemCodes)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                return itemCodes.SplitContains((tmpCodes) =>
                {
                    return Query<Item>().Where(p => tmpCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            }
        }

        /// <summary>
        /// 通过物料编码获取 物料集合
        /// </summary>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemDrList(List<string> itemCodes)
        {
            return itemCodes.SplitContains((tmpCodes) =>
            {
                return Query<Item>().Where(p => tmpCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 子分类中是否存在不相同的物料类型
        /// </summary>
        /// <param name="itemCategory">分类</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistNotSameItemType(ItemCategory itemCategory)
        {
            return Query<ItemCategory>().Where(p => p.TreePId == itemCategory.Id)
                                        .Where(p => p.ItemType != itemCategory.ItemType).Count() > 0;
        }

        /// <summary>
        /// 父分类中是否存在不相同的物料类型
        /// </summary>
        /// <param name="itemCategory">分类</param>
        /// <returns>true/false</returns>
        public virtual bool ParentIsExistNotSameItemType(ItemCategory itemCategory)
        {
            return Query<ItemCategory>().Where(p => p.Id == itemCategory.TreePId)
                                        .Where(p => p.ItemType != itemCategory.ItemType).Count() > 0;
        }

        /// <summary>
        /// 获取子分类
        /// </summary>
        /// <param name="parentId">父分类ID</param> 
        /// <returns>子分类列表</returns>
        public virtual EntityList<ItemCategory> GetChildItemCategory(double? parentId)
        {
            if (parentId == null || parentId <= 0)
            {
                return Query<ItemCategory>().Where(p => p.TreePId == null).ToList();
            }
            else
            {
                return Query<ItemCategory>().Where(p => p.TreePId == parentId).ToList();
            }
        }

        /// <summary>
        /// 获取子分类层级
        /// </summary>
        /// <param name="categoryId">父分类ID</param> 
        /// <returns>子分类层级列表</returns>
        public virtual EntityList<ItemCategoryLevel> GetChildLevel(double? categoryId)
        {
            if (categoryId == null)
            {
                return Query<ItemCategoryLevel>().Where(p => p.TreePId == null).ToList();
            }

            var parent = GetById<ItemCategory>(categoryId);
            if (parent != null)
            {
                return Query<ItemCategoryLevel>().Where(p => p.TreePId == parent.LevelId).ToList();
            }

            return new EntityList<ItemCategoryLevel>();
        }
        /// <summary>
        /// 获取子分类层级
        /// </summary>
        /// <param name="parentId">父分类层级ID</param> 
        /// <returns>子分类层级列表</returns>
        public virtual EntityList<ItemCategoryLevel> GetChildItemCategoryLevel(double? parentId)
        {
            if (parentId == null || parentId < 0)
            {
                return Query<ItemCategoryLevel>().Where(p => p.TreePId == null).ToList();
            }
            else
            {
                return Query<ItemCategoryLevel>().Where(p => p.TreePId == parentId).ToList();
            }
        }

        /// <summary>
        /// 获取物料分类层级
        /// </summary>
        /// <param name="categoryType">类型</param>
        /// <returns>分类层级</returns>
        public virtual ItemCategoryLevel GetItemCategoryLevel(CategoryType categoryType)
        {
            return Query<ItemCategoryLevel>().Where(p => p.Type == categoryType && p.TreePId == null).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料分类层级
        /// </summary>
        /// <param name="categoryType">类型</param>
        /// <param name="keyword">编码或名称</param>
        /// <returns></returns>
        public virtual ItemCategoryLevel GetItemCategoryLevel(CategoryType categoryType, string keyword)
        {
            return Query<ItemCategoryLevel>().Where(p => p.Type == categoryType && p.TreePId != null && (p.Code.Contains(keyword) || p.Name.Contains(keyword))).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料分类层级
        /// </summary>
        /// <param name="categoryType">类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>分类层级</returns>
        public virtual EntityList<ItemCategoryLevel> GetItemCategoryLevel(CategoryType categoryType, string keyword, PagingInfo pagingInfo)
        {
            return Query<ItemCategoryLevel>().Where(p => p.Type == categoryType).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取物料分类层级
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>物料分类层级</returns>
        public virtual EntityList<ItemCategoryLevel> GetItemCategoryLevels(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<ItemCategoryLevel>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 获取物料ID和编码的字典
        /// </summary>
        /// <param name="ids">物料id列表</param>
        /// <returns>物料ID和编码的字典</returns>
        public virtual Dictionary<double, string> GetItemIdCodeDic(List<double> ids)
        {
            return Query<Item>().Where(p => ids.Contains(p.Id)).ToList().GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.First().Code);
        }

        /// <summary>
        /// 获取分类层级下所有子节点
        /// </summary>
        /// <param name="querySql">查询条件动态语句</param>
        /// <returns>分类层级下所有子节点</returns>
        public virtual List<double> GetCategoryLevelAllNodeIds(string querySql)
        {
            var meta = RF.Find<ItemCategoryLevel>().EntityMeta;
            List<double> idList = new List<double>();
            var sqlPre = "with relation_child(id) as ";
            var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<ItemCategoryLevel>()).DbSetting;
            if (setting.IsPostgreSqlServer())
            {
                sqlPre = "with recursive relation_child as ";
            }
            var sql = sqlPre + string.Format(@"(select {2} from {0} where 1=1 {3}
                 UNION ALL (SELECT a.{2} from {0} a inner join relation_child b on a.{1} = b.{2})) SELECT * FROM relation_child",
                 meta.TableMeta.TableName, meta.Property(ItemCategoryLevel.TreePIdProperty).ColumnMeta.ColumnName, meta.Property(ItemCategoryLevel.IdProperty).ColumnMeta.ColumnName, querySql);

            using (var db = DbAccesserFactory.Create(ItemEntityDataProvider.ConnectionStringName))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        idList.Add(dr.GetDecimal(0).ConvertTo<double>());
                    }
                }
            }

            return idList;
        }

        /// <summary>
        /// 根据根节点获取分类层级下所有子节点
        /// </summary>
        /// <param name="roodId">根节点集合</param>
        /// <returns>分类层级下所有子节点</returns>
        public virtual List<double> GetCateLevelNodesByPtreeId(double roodId)
        {
            StringBuilder sb = new StringBuilder();
            var meta = RF.Find<ItemCategoryLevel>().EntityMeta;
            string invOrgId = meta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            string isphantom = meta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
            sb.Append(" and {0} = {1} and {2} = 0 ".FormatArgs(invOrgId, RT.InvOrg, isphantom));
            sb.Append(" and {0} = {1} ".FormatArgs(meta.Property(ItemCategoryLevel.IdProperty).ColumnMeta.ColumnName, roodId));
            return GetCategoryLevelAllNodeIds(sb.ToString());
        }

        /// <summary>
        /// 获取物料分类小类
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>物料分类小类</returns>
        public virtual EntityList<ItemSmallCategory> GetItemSmallCategories(PagingInfo pagingInfo, string keyword)
        {
            return Query<ItemSmallCategory>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取质量分类小类
        /// </summary>
        /// <param name="typeList">物料类型列表</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>质量分类小类列表</returns>
        public virtual EntityList<QualityCategory> GetQualitySmallCategories(List<int> typeList, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<QualityCategory>().Where(p => p.TreePId != null).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            query.NotExists<ItemCategory>((x, y) => y.Where(b => b.TreePId == x.Id));
            if (typeList != null)
                query.Where(p => typeList.Contains((int)p.ItemType));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取质量分类叶子结点
        /// </summary>
        /// <param name="typeList">物料类型列表</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>质量分类叶子结点列表</returns>
        public virtual EntityList<QualityCategory> GetLeafNodes(List<int> typeList, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<QualityCategory>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            query.NotExists<ItemCategory>((x, y) => y.Where(b => b.TreePId == x.Id));
            if (typeList != null)
                query.Where(p => typeList.Contains((int)p.ItemType));
            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取质量分类与物料关系
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="type">分类类型</param>
        /// <returns>物料与分类关系</returns>
        /// <exception cref="ValidationException">分类为空</exception>
        public virtual EntityList<ItemCategoryRelation> GetQualityCategory(List<double> itemIds, CategoryType? type = null)
        {
            var list = itemIds.SplitContains(ids =>
            {
                return Query<ItemCategoryRelation>().Where(p => ids.Contains(p.ItemId) && p.Type == type).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 获取质量分类与物料关系
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="type">分类类型</param>
        /// <returns>物料与分类关系</returns>
        /// <exception cref="ValidationException">分类为空</exception>
        public virtual ItemCategoryRelation GetQualityCategory(double itemId, CategoryType? type = null)
        {
            var query = Query<ItemCategoryRelation>().Where(p => p.ItemId == itemId);
            if (type != null)
                query.Where(p => p.Type == type);
            var relation = query.FirstOrDefault();
            if (relation != null && relation.ItemCategory == null)
                throw new ValidationException("物料[{0}]未维护分类类型[{1}]的分类".L10nFormat(relation.Item?.Code, EnumViewModel.EnumToLabel(relation.Type).L10N()));
            return relation;
        }

        /// <summary>
        /// 获取质量分类与物料关系
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns>物料与分类关系</returns>
        /// <exception cref="ValidationException">分类为空</exception>
        public virtual ItemCategoryRelation GetQualityCategoryRelation(double itemId)
        {
            var query = Query<ItemCategoryRelation>().Where(p => p.ItemId == itemId);
            var relation = query.FirstOrDefault();
            if (relation != null && relation.ItemCategory != null)
                return relation;
            return new ItemCategoryRelation();
        }
        /// <summary>
        /// 查询质量分类信息
        /// </summary>
        /// <param name="criteria">质量分类查询实体</param>
        /// <returns>质量分类列表</returns>
        /// <exception cref="ArgumentNullException">查询实体为空</exception>
        public virtual EntityList<QualityCategory> GetItemCategorys(QualityCategoryCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var query = DB.Query<QualityCategory>("i1");
            var meta = RF.Find<QualityCategory>().EntityMeta;
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));

            query.Where(f => f.SQL<bool>(new FormattedSql(@" 
                          i1.{2} is not null and
                          not exists(
                          select 1 from {0} i2                       
                              where i1.{1} = i2.{2}  and i2.IS_PHANTOM=0
                  )".FormatArgs(meta.TableMeta.TableName, meta.Property(QualityCategory.IdProperty).ColumnMeta.ColumnName, meta.Property(QualityCategory.TreePIdProperty).ColumnMeta.ColumnName))));

            return query.ToList(criteria.PagingInfo);
        }

        /// <summary>
        /// 获得所有质量分类 列表
        /// </summary>
        /// <returns>QualityCategory列表</returns>
        public virtual EntityList<QualityCategory> GetItemCategoryList()
        {
            var q = Query<QualityCategory>();
            return q.ToList();
        }

        /// <summary>
        /// 获取质量分类
        /// </summary>
        /// <param name="itemType">物料类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">搜索条件</param>
        /// <returns></returns>
        public virtual EntityList<QualityCategory> GetQualityCategorys(ItemType itemType, PagingInfo pagingInfo, string keyword)
        {
            return Query<QualityCategory>().Where(p => p.ItemType == itemType).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 查询物料分类最底层级
        /// </summary>
        /// <param name="criteria">物料分类查询实体</param>
        /// <returns>物料分类集合</returns>
        public virtual EntityList<ItemSmallCategory> GetItemSmallCategorys(ItemSmallCategoryCriteria criteria)
        {
            var meta = RF.Find<ItemSmallCategory>().EntityMeta;
            var query = DB.Query<ItemSmallCategory>("i1");
            query.Where(f => f.SQL<bool>(new FormattedSql(@"not exists(select 1 from {0} i2 where i1.{1} = i2.{2})".FormatArgs(meta.TableMeta.TableName, meta.Property(ItemCategory.IdProperty).ColumnMeta.ColumnName, meta.Property(ItemCategory.TreePIdProperty).ColumnMeta.ColumnName))));
            if (!criteria.Code.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (!criteria.Name.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.Type.HasValue)
                query.Where(p => p.Type == criteria.Type);
            if (criteria.LevelId.HasValue)
                query.Where(p => p.LevelId == criteria.LevelId);
            if (criteria.ItemType.HasValue)
                query.Where(p => p.ItemType == criteria.ItemType);
            return query.ToList(criteria.PagingInfo);
        }

        /// <summary>
        /// 根据编码集合获取物料分类
        /// </summary>
        /// <param name="codes">编码集合，已“;”分隔</param>
        /// <returns>物料分类</returns>
        public virtual EntityList<MultiItemCategory> GetMultiItemCategorys(string codes)
        {
            var query = Query<MultiItemCategory>();
            if (!codes.IsNullOrEmpty())
            {
                query.Where(p => codes.Split(';', StringSplitOptions.None).ToList().Contains(p.Code));
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取物料分类
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>物料分类</returns>
        public virtual EntityList<MultiItemCategory> GetMultiItemCategorys(MultiItemCategoryCriteria criteria)
        {
            var query = Query<MultiItemCategory>();
            if (!criteria.Code.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (!criteria.Name.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.Type.HasValue)
                query.Where(p => p.Type == criteria.Type);
            if (criteria.LevelId.HasValue)
                query.Where(p => p.LevelId == criteria.LevelId);
            if (criteria.ItemType.HasValue)
                query.Where(p => p.ItemType == criteria.ItemType);
            return query.ToList(criteria.PagingInfo);
        }

        /// <summary>
        /// 获取物料分类下所有子节点
        /// </summary>
        /// <param name="querySql">查询条件动态语句</param>
        /// <returns>物料分类下所有子节点</returns>
        public virtual List<double> GetItemCategoryAllNodeIds(string querySql)
        {
            var meta = RF.Find<ItemCategory>().EntityMeta;
            List<double> idList = new List<double>();
            var sqlPre = "with relation_child(id) as ";
            var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<ItemCategory>()).DbSetting;
            if (setting.IsPostgreSqlServer() || setting.IsMysqlDbServer()||setting.IsVastDataServer())
            {
                sqlPre = "with recursive relation_child as ";
            }
            var sql = sqlPre + string.Format(@"(select {2} from {0} where 1=1 {3}
                 UNION ALL (SELECT a.{2} from {0} a inner join relation_child b on a.{1} = b.{2})) SELECT * FROM relation_child",
                 meta.TableMeta.TableName, meta.Property(ItemCategory.TreePIdProperty).ColumnMeta.ColumnName, meta.Property(ItemCategory.IdProperty).ColumnMeta.ColumnName, querySql);

            using (var db = DbAccesserFactory.Create(ItemEntityDataProvider.ConnectionStringName))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        idList.Add(dr.GetDecimal(0).ConvertTo<double>());
                    }
                }
            }

            return idList;
        }

        /// <summary>
        /// 根据根节点获取物料分类下所有子节点
        /// </summary>
        /// <param name="roodCodeList">根节点编码集合</param>
        /// <returns>物料分类下所有子节点</returns>
        public virtual List<double> GetItemCateNodesByPtreeCode(List<string> roodCodeList)
        {
            StringBuilder sb = new StringBuilder();
            var meta = RF.Find<ItemCategory>().EntityMeta;
            string codeStr = "'" + string.Join("','", roodCodeList) + "'";
            string invOrgId = meta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            string isphantom = meta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
            sb.Append(" and {0} = {1} and {2} = 0 ".FormatArgs(invOrgId, RT.InvOrg, isphantom));
            sb.Append(" and {0} in ({1}) ".FormatArgs(meta.Property(ItemCategory.CodeProperty).ColumnMeta.ColumnName, codeStr));
            return GetItemCategoryAllNodeIds(sb.ToString());
        }

        /// <summary>
        /// 根据根节点获取物料分类下所有子节点
        /// </summary>
        /// <param name="roodId">根节点集合</param>
        /// <returns>物料分类下所有子节点</returns>
        public virtual List<double> GetItemCateNodesByPtreeCode(double roodId)
        {
            StringBuilder sb = new StringBuilder();
            var meta = RF.Find<ItemCategory>().EntityMeta;
            string invOrgId = meta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            string isphantom = meta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
            sb.Append(" and {0} = {1} and {2} = 0 ".FormatArgs(invOrgId, RT.InvOrg, isphantom));
            sb.Append(" and {0} = {1} ".FormatArgs(meta.Property(ItemCategory.IdProperty).ColumnMeta.ColumnName, roodId));
            return GetItemCategoryAllNodeIds(sb.ToString());
        }

        /// <summary>
        /// 获取物料对应分类不属于特定分类行数
        /// </summary>
        /// <param name="itemIdList">物料ID集合</param>
        /// <param name="itemCateId">物料分类ID</param>
        /// <returns>物料对应分类不属于特地分类行数</returns>
        public virtual int ValidateItemCateCount(List<double> itemIdList, double itemCateId)
        {
            //获取物料分类所有子节点
            var itemCadeChildIdList = GetItemCateNodesByPtreeCode(itemCateId);
            var query = Query<ItemCategoryRelation>();
            query.Where(p => p.Type == CategoryType.Item && itemIdList.Contains(p.ItemId) && !itemCadeChildIdList.Contains(p.ItemCategory.Id));
            return query.Count();
        }

        /// <summary>
        /// 获取所有质量小类
        /// </summary>
        /// <param name="info">分页信息</param>
        /// <returns>质量小类列表</returns>
        public virtual EntityList<QualityCategory> GetQualityCategoryByType(PagingInfo info = null)
        {
            return Query<QualityCategory>().ToList(info);
        }

        /// <summary>
        /// 根据物料Id获取分类集合
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>分类集合</returns>
        public virtual QualityCategory GetItemCategoryFromItemId(double itemId)
        {
            return Query<QualityCategory>().Join<ItemCategoryRelation>((x, y) => x.Id == y.ItemCategoryId && y.ItemId == itemId).FirstOrDefault();
        }
        #endregion

        #region 物料 
        /// <summary>
        /// 查询物料列表
        /// </summary>
        /// <param name="criteria">物料查询实体</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItems(ItemCriteria criteria)
        {
            var query = DB.Query<Item>("i");
            if (!criteria.Code.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (!criteria.Name.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (!criteria.SpecificationModel.IsNullOrEmpty())
                query.Where(p => p.SpecificationModel.Contains(criteria.SpecificationModel));
            if (criteria.Type.HasValue)
                query.Where(p => p.Type == criteria.Type);
            if (criteria.SelType.HasValue)
                query.Where(p => p.Type != criteria.SelType);//排除类型
            if (criteria.ItemSourceType.HasValue)
                query.Where(p => p.ItemSourceType == criteria.ItemSourceType);
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
            if (criteria.SourceType.HasValue)
                query.Where(p => p.SourceType == criteria.SourceType);
            if (criteria.UpdateDate.BeginValue.HasValue)
                query.Where(p => p.UpdateDate >= criteria.UpdateDate.BeginValue);
            if (criteria.UpdateDate.EndValue.HasValue)
                query.Where(p => p.UpdateDate <= criteria.UpdateDate.EndValue);
            if (!criteria.UpdateBy.IsNullOrEmpty())
                query.Join<Employee>((x, y) => x.UpdateBy == y.Id && y.Name.Contains(criteria.UpdateBy));
            if (criteria.PurchasingAgentId.HasValue)
                query.Where(p => p.PurchasingAgentId == criteria.PurchasingAgentId);
            if (criteria.CategoryType.HasValue || criteria.ItemCategoryId.HasValue || !criteria.ItemCategorys.IsNullOrEmpty())
            {
                query.Exists<ItemCategoryRelation>((x, y) => y.Where(f => f.ItemId == x.Id)
                .WhereIf(criteria.CategoryType.HasValue, f => f.Type == criteria.CategoryType && f.ItemCategoryId > 0)
                .WhereIf(criteria.ItemCategoryId.HasValue, f => f.ItemCategoryId == criteria.ItemCategoryId)
                .WhereIf(!criteria.ItemCategorys.IsNullOrEmpty(), f => f.Type == CategoryType.Item
                && GetItemCateNodesByPtreeCode(criteria.ItemCategorys.Split(';', StringSplitOptions.None).ToList()).Contains(f.ItemCategory.Id)));
            }
            if (criteria.NotItemIds.IsNotEmpty())
            {
                query.Where(p => p.SQL<bool>(new Data.FormattedSql(@"i.Id not in ({0})".FormatArgs(criteria.NotItemIds))));
            }
            if (!criteria.ShortDescription.IsNullOrEmpty())
                query.Where(p => p.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.Bismt.IsNullOrEmpty())
                query.Exists<ParentItem>((x, y) => y.Where(p => p.ItemId == x.Id && p.Bismt.Contains(criteria.Bismt)));

            return query.OrderBy(criteria.OrderInfoList)
                .ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWith(Item.UnitProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 查询物料列表
        /// </summary>
        /// <param name="criteria">物料查询实体</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItems(MultiQueryItemCriteria criteria)
        {
            var query = Query<Item>();
            query.Where(p => p.State == State.Enable);
            if (!criteria.Code.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (!criteria.Name.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (!criteria.SpecificationModel.IsNullOrEmpty())
                query.Where(p => p.SpecificationModel.Contains(criteria.SpecificationModel));
            if (criteria.Type.HasValue)
                query.Where(p => p.Type == criteria.Type);
            if (criteria.ItemSourceType.HasValue)
                query.Where(p => p.ItemSourceType == criteria.ItemSourceType);
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);
            if (criteria.SourceType.HasValue)
                query.Where(p => p.SourceType == criteria.SourceType);
            if (!criteria.ItemCategorys.IsNullOrEmpty())
            {
                var list = criteria.ItemCategorys.Split(';').ToList();
                var idList = GetItemCateNodesByPtreeCode(list);
                query.Exists<ItemCategoryRelation>((x, y) => y.Where(f => f.ItemId == x.Id && f.Type == CategoryType.Item && idList.Contains(f.ItemCategory.Id)));
            }
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(Item.UnitProperty);
            elo.LoadWith(Item.BaseCodeProperty);
            elo.LoadWith(Item.ModelProperty);
            elo.LoadWith(Item.PurchasingGroupProperty);
            elo.LoadWithViewProperty();
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="codeList">编码集合</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItems(List<string> codeList, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Item>().Where(p => codeList.Contains(p.Code));
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="code">物料编码</param>
        /// <param name="name">物料名称</param>
        /// <param name="type">类型</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItems(string code, string name, ItemType? type, PagingInfo pagingInfo)
        {
            var query = Query<Item>();
            if (!code.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(code));
            }
            if (!name.IsNullOrEmpty())
            {
                query.Where(p => p.Name.Contains(name));
            }
            if (type.HasValue)
            {
                query.Where(p => p.Type == type);
            }

            return query.ToList(paging: pagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询物料
        /// </summary>
        /// <param name="criteria">替代料查询实体</param>
        /// <param name="filterId">待过滤物料ID</param>
        /// <returns>Item列表</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public virtual EntityList<Item> GetItems(AlternativeCriteria criteria, double[] filterId = null)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
            var query = DB.Query<Item>("i");
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(e => e.Code.Contains(criteria.Code));
            }

            if (criteria.Name.IsNotEmpty())
            {
                query.Where(e => e.Name.Contains(criteria.Name));
            }

            if (criteria.Description.IsNotEmpty())
            {
                query.Where(e => e.Description.Contains(criteria.Description));
            }

            if (criteria.Type.HasValue)
            {
                query.Where(e => e.Type == criteria.Type);
            }

            if (criteria.State != null)
            {
                query.Where(e => e.State == criteria.State);
            }

            if (criteria.DrawingNo.IsNotEmpty())
            {
                query.Where(e => e.DrawingNo.Contains(criteria.DrawingNo));
            }

            if (criteria.Model.IsNotEmpty())
            {
                query.Where(e => e.Model.Code.Contains(criteria.Model));
            }

            if (criteria.Person.IsNotEmpty())
            {
                query.Where(e => e.Person == criteria.Person);
            }

            if (criteria.DataSource.HasValue)
            {
                query.Where(e => e.SourceType == criteria.DataSource);
            }

            if (criteria.ItemLabelType.HasValue)
            {
                query.Where(e => e.ItemLabelType == criteria.ItemLabelType);
            }

            if (filterId != null && filterId.Any())
            {
                var meta = RF.Find<Item>().EntityMeta;
                StringBuilder ids = new StringBuilder();
                for (int i = 0; i < filterId.Count(); i++)
                {
                    if (i == filterId.Count() - 1)
                        ids.Append(filterId[i]);
                    else
                        ids.Append(filterId[i] + ",");
                }

                query.Where(p => p.SQL<bool>(new FormattedSql(@"i.{0} not in ({1})".FormatArgs(meta.Property(Item.IdProperty).ColumnMeta.ColumnName, ids.ToString()))));
            }

            return query.ToList(criteria.PagingInfo/*, el*/);
        }

        /// <summary>
        /// 物料保存前事件
        /// </summary>
        /// <param name="items"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<Item> OnSavingItems(EntityList<Item> items)
        {
            //是否初始化
            bool isInitialized = RT.Service.Resolve<UnitsController>().CheckInitUnitList();
            var databaseItems = RT.Service.Resolve<ItemController>().GetItemUnits(items.Select(p => p.Id).ToList());
            var databaseItemUnits = RT.Service.Resolve<ItemUnitController>().GetItemUnits();
            foreach (var item in items)
            {
                if (item.UnitList.Count > 0)
                {
                    if (!isInitialized)
                    {
                        // throw new ValidationException("单位未初始化，物料".L10N() + "[" + item.Code + " ]" + "无法添加转换单位！".L10N());
                        throw new ValidationException("单位未初始化，物料[{0}]无法添加转换单位！".L10nFormat(item.Code));
                    }
                }
                var itemTem = databaseItems.FirstOrDefault(i => i.ItemId == item.Id);
                if (itemTem != null && item.UnitId != itemTem.UnitId && item.UnitList.Count > 0)
                {
                    // throw new ValidationException("物料".L10N() + "[" + item.Code + "]" + "已存在转换单位数据，不允许修改基本计量单位！".L10N());
                    throw new ValidationException("物料[{0}]已存在转换单位数据，不允许修改基本计量单位！".L10nFormat(item.Code));

                }
                foreach (var itemUnit in item.UnitList)
                {
                    //var isData = RT.Service.Resolve<ItemUnitController>().GetItemUnits(itemUnit);

                    //判断插入的主单位和辅助单位是否已经在数据库里存在
                    var isData = true;
                    var itemUnits = databaseItemUnits.Where(p => p.MainUnitId == itemUnit.MainUnitId && p.UnitId == itemUnit.UnitId && p.IsBaseUnit).ToList();
                    if (!itemUnits.Any())
                    {
                        var itemUnitList = databaseItemUnits.Where(p => p.MainUnitId == itemUnit.MainUnitId && p.UnitId == itemUnit.UnitId && !p.IsBaseUnit && p.ItemId == itemUnit.ItemId && p.Id != itemUnit.Id).ToList();
                        if (!itemUnitList.Any())
                        {
                            isData = false;
                        }
                    }


                    if (isData)
                    {
                        //throw new ValidationException("物料".L10N() + "[" + item.Code + "]" + "-[转换单位页签]-不可插入数据!".L10N() + "\n" + "已存在相同的主单位和辅助单位数据".L10N());
                        throw new ValidationException("物料[{0}]-[转换单位页签]-不可插入数据!".L10nFormat(item.Code) + "\n" + "已存在相同的主单位和辅助单位数据".L10N());

                    }
                    if (itemUnit.Unit == null)
                    {
                        //throw new ValidationException("物料".L10N() + "[" + item.Code + " ]" + " -[转换单位页签]- [辅助单位] 不能为空".L10N());
                        throw new ValidationException("物料[{0}] -[转换单位页签]- [辅助单位] 不能为空".L10nFormat(item.Code));

                    }
                    if (itemUnit.Denominator < 1)
                    {
                        //throw new ValidationException("物料".L10N() + "[" + item.Code + " ]" + " -[转换单位页签]- [分母]最小值1，当前值".L10N() + itemUnit.Denominator + "!");
                        throw new ValidationException("物料[{0}] -[转换单位页签]- [分母]最小值1，当前值{1}!".L10nFormat(item.Code, itemUnit.Denominator));

                    }
                    if (itemUnit.Numerator < 1)
                    {
                        throw new ValidationException("物料[{0}] -[转换单位页签]- [分子]最小值1，当前值{1}!".L10nFormat(item.Code, itemUnit.Numerator));

                        //throw new ValidationException("物料".L10N() + "[" + item.Code + " ]" + " -[转换单位页签]- [分子]最小值1，当前值".L10N() + itemUnit.Denominator + "!");
                    }
                }
                foreach (var itemUnit in item.UnitList.DeletedList)
                {
                    var delItemUnit = itemUnit as ItemUnit;
                    if (delItemUnit.IsDefault)
                    {
                        item.SecondUnitId = null;
                    }
                }
            }
            return items;
        }
        /// <summary>
        /// 查询物料
        /// </summary>
        /// <param name="itemType">物料类型</param>
        /// <param name="pagingInfo">pagingInfo</param>
        /// <param name="keyword">keyword</param>
        /// <param name="filterId">待过滤物料ID</param>
        /// <param name="state">物料状态</param>
        /// <returns>物料列表</returns>
        /// /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public virtual EntityList<Item> GetItems(ItemType itemType, PagingInfo pagingInfo, string keyword, double[] filterId = null, State? state = State.Enable)
        {
            var query = DB.Query<Item>("i");

            query.Where(e => e.Type == itemType);

            if (filterId != null && filterId.Any())
            {
                var meta = RF.Find<Item>().EntityMeta;
                StringBuilder ids = new StringBuilder();
                for (int i = 0; i < filterId.Count(); i++)
                {
                    if (i == filterId.Count() - 1)
                        ids.Append(filterId[i]);
                    else
                        ids.Append(filterId[i] + ",");
                }

                query.Where(p => p.SQL<bool>(new FormattedSql(@"i.{0} not in ({1})".FormatArgs(meta.Property(Item.IdProperty).ColumnMeta.ColumnName, ids.ToString()))));
            }

            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (state != null)
                query.Where(p => p.State == state);
            return query.ToList(pagingInfo/*, el*/);
        }

        /// <summary>
        /// 获取所有物料
        /// </summary>
        /// <returns>物料集合</returns>
        public virtual EntityList<Item> GetItems()
        {
            return Query<Item>().ToList(new PagingInfo() { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 获取物料单位
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual List<ItemAndUnit> GetItemUnits(List<double> ids)
        {
            List<ItemAndUnit> itemAndUnit = new List<ItemAndUnit>();
            ids.SplitDataExecute(tempIds =>
            {
                var list = Query<Item>().Where(p => tempIds.Contains(p.Id)).Select(p => new
                {
                    ItemId = p.Id,
                    UnitId = p.UnitId,
                }).ToList<ItemAndUnit>();
                itemAndUnit.AddRange(list);
            });
            return itemAndUnit;
        }

        /// <summary>
        /// 获取成品、半成品物料
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetProductItems(string keyword, PagingInfo pagingInfo)
        {
            List<ItemType> itemTypes = new List<ItemType> { ItemType.Product, ItemType.SemiFinished };
            return Query<Item>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .Where(p => itemTypes.Contains(p.Type))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过物料名称或编码查询物料
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemList(string keyword, PagingInfo pagingInfo)
        {
            return Query<Item>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过物料名称或编码查询物料
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetEnableItemList(string keyword, PagingInfo pagingInfo)
        {
            return Query<Item>()
                .Where(p => p.State == State.Enable)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料编码
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual List<ItemAndCode> GetItemCodes(List<double> itemIds)
        {
            List<ItemAndCode> itemAndCodes = new List<ItemAndCode>();
            itemIds.SplitDataExecute(tempIds =>
            {
                var list = Query<Item>().Where(p => tempIds.Contains(p.Id)).Select(p => new
                {
                    ItemId = p.Id,
                    ItemCode = p.Code,
                }).ToList<ItemAndCode>();
                itemAndCodes.AddRange(list);
            });
            return itemAndCodes;
        }

        /// <summary>
        /// 获取物料集合
        /// </summary>
        /// <param name="sourceType">物料来源类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>物料集合</returns>
        public virtual EntityList<Item> GetItems(ItemSourceType sourceType, string keyword = null, PagingInfo pagingInfo = null)
        {
            var query = Query<Item>().Where(p => p.ItemSourceType == sourceType && p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取成品与半成品物料 
        /// </summary>
        /// <param name="itemTypes">查询的物料类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>物料集合</returns>
        public virtual EntityList<Item> GetItems(List<int> itemTypes, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Item>();

            query.Where(p => itemTypes.Contains((int)p.Type));

            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料集合
        /// </summary>
        /// <param name="itemType">排除物料类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <param name="modelId">产品机型</param>
        /// <returns>物料集合</returns>
        public virtual EntityList<Item> GetItems(ItemType? itemType, PagingInfo pagingInfo, string keyword, double modelId)
        {
            var query = Query<Item>();
            if (itemType != null)
                query.Where(p => p.Type != itemType);
            return query.Where(p => p.ModelId == modelId).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="codes">编码</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>物料</returns>
        public virtual EntityList<Item> GetItems(List<string> codes, EagerLoadOptions elo = null)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<Item>().Where(p => pCodes.Contains(p.Code)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 下拉选择框数据查询
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="pageinfo">pageinfo</param>
        /// <returns>Item列表</returns>
        public virtual EntityList<Item> GetItems(string keyword, PagingInfo pageinfo = null)
        {
            var query = Query<Item>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)
            || p.Description.Contains(keyword)
            || p.DrawingNo.Contains(keyword) || p.Version.Contains(keyword) || p.Model.Name.Contains(keyword) || p.BaseModel.Contains(keyword)
            || p.Unit.Name.Contains(keyword) || p.PurchasingAgent.Name.Contains(keyword) || p.Person.Contains(keyword) || p.MrpPerson.Contains(keyword));
            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 物料下拉框数据查询（排除例外物料）
        /// </summary>
        /// <param name="excludeItemId">例外物料Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pageinfo">分页信息</param>
        /// <returns>物料数据</returns>
        public virtual EntityList<Item> GetItems(double excludeItemId, string keyword, PagingInfo pageinfo = null)
        {
            var query = Query<Item>();
            if (excludeItemId > 0)
                query.Where(p => p.Id != excludeItemId);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 下拉选择框数据查询
        /// 查询指定的库存组织下可用的物料
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageinfo"></param>
        /// <param name="invOrgId"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetInvOrgItems(string keyword, PagingInfo pageinfo = null, double? invOrgId = null)
        {
            if (invOrgId == null)
                invOrgId = RT.InvOrg;
            //查询指定的库存组织
            using (InvOrgs.With(new List<double> { double.Parse(invOrgId.ToString()) }))
            {
                var query = Query<Item>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)
            || p.Description.Contains(keyword)
            || p.DrawingNo.Contains(keyword) || p.Version.Contains(keyword) || p.Model.Name.Contains(keyword) || p.BaseModel.Contains(keyword)
            || p.Unit.Name.Contains(keyword) || p.PurchasingAgent.Name.Contains(keyword) || p.Person.Contains(keyword) || p.MrpPerson.Contains(keyword));
                query.Where(p => p.State == State.Enable);
                return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 获取物料（提升性能只获取Id和Code字段）
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>物料</returns>
        public virtual EntityList<Item> GetItemsIdByCode(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<Item>().Select(p => new { p.Id, p.Code }).Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 导入根据物料编码获取物料Id key:物料编码 value:物料Id
        /// </summary>
        /// <param name="codes">物料编码</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetItemIdByCodes(IEnumerable<string> codes)
        {
            List<BaseDataInfo> items = new List<BaseDataInfo>();
            codes.SplitDataExecute(temps =>
            {
                var list = Query<Item>().Where(p => temps.Contains(p.Code)).Select(p => new
                {
                    Code = p.Code,
                    Id = p.Id,
                }).ToList<BaseDataInfo>();
                items.AddRange(list);
            });
            return items.ToDictionary(p => p.Code, p => p.Id);
        }

        /// <summary>
        /// 根据物料编号或者物料名称获取物料信息
        /// </summary>
        /// <param name="codeOrNameList">物料编号或者物料名称集合</param>
        /// <returns>返回物料信息</returns>
        public virtual EntityList<Item> GetItemsByCodeOrName(List<string> codeOrNameList)
        {
            return codeOrNameList.SplitContains(tmps =>
            {
                return Query<Item>().Where(p => tmps.Contains(p.Code) || tmps.Contains(p.Name)).ToList();
            });
        }

        /// <summary>
        /// 获取对应条件的物料 
        /// </summary>
        /// <param name="itemTypes">查询的物料类型</param>
        /// <param name="consumeModes">查询的物料消耗类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>物料集合</returns>
        public virtual EntityList<Item> GetItemsByTypeAndMode(List<int> itemTypes, List<int> consumeModes, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Item>();
            query.Where(p => itemTypes.Contains((int)p.Type) && consumeModes.Contains((int)p.ConsumeMode));
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemListNoViewProperty(List<double> itemIds)
        {
            EntityList<Item> Items = new EntityList<Item>();
            DataProcessEx.SplitDataExecute(itemIds, sons =>
            {
                var itemList = Query<Item>().Where(p => sons.Contains(p.Id)).ToList();
                Items.AddRange(itemList);
            });

            return Items;
        }

        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>物料列表</returns>
        public virtual IList<SimpleItemData> GetSimpleItemList(List<double> itemIds)
        {

            return DataProcessEx.SplitContains(itemIds, sons =>
            {
                var query = Query<Item>().LeftJoin<Unit>((x, y) => x.UnitId == y.Id);
                query.Select<Unit>((i, u) => new
                {
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                    UnitName = u.Name,
                    SpecificationModel = i.SpecificationModel
                });
                query.Where(p => sons.Contains(p.Id));
                return query.ToList<SimpleItemData>();
            });
        }

        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemIds">物料ID列表</param>        
        /// <returns>物料列表</returns>
        public virtual IList<SimpleItemData> GetNewSimpleItemList(List<double> itemIds)
        {

            return DataProcessEx.SplitContains(itemIds, sons =>
            {
                var query = Query<Item>();
                query.Select((i) => new
                {
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                });
                query.Where(p => sons.Contains(p.Id));
                return query.ToList<SimpleItemData>();
            });
        }

        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemCodes">物料ID列表</param>        
        /// <returns>物料列表</returns>
        public virtual IList<SimpleItemData> GetNewSimpleItemList(List<string> itemCodes)
        {

            return DataProcessEx.SplitContains(itemCodes, sons =>
            {
                var query = Query<Item>();
                query.Select((i) => new
                {
                    ItemId = i.Id,
                    ItemCode = i.Code,
                    ItemName = i.Name,
                });
                query.Where(p => sons.Contains(p.Code));
                return query.ToList<SimpleItemData>();
            });
        }
        /// <summary>
        /// 根据编码集合获取物料
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemListByCodeNoViewProperty(List<string> codes)
        {
            EntityList<Item> Items = new EntityList<Item>();
            DataProcessEx.SplitDataExecute(codes, sons =>
            {
                var itemList = Query<Item>().Where(p => sons.Contains(p.Code)).ToList();
                Items.AddRange(itemList);
            });

            return Items;
        }


        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemIds">物料ID列表</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemList(List<double> itemIds, EagerLoadOptions elo = null)
        {
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            EntityList<Item> Items = new EntityList<Item>();
            DataProcessEx.SplitDataExecute(itemIds, sons =>
            {
                var customersList = Query<Item>().Where(p => sons.Contains(p.Id)).ToList(null, elo);
                Items.AddRange(customersList);
            });
            return Items;
        }

        /// <summary>
        /// 查找与质量分类关联的物料，查找与供应商关联的物料，取二者的交集
        /// </summary>
        /// <param name="pagingInfo">pagingInfo</param>
        /// <param name="itemIds">与质量分类关联的物料集合</param>
        /// <param name="filterId">与供应商关联的物料集合</param>
        /// <param name="state">启用状态</param>
        /// <param name="keyword">关键字，编码或者名称，支持模糊查询</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemList(PagingInfo pagingInfo, double[] itemIds, double[] filterId = null, State? state = State.Enable, string keyword = null)
        {
            if (!itemIds.Any()) return new EntityList<Item>();
            var query = DB.Query<Item>("i");

            if (itemIds.IsNotEmpty())
            {
                var exp = itemIds.ToList<double>().CreateContainsExpression<Item>("x", nameof(Item.Id));
                query.Where(exp);
            }

            if (filterId != null && filterId.Any())
            {
                var exp = filterId.ToList<double>().CreateContainsExpression<Item>("x", nameof(Item.Id));
                query.Where(exp);
            }
            query.WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            if (state != null)
                query.Where(p => p.State == state);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过产品Id获取产品下所有的物料列表
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemList(double productId, string keyword, PagingInfo pagingInfo)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ProductBom.DetailListProperty);
            elo.LoadWith(ProductBomDetail.ItemProperty);
            elo.LoadWithViewProperty();

            EntityList<Item> itemList = new EntityList<Item>();
            var bom = RT.Service.Resolve<ItemController>().GetDefaultProductBom(productId, elo);
            if (bom != null)
            {
                IEnumerable<Item> detailItems = bom.DetailList.Select(p => p.Item).Where(p => p.Type != ItemType.Material && p.ItemSourceType == ItemSourceType.SelfMade);
                itemList.AddRange(detailItems);

                // 获取产品BOM关系
                List<double> itemIds = itemList.Select(p => p.Id).Distinct().ToList();
                List<ItemSourceType> typeList = new List<ItemSourceType>() { ItemSourceType.SelfMade };
                List<ProductBomRelationViewModel> bomRelations = RT.Service.Resolve<ProductBoms.ProductBomRelationController>().GetChildProductBomList(itemIds, typeList);

                foreach (var detail in bomRelations)
                {
                    if (detail.ChildItemId > 0 && detail.ChildItemSourceType == ItemSourceType.SelfMade && !itemIds.Contains(detail.ChildItemId.Value))
                    {
                        itemIds.Add(detail.ChildItemId.Value);
                    }
                }

                itemList = GetItemDataList(itemIds, pagingInfo, keyword);
            }

            return itemList;
        }


        /// <summary>
        /// 通过产品Id获取产品下所有的物料列表
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemBomList(double productId, string keyword, PagingInfo pagingInfo)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ProductBom.DetailListProperty);
            elo.LoadWith(ProductBomDetail.ItemProperty);
            elo.LoadWithViewProperty();

            EntityList<Item> itemList = new EntityList<Item>();
            var bom = RT.Service.Resolve<ItemController>().GetDefaultProductBom(productId, elo);
            if (bom != null)
            {
                IEnumerable<Item> detailItems = bom.DetailList.Select(p => p.Item).ToList();
                itemList.AddRange(detailItems);

                // 获取产品BOM关系
                List<double> itemIds = itemList.Select(p => p.Id).Distinct().ToList();
                List<ItemSourceType> typeList = new List<ItemSourceType>() { ItemSourceType.SelfMade };
                List<ProductBomRelationViewModel> bomRelations = RT.Service.Resolve<ProductBoms.ProductBomRelationController>().GetChildProductBomList(itemIds, typeList);

                foreach (var detail in bomRelations)
                {
                    if (detail.ChildItemId > 0 && detail.ChildItemSourceType == ItemSourceType.SelfMade && !itemIds.Contains(detail.ChildItemId.Value))
                    {
                        itemIds.Add(detail.ChildItemId.Value);
                    }
                }

                itemList = GetItemDataList(itemIds, pagingInfo, keyword);
            }

            return itemList;
        }


        /// <summary>
        /// 通过产品Id获取产品下所有的物料列表（包含产品）
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemBomAndProductIdList(double productId, string keyword, PagingInfo pagingInfo)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(ProductBom.DetailListProperty);
            elo.LoadWith(ProductBomDetail.ItemProperty);
            elo.LoadWithViewProperty();

            EntityList<Item> itemList = new EntityList<Item>();
            var bom = RT.Service.Resolve<ItemController>().GetDefaultProductBom(productId, elo);
            if (bom != null)
            {
                IEnumerable<Item> detailItems = bom.DetailList.Select(p => p.Item).ToList();
                itemList.AddRange(detailItems);

                // 获取产品BOM关系
                List<double> itemIds = itemList.Select(p => p.Id).Distinct().ToList();
                List<ItemSourceType> typeList = new List<ItemSourceType>() { ItemSourceType.SelfMade };
                List<ProductBomRelationViewModel> bomRelations = RT.Service.Resolve<ProductBoms.ProductBomRelationController>().GetChildProductBomList(itemIds, typeList);

                foreach (var detail in bomRelations)
                {
                    if (detail.ChildItemId > 0 && detail.ChildItemSourceType == ItemSourceType.SelfMade && !itemIds.Contains(detail.ChildItemId.Value))
                    {
                        itemIds.Add(detail.ChildItemId.Value);
                    }
                }
                //包含当前产品
                itemIds.Add(productId);
                itemList = GetItemDataList(itemIds, pagingInfo, keyword);
            }

            return itemList;
        }

        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemIds">物料ID列表</param>        
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="elo">婪加载</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemDataListNoEager(List<double> itemIds, PagingInfo pagingInfo = null, EagerLoadOptions elo = null)
        {
            if (itemIds == null || itemIds.Count == 0)
            {
                return new EntityList<Item>();
            }
            EntityList<Item> Items = new EntityList<Item>();
            DataProcessEx.SplitDataExecute(itemIds, sons =>
            {
                EntityList<Item> ItemsList;
                ItemsList = Query<Item>().Where(p => sons.Contains(p.Id)).ToList(pagingInfo, elo);

                Items.AddRange(ItemsList);
            });
            return Items;
        }

        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemIds">物料ID列表</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemDataList(List<double> itemIds, PagingInfo pagingInfo = null, string keyword = null)
        {
            if (itemIds == null || itemIds.Count == 0)
            {
                return new EntityList<Item>();
            }
            EntityList<Item> Items = new EntityList<Item>();
            DataProcessEx.SplitDataExecute(itemIds, sons =>
            {
                EntityList<Item> ItemsList;
                if (!keyword.IsNullOrEmpty())
                {
                    ItemsList = Query<Item>().Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).Where(i => sons.Contains(i.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                }
                else
                {
                    ItemsList = Query<Item>().Where(p => sons.Contains(p.Id)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                }
                Items.AddRange(ItemsList);
            });
            return Items;
        }

        /// <summary>
        /// 获取物料数据
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>我要看</returns>
        public virtual EntityList<Item> GetItemDataList(List<string> codes, EagerLoadOptions elo = null)
        {
            return codes.SplitContains(p =>
             {
                 return Query<Item>().Where(a => p.Contains(a.Code)).ToList(null, elo);
             });
        }

        /// <summary>
        /// 根据物料Id列表 获取物料列表
        /// </summary> 
        /// <param name="itemIds">物料ID列表</param>
        /// <returns>物料列表</returns>
        public virtual Dictionary<double, int> GetItemPurchaseLeadTimeDataList(List<double> itemIds)
        {
            List<IdIntObject> dataList = DataProcessEx.SplitContains(itemIds, (tmpIds) =>
            {
                return Query<Item>().Where(p => tmpIds.Contains(p.Id))
                .Select(p => new { Id = p.Id, IntObj = p.PurchaseLeadTime }).ToList<IdIntObject>().ToList();
            });

            return dataList.Where(x => x.IntObj.HasValue).ToDictionary(p => p.Id, p => p.IntObj ?? 0);
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemDatas(PagingInfo pagingInfo, string keyword)
        {
            var q = Query<Item>().Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
            {
                q.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过编码获取来源类型是自制或者外协的物料
        /// </summary>
        /// <param name="code">物料编码</param>
        /// <returns>物料</returns>
        public virtual Item GetItemListByItemSourceType(string code)
        {
            return Query<Item>().Where(p => p.Code == code && (p.ItemSourceType == ItemSourceType.OutMade || p.ItemSourceType == ItemSourceType.SelfMade)).FirstOrDefault();
        }

        /// <summary>
        /// 校验是否是物料
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual bool CheckIsItem(string code)
        {
            return Query<Item>().Where(p => code.Contains(p.Code) || code.Contains(p.Name)).Count() > 0;
        }
        /// <summary>
        /// 根据产品编码获取产品
        /// </summary>
        /// <param name="code">产品编码</param>
        /// <returns>返回产品</returns>
        public virtual Item GetItemFromCode(string code)
        {
            var itemEntity = Query<Item>().Where(p => p.Code == code).FirstOrDefault();

            return itemEntity;
        }

        /// <summary>
        /// 根据产品名称获取产品
        /// </summary>
        /// <param name="name">产品名称</param>
        /// <returns>返回产品</returns>
        public virtual Item GetItemFromName(string name)
        {
            var itemEntity = Query<Item>().Where(p => p.Name == name).FirstOrDefault();

            return itemEntity;
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>物料</returns>
        public virtual Item GetItem(double itemId)
        {
            return Query<Item>().Where(p => p.Id == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 查找物料
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>物料</returns> 
        public virtual Item GetItem(string code)
        {
            return Query<Item>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料id返回类型
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual ItemType GetType(double itemId)
        {
            return Query<Item>().Where(p => p.Id == itemId).Select(p => p.Type).ToList<ItemType>().FirstOrDefault();
        }

        /// <summary>
        /// 获取新的物料编码
        /// </summary>
        /// <returns>物料编码</returns>
        public virtual string GetItemCode()
        {
            var config = ConfigService.GetConfig(new ItemCodeNoConfig(), typeof(Item));
            if (config == null || config.ItemCodeRule == null)
                return ""; //增加逻辑：当配置项【物料编码生成规则】未配置时，可手工输入物料编码
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.ItemCodeRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取可用的物料列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>        
        /// <returns>可用的物料列表</returns>
        public virtual EntityList<Item> GetEnableItems(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Item>();
            query.Where(p => p.State == State.Enable)
                .Where(p => p.ItemSourceType == ItemSourceType.SelfMade || p.ItemSourceType == ItemSourceType.OutMade)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取所有可用物料列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>   
        /// <param name="itemIds">物料Id</param>
        /// <returns>可用的物料列表</returns>
        public virtual EntityList<Item> GetAllEnableItems(PagingInfo pagingInfo, string keyword, List<double> itemIds = null)
        {
            var query = Query<Item>();
            query.Where(p => p.State == State.Enable);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (itemIds != null && itemIds.Count > 0)
                query.Where(p => itemIds.Contains(p.Id));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有可用物料列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>   
        /// <param name="unitName">单位名称</param>
        /// <returns>可用的物料列表</returns>
        public virtual EntityList<Item> GetAllEnableItems(PagingInfo pagingInfo, string keyword, string unitName)
        {
            var query = Query<Item>();
            query.Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (!unitName.IsNullOrEmpty())
            {
                query.Join<Unit>((x, y) => x.UnitId == y.Id && y.Name == unitName);
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据类型获取所有可用物料列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>   
        /// <param name="consumeMode">类型</param>
        /// <returns>可用的物料列表</returns>
        public virtual EntityList<Item> GetAllEnableItems(PagingInfo pagingInfo, string keyword, ConsumeMode consumeMode)
        {
            var query = Query<Item>();
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.Where(p => p.State == State.Enable && p.ConsumeMode == consumeMode).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取对应检验类型可用物料列表
        /// </summary>
        /// <param name="typeList">类型集合</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>   
        /// <param name="itemIds">物料Id</param>
        /// <returns>可用的物料列表</returns>
        public virtual EntityList<Item> GetCorrespondEnableItems(List<int> typeList, PagingInfo pagingInfo, string keyword, List<double> itemIds = null)
        {
            var query = Query<Item>();
            query.Where(p => p.State == State.Enable);
            if (typeList != null)
            {
                query.Where(p => typeList.Contains((int)p.Type));
            }
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (itemIds != null && itemIds.Count > 0)
                query.Where(p => itemIds.Contains(p.Id));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有成品物料
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetProductItems(PagingInfo pagingInfo, string keyword, List<double> itemIds = null)
        {
            var query = Query<Item>();
            query.Where(p => p.State == State.Enable && p.Type == ItemType.Product);
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (itemIds != null && itemIds.Count > 0)
                query.Where(p => itemIds.Contains(p.Id));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品集合
        /// </summary>
        /// <param name="itemType">物料类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param> 
        /// <returns>产品集合</returns>
        public virtual EntityList<Item> GetProducts(ItemType itemType, PagingInfo pagingInfo, string keyword)
        {
            return Query<Item>().Where(p => p.Type == itemType).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取可用的自制件物料列表
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>        
        /// <returns>可用的物料列表</returns>
        public virtual EntityList<Item> GetEnableItemList(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Item>();
            query.Where(p => p.State == State.Enable)
                .Where(p => p.Type == ItemType.Product || p.Type == ItemType.SemiFinished)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料分类
        /// </summary>
        /// <param name="categoryList">物料小类</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回物料信息</returns>
        public virtual EntityList<Item> GetItemByCategorys(List<string> categoryList, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Item>();
            query.Exists<ItemCategoryRelation>((x, y) => y.Where(f => f.ItemId == x.Id && f.Type == CategoryType.Item && categoryList.Contains(f.ItemCategory.Code)));
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料库存分类查询物料
        /// </summary>
        /// <param name="itemCategoryId">物料小类ID</param>
        /// <param name="pagingInfo">分页对象</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回物料信息</returns>
        public virtual EntityList<Item> GetItemByItemCategoryId(double itemCategoryId, string keyword, PagingInfo pagingInfo = null)
        {
            var query = Query<Item>()
                .Where(x => x.State == State.Enable)
                .Exists<ItemCategoryRelation>((x, y) => y.Where(f => f.ItemId == x.Id
                    && f.Type == CategoryType.Item && f.ItemCategoryId == itemCategoryId));

            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料ID中启用了扩展属性的ID
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <returns>启用了扩展属性的ID</returns>
        public virtual HashSet<double> GetItemIdsEnableExtProp(List<double> itemIds)
        {
            HashSet<double> results = new HashSet<double>();
            itemIds.SplitDataExecute(ids =>
            {
                var tmpIds = Query<Item>().Where(p => ids.Contains(p.Id) && p.EnableExtendProperty).Select(p => p.Id).ToList<double>().ToList();
                foreach (var item in tmpIds)
                {
                    results.Add(item);
                }
            });
            return results;
        }

        /// <summary>
        /// 获取非批次物料
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemNotBatch(string keyword = null, PagingInfo pagingInfo = null)
        {
            var query = Query<Item>().Where(p => (p.Type == ItemType.Product || p.Type == ItemType.SemiFinished))
                .NotExists<ItemBatchRule>((x, y) => y.Where(e => e.ItemId == x.Id && e.RetrospectType == RetrospectType.Batch));
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据字符串获取物料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemByCode(string code, PagingInfo page)
        {
            var q = Query<Item>().Where(p => p.State == State.Enable);
            if (!code.IsNullOrEmpty())
                q.Where(o => o.Code.Contains(code) || o.Name.Contains(code));
            return q.ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据字符串匹配物料的编号或者名称
        /// </summary>
        /// <param name="context">编号</param>
        /// <returns>返回物料信息</returns>
        public virtual Item GetItemByImport(string context)
        {
            return Query<Item>().Where(p => p.Code == context || p.Name == context).FirstOrDefault();
        }

        /// <summary>
        /// 获取拉式物料
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键值</param>
        /// <param name="mode">消耗模式</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetConsumeModeItems(PagingInfo pagingInfo, string keyword, ConsumeMode? mode = null)
        {
            var query = DB.Query<Item>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            if (mode.HasValue)
            {
                query.Where(p => p.ConsumeMode == mode.Value);
            }

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有物料的扩展属性
        /// </summary>
        /// <returns>返回所有物料的扩展属性</returns>
        public virtual List<ItemPropertyInfo> GetPropertyValueListByItemId(List<double> itemIds = null)
        {
            List<ItemPropertyInfo> results = new List<ItemPropertyInfo>();
            if (itemIds != null)
            {
                itemIds.SplitDataExecute(iIds =>
                {
                    var query = Query<ItemPropertyValue>().Join<ItemPropertyDefinition>((a, b) => a.DefinitionId == b.Id);
                    query.Where(a => iIds.Contains(a.ItemId)).ToList();
                    var toAdd = query.Select<ItemPropertyDefinition>((p, q) => new
                    {
                        RelationId = p.ItemId,
                        DefinitionId = p.DefinitionId,
                        DefinitionName = q.Name,
                        Value = p.Value,
                        PropertyGroup = p.PropertyGroup,
                    }).ToList<ItemPropertyInfo>().ToList();
                    results.AddRange(toAdd);
                });
            }

            return results;
        }

        /// <summary>
        /// 获取物料基本信息
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <returns>物料基本信息字典</returns>
        public virtual Dictionary<double, ItemBaseData> GetItemBaseDataDic(List<double> itemIds)
        {
            var exp = itemIds.CreateContainsExpression<Item>("x", nameof(Item.Id));
            if (exp == null)
            {
                return new Dictionary<double, ItemBaseData>();
            }
            var query = Query<Item>().Join<Unit>((p, u) => p.UnitId == u.Id);
            query.Select<Unit>((p, u) => new { p.Id, p.Code, p.Name, UnitName = u.Name, p.SpecificationModel });
            query.Where(exp);

            return query.ToList<ItemBaseData>().ToDictionary(p => p.Id);
        }

        /// <summary>
        /// 根据物料id列表获取相对应的编码和名称
        /// </summary>
        /// <param name="itemIds">物料id列表</param>
        /// <returns>字典key：物料id；value：物料编码、物料名称</returns>
        public virtual Dictionary<double, Tuple<string, string>> GetItemCodeAndNameById(List<double> itemIds)
        {
            List<ItemInfo> infos = new List<ItemInfo>();
            itemIds.SplitDataExecute(ids =>
            {
                var tmpList = Query<Item>().Where(p => ids.Contains(p.Id)).Select(p => new
                {
                    ItemId = p.Id,
                    ItemCode = p.Code,
                    ItemName = p.Name
                }).ToList<ItemInfo>().ToList();
                infos.AddRange(tmpList);
            });
            return infos.GroupBy(p => p.ItemId).ToDictionary(p => p.Key, p => new Tuple<string, string>(p.First().ItemCode, p.First().ItemName));
        }


        /// <summary>
        /// 根据产品族获取物料列表
        /// </summary>
        /// <param name="key">输入的信息</param>
        /// <param name="productFamilieId">产品族分类ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemProductFamilieId(string key, double? productFamilieId, PagingInfo pagingInfo)
        {
            var query = Query<Item>().LeftJoin<ProductFamily>((x, y) => x.ProductFamilyId == y.Id);
            if (key.IsNotEmpty())
                query.Where(p => p.Code.Contains(key) || p.Name.Contains(key));
            if (productFamilieId > 0)
                query.Where(x => x.ProductFamilyId == productFamilieId);
            return query.ToList(pagingInfo);
        }


        #endregion

        #region 产品BOM
        /// <summary>
        /// 查询所有在产品bom中的物料
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Item> GetBomProducts(string keyword = null, PagingInfo pagingInfo = null)
        {
            EntityList<ProductBom> itemList = new EntityList<ProductBom>();
            var SelectList = Query<ProductBom>().ToList();
            itemList.AddRange(SelectList);

            List<double> Ids = itemList.Select(p => p.ProductId).Distinct().ToList();

            var query = Query<Item>();
            query.Where(p => (p.Type == ItemType.Product || p.Type == ItemType.SemiFinished) && Ids.Contains(p.Id));
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty()); //贪婪加载，把创建人等一块加载

        }

        /// <summary>
        /// 根据物料获取产品BOM
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual ProductBom GetProductBomByItemId(double itemId)
        {
            var q = DB.Query<ProductBomDetail>();
            var productBomDetail = q.Join<ProductBom>((x, y) => x.ProductBomId == y.Id && x.ItemId == itemId).OrderByDescending(m => m.CreateDate).FirstOrDefault();
            return productBomDetail != null ? productBomDetail.ProductBom : null;
        }

        /// <summary>
        /// 查询产品BOM
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>ProductBom列数</returns>
        /// <exception cref="ArgumentNullException">ProductBom</exception>
        public virtual EntityList<ProductBom> GetProductBoms(ProductBomCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var query = Query<ProductBom>();
            if (criteria.Code.IsNotEmpty())
                query.Where(e => e.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(e => e.Name.Contains(criteria.Name));
            if (criteria.ProductCode.IsNotEmpty())
                query.Where(e => e.Product.Code.Contains(criteria.ProductCode));
            if (criteria.ProductName.IsNotEmpty())
                query.Where(e => e.Product.Name.Contains(criteria.ProductName));
            if (criteria.ProductId.IsNotEmpty())
            {
                string[] strarr = criteria.ProductId.Split(',');
                query.Where(e => strarr.Contains(e.ProductId + ""));
            }
            if (criteria.SpecificationModel.IsNotEmpty())
                query.Where(e => e.Product.SpecificationModel.Contains(criteria.SpecificationModel));

            return query.OrderBy(criteria.OrderInfoList)
                .ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty()); //贪婪加载，把创建人等一块加载进来
        }

        /// <summary>
        /// 查询产品列表的默认产品BOM列表
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns>产品BOM列表</returns>        
        public virtual EntityList<ProductBom> GetDefaultProductBoms(List<double> productIds)
        {
            return productIds.SplitContains(tempIds =>
            {
                var query = Query<ProductBom>()
                    .Where(p => tempIds.Contains(p.ProductId));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据产品ID找到对应的默认产品BOM
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品BOM</returns>
        /// <exception cref="ArgumentNullException">产品ID不存在</exception>
        public virtual ProductBom FindProductBom(double productId)
        {
            if (productId <= 0)
                throw new ArgumentNullException(nameof(productId));
            var query = Query<ProductBom>();
            query.Where(p => p.ProductId == productId && p.IsDefault);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品BOM明细
        /// </summary>
        /// <param name="productBomId">产品BOM Id</param>
        /// <returns>BOM明细</returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetail(double productBomId)
        {
            return Query<ProductBomDetail>().Where(p => p.ProductBomId == productBomId).ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(ProductBomDetail.ItemProperty));
        }

        /// <summary>
        /// 设置产品产品bom为缺省
        /// </summary>
        /// <param name="prodcutId">产品ID</param>
        /// <param name="bomId">产品BOM ID</param>
        public virtual void SetDefaultProductBom(double prodcutId, double bomId)
        {
            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                DB.Update<ProductBom>().Set(p => p.IsDefault, true).Where(p => p.Id == bomId).Execute();
                DB.Update<ProductBom>().Set(p => p.IsDefault, false).Where(p => p.ProductId == prodcutId && p.Id != bomId).Execute();
                tran.Complete();
            }
        }

        /// <summary>
        /// 判断产品是否存缺省bom
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistDefaultProductBom(double productId)
        {
            return Query<ProductBom>().Where(p => p.ProductId == productId && p.IsDefault).Count() > 0;
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="bomId">产品明细BomId</param>
        /// <returns>ProductBomDetail列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用异常</exception>
        public virtual EntityList<ProductBomDetail> GetProductBomDetails(double bomId)
        {
            if (bomId <= 0)
            {
                throw new ArgumentNullException(nameof(bomId));
            }

            return Query<ProductBomDetail>().Where(p => p.ProductBomId == bomId).ToList();
        }

        /// <summary>
        /// 获取产品BOM明细按ID列表
        /// </summary>
        /// <param name="bomIds">产品 Bom Id列表</param>
        /// <returns>产品BOM明细列表</returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetails(List<double> bomIds)
        {
            return bomIds.SplitContains(tempIds =>
            {
                return Query<ProductBomDetail>().Where(p => bomIds.Contains(p.ProductBomId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 产品明细BomId
        /// </summary>
        /// <param name="itemId">产品ID</param>
        /// <returns>产品BOM明细</returns>
        public virtual EntityList<ProductBomDetail> GetProductBomDetailsByItemId(double itemId)
        {
            if (itemId <= 0)
            {
                return new EntityList<ProductBomDetail>();
            }

            var q = DB.Query<ProductBomDetail>();
            q.Join<ProductBom>((x, y) => x.ProductBomId == y.Id && y.ProductId == itemId && y.IsDefault);

            return q.ToList();
        }

        /// <summary>
        /// 获取缺省产品BOM
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>产品BOM</returns>
        public virtual ProductBom GetDefaultProductBom(double productId, EagerLoadOptions elo = null)
        {
            return Query<ProductBom>().Where(p => p.ProductId == productId && p.IsDefault).FirstOrDefault(elo);
        }

        /// <summary>
        /// 根据产品BOM Id获取下层的物料
        /// </summary>
        /// <param name="bomId">产品BOM Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键词</param>
        /// <returns>下层的物料</returns>
        public virtual EntityList<Item> GetLowerLevelItemsByBomId(double bomId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<Item>().Exists<ProductBomDetail>((i, d) => d.Where(p => p.ItemId == i.Id && p.ProductBomId == bomId));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 删除已删除，保存新增属性
        /// </summary>
        /// <param name="values">当前所有属性值</param>
        /// <param name="bomId">产品BomId</param>
        public virtual void SaveBomPropertyValues(List<ItemPropertyValue> values, double bomId)
        {
            using (var tran = DB.AutonomousTransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                var propertyValues = RT.Service.Resolve<ProductBomController>().GetProductBomPropertyValues(bomId);  //产品Bom的所有属性值

                //删除属性
                propertyValues.Where(p => !values.Any(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value)).ForEach(p =>
                {
                    p.PersistenceStatus = PersistenceStatus.Deleted;

                    var result = values.FirstOrDefault(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value);
                    values.Remove(result);
                });

                propertyValues.ForEach(p =>
                {
                    var result = values.FirstOrDefault(q => q.DefinitionId == p.DefinitionId && q.Value == p.Value);
                    values.Remove(result); //移除已经存在的属性值
                });

                if (values.Count > 0)
                {
                    foreach (var value in values)  //新增属性值
                    {
                        ProductBomPropertyValue bomPropertyValue = new ProductBomPropertyValue()
                        {
                            Definition = value.Definition,
                            Value = value.Value,
                            ProductBomId = bomId
                        };

                        bomPropertyValue.PersistenceStatus = PersistenceStatus.New;
                        propertyValues.Add(bomPropertyValue);
                    }
                }

                RF.Save(propertyValues);
                tran.Complete();
            }
        }
        #endregion

        #region 物料属性
        /// <summary>
        /// 查询物料属性
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <param name="filterId">待过滤属性值ID</param>
        /// <returns>ItemPropertyValue列表</returns>
        /// <exception cref="ArgumentNullException">空引用异常</exception>
        public virtual EntityList<ItemPropertyValue> GetItemPropertys(ItemPropertyValueCriteria criteria, double[] filterId = null)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            var query = DB.Query<ItemPropertyValue>("i");
            if (criteria.Item != null)
            {
                query.Where(p => p.ItemId == criteria.ItemId);
            }

            if (criteria.Definition != null)
            {
                query.Where(p => p.DefinitionId == criteria.DefinitionId);
            }

            if (criteria.Value.IsNotEmpty())
            {
                query.Where(p => p.Value == criteria.Value);
            }

            if (filterId != null && filterId.Any())
            {
                var meta = RF.Find<ItemPropertyValue>().EntityMeta;
                StringBuilder ids = new StringBuilder();
                for (int i = 0; i < filterId.Count(); i++)
                {
                    if (i == filterId.Count() - 1)
                    {
                        ids.Append(filterId[i]);
                    }
                    else
                    {
                        ids.Append(filterId[i] + ",");
                    }
                }

                query.Where(p => p.SQL<bool>(new FormattedSql(@"i.{0} not in ({1})".FormatArgs(meta.Property(ItemPropertyValue.IdProperty).ColumnMeta.ColumnName, ids.ToString()))));
            }

            return query.ToList(criteria.PagingInfo);
        }

        /// <summary>
        /// 获取物料属性值
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="definitionId">属性Id</param>
        /// <returns>物料属性值</returns>
        public virtual EntityList<ItemPropertyValue> GetItemPropertys(double productId, double definitionId)
        {
            var query = DB.Query<ItemPropertyValue>().Where(p => p.ItemId == productId && p.DefinitionId == definitionId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns>物料属性列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<ItemPropertyValue> GetItemPropertys(double itemId)
        {
            return GetItemPropertys(itemId, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="elo">贪婪对象</param>
        /// <returns>物料属性列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<ItemPropertyValue> GetItemPropertys(double itemId, EagerLoadOptions elo)
        {
            if (itemId <= 0)
            {
                throw new ArgumentNullException(nameof(itemId));
            }
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                return Query<ItemPropertyValue>().Where(p => p.ItemId == itemId).ToList(null, elo);
            }
        }

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns>物料属性列表</returns>       
        public virtual EntityList<ItemPropertyValue> GetItemPropertys(double itemId, string keyword, PagingInfo pageInfo)
        {
            var query = Query<ItemPropertyValue>().Where(p => p.ItemId == itemId);
            if (!keyword.IsNullOrWhiteSpace())
            {
                query.Where(p => p.Name.Contains(keyword));
            }
            return query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="propValIds">Id集合</param>
        /// <returns>物料属性</returns>
        public virtual EntityList<ItemPropertyValue> GetItemPropertys(List<double> propValIds)
        {
            return Query<ItemPropertyValue>().Where(p => propValIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品BOM明细下该物料出现过的扩展属性
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="productBomId">产品BOM ID</param>
        /// <returns>物料属性</returns>
        public virtual EntityList<ItemPropertyValue> GetItemPropertysWithBomIs(double itemId, double productBomId)
        {
            List<string> itemExtProps = Query<ProductBomDetail>().Where(p => p.ProductBomId == productBomId && p.ItemId == itemId
            && p.ItemExtProp != null && p.ItemExtProp != "").Select(p => p.ItemExtProp).ToList<string>().Distinct().ToList();
            if (!itemExtProps.Any())
            {
                return new EntityList<ItemPropertyValue>();
            }
            return ConvertItemPropertysFromStrToEntities(itemId, itemExtProps);
        }

        /// <summary>
        /// 把扩展属性字符串转换成对应的物料属性值实体列表
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemExtProps">物料扩展属性字符串</param>
        /// <returns>物料属性</returns>
        public virtual EntityList<ItemPropertyValue> ConvertItemPropertysFromStrToEntities(double itemId, List<string> itemExtProps)
        {
            if (!itemExtProps.Any(p => p != ""))
            {
                return new EntityList<ItemPropertyValue>();
            }
            EntityList<ItemPropertyValue> results = new EntityList<ItemPropertyValue>();
            var definitionIdValuePropertyDic = Query<ItemPropertyValue>().Where(p => p.ItemId == itemId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty())
                .GroupBy(p => p.DefinitionId).ToDictionary(p => p.Key, p =>
                p.GroupBy(e => e.Value).ToDictionary(e => e.Key, e => e.First()));
            foreach (var itemExtProp in itemExtProps)
            {
                string[] pvalues = itemExtProp.Split(';');
                foreach (string val in pvalues)
                {
                    if (val.IsNullOrEmpty())
                    {
                        continue;
                    }

                    double definitionId;
                    string[] vals = val.Split(':');
                    if (vals.Length == 2 && double.TryParse(vals[0], out definitionId))
                    {
                        string[] arrvalue = vals[1].Split(',');
                        foreach (string v in arrvalue)
                        {
                            if (!v.IsNullOrEmpty())
                            {
                                Dictionary<string, ItemPropertyValue> tmpDic;
                                if (definitionIdValuePropertyDic.TryGetValue(definitionId, out tmpDic))
                                {
                                    ItemPropertyValue tmpValue;
                                    if (tmpDic.TryGetValue(v, out tmpValue))
                                    {
                                        results.Add(tmpValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// 获取去重的全部物料属性（根据值和属性类的组合来去重）
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns>物料属性实体列表</returns>
        public virtual EntityList<ItemPropertyValue> GetDistinctItemPropertys(string keyword, PagingInfo pageInfo)
        {
            var query = Query<ItemPropertyValue>().Join<ItemPropertyDefinition>((v, d) => v.DefinitionId == d.Id);
            if (!keyword.IsNullOrWhiteSpace())
            {
                query.Where<ItemPropertyDefinition>((v, d) => d.Name.Contains(keyword) || v.Value.Contains(keyword));
            }
            var returns = query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty()).DistinctBy(p => p.Value + p.DefinitionId).AsEntityList();
            return returns;

        }

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="itemIds">物料ID集合</param>
        /// <returns>物料属性列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用</exception>
        public virtual EntityList<ItemPropertyValue> GetItemPropertyList(List<double> itemIds)
        {
            if (itemIds == null || itemIds.Count == 0)
            {
                return new EntityList<ItemPropertyValue>();
            }
            return itemIds.SplitContains(sons =>
            {
                return Query<ItemPropertyValue>().Where(p => sons.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="definitionId">物料属性ID</param>
        /// <returns>物料属性</returns>
        /// <exception cref="ArgumentNullException">空引用异常</exception>
        public virtual ItemPropertyValue GetItemPropertyValue(double definitionId)
        {
            if (definitionId <= 0)
            {
                throw new ArgumentNullException(nameof(definitionId));
            }

            return Query<ItemPropertyValue>()
                .Where(p => p.DefinitionId == definitionId)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取物料属性值
        /// </summary>
        /// <param name="definitionId">属性定义ID</param>
        /// <param name="value">属性值</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>物料属性模板</returns>
        /// <exception cref="ArgumentNullException">参数空引用异常</exception>
        public virtual ItemPropertyValue GetItemPropertyValue(double definitionId, string value, double itemId)
        {
            if (definitionId <= 0)
            {
                throw new ArgumentNullException(nameof(definitionId));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (itemId <= 0)
            {
                throw new ArgumentNullException(nameof(itemId));
            }

            return Query<ItemPropertyValue>().Where(p => p.DefinitionId == definitionId && p.Value == value && p.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料属性值
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="definitionId">属性定义ID</param>
        /// <returns>物料属性列表</returns>
        /// <exception cref="ArgumentNullException">参数空引用异常</exception>
        public virtual EntityList<ItemPropertyValue> GetItemPropertyValues(double itemId, double definitionId)
        {
            if (itemId <= 0)
            {
                throw new ArgumentNullException(nameof(itemId));
            }

            if (definitionId <= 0)
            {
                throw new ArgumentNullException(nameof(definitionId));
            }

            return Query<ItemPropertyValue>().Where(p => p.DefinitionId == definitionId && p.ItemId == itemId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证物料属性
        /// </summary>
        /// <param name="toValidateItemInfo">待验证物料</param>
        /// <param name="tagetItemInfo">目标物料（要求匹配物料）</param>
        /// <param name="msg">格式："XXX{0}不一致"</param>
        /// <exception cref="ValidationException">物料不一致、物料属性不一致、物料属性值不一致</exception>
        [IgnoreProxy]
        public virtual void ValidateItemPorperty(ToValidateItemInfo toValidateItemInfo, TagetItemInfo tagetItemInfo, string msg)
        {
            if (toValidateItemInfo == null || tagetItemInfo == null)
            {
                return;
            }
            if (toValidateItemInfo.ItemId == tagetItemInfo.ItemId)
            {
                //验证物料属性
                ValidateItemInfo(toValidateItemInfo, tagetItemInfo, msg);
            }
            else
            {
                //验证替代料
                ValidateAlterItemInfo(toValidateItemInfo, tagetItemInfo, msg);
            }
        }

        /// <summary>
        /// 验证物料扩展属性是否属于某物料
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemExtPropName"></param>
        public virtual void ValidateItemPorperty(double itemId, string itemExtPropName)
        {
            var propertyValues = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId);
            var definitions = GetDefinitions(itemExtPropName);
            definitions.ForEach(p =>
            {
                if (!propertyValues.Any(t => t.DefinitionName == p.Key && t.Value == p.Value))
                {
                    throw new ValidationException("物料扩展属性[{0}]与该物料不一致".L10nFormat(p.Key + ':' + p.Value));
                }
            });
        }

        /// <summary>
        /// 验证替代料
        /// </summary>
        /// <param name="toValidateItemInfo"></param>
        /// <param name="tagetItemInfo"></param>
        /// <param name="msg"></param>
        private static void ValidateAlterItemInfo(ToValidateItemInfo toValidateItemInfo, TagetItemInfo tagetItemInfo, string msg)
        {
            var alternative = tagetItemInfo.AlternativeList.FirstOrDefault(p => p.ItemId == toValidateItemInfo.ItemId);
            if (alternative == null)
                throw new ValidationException(msg.L10nFormat("物料"));
            if (toValidateItemInfo.ValidateProperty)
            {
                //验证替代料属性
                foreach (var item in alternative.ValueList)
                {
                    var property = toValidateItemInfo.ValueList.FirstOrDefault(f => f.DefinitionId == item.DefinitionId);
                    if (property == null)
                    {
                        throw new ValidationException(msg.L10nFormat("物料属性"));
                    }
                    if (property.Value != item.Value)
                    {
                        throw new ValidationException(msg.L10nFormat("物料属性值"));
                    }
                }
            }
        }

        /// <summary>
        /// 验证物料属性
        /// </summary>
        /// <param name="toValidateItemInfo"></param>
        /// <param name="tagetItemInfo"></param>
        /// <param name="msg"></param>
        private static void ValidateItemInfo(ToValidateItemInfo toValidateItemInfo, TagetItemInfo tagetItemInfo, string msg)
        {
            if (toValidateItemInfo.ValidateProperty)
            {
                foreach (var item in tagetItemInfo.ValueList)
                {
                    var property = toValidateItemInfo.ValueList.FirstOrDefault(f => f.DefinitionId == item.DefinitionId);
                    if (property == null)
                    {
                        throw new ValidationException(msg.L10nFormat("物料属性"));
                    }
                    if (property.Value != item.Value)
                    {
                        throw new ValidationException(msg.L10nFormat("物料属性值"));
                    }
                }
            }
            else
            {
                throw new ValidationException(msg.L10nFormat("物料"));
            }
        }

        /// <summary>
        /// 拆分选择物料扩展属性
        /// </summary>
        /// <param name="selectPropNames">选择物料扩展属性</param>
        /// <returns>物料扩展属性</returns>
        public virtual Dictionary<string, string> GetDefinitions(string selectPropNames)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            var names = selectPropNames?.Split(';', (char)StringSplitOptions.RemoveEmptyEntries);
            if (names == null)
            {
                return results;
            }
            foreach (var name in names)
            {
                var definition = name.Split(':')[0];
                var propertyValue = name.Split(':')[1];
                results.Add(definition, propertyValue);
            }
            return results;
        }

        /// <summary>
        /// 根据物料ID，属性组获取物料定义ID
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="propertyGroup">属性组</param>
        /// <returns>返回物料定义ID</returns>
        public virtual ItemPropertyDefinition GetDefinitionIdByPropertyGroup(double itemId, string propertyGroup)
        {
            var definition = Query<ItemPropertyDefinition>().Exists<ItemPropertyValue>((x, y) => y.Where(p => p.DefinitionId == x.Id && p.ItemId == itemId && p.PropertyGroup == propertyGroup))
                            .ToList().FirstOrDefault();

            return definition;
        }

        /// <summary>
        /// 获取扩展属性ID和其属性名字的键值对
        /// </summary>
        /// <returns>扩展属性ID和其属性名字的字典</returns>
        public virtual Dictionary<double, string> GetDicItemProIdDefinitionName()
        {
            return Query<ItemPropertyValue>().ToList().GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.First().Definition.Name);
        }

        /// <summary>
        /// 根据形式“扩展属性名字:扩展属性值”的字符串得出扩展属性实体
        /// </summary>
        /// <param name="propString">形式“扩展属性名字:扩展属性值”的字符串</param>
        /// <returns>扩展属性实体</returns>
        public virtual ItemPropertyValue GetDicPropStringPropId(string propString)
        {
            var strs = propString.Split(':');
            if (strs.Length != 2)
            {
                return null;
            }
            var definitionName = strs[0];
            var propValue = strs[1];
            return Query<ItemPropertyValue>().Join<ItemPropertyDefinition>((v, d) => v.DefinitionId == d.Id && definitionName == d.Name)
                .Where(p => p.Value == propValue).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取参数中物料中启用了扩展属性的物料的id列表
        /// </summary>
        /// <returns></returns>
        public virtual List<double> GetItemIdsEnableExt(List<double> itemIds)
        {
            List<double> ids = new List<double>();
            if (itemIds == null || !itemIds.Any())
            {
                return ids;
            }
            itemIds.SplitDataExecute(list =>
            {
                var tmpList = Query<Item>().Where(p => list.Contains(p.Id) && p.EnableExtendProperty).Select(p => p.Id).ToList<double>().ToList();
                ids.AddRange(tmpList);
            });
            return ids.Distinct().ToList();
        }

        /// <summary>
        /// 获取物料属性定义
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>物料属性定义</returns>
        public virtual EntityList<ItemPropertyDefinition> GetItemPropertyDefinitions(ItemPropertyDefinitionCriteria criteria)
        {
            var query = Query<ItemPropertyDefinition>();
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }

            if (criteria.PropertyType.HasValue)
            {
                query.Where(p => p.PropertyType == criteria.PropertyType.Value);
            }

            if (criteria.CatalogTypeId.HasValue)
            {
                query.Where(p => p.CatalogTypeId == criteria.CatalogTypeId.Value);
            }

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion 物料属性 

        #region 单位

        /// <summary>
        /// 根据Id列表 获取单位列表
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>单位列表</returns>
        public virtual EntityList<Unit> GetUnitList(List<double> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new EntityList<Unit>();
            }
            return ids.SplitContains(sons =>
            {
                return Query<Unit>().Where(p => sons.Contains(p.Id)).ToList();
            });
        }
        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<Unit> GetUnits(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Unit>();
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据Id获取非此ID的单位列表
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="keyword">查询关键值</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>单位列表</returns>
        public virtual EntityList<Unit> GetUnitList(double id, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<Unit>().Where(p => p.Id != id);
            if (!string.IsNullOrEmpty(keyword))
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>单位</returns>
        public virtual EntityList<Unit> GetUnitList(List<string> codes)
        {
            return codes.SplitContains(pCodes =>
            {
                return Query<Unit>().Where(p => pCodes.Contains(p.Code)).ToList();
            });
        }

        /// <summary>
        /// 获取单位ID和名称字典
        /// </summary>
        /// <param name="unitIds">单位ID列表</param>
        /// <returns>key：单位ID；value：单位名称</returns>
        public virtual Dictionary<double, string> GetUnitIdNameDic(List<double> unitIds)
        {
            List<ItemInfo> infos = new List<ItemInfo>();
            unitIds.SplitDataExecute(ids =>
            {
                infos.AddRange(
                    Query<Unit>().Where(p => ids.Contains(p.Id)).Select(p => new
                    {
                        ItemId = p.Id,
                        ItemName = p.Name
                    }).ToList<ItemInfo>().ToList());
            });
            return infos.GroupBy(p => p.ItemId).ToDictionary(p => p.Key, p => p.First().ItemName);
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>单位</returns>
        public virtual Unit GetUnit(string name)
        {
            return Query<Unit>().Where(p => name.Contains(p.Name)).FirstOrDefault();
        }
        #endregion

        #region 产品机型 
        /// <summary>
        /// 产品机型默认查询方法
        /// </summary>
        /// <param name="criteria">产品机型查询实体</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">查询实体为空</exception>
        public virtual EntityList<ProductModel> GetProductModelList(ProductModelCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));
            var query = Query<ProductModel>();
            if (criteria.Code.IsNotEmpty())
                query.Where(e => e.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(e => e.Name.Contains(criteria.Name));
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 排除被产品族选择的产品机型查询方法
        /// </summary>
        /// <param name="criteria">产品机型查询实体</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>EntityList</returns>
        /// <exception cref="ArgumentNullException">查询实体为空</exception>
        public virtual EntityList<ProductModel> GetProductModelList(ProductModelCriteria criteria, PagingInfo pagingInfo)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            var query = Query<ProductModel>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(e => e.Code.Contains(criteria.Code));
            }

            if (criteria.Name.IsNotEmpty())
            {
                query.Where(e => e.Name.Contains(criteria.Name));
            }

            query.Where(e => e.ProductFamilyId == null);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取排除被产品族选择的产品机型列表(无查询实体参数)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ProductModel> GetProductModels()
        {
            return Query<ProductModel>().Where(p => p.ProductFamilyId == null).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品机型byIds
        /// </summary>
        /// <returns>产品机型</returns>
        public virtual EntityList<ProductModel> GetProductModels(List<double> ids)
        {
            return Query<ProductModel>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获得产品族分类对应的产品机型列表
        /// </summary>
        /// <param name="id">产品族分类Id</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>产品机型列表</returns>
        public virtual EntityList<ProductModel> GetProductModels(double id, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            var query = Query<ProductModel>();
            if (sortInfo != null)
                query.OrderBy(sortInfo);
            return query.Where(p => p.ProductFamilyId == id).ToList(pagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 移除产品机型与产品族关联
        /// </summary>
        /// <param name="selectedItems">选择的产品机型</param>
        /// <param name="criteria">产品机型查询实体</param>
        /// <returns>EntityList</returns>
        public virtual EntityList<ProductModel> RemoveProductModel(List<ProductModel> selectedItems, ProductModelCriteria criteria)
        {
            var productModelList = new EntityList<ProductModel>();
            var productFamilyCategoryId = selectedItems.Select(o => o.ProductFamilyId).FirstOrDefault().Value;
            foreach (var productModel in selectedItems)
            {
                productModel.ProductFamilyId = null;
                productModel.PersistenceStatus = PersistenceStatus.Modified;
                productModelList.Add(productModel);
            }

            RF.Save(productModelList);
            return GetProductModels(productFamilyCategoryId, (List<OrderInfo>)criteria.OrderInfoList, criteria.PagingInfo);
        }

        /// <summary>
        /// 添加产品机型与产品族关联并返回数据
        /// </summary>
        /// <param name="selectedItems">选择的产品机型</param>
        /// <param name="criteria">产品机型查询实体</param>
        /// <param name="productFamilyCategoryId">产品族Id</param>      
        /// <returns>EntityList</returns>
        public virtual EntityList<ProductModel> AddRangeProductModel(List<ProductModel> selectedItems, ProductModelCriteria criteria, double productFamilyCategoryId)
        {
            SetProductModelFamily(selectedItems, productFamilyCategoryId);
            return GetProductModels(productFamilyCategoryId, (List<OrderInfo>)criteria.OrderInfoList, criteria.PagingInfo);
        }

        /// <summary>
        /// 添加产品机型与产品族关联
        /// </summary>
        /// <param name="selectedItems">选择的产品机型</param>     
        /// <param name="productFamilyCategoryId">产品族Id</param>      
        public virtual void SetProductModelFamily(List<ProductModel> selectedItems, double productFamilyCategoryId)
        {
            var productModelList = new EntityList<ProductModel>();
            productModelList.AddRange(selectedItems);
            productModelList.ForEach(p => p.ProductFamilyId = productFamilyCategoryId);
            RF.Save(productModelList);
        }

        /// <summary>
        /// 根据产品编码获取产品机型
        /// </summary>
        /// <param name="code">产品编码</param>
        /// <returns>产品机型</returns>
        internal ProductModel GetProductModel(string code)
        {
            return Query<ProductModel>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获得产品族获取对应的产品机型
        /// </summary>
        /// <param name="familyId">产品族Id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>产品机型列表</returns>
        public virtual EntityList<ProductModel> GetProductModelsByFamily(double familyId, string keyword = "", PagingInfo pagingInfo = null)
        {
            var query = Query<ProductModel>();
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            }

            return query.Where(p => p.ProductFamilyId == familyId).ToList(pagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获得产品族Id列表获取对应的产品机型
        /// </summary>
        /// <param name="productFamilyIds">产品族Id列表</param>
        /// <returns>产品机型列表</returns>
        public virtual EntityList<ProductModel> GetProductModelsByFamily(List<double> productFamilyIds)
        {
            var allProductFamilyIds = productFamilyIds.Cast<double?>().ToList();

            var productModels = new EntityList<ProductModel>();

            for (int i = 0; i < Math.Ceiling((double)allProductFamilyIds.Count / 1000); i++)
            {
                var q = Query<ProductModel>().Where(x => allProductFamilyIds.Skip(i * 1000).Take(1000).Contains(x.ProductFamilyId));

                productModels.AddRange(q.ToList(null, new EagerLoadOptions().LoadWith(ProductModel.ProductFamilyProperty)));
            }

            return productModels;
        }

        /// <summary>
        /// 根据物料Id集合获取模型字典
        /// </summary>
        /// <param name="itemIds">物料Id集合</param>
        /// <returns>模型字典，Key：物料Id</returns>
        public virtual Dictionary<double, ProductModel> GetItemIdToModels(List<double> itemIds)
        {
            Dictionary<double, ProductModel> dics = new Dictionary<double, ProductModel>();

            itemIds.SplitDataExecute(tempIds =>
            {
                var models = Query<ProductModel>().Join<Item>((x, y) => x.Id == y.ModelId && tempIds.Contains(y.Id)).ToList();
                var items = GetItemList(tempIds.ToList());
                items.ForEach(p => dics[p.Id] = models.FirstOrDefault(q => q.Id == p.ModelId));
            });
            return dics;
        }
        #endregion 

        #region 查询
        /// <summary>
        /// 下拉选择框数据查询
        /// </summary>
        /// <param name="itemTypeValueList">物料类型列表</param>
        /// <param name="state">物料状态</param>
        /// <param name="keyword">关键词</param>
        /// <param name="pageinfo">分页</param>        
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItemsFormType(List<int> itemTypeValueList, State? state, string keyword, PagingInfo pageinfo = null)
        {
            var query = Query<Item>().WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)
            || p.Description.Contains(keyword) || p.Model.Name.Contains(keyword)
            || p.PurchasingAgent.Name.Contains(keyword));
            if (itemTypeValueList != null)
            {
                query = query.Where(p => itemTypeValueList.Contains((int)p.Type));
            }

            if (state != null)
            {
                query.Where(p => p.State == state);
            }

            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取长度单位类型
        /// </summary>
        /// <returns>长度单位类型</returns>
        public virtual string GetLengthUnitTypeCode()
        {
            var config = ConfigService.GetConfig(new LengthUnitNoConfig(), typeof(Item));
            if (config == null || string.IsNullOrEmpty(config.LengthTypeCode))
            {
                return string.Empty;
            }

            return config.LengthTypeCode;
        }

        /// <summary>
        /// 获取重量单位类型
        /// </summary>
        /// <returns>重量单位类型</returns>
        public virtual string GetWeightUnitTypeCode()
        {
            var config = ConfigService.GetConfig(new WeightUnitNoConfig(), typeof(Item));
            if (config == null || string.IsNullOrEmpty(config.WeightTypeCode))
            {
                return string.Empty;
            }

            return config.WeightTypeCode;
        }

        /// <summary>
        /// 获取体积单位类型
        /// </summary>
        /// <returns>体积单位类型</returns>
        public virtual string GetVolumeUnitTypeCode()
        {
            var config = ConfigService.GetConfig(new VolumeUnitNoConfig(), typeof(Item));
            if (config == null || string.IsNullOrEmpty(config.VolumeTypeCode))
            {
                return string.Empty;
            }

            return config.VolumeTypeCode;
        }

        /// <summary>
        /// 根据物料ID获取物料与分类关系
        /// </summary>
        /// <param name="itemID">物料ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>物料与分类关系</returns>
        public virtual EntityList<ItemCategoryRelation> GetItemCategoryRelation(double itemID, PagingInfo pagingInfo)
        {
            return Query<ItemCategoryRelation>().Where(p => p.ItemId == itemID)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过检验组与分类关系获取分类与物料关系
        /// </summary>
        /// <param name="quality">检验组与分类关系对象</param>
        /// <returns>返回分类与物料关系</returns>
        public virtual IEnumerable<ItemCategoryRelation> GetItemCategoryRelation(List<ItemCategory> quality)
        {
            var meta = RF.Find<ItemCategoryRelation>().EntityMeta;

            //检验组关联的所有质量分类的Id
            string ids = string.Join(",", quality.Select(p => p.Id.ToString()).Distinct((x, y) => x == y).ToList());
            IEnumerable<ItemCategoryRelation> result = Query<ItemCategoryRelation>().Where(p => p.SQL<bool>(
                                 new FormattedSql(@"{0} in ({1})".FormatArgs(meta.Property(ItemCategoryRelation.ItemCategoryIdProperty).ColumnMeta.ColumnName, ids)))).ToList().AsEnumerable();
            return result;
        }

        /// <summary>
        /// 获取质量分类与物料关系
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="type">分类类型</param>
        /// <returns>物料与分类关系</returns>
        /// <exception cref="ValidationException">分类为空</exception>
        public virtual ItemCategoryRelation GetItemCategoryRelationData(double itemId, CategoryType? type = null)
        {
            var query = Query<ItemCategoryRelation>().Where(p => p.ItemId == itemId);
            if (type != null)
                query.Where(p => p.Type == type);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料ID获取物料与质量分类关系
        /// </summary>
        /// <param name="itemID">物料ID</param>
        /// <returns>物料与质量分类关系</returns>
        public virtual ItemCategoryRelation GetItemQualityCategoryRelation(double itemID)
        {
            return Query<ItemCategoryRelation>().Where(p => p.ItemId == itemID && p.Type == CategoryType.Quality).FirstOrDefault();
        }

        /// <summary>
        /// 根据物料ID获取物料与质量分类关系
        /// </summary>
        /// <param name="itemIds">物料ID集合</param>
        /// <returns>物料与质量分类关系</returns>
        public virtual EntityList<ItemCategoryRelation> GetItemCategoryRelations(List<double> itemIds)
        {
            var exp = itemIds.CreateContainsExpression<ItemCategoryRelation>("x", "ItemId");
            if (exp == null)
                return new EntityList<ItemCategoryRelation>();
            return Query<ItemCategoryRelation>().Where(exp).Where(p => p.Type == CategoryType.Quality).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料ID获取物料与质量分类关系
        /// </summary>
        /// <param name="itemCodes">物料ID集合</param>
        /// <returns>物料与质量分类关系</returns>
        public virtual EntityList<ItemCategoryRelation> GetItemCategoryRelations(List<string> itemCodes)
        {
            return itemCodes.SplitContains(ids =>
            {
                return Query<ItemCategoryRelation>().Where(f => ids.Contains(f.Item.Code)).Where(f => f.Type == CategoryType.Item).ToList();
            });
        }

        /// <summary>
        /// 检查物料类别
        /// </summary>
        /// <param name="itemIds">物料Id</param>      
        public virtual void CheckItemIdItemCategory(List<double> itemIds)
        {
            itemIds = itemIds.Distinct().ToList();
            var exp = itemIds.CreateContainsExpression<ItemCategoryRelation>("x", "ItemId");
            if (exp == null)
                throw new ValidationException("物料没有设置质量类别".L10N());
            var itemCount = Query<ItemCategoryRelation>().Where(exp).Where(p => p.Type == CategoryType.Quality && p.ItemCategoryId != null).Count();
            if (itemCount != itemIds.Count)
                throw new ValidationException("物料没有设置质量类别".L10N());
        }

        /// <summary>
        /// 根据物料分类Id列表获取物料分类
        /// </summary>
        /// <param name="categoryIds">物料小类Id列表</param>
        /// <param name="consumeMode">物料消耗模式</param>
        /// <returns>返回物料信息</returns>
        public virtual EntityList<ItemCategoryRelation> GetItemCategoryRelations(List<double?> categoryIds, ConsumeMode? consumeMode = null)
        {
            return categoryIds.SplitContains(tempIds =>
            {
                if (consumeMode != null)
                {
                    return Query<ItemCategoryRelation>()
                    .Where(x => x.Type == CategoryType.Item && tempIds.Contains(x.ItemCategoryId)
                        && x.Item.State == State.Enable && x.Item.ConsumeMode == consumeMode)
                    .ToList();
                }

                return Query<ItemCategoryRelation>()
                    .Where(x => x.Type == CategoryType.Item && tempIds.Contains(x.ItemCategoryId)
                        && x.Item.State == State.Enable)
                    .ToList();
            });
        }

        /// <summary>
        /// 根据物料ID获取物料与质量分类关系
        /// </summary>
        /// <param name="itemIds">物料ID集合</param>
        /// <param name="categoryType">分类类型</param>
        /// <returns>物料与质量分类关系</returns>
        public virtual EntityList<ItemCategoryRelation> GetItemCategoryRelationByCategoryTypes(List<double> itemIds, CategoryType categoryType)
        {
            if (itemIds == null || itemIds.Count == 0)
            {
                return new EntityList<ItemCategoryRelation>();
            }
            return itemIds.SplitContains(sons =>
            {
                return Query<ItemCategoryRelation>().Where(i => sons.Contains(i.ItemId)).Where(p => p.Type == categoryType).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据分类ID集合获取物料
        /// </summary>
        /// <param name="ctgIds">分类ID</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>物料与分类关系</returns>
        public virtual EntityList<Item> GetItemFromCategoryIds(List<double> ctgIds, PagingInfo pagingInfo = null)
        {
            return Query<Item>().Join<ItemCategoryRelation>((x, y) => y.ItemId == x.Id && ctgIds.Contains((double)y.ItemCategoryId)).ToList(pagingInfo);
        }

        /// <summary>
        /// 根据物料ID获取日记消息
        /// </summary>
        /// <param name="itemID">物料ID</param>
        /// <returns>日记消息</returns>
        public virtual EntityList<ItemLog> GetItemLogs(double itemID)
        {
            return Query<ItemLog>().Where(p => p.ItemId == itemID).ToList();
        }

        /// <summary>
        /// 根据分类类型获取物料小类
        /// </summary>
        /// <param name="type">分类类型</param>
        /// <param name="itemType">物料类型</param>
        /// <param name="keyword">搜索文本</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>物料小类</returns>
        public virtual EntityList<ItemCategory> GetItemSmallCategory(CategoryType? type, ItemType? itemType, string keyword, PagingInfo pagingInfo)
        {
            var meta = RF.Find<ItemCategory>().EntityMeta;
            var query = DB.Query<ItemCategory>("i1");
            if (type.HasValue)
                query.Where(p => p.Type == type);
            if (type == CategoryType.Item)
            {
                if (itemType.HasValue)
                    query.Where(p => p.ItemType == itemType || p.ItemType == null);
            }
            else
            {
                if (itemType.HasValue)
                    query.Where(p => p.ItemType == itemType);
            }
            query.Where(f => f.SQL<bool>(new FormattedSql(@"not exists(select 1 from {0} i2 where i1.{1} = i2.{2} and i2.{3} = '{4}')".FormatArgs(meta.TableMeta.TableName, meta.Property(QualityCategory.IdProperty).ColumnMeta.ColumnName, meta.Property(QualityCategory.TreePIdProperty).ColumnMeta.ColumnName, meta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName, 0))));
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取质量分类
        /// </summary>
        /// <param name="itemType">物料类型</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键词</param>
        /// <returns>质量分类</returns>
        public virtual EntityList<QualityCategory> GetQualityCategories(ItemType? itemType, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<QualityCategory>().Where(p => p.TreePId != null && p.ItemType == itemType).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            query.NotExists<ItemCategory>((x, y) => y.Where(b => b.TreePId == x.Id));
            return query.ToList(pagingInfo);
        }
        #endregion

        #region 产品族
        /// <summary>
        /// 产品机型与产品族关联关系配置
        /// </summary>
        public virtual void ConfigProductModel(List<double> productModelIds, double? productFamilyCategoryId)
        {
            var res = Query<ProductModel>().Where(p => productModelIds.Contains(p.Id)).ToList();
            foreach (var item in res)
            {
                item.ProductFamilyId = productFamilyCategoryId;
            }
            RF.Save(res);
        }
        /// <summary>
        /// 获取产品族列表
        /// </summary>
        /// <param name="categoryId">产品族分类ID</param>
        /// <returns>产品族列表</returns>
        public virtual EntityList<ProductFamily> GetProductFamilies(double categoryId)
        {
            return Query<ProductFamily>().Where(p => p.CategoryId == categoryId).ToList();
        }

        /// <summary>
        /// 获取所有产品族列表
        /// </summary> 
        /// <returns>产品族列表</returns>
        public virtual EntityList<ProductFamily> GetProductFamilies()
        {
            return Query<ProductFamily>().ToList(new PagingInfo { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        /// <summary>
        /// 获取所有产品族分类列表
        /// </summary> 
        /// <returns>产品族分类列表</returns>
        public virtual EntityList<ProductFamilyCategory> GetProductFamilyCategories()
        {
            return Query<ProductFamilyCategory>().ToList(new PagingInfo { PageNumber = 1, PageSize = int.MaxValue - 1 });
        }

        #endregion

        #region 产品批次规则 
        /// <summary>
        /// 获取产品批次规则
        /// </summary>
        /// <param name="itemId">产品Id</param>
        /// <returns></returns>
        public virtual ItemBatchRule GetBatchRule(double itemId)
        {
            var query = Query<ItemBatchRule>().Where(p => p.ItemId == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 获取默认规则项
        /// </summary>
        public virtual RetrospectType GetRetrospectTypeConfig()
        {
            var config = ConfigService.GetConfig(new RetrospectTypeConfig());
            if (config == null)
                throw new ValidationException("批次追溯类型信息未配置".L10N());
            return config.RetrospectType;
        }

        /// <summary>
        /// 获取物料追溯方式
        /// </summary>
        /// <param name="itemId">产品Id</param>
        /// <returns>物料追溯方式</returns>
        public virtual RetrospectType GetRetrospectType(double itemId)
        {
            var rule = GetBatchRule(itemId);
            if (rule == null)
                throw new ValidationException("未找到物料批次规则，请检查数据".L10N());
            return rule.RetrospectType;
        }
        #endregion

        #region 产品等级
        /// <summary>
        /// 获取快码组
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>快码组集合</returns>
        public virtual EntityList<CatalogType> GetCatalogTypes(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<CatalogType>();
            if (keyword.IsNotEmpty())
            {
                query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword) || x.Description.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 获取指定快码组的快码集合
        /// </summary>
        /// <param name="catalogTypeCode">快码组编码</param>
        /// <returns>指定快码组的快码集合</returns>
        public virtual EntityList<Catalog> GetCatalogs(string catalogTypeCode)
        {
            var qrys = Query<Catalog>();
            if (catalogTypeCode.IsNotEmpty())
            {
                qrys.Where(x => x.CatalogType.Code == catalogTypeCode);
            }
            var result = qrys.ToList();
            return result;
        }

        /// <summary>
        /// 获取产品等级的快码组信息
        /// </summary>
        /// <param name="criteria">产品等级查询实体</param>
        /// <returns>产品等级的快码组信息</returns>
        public virtual EntityList<Catalog> GetProductGradeCatalogs(ProductGradeCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            string catalogTypeCode = "PRODUCTGRADE_TYPE";
            var code = criteria.Code;
            var name = criteria.Name;
            var desc = criteria.Describe;
            var query = Query<Catalog>();
            if (catalogTypeCode.IsNotEmpty())
            {
                query.Where(x => x.CatalogType.Code == catalogTypeCode);
            }

            if (code.IsNotEmpty())
            {
                query.Where(e => e.Code == code);
            }

            if (name.IsNotEmpty())
            {
                query.Where(e => e.Name == name);
            }

            if (desc.IsNotEmpty())
            {
                query.Where(e => e.Description == desc);
            }

            var list = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 获取产品等级集合
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="orderInfos">排序信息</param>
        /// <returns>产品等级集合</returns>
        public virtual EntityList<ProductGrade> GetProductGrades(double itemId, PagingInfo pagingInfo = null, List<OrderInfo> orderInfos = null)
        {
            return Query<ProductGrade>().Where(p => p.ItemId == itemId).OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品等级快码组的快码集合，转换为产品等级
        /// </summary>
        /// <param name="catalogTypeCode"></param>
        /// <returns></returns>
        public virtual EntityList<ProductGrade> GetProductGrades(string catalogTypeCode)
        {
            var productGrades = new EntityList<ProductGrade>();
            var catalogs = GetCatalogs(catalogTypeCode);
            foreach (var curCatalog in catalogs)
            {
                var curProductGrade = ConvertCatalogToProductGrade(curCatalog);
                productGrades.Add(curProductGrade);
            }
            return productGrades;
        }

        /// <summary>
        /// 获取产品等级信息
        /// </summary>
        /// <param name="criteria">产品等级查询实体</param>
        /// <returns>产品等级信息</returns>
        public virtual EntityList<ProductGrade> GetProductGrades(ProductGradeCriteria criteria)
        {
            List<string> existGradeCodes = new List<string>();
            var productGrades = new EntityList<ProductGrade>();
            var catalogs = GetProductGradeCatalogs(criteria);

            if (criteria.ItemId != 0)
            {
                var existProductGrades = GetProductGrades(criteria.ItemId, criteria.PagingInfo, (List<OrderInfo>)criteria.OrderInfoList);
                existGradeCodes = existProductGrades.Select(x => x.Code).ToList();
            }

            foreach (var curCatalog in catalogs)
            {
                if (!existGradeCodes.Contains(curCatalog.Code))
                {
                    var curProductGrade = ConvertCatalogToProductGrade(curCatalog);
                    productGrades.Add(curProductGrade);
                }
            }
            productGrades.SetTotalCount(productGrades.Count);
            return productGrades;
        }

        /// <summary>
        /// 获取产品等级信息
        /// </summary>
        /// <param name="criteria">产品等级查询实体</param>
        /// <param name="itemId">当前物料Id</param>
        /// <returns>产品等级信息</returns>
        public virtual EntityList<ProductGrade> GetProductGrades(ProductGradeCriteria criteria, double itemId)
        {
            var productGrades = new EntityList<ProductGrade>();
            var catalogs = GetProductGradeCatalogs(criteria);
            var existProductGrades = GetProductGrades(itemId, criteria.PagingInfo, (List<OrderInfo>)criteria.OrderInfoList);
            var existGradeCodes = existProductGrades.Select(x => x.Code);
            foreach (var curCatalog in catalogs)
            {
                if (!existGradeCodes.Contains(curCatalog.Code))
                {
                    var curProductGrade = ConvertCatalogToProductGrade(curCatalog);
                    productGrades.Add(curProductGrade);
                }
            }
            productGrades.SetTotalCount(productGrades.Count);
            return productGrades;
        }

        /// <summary>
        /// 根据产品等级ids获取产品等级列表
        /// </summary>
        /// <param name="ids">产品等级ids</param>
        /// <returns>产品等级列表</returns>
        public virtual EntityList<ProductGrade> GetProductGrades(List<double> ids)
        {
            return Query<ProductGrade>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取同一个物料、同一个产品等级编码的个数
        /// </summary>
        /// <param name="itemId">物料编码</param>
        /// <param name="code">产品等级编码</param>
        /// <returns>返回个数</returns>
        public virtual int CountProductGrade(double itemId, string code)
        {
            var qryCount = Query<ProductGrade>().Where(p => p.ItemId == itemId && p.Code == code).Count();
            return qryCount;
        }

        /// <summary>
        /// 给指定物料添加产品等级信息
        /// </summary>
        /// <param name="selectedItems">选择的产品等级信息</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>指定物料的产品等级信息</returns>
        public virtual EntityList<ProductGrade> AddRangeProductGrade(List<ProductGrade> selectedItems, double itemId, PagingInfo pagingInfo = null)
        {
            var productGrades = new EntityList<ProductGrade>();
            productGrades.AddRange(selectedItems);
            productGrades.ForEach(p => p.ItemId = itemId);
            RF.Save(productGrades);

            var list = GetProductGrades(itemId, pagingInfo);
            return list;
        }

        /// <summary>
        /// 删除指定物料选中的产品等级
        /// </summary>
        /// <param name="selectedIds">选中的产品等级ids</param>
        public virtual void RemoveProductGrade(List<double> selectedIds)
        {
            var productGrades = GetProductGrades(selectedIds);
            RemoveProductGrade(productGrades);
        }

        /// <summary>
        /// 删除指定物料选中的产品等级
        /// </summary>
        /// <param name="selectedItems">选中的产品等级</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>指定物料的产品等级信息</returns>
        public virtual EntityList<ProductGrade> RemoveProductGrade(IEnumerable<ProductGrade> selectedItems, PagingInfo pagingInfo = null)
        {
            var removeProductGrades = new EntityList<ProductGrade>();
            var itemId = selectedItems.Select(p => p.ItemId).FirstOrDefault();

            foreach (var curProductGrade in selectedItems)
            {
                curProductGrade.PersistenceStatus = PersistenceStatus.Deleted;
                removeProductGrades.Add(curProductGrade);
            }

            RF.Save(removeProductGrades);

            var list = GetProductGrades(itemId, pagingInfo);
            return list;
        }

        /// <summary>
        /// 把Catalog实体转换为ProductGrade实体
        /// </summary>
        /// <param name="curCatalog">Catalog实体</param>
        /// <returns>ProductGrade实体</returns>
        private ProductGrade ConvertCatalogToProductGrade(Catalog curCatalog)
        {
            var curProductGrade = new ProductGrade();
            curProductGrade.Code = curCatalog.Code;
            curProductGrade.Name = curCatalog.Name;
            curProductGrade.Describe = curCatalog.Description;
            curProductGrade.CreateBy = curCatalog.CreateBy;
            curProductGrade.CreateDate = curCatalog.CreateDate;
            curProductGrade.UpdateBy = curCatalog.UpdateBy;
            curProductGrade.UpdateDate = curCatalog.UpdateDate;
            return curProductGrade;
        }
        #endregion

        /// <summary>
        /// 根据物料id获取打印设置
        /// </summary>
        /// <param name="itemId">物料id</param>
        /// <returns>LabelPrintTemplate</returns>
        public virtual LabelPrintTemplate GetTemplateByItemId(double itemId)
        {
            return Query<LabelPrintTemplate>().Join<Item>((x, y) => x.Id == y.TemplateId).Where<Item>((x, y) => y.Id == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料id获取打印设置
        /// </summary>
        /// <param name="labelPrintTemplateIds">打印模板设置id列表</param>
        /// <returns>LabelPrintTemplate 列表</returns>
        public virtual EntityList<LabelPrintTemplate> GetTemplates(List<double> labelPrintTemplateIds)
        {
            return labelPrintTemplateIds.SplitContains(tempIds =>
            {
                return Query<LabelPrintTemplate>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// <para>获取物料表中的生产提前期数据（为了MRP生产提前期计算）</para>
        /// <para>创建人：丁度然</para>
        /// </summary>
        /// <returns>物料生产提前期</returns>
        public virtual List<ItemProductLeadDay> GetItemProductLeadDay()
        {
            return Query<Item>().Select(x => new
            {
                ItemId = x.Id,
                Code = x.Code,
                Name = x.Name,
                ItemSourceType = x.ItemSourceType,
                ProductModelId = x.ModelId,
                Type = x.Type,
                ProductLeadDay = x.ProductLeadDay,
                PurchaseLeadTime = x.PurchaseLeadTime,
                IsMarked = x.IsMarked
            }).ToList<ItemProductLeadDay>().ToList();
        }

        /// <summary>
        /// 获取打印模板By编码规则
        /// </summary>
        /// <param name="ruleId">编码规则</param>
        /// <param name="templateIds">模板ID集合</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>打印模板</returns>
        public virtual EntityList<PrintTemplate> GetRulePrintTemplates(double ruleId, List<double> templateIds, PagingInfo pagingInfo)
        {
            //添加过滤只能选启用的打印模板
            var query = Query<PrintTemplate>().Join<NumberRuleInTemplate>((x, y) => x.Id == y.TemplateId && y.RuleId == ruleId)
                .Where(x => x.State == State.Enable);

            if (templateIds.Any())
            {
                var exp = templateIds.CreateContainsExpression<PrintTemplate>("x", nameof(PrintTemplate.Id));
                if (exp != null)
                {
                    query.Where(exp);
                }
            }

            return query.ToList(pagingInfo);

        }

        /// <summary>
        /// 通过Id列表获取物料名称列表（简易对象）
        /// </summary>
        /// <param name="ids">物料Id列表</param>
        /// <returns>生产订单列表</returns>
        public virtual List<ItemInfo> GetSimpleItemNameList(List<double> ids)
        {
            return DataProcessEx.SplitContains(ids, (tmpIds) =>
            {
                return Query<Item>().Where(p => tmpIds.Contains(p.Id)).Select(p => new
                {
                    ItemId = p.Id,
                    ItemName = p.Name
                }).ToList<ItemInfo>().ToList();
            });
        }


        #region 导入
        /// <summary>
        /// 根据物料编码获取物料属性定义
        /// </summary>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public virtual EntityList<ItemPropertyValue> GetItemPropertyValueList(List<string> itemCodes)
        {
            var itemProValue = itemCodes.SplitContains(tempCodes =>
            {
                return Query<ItemPropertyValue>().Join<Item>((x, y) => y.Id == x.ItemId && tempCodes.Contains(y.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return itemProValue;
        }

        /// <summary>
        /// 根据ids获取物料属性
        /// </summary>
        /// <param name="defineIds"></param>
        /// <returns></returns>
        public virtual EntityList<ItemPropertyDefinition> GetItemPropertyDefinitionList(List<double> defineIds)
        {
            var definitionList = defineIds.SplitContains(tempIds =>
            {
                return Query<ItemPropertyDefinition>().Where(p => defineIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return definitionList;
        }

        /// <summary>
        /// 根据快码组ids获取快码明细
        /// </summary>
        /// <param name="cataTypeIds"></param>
        /// <returns></returns>
        public virtual EntityList<Catalog> GetCatalogList(List<double?> cataTypeIds)
        {
            var catalogList = cataTypeIds.SplitContains(tempIds =>
            {
                return Query<Catalog>().Where(p => cataTypeIds.Contains(p.CatalogTypeId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return catalogList;
        }


        #endregion

        /// <summary>
        /// 根据物料Id获取单位
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string GetUnitNameGetItemId(double Id)
        {
            var item = RF.GetById<Item>(Id, new EagerLoadOptions().LoadWithViewProperty());
            return item.UnitName;
        }
    }

    /// <summary>
    /// 待验证物料
    /// </summary>
    [Serializable]
    public class ToValidateItemInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 是否验证属性
        /// </summary>
        public bool ValidateProperty { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public List<PropertyValueInfo> ValueList { get; set; } = new List<PropertyValueInfo>();
    }

    /// <summary>
    /// 目标物料（要求匹配物料）
    /// </summary>
    [Serializable]
    public class TagetItemInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public List<PropertyValueInfo> ValueList { get; set; } = new List<PropertyValueInfo>();

        /// <summary>
        /// 替代料
        /// </summary>
        public List<AlternativeItemInfo> AlternativeList { get; set; } = new List<AlternativeItemInfo>();
    }

    /// <summary>
    /// 替代料信息
    /// </summary>
    [Serializable]
    public class AlternativeItemInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public List<PropertyValueInfo> ValueList { get; set; } = new List<PropertyValueInfo>();
    }

    /// <summary>
    /// 属性值信息
    /// </summary>
    [Serializable]
    public class PropertyValueInfo
    {
        /// <summary>
        /// 属性定义ID
        /// </summary>
        public double DefinitionId { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}