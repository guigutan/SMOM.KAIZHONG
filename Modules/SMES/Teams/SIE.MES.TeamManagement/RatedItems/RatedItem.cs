using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.RatedItems
{
    /// <summary>
    /// 评分项目
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("评分项目")]
    [DisplayMember(nameof(RatedItem.Code))]
    public class RatedItem : DataEntity, IStateEntity
    {
        #region 评分项目分类 Category
        /// <summary>
        /// 评分项目分类Id
        /// </summary>
        [Label("评分项目分类")]
        public static readonly IRefIdProperty CategoryIdProperty =
            P<RatedItem>.RegisterRefId(e => e.CategoryId, ReferenceType.Normal);

        /// <summary>
        /// 评分项目分类Id
        /// </summary>
        public double CategoryId
        {
            get { return (double)this.GetRefId(CategoryIdProperty); }
            set { this.SetRefId(CategoryIdProperty, value); }
        }

        /// <summary>
        /// 评分项目分类
        /// </summary>
        public static readonly RefEntityProperty<RatedItemCategory> CategoryProperty =
            P<RatedItem>.RegisterRef(e => e.Category, CategoryIdProperty);

        /// <summary>
        /// 评分项目分类
        /// </summary>
        public RatedItemCategory Category
        {
            get { return this.GetRefEntity(CategoryProperty); }
            set { this.SetRefEntity(CategoryProperty, value); }
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> CodeProperty = P<RatedItem>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NameProperty = P<RatedItem>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(100)]
        public static readonly Property<string> RemarkProperty = P<RatedItem>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 最低分值 MinScore
        /// <summary>
        /// 最低分值
        /// </summary>
        [Label("最低分值")]
        [Required]
        public static readonly Property<decimal> MinScoreProperty = P<RatedItem>.Register(e => e.MinScore);

        /// <summary>
        /// 最低分值
        /// </summary>
        public decimal MinScore
        {
            get { return this.GetProperty(MinScoreProperty); }
            set { this.SetProperty(MinScoreProperty, value); }
        }
        #endregion

        #region 最大分值 MaxScore
        /// <summary>
        /// 最大分值
        /// </summary>
        [Label("最大分值")]
        [Required]
        public static readonly Property<decimal> MaxScoreProperty = P<RatedItem>.Register(e => e.MaxScore);

        /// <summary>
        /// 最大分值
        /// </summary>
        public decimal MaxScore
        {
            get { return this.GetProperty(MaxScoreProperty); }
            set { this.SetProperty(MaxScoreProperty, value); }
        }
        #endregion

        #region 是否系统评分项目 IsSystem
        /// <summary>
        /// 是否系统评分项目
        /// </summary>
        [Label("是否系统评分项目")]
        public static readonly Property<bool> IsSystemProperty = P<RatedItem>.Register(e => e.IsSystem);

        /// <summary>
        /// 是否系统评分项目
        /// </summary>
        public bool IsSystem
        {
            get { return this.GetProperty(IsSystemProperty); }
            set { this.SetProperty(IsSystemProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<RatedItem>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 分类编码 CategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> CategoryCodeProperty = P<RatedItem>.RegisterView(e => e.CategoryCode, p => p.Category.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode
        {
            get { return this.GetProperty(CategoryCodeProperty); }
        }
        #endregion

        #region 分类名称 CategoryName
        /// <summary>
        /// 分类名称
        /// </summary>
        [Label("分类名称")]
        public static readonly Property<string> CategoryNameProperty = P<RatedItem>.RegisterView(e => e.CategoryName, p => p.Category.Name);

        /// <summary>
        /// 描述
        /// </summary>
        public string CategoryName
        {
            get { return this.GetProperty(CategoryNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 评分项目分类 实体配置
    /// </summary>
    internal class RatedItemConfig : EntityConfig<RatedItem>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_RATED_ITEM").MapAllProperties();
            Meta.Property(RatedItem.RemarkProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }
    }
}