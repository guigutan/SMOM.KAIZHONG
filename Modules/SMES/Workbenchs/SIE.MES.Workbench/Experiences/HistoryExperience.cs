using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.Experiences
{
    /// <summary>
    /// 历史经验库
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("历史经验库")]
    public partial class HistoryExperience : DataEntity
    {
        #region 经验明细列表 ExperienceDetailList
        /// <summary>
        /// 经验明细列表
        /// </summary>
        [Label("经验明细")]
        public static readonly ListProperty<EntityList<ExperienceDetail>> ExperienceDetailListProperty = P<HistoryExperience>.RegisterList(e => e.ExperienceDetailList);

        /// <summary>
        /// 经验明细列表
        /// </summary>
        public EntityList<ExperienceDetail> ExperienceDetailList
        {
            get { return this.GetLazyList(ExperienceDetailListProperty); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<HistoryExperience>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<HistoryExperience>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 历史经验库 实体配置
    /// </summary>
    internal class HistoryExperienceConfig : EntityConfig<HistoryExperience>
    {
        /// <summary>
        /// 配置数据表
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WB_HIS_EXP").MapAllProperties();
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 添加自定义提示规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = { HistoryExperience.ItemIdProperty },
                MessageBuilder = (e) => { return "不能添加重复的物料"; }
            });
        }
    }
}