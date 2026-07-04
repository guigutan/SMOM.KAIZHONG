using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Stations;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线工位货区
    /// </summary>
    [RootEntity, Serializable]
    [Label("产线工位货区")]
    public partial class StationStorageArea : DataEntity
    {
        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<StationStorageArea>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty = P<StationStorageArea>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 工位货区 StorageArea
        /// <summary>
        /// 工位货区Id
        /// </summary>
        [Label("工位货区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<StationStorageArea>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Parent);

        /// <summary>
        /// 工位货区Id
        /// </summary>
        public double StorageAreaId
        {
            get { return (double)GetRefId(StorageAreaIdProperty); }
            set { SetRefId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 工位货区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<StationStorageArea>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 工位货区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工位编码 StationCode
        /// <summary>
        /// 工位编码
        /// </summary>
        [Label("工位编码")]
        public static readonly Property<string> StationCodeProperty = P<StationStorageArea>.RegisterView(e => e.StationCode, p => p.Station.Code);

        /// <summary>
        /// 工位编码
        /// </summary>
        public string StationCode
        {
            get { return this.GetProperty(StationCodeProperty); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<StationStorageArea>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产线工位货区 实体配置
    /// </summary>
    internal class StationStorageAreaConfig : EntityConfig<StationStorageArea>
    {
        /// <summary>
        /// 增加实体的数据验证
        /// </summary>
        /// <param name="rules">实体验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddServerRule((s, e) =>
            {
                var m = s as StationStorageArea;
                if (m.StorageArea != null && m.Station != null)
                {
                    if (RT.Service.Resolve<StorageController>().IsStationAreaExists(m.StationId, m.StorageArea.Type))
                        e.BrokenDescription = "工位[{0}:{1}]已存在[{2}]货区".L10nFormat(m.Station.Code, m.Station.Name, m.StorageArea.Type.ToLabel());
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add });
        }

        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_WH_AREA_STATION").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(StationStorageArea.StorageAreaIdProperty).ColumnMeta.HasIndex();
            Meta.Property(StationStorageArea.StationIdProperty).ColumnMeta.HasIndex();
        }
    }
}