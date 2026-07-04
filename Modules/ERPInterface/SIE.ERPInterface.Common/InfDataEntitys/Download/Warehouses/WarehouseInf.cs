using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 仓库中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("仓库中间表")]
    public partial class WarehouseInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<WarehouseInf>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<WarehouseInf>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 分类 Category
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<string> CategoryProperty = P<WarehouseInf>.Register(e => e.Category);

        /// <summary>
        /// 分类
        /// </summary>
        public string Category
        {
            get { return GetProperty(CategoryProperty); }
            set { SetProperty(CategoryProperty, value); }
        }
        #endregion

        #region 简码 SimpleCode
        /// <summary>
        /// 简码
        /// </summary>
        [Label("简码")]
        public static readonly Property<string> SimpleCodeProperty = P<WarehouseInf>.Register(e => e.SimpleCode);

        /// <summary>
        /// 简码
        /// </summary>
        public string SimpleCode
        {
            get { return GetProperty(SimpleCodeProperty); }
            set { SetProperty(SimpleCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 仓库中间表 实体配置
    /// </summary>
    internal class WarehouseInfConfig : EntityConfig<WarehouseInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_WH").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}