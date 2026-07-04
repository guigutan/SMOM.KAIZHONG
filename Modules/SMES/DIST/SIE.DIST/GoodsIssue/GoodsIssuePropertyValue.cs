using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DIST
{
    /// <summary>
    /// 工单发料属性值
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单发料属性值")]
    [DisplayMember(nameof(GoodsIssuePropertyValue.DefinitionId))]
    public partial class GoodsIssuePropertyValue : DataEntity
    {
        #region 属性值 Value
        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<GoodsIssuePropertyValue>.Register(e => e.Value);

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region ErpId ErpId
        /// <summary>
        /// ErpId
        /// </summary>
        [MaxLength(40)]
        [Label("ErpId")]
        public static readonly Property<string> ErpIdProperty = P<GoodsIssuePropertyValue>.Register(e => e.ErpId);

        /// <summary>
        /// ErpId
        /// </summary>
        public string ErpId
        {
            get { return GetProperty(ErpIdProperty); }
            set { SetProperty(ErpIdProperty, value); }
        }
        #endregion

        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Required]
        [Label("物料属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty =
            P<GoodsIssuePropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)this.GetRefId(DefinitionIdProperty); }
            set { this.SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<GoodsIssuePropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 发料单Id GoodsIssue
        /// <summary>
        /// 发料单Id
        /// </summary>
        [Required]
        [Label("发料单")]
        public static readonly IRefIdProperty GoodsIssueIdProperty = P<GoodsIssuePropertyValue>.RegisterRefId(e => e.GoodsIssueId, ReferenceType.Parent);

        /// <summary>
        /// 发料单Id
        /// </summary>
        public double GoodsIssueId
        {
            get { return (double)GetRefId(GoodsIssueIdProperty); }
            set { SetRefId(GoodsIssueIdProperty, value); }
        }

        /// <summary>
        /// 发料属性
        /// </summary>
        public static readonly RefEntityProperty<GoodsIssue> GoodsIssueProperty = P<GoodsIssuePropertyValue>.RegisterRef(e => e.GoodsIssue, GoodsIssueIdProperty);

        /// <summary>
        /// 发料属性
        /// </summary>
        public GoodsIssue GoodsIssue
        {
            get { return GetRefEntity(GoodsIssueProperty); }
            set { SetRefEntity(GoodsIssueProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 属性名称 DefinitionName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> DefinitionNameProperty = P<GoodsIssuePropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性名称
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class GoodsIssuePropertyValueConfig : EntityConfig<GoodsIssuePropertyValue>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_GOODS_ISSUE_PROP_VAL").MapAllProperties();
            Meta.Property(GoodsIssuePropertyValue.GoodsIssueIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}