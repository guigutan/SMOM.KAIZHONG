using SIE.Core.Equipments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.Tech.Stations
{
    /// <summary>
    /// 工位设备
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工位设备")]
    public partial class StationEquipment : DataEntity
    {
        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        public static readonly IRefIdProperty StationIdProperty = P<StationEquipment>.RegisterRefId(e => e.StationId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<StationEquipment>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<StationEquipment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<StationEquipment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 是否主设备 IsMaster
        /// <summary>
        /// 是否主设备
        /// </summary>
        [Label("是否主设备")]
        public static readonly Property<bool> IsMasterProperty = P<StationEquipment>.Register(e => e.IsMaster);

        /// <summary>
        /// 是否主设备
        /// </summary>
        public bool IsMaster
        {
            get { return this.GetProperty(IsMasterProperty); }
            set { this.SetProperty(IsMasterProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<StationEquipment>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }

        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<StationEquipment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 工位设备 实体配置
    /// </summary>
    internal class StationEquipmentConfig : EntityConfig<StationEquipment>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_STATION_EQUIP").MapAllProperties();
            Meta.Property(StationEquipment.StationIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
