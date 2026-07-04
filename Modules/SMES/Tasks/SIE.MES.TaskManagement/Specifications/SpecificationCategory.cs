using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Specifications
{
    /// <summary>
	/// 规格件分类
	/// </summary>
	[RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(Name))]
    [Label("规格件分类")]
    public partial class SpecificationCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<SpecificationCategory>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<SpecificationCategory>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 规格件分类 实体配置
    /// </summary>
    internal class SpecificationCategoryConfig : EntityConfig<SpecificationCategory>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_SPEC_CATEGORY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
