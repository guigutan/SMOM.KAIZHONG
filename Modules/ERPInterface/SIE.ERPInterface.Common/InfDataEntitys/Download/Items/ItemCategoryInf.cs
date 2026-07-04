using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 分类中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("分类中间表")]
    public partial class ItemCategoryInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ItemCategoryInf>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<ItemCategoryInf>.Register(e => e.Name);

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
        /// 物料分类层级
        /// </summary>
        [Label("物料分类层级")]
        public static readonly Property<string> LevelProperty = P<ItemCategoryInf>.Register(e => e.Level);

        /// <summary>
        /// 物料分类层级
        /// </summary>
        public string Level
        {
            get { return GetProperty(LevelProperty); }
            set { SetProperty(LevelProperty, value); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<int> ItemTypeProperty = P<ItemCategoryInf>.Register(e => e.ItemType);

        /// <summary>
        /// 物料类型
        /// </summary>
        public int ItemType
        {
            get { return GetProperty(ItemTypeProperty); }
            set { SetProperty(ItemTypeProperty, value); }
        }
        #endregion

        #region 父编码 ParentCode
        /// <summary>
        /// 父编码
        /// </summary>
        [Label("父编码")]
        public static readonly Property<string> ParentCodeProperty = P<ItemCategoryInf>.Register(e => e.ParentCode);

        /// <summary>
        /// 父编码
        /// </summary>
        public string ParentCode
        {
            get { return this.GetProperty(ParentCodeProperty); }
            set { this.SetProperty(ParentCodeProperty, value); }
        }
        #endregion

        #region 层次 CategoryLevelNum
        /// <summary>
        /// 层次
        /// </summary>
        [Label("层次")]
        public static readonly Property<int> CategoryLevelNumProperty = P<ItemCategoryInf>.Register(e => e.CategoryLevelNum);

        /// <summary>
        /// 层次
        /// </summary>
        public int CategoryLevelNum
        {
            get { return this.GetProperty(CategoryLevelNumProperty); }
            set { this.SetProperty(CategoryLevelNumProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 分类中间表 实体配置
    /// </summary>
    internal class ItemCategoryInfConfig : EntityConfig<ItemCategoryInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_ITEM_CATE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}