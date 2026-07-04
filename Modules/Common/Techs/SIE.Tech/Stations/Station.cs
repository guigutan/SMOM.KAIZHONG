using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations.Configs;
using System;

namespace SIE.Tech.Stations
{
    /// <summary>
    /// 工位
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工位")]
    [EntityWithConfig(typeof(DirectWeightConfig))]
    [EntityWithConfig(typeof(DirectWipPackingConfig))]
    [EntityWithConfig(typeof(DirectWipPackingBillConfig))]
    [EntityWithConfig(typeof(DirectPackingPrintModeConfig))]
    [EntityWithConfig(typeof(NewPackingPrintModeConfig))]
    [DisplayMember(nameof(Station.Name))]
    public partial class Station : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Station>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Station>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("所属资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<Station>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<Station>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工位工序列表 StationProcessList
        /// <summary>
        /// 工位工序列表
        /// </summary>
        public static readonly ListProperty<EntityList<StationProcess>> StationProcessListProperty = P<Station>.RegisterList(e => e.StationProcessList);

        /// <summary>
        /// 工位工序列表
        /// </summary>
        public EntityList<StationProcess> StationProcessList
        {
            get { return this.GetLazyList(StationProcessListProperty); }
        }
        #endregion

        #region 工位设备列表 StationEquipmentList
        /// <summary>
        /// 工位设备列表
        /// </summary>
        public static readonly ListProperty<EntityList<StationEquipment>> StationEquipmentListProperty = P<Station>.RegisterList(e => e.StationEquipmentList);

        /// <summary>
        /// 工位设备列表
        /// </summary>
        public EntityList<StationEquipment> StationEquipmentList
        {
            get { return this.GetLazyList(StationEquipmentListProperty); }
        }
        #endregion
        
        #region 来源资源Id SourceWipResourceId
        /// <summary>
        /// 来源资源Id
        /// </summary>
        [Label("来源资源Id")]
        public static readonly Property<double?> SourceWipResourceIdProperty = P<Station>.Register(e => e.SourceWipResourceId);

        /// <summary>
        /// 来源资源Id
        /// </summary>
        public double? SourceWipResourceId
        {
            get { return this.GetProperty(SourceWipResourceIdProperty); }
            set { this.SetProperty(SourceWipResourceIdProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<Station>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 是否是导入数据 IsImportData
        /// <summary>
        /// 是否是导入数据
        /// </summary>
        [Label("是否是导入数据")]
        public static readonly Property<bool?> IsImportDataProperty = P<Station>.Register(e => e.IsImportData);

        /// <summary>
        /// 是否是导入数据
        /// </summary>
        public bool? IsImportData
        {
            get { return this.GetProperty(IsImportDataProperty); }
            set { this.SetProperty(IsImportDataProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工位 实体配置
    /// </summary>
    internal class StationConfig : EntityConfig<Station>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_STATION").MapAllProperties();
            Meta.Property(Station.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(Station.IsImportDataProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}