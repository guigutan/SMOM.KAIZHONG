using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.ItemChecker;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIE.MES.BlueLable
{
    /// <summary>
    /// 蓝标层级
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BlueLableLevelCriterial))]
    [Label("蓝标层级")]
    public class BlueLableLevel : DataEntity
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<BlueLableLevel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<BlueLableLevel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 层级 LevelName
        /// <summary>
        /// 层级
        /// </summary>
        [Label("层级")]
        public static readonly Property<string> LevelNameProperty = P<BlueLableLevel>.Register(e => e.LevelName);

        /// <summary>
        /// 层级
        /// </summary>
        public string LevelName
        {
            get { return this.GetProperty(LevelNameProperty); }
            set { this.SetProperty(LevelNameProperty, value); }
        }
        #endregion

        #region 数量计算方式 CalcMethod
        /// <summary>
        /// 数量计算方式
        /// </summary>
        [Label("数量计算方式")]
        public static readonly Property<CalcMethodEnum?> CalcMethodProperty = P<BlueLableLevel>.Register(e => e.CalcMethod);

        /// <summary>
        /// 数量计算方式
        /// </summary>
        public CalcMethodEnum? CalcMethod
        {
            get { return this.GetProperty(CalcMethodProperty); }
            set { this.SetProperty(CalcMethodProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<BlueLableLevel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BlueLableLevel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }


        #endregion
        #endregion
    }

    /// <summary>
    /// 装箱标识
    /// </summary>
    public enum CalcMethodEnum
    {
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        BatchLable = 0,

        /// <summary>
        /// SN标签
        /// </summary>
        [Label("SN标签")]
        SnLabel = 1,
    }
    /// <summary>
    /// 蓝标层级 实体配置
    /// </summary>
    internal class BlueLablleLevelConfig : EntityConfig<BlueLableLevel>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    BlueLableLevel.ItemIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BLUEL_LABLE_LEVEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 蓝标级别验证
    /// </summary>
    [System.ComponentModel.DisplayName("蓝标级别验证")]
    [System.ComponentModel.Description("只允许输入数字和逗号正则")]
    class BlueLableLevelRule : EntityRule<BlueLableLevel>
    {
        public BlueLableLevelRule()
        {
            Property = BlueLableLevel.LevelNameProperty;
            ConnectToDataSource = false;
        }
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var blueLable = entity as BlueLableLevel;
            if (!blueLable.LevelName.IsNullOrWhiteSpace())
            {
                const string levelName = @"^[0-9]+(,[0-9]+)*$";
                Regex phoneRegex = new Regex(levelName);
                var matches = phoneRegex.IsMatch(blueLable.LevelName);
                if (!matches)
                {
                    e.BrokenDescription = "蓝标层级格式不正确!".L10nFormat();
                }
            }
        }
    }
}
