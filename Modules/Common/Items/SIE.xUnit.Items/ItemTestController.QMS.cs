using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Common;

namespace SIE.xUnit.Items
{
    /// <summary>
    /// 测试物料数据控制器  QMS
    /// </summary>
    public partial class ItemTestController : DomainController
    {
        /// <summary>
        /// 分类层级
        /// </summary>
        private readonly List<string> levelCodeList = new List<string> { "质量大类", "质量中类", "质量小类" };

        /// <summary>
        /// 质量分类
        /// </summary>
        private readonly List<string> codeList = new List<string>() { "质量分类1", "质量分类1-1", "质量分类1-1-1", "质量分类1-1-2" };

        #region 固定数据(存在则获取，没有则创建)
        /// <summary>
        /// 获取所有质量分类层级
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ItemCategoryLevel> GetItemCategoryLevelQualityList()
        {

            var levelList = Query<ItemCategoryLevel>().Where(p => levelCodeList.Contains(p.Code)).ToList();

            if (levelList?.Count == 3)
                return levelList;
            if (levelList != null && levelList.Count > 0)   //1到2个之间，删除。重新创建
            {
                foreach (var level in levelList)
                    level.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(levelList);
            }

            //一级  
            var first = CreateItemCategoryLevelQms(levelCodeList[0], levelCodeList[0]);
            levelList.Add(first);
            //二级
            var second = CreateItemCategoryLevelQms(levelCodeList[1], levelCodeList[1]);
            second.TreePId = first.Id;
            levelList.Add(second);
            //三级
            var third = CreateItemCategoryLevelQms(levelCodeList[2], levelCodeList[2]);
            third.TreePId = second.Id;
            levelList.Add(third);
            RF.Save(levelList);
            return levelList;
        }

        /// <summary>
        /// 取分类层级
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ItemCategoryLevel CreateItemCategoryLevelQms(string code, string name)
        {
            ItemCategoryLevel level = new ItemCategoryLevel()
            {
                Name = name,
                Code = code,
                Type = CategoryType.Quality
            };
            level.GenerateId();
            return level;
        }

        /// <summary>
        /// 获取质量分类
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ItemCategory> GetItemItemCategoryListQuality(ItemType itemType = ItemType.Material)
        {

            var categoryList = Query<ItemCategory>().Where(p => codeList.Contains(p.Code)).ToList();

            if (categoryList?.Count == codeList.Count)
                return categoryList;
            else if (categoryList != null && categoryList.Count > 0)   //删除。重新创建
            {
                foreach (var level in categoryList)
                    level.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(categoryList);
            }

            //1级
            var first = CreateItemCategoryQuality(1, codeList[0], codeList[0], itemType);
            categoryList.Add(first);
            //2级
            var second = CreateItemCategoryQuality(2, codeList[1], codeList[1], itemType);
            second.TreePId = first.Id;
            categoryList.Add(second);
            //3级
            var third1 = CreateItemCategoryQuality(3, codeList[2], codeList[2], itemType);
            third1.TreePId = second.Id;
            categoryList.Add(third1);
            var third2 = CreateItemCategoryQuality(3, codeList[3], codeList[3], itemType);
            third2.TreePId = second.Id;
            categoryList.Add(third2);
            RF.Save(categoryList);
            return categoryList;
        }

        /// <summary>
        /// 获取质量分类(叶子结点)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ItemCategory> GetItemItemCategoryLeafListQuality(ItemType itemType = ItemType.Material)
        {
            var list = GetItemItemCategoryListQuality(itemType);
            var leafCodes = new List<string>() { codeList[2], codeList[3] };
            return list.Where(p => leafCodes.Contains(p.Code)).AsEntityList();
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <returns></returns>
        public virtual ItemCategory CreateItemCategoryQuality(int level, string code, string name, ItemType itemType = ItemType.Material)
        {
            var levelList = GetItemCategoryLevelQualityList();
            var itemCategory = new ItemCategory();
            itemCategory.GenerateId();
            var id = itemCategory.Id;
            itemCategory.Name = code ?? $"ItemCategoryName{id}";
            itemCategory.Code = name ?? $"ItemCategoryCode{id}";
            itemCategory.Level = levelList.FirstOrDefault(p => p.Name == levelCodeList[level - 1]);
            itemCategory.ItemType = itemType;
            itemCategory.Type = CategoryType.Quality;
            return itemCategory;
        }
        #endregion

        /// <summary>
        /// 创建质量分类，关联物料
        /// </summary>
        /// <returns></returns>
        public virtual ItemCategory CreateItemCategoryWithRelationQuality(double itemId)
        {
            return CreateItemCategoryWithRelation(itemId, CategoryType.Quality);
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <returns></returns>
        public virtual ItemCategory CreateItemCategoryWithRelation(double itemId, CategoryType categoryType)
        {
            var categoryList = GetItemItemCategoryLeafListQuality();
            int index = new Random().Next(categoryList.Count);
            var category = categoryList[index];
            var qRelation = RT.Service.Resolve<ItemController>().GetItemCategoryRelation(itemId, null).FirstOrDefault(p => p.Type == categoryType);
            if (qRelation != null)
                return qRelation.ItemCategory;

            using (var tran = DB.TransactionScope(ItemEntityDataTestProvider.ConnectionStringName))
            {
                var itemCategory = category;
                ItemCategoryRelation relation = new ItemCategoryRelation();
                relation.ItemId = itemId;
                relation.ItemCategoryId = itemCategory.Id;
                relation.Type = categoryType;
                RF.Save(relation);
                tran.Complete();
                return itemCategory;
            }
        }

        /// <summary>
        /// 创建质量分类
        /// </summary>
        /// <returns>返回质量分类</returns>
        public virtual QualityCategory CreateQualityCategory()
        {
            var qualityCategory = new QualityCategory();
            qualityCategory.GenerateId();
            double id = qualityCategory.Id;
            qualityCategory.Code = $"CategoryCode{id}";
            qualityCategory.Name = $"CategoryName{id}";
            var levelList = GetItemCategoryLevelQualityList();
            qualityCategory.Level = levelList.FirstOrDefault();
            qualityCategory.ItemType = ItemType.Material;
            qualityCategory.Type = CategoryType.Quality;
            RF.Save(qualityCategory);
            return qualityCategory;
        }
    }
}
