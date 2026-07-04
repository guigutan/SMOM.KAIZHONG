using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Packages;
using System;

namespace SIE.MES.WIP.PackRecombine.Logs
{
    /// <summary>
    /// 包装管理操作日志
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(RecombineLogCriteria))]    
    [Label("包装管理操作日志")]
    public partial class RecombineLog : DataEntity
    {
        #region 包装号 PackageNo
        /// <summary>
        /// 包装号
        /// </summary>
        [Label("包装号")]
        public static readonly Property<string> PackageNoProperty = P<RecombineLog>.Register(e => e.PackageNo);

        /// <summary>
        /// 包装号
        /// </summary>
        public string PackageNo
        {
            get { return GetProperty(PackageNoProperty); }
            set { SetProperty(PackageNoProperty, value); }
        }
        #endregion

        #region 外层包装号 ParentNo
        /// <summary>
        /// 外层包装号
        /// </summary>
        [Label("外层包装号")]
        public static readonly Property<string> ParentNoProperty = P<RecombineLog>.Register(e => e.ParentNo);

        /// <summary>
        /// 外层包装号
        /// </summary>
        public string ParentNo
        {
            get { return GetProperty(ParentNoProperty); }
            set { SetProperty(ParentNoProperty, value); }
        }
        #endregion

        #region 操作类型 ScanMode
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<ScanMode> ScanModeProperty = P<RecombineLog>.Register(e => e.ScanMode);

        /// <summary>
        /// 操作类型
        /// </summary>
        public ScanMode ScanMode
        {
            get { return GetProperty(ScanModeProperty); }
            set { SetProperty(ScanModeProperty, value); }
        }
        #endregion

        #region 包装单位 PackageUnit
        /// <summary>
        /// 包装单位IDProperty
        /// </summary>
        [Label("包装单位")]
        public static readonly IRefIdProperty PackageUnitIdProperty = P<RecombineLog>.RegisterRefId(e => e.PackageUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId
        {
            get { return (double)this.GetRefId(PackageUnitIdProperty); }
            set { this.SetRefId(PackageUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位Property
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackageUnitProperty = P<RecombineLog>.RegisterRef(e => e.PackageUnit, PackageUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackageUnit
        {
            get { return this.GetRefEntity(PackageUnitProperty); }
            set { this.SetRefEntity(PackageUnitProperty, value); }
        }
        #endregion

        #region 外层包装单位 ParentUnit
        /// <summary>
        /// 外层包装单位IDProperty
        /// </summary>
        [Label("外层包装单位")]
        public static readonly IRefIdProperty ParentUnitIdProperty = P<RecombineLog>.RegisterRefId(e => e.ParentUnitId, ReferenceType.Normal);

        /// <summary>
        /// 外层包装单位ID
        /// </summary>
        public double ParentUnitId
        {
            get { return (double)this.GetRefId(ParentUnitIdProperty); }
            set { this.SetRefId(ParentUnitIdProperty, value); }
        }

        /// <summary>
        /// 外层包装单位Property
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> ParentUnitProperty = P<RecombineLog>.RegisterRef(e => e.ParentUnit, ParentUnitIdProperty);

        /// <summary>
        /// 外层包装单位
        /// </summary>
        public PackingUnit ParentUnit
        {
            get { return this.GetRefEntity(ParentUnitProperty); }
            set { this.SetRefEntity(ParentUnitProperty, value); }
        }
        #endregion

        #region 是否批次 IsBatch
        /// <summary>
        /// 是否批次
        /// </summary>
        [Label("是否批次")]
        public static readonly Property<bool> IsBatchProperty = P<RecombineLog>.Register(e => e.IsBatch);

        /// <summary>
        /// 是否批次
        /// </summary>
        public bool IsBatch
        {
            get { return GetProperty(IsBatchProperty); }
            set { SetProperty(IsBatchProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 包装单位 PackageUnitName
        /// <summary>
        /// 包装单位
        /// </summary>
        [Label("包装单位")]
        public static readonly Property<string> PackageUnitNameProperty = P<RecombineLog>.RegisterView(e => e.PackageUnitName, p => p.PackageUnit.Name);

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnitName
        {
            get { return this.GetProperty(PackageUnitNameProperty); }
        }
        #endregion

        #region 外层包装单位 ParentUnitName
        /// <summary>
        /// 外层包装单位
        /// </summary>
        [Label("外层包装单位")]
        public static readonly Property<string> ParentUnitNameProperty = P<RecombineLog>.RegisterView(e => e.ParentUnitName, p => p.ParentUnit.Name);

        /// <summary>
        /// 外层包装单位
        /// </summary>
        public string ParentUnitName
        {
            get { return this.GetProperty(ParentUnitNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 包装管理操作日志 实体配置
    /// </summary>
    internal class RecombineLogEntityConfig : EntityConfig<RecombineLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PACK_REC_LOG").MapAllProperties();
            Meta.DisableDataSync();
            Meta.DisablePhantoms();
        }
    }
}
