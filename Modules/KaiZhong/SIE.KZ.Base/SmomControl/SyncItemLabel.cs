using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.SmomControl
{
    /// <summary>
    /// 跨组织物料标签同步
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("跨组织物料标签同步")]
    public class SyncItemLabel : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<SyncItemLabel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<SyncItemLabel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 来源工厂 SourceFactory
        /// <summary>
        /// 来源工厂
        /// </summary>
        [Label("来源工厂")]
        public static readonly Property<string> SourceFactoryProperty = P<SyncItemLabel>.Register(e => e.SourceFactory);

        /// <summary>
        /// 来源工厂
        /// </summary>
        public string SourceFactory
        {
            get { return this.GetProperty(SourceFactoryProperty); }
            set { this.SetProperty(SourceFactoryProperty, value); }
        }
        #endregion

        #region 目标工厂 ToFactory
        /// <summary>
        /// 目标工厂
        /// </summary>
        [Label("目标工厂")]
        public static readonly Property<string> ToFactoryProperty = P<SyncItemLabel>.Register(e => e.ToFactory);

        /// <summary>
        /// 目标工厂
        /// </summary>
        public string ToFactory
        {
            get { return this.GetProperty(ToFactoryProperty); }
            set { this.SetProperty(ToFactoryProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SyncItemLabel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<SyncItemLabel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #endregion
    }

    internal class SyncItemLabelConfig : EntityConfig<SyncItemLabel>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(SyncItemLabel.SourceFactoryProperty, new RequiredRule());
            rules.AddRule(SyncItemLabel.ToFactoryProperty, new RequiredRule());
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                SyncItemLabel.ItemIdProperty,
                SyncItemLabel.SourceFactoryProperty,
                SyncItemLabel.ToFactoryProperty
                },
                MessageBuilder = (e) =>
                {
                    return "存在相同物料、相同来源工厂、相同目标工厂的数据".L10N();
                }
            });

        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("SYNC_ITEM_LABEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
