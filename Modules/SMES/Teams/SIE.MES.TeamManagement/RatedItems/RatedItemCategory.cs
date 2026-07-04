using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.RatedItems
{
    /// <summary>
    /// 评分项目分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("评分项目分类")]
    [DisplayMember(nameof(Name))]
    public class RatedItemCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [NotDuplicate]
        [Required]
        public static readonly Property<string> CodeProperty = P<RatedItemCategory>.Register(e => e.Code);

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
        [NotDuplicate]
        [Required]
        public static readonly Property<string> NameProperty = P<RatedItemCategory>.Register(e => e.Name);

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
        public static readonly Property<string> RemarkProperty = P<RatedItemCategory>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 评分项目分类 实体配置
    /// </summary>
    internal class RatedItemCategoryConfig : EntityConfig<RatedItemCategory>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WG_RATED_ITEM_CATE").MapAllProperties();
            Meta.Property(RatedItemCategory.RemarkProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }
    }
}
