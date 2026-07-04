using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions
{
    /// <summary>
    /// 返工工艺路线版本
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("返工工艺路线版本")]
    [DisplayMember(nameof(Version))]
    public class ReworkLayoutVersion : DataEntity
    {
        #region 生产版本 Version
        /// <summary>
        /// 生产版本
        /// </summary>
        [Label("生产版本")]
        public static readonly Property<string> VersionProperty = P<ReworkLayoutVersion>.Register(e => e.Version);

        /// <summary>
        /// 生产版本
        /// </summary>
        public string Version
        {
            get { return this.GetProperty(VersionProperty); }
            set { this.SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 版本描述 Desc
        /// <summary>
        /// 版本描述
        /// </summary>
        [Label("版本描述")]
        public static readonly Property<string> DescProperty = P<ReworkLayoutVersion>.Register(e => e.Desc);

        /// <summary>
        /// 版本描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ReworkLayoutVersion>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ReworkLayoutVersion>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<ReworkLayoutVersion>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 基本开始日期 BeginDateTime
        /// <summary>
        /// 基本开始日期
        /// </summary>
        [Label("基本开始日期")]
        public static readonly Property<DateTime?> BeginDateTimeProperty = P<ReworkLayoutVersion>.Register(e => e.BeginDateTime);

        /// <summary>
        /// 基本开始日期
        /// </summary>
        public DateTime? BeginDateTime
        {
            get { return this.GetProperty(BeginDateTimeProperty); }
            set { this.SetProperty(BeginDateTimeProperty, value); }
        }
        #endregion

        #region 基本完成日期 EndDateTime
        /// <summary>
        /// 基本完成日期
        /// </summary>
        [Label("基本完成日期")]
        public static readonly Property<DateTime?> EndDateTimeProperty = P<ReworkLayoutVersion>.Register(e => e.EndDateTime);

        /// <summary>
        /// 基本完成日期
        /// </summary>
        public DateTime? EndDateTime
        {
            get { return this.GetProperty(EndDateTimeProperty); }
            set { this.SetProperty(EndDateTimeProperty, value); }
        }
        #endregion

        #region 任务清单类型 Type
        /// <summary>
        /// 任务清单类型
        /// </summary>
        [Label("任务清单类型")]
        public static readonly Property<string> TypeProperty = P<ReworkLayoutVersion>.Register(e => e.Type);

        /// <summary>
        /// 任务清单类型
        /// </summary>
        public string Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 有效开始日期 EffBeginDateTime
        /// <summary>
        /// 有效开始日期
        /// </summary>
        [Label("有效开始日期")]
        public static readonly Property<DateTime?> EffBeginDateTimeProperty = P<ReworkLayoutVersion>.Register(e => e.EffBeginDateTime);

        /// <summary>
        /// 有效开始日期
        /// </summary>
        public DateTime? EffBeginDateTime
        {
            get { return this.GetProperty(EffBeginDateTimeProperty); }
            set { this.SetProperty(EffBeginDateTimeProperty, value); }
        }
        #endregion

        #region 有效截止日期 EffEndDateTime
        /// <summary>
        /// 有效截止日期
        /// </summary>
        [Label("有效截止日期")]
        public static readonly Property<DateTime?> EffEndDateTimeProperty = P<ReworkLayoutVersion>.Register(e => e.EffEndDateTime);

        /// <summary>
        /// 有效截止日期
        /// </summary>
        public DateTime? EffEndDateTime
        {
            get { return this.GetProperty(EffEndDateTimeProperty); }
            set { this.SetProperty(EffEndDateTimeProperty, value); }
        }
        #endregion

        #region 任务清单组 Group
        /// <summary>
        /// 任务清单组
        /// </summary>
        [Label("任务清单组")]
        public static readonly Property<string> GroupProperty = P<ReworkLayoutVersion>.Register(e => e.Group);

        /// <summary>
        /// 任务清单组
        /// </summary>
        public string Group
        {
            get { return this.GetProperty(GroupProperty); }
            set { this.SetProperty(GroupProperty, value); }
        }
        #endregion

        #region 组计数器 Counter
        /// <summary>
        /// 组计数器
        /// </summary>
        [Label("组计数器")]
        public static readonly Property<string> CounterProperty = P<ReworkLayoutVersion>.Register(e => e.Counter);

        /// <summary>
        /// 组计数器
        /// </summary>
        public string Counter
        {
            get { return this.GetProperty(CounterProperty); }
            set { this.SetProperty(CounterProperty, value); }
        }
        #endregion

        #region 工艺路线明细 ReworkLayoutList
        /// <summary>
        /// 工艺路线明细
        /// </summary>
        [Label("工艺路线明细")]
        public static readonly ListProperty<EntityList<ReworkLayout>> ReworkLayoutListProperty = P<ReworkLayoutVersion>.RegisterList(e => e.ReworkLayoutList);

        /// <summary>
        /// 工艺路线明细
        /// </summary>
        public EntityList<ReworkLayout> ReworkLayoutList
        {
            get { return this.GetLazyList(ReworkLayoutListProperty); }
        }
        #endregion

        #region 视图属性

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<ReworkLayoutVersion>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<ReworkLayoutVersion>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 产品旧料号 ShortDescription
        /// <summary>
        /// 产品旧料号
        /// </summary>
        [Label("产品旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ReworkLayoutVersion>.RegisterView(e => e.ShortDescription,p=>p.Item.ShortDescription);

        /// <summary>
        /// 产品旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #endregion
    }

    internal class ReworkLayoutVersionConfig : EntityConfig<ReworkLayoutVersion>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            //rules.AddRule(ReworkLayoutVersion.VersionProperty, new NotDuplicateRule());
            //rules.AddRule(ReworkLayoutVersion.DescProperty, new NotDuplicateRule());
            //rules.AddRule(ReworkLayoutVersion.FactoryProperty, new NotDuplicateRule());
            //rules.AddRule(ReworkLayoutVersion.TypeProperty, new NotDuplicateRule());
            //rules.AddRule(ReworkLayoutVersion.GroupProperty, new NotDuplicateRule());
            //rules.AddRule(ReworkLayoutVersion.CounterProperty, new NotDuplicateRule());
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                ReworkLayoutVersion.VersionProperty,ReworkLayoutVersion.FactoryProperty,ReworkLayoutVersion.ItemIdProperty
                },
                MessageBuilder = (e) => {
                    return "存在相同生产本版,相同工厂,相同物料的数据".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("REWORK_LAYOUT_VERSION").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
