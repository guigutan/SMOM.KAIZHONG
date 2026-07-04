using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 质量分类查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("质量分类查询")]
    public partial class QualityCategoryCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<QualityCategoryCriteria>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<QualityCategoryCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 物料分类层级 Level
        /// <summary>
        /// 物料分类层级Id
        /// </summary>
        [Label("物料分类层级")]
        public static readonly IRefIdProperty LevelIdProperty = P<QualityCategoryCriteria>.RegisterRefId(e => e.LevelId, ReferenceType.Normal);

        /// <summary>
        /// 物料分类层级Id
        /// </summary>
        public double? LevelId
        {
            get { return (double?)GetRefNullableId(LevelIdProperty); }
            set { SetRefNullableId(LevelIdProperty, value); }
        }

        /// <summary>
        /// 物料分类层级
        /// </summary>
        public static readonly RefEntityProperty<ItemCategoryLevel> LevelProperty = P<QualityCategoryCriteria>.RegisterRef(e => e.Level, LevelIdProperty);

        /// <summary>
        /// 物料分类层级
        /// </summary>
        public ItemCategoryLevel Level
        {
            get { return GetRefEntity(LevelProperty); }
            set { SetRefEntity(LevelProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>EntityList</returns>
        protected override EntityList Fetch()
        {
            var ctl = RT.Service.Resolve<ItemController>();
            return ctl.GetItemCategorys(this);
        }
    }
}
