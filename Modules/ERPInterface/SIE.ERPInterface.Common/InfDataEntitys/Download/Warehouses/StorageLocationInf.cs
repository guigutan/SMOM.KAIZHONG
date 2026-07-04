using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 库位中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("")]
    public partial class StorageLocationInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<StorageLocationInf>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<StorageLocationInf>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 库区编码 AreaCode
        /// <summary>
        /// 库区编码
        /// </summary>
        [Label("库区编码")]
        public static readonly Property<string> AreaCodeProperty = P<StorageLocationInf>.Register(e => e.AreaCode);

        /// <summary>
        /// 库区编码
        /// </summary>
        public string AreaCode
        {
            get { return GetProperty(AreaCodeProperty); }
            set { SetProperty(AreaCodeProperty, value); }
        }
        #endregion

        #region 仓库编码 WhCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WhCodeProperty = P<StorageLocationInf>.Register(e => e.WhCode);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WhCode
        {
            get { return this.GetProperty(WhCodeProperty); }
            set { this.SetProperty(WhCodeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class StorageLocationInfConfig : EntityConfig<StorageLocationInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_WH_LOCATION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}