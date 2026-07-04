using SIE.Domain;
using SIE.Items.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 质量分类
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(QualityCategoryCriteria))]
    [DisplayMember(nameof(Code))]
    [Label("质量分类")]
    public partial class QualityCategory : ItemCategory
    {
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class QualityCategoryConfig : EntityConfig<QualityCategory>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            var itemCateogry = RF.Find<ItemCategory>().EntityMeta;
            var level = RF.Find<ItemCategoryLevel>().EntityMeta;
            var levelId = itemCateogry.Property(ItemCategory.LevelIdProperty).ColumnMeta.ColumnName;
            var id = level.Property(ItemCategoryLevel.IdProperty).ColumnMeta.ColumnName;
            string view = string.Empty;
            if (!Data.DbMigration.DbMigration.IsRuning)
            {
                //查找类型为质量的分类
                view = "(select q.* from " + itemCateogry.TableMeta.TableName + " q join " + level.TableMeta.TableName + " l on l." + id + "=q." + levelId + " and q.type_ = " + "'" + (int)CategoryType.Quality + "'" + ")";
            }

            Meta.MapView(view).MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
            Meta.IsTreeEntity = false;
        }
    }
}