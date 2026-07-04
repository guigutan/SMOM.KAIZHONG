using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检结果
    /// TODO huchuqiang demo数据
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工位点检结果")]
    public class StationCheckResult : DataEntity
    {
        #region 是否已点检 IsCheck
        /// <summary>
        /// 是否已点检
        /// </summary>
        [Label("是否已点检")]
        public static readonly Property<bool> IsCheckProperty = P<StationCheckResult>.Register(e => e.IsCheck);

        /// <summary>
        /// 是否已点检
        /// </summary>
        public bool IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        #region 点检日期 CheckDate
        /// <summary>
        /// 点检日期
        /// </summary>
        [Label("点检日期")]
        public static readonly Property<DateTime> CheckDateProperty = P<StationCheckResult>.Register(e => e.CheckDate, new PropertyMetadata<DateTime>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 点检日期
        /// </summary>
        public DateTime CheckDate
        {
            get { return this.GetProperty(CheckDateProperty); }
            set { this.SetProperty(CheckDateProperty, value); }
        }
        #endregion

        #region 点检类型 CheckType
        /// <summary>
        /// 点检类型
        /// </summary>
        [Label("点检类型")]
        public static readonly Property<CheckType> CheckTypeProperty = P<StationCheckResult>.Register(e => e.CheckType);

        /// <summary>
        /// 点检类型
        /// </summary>
        public CheckType CheckType
        {
            get { return this.GetProperty(CheckTypeProperty); }
            set { this.SetProperty(CheckTypeProperty, value); }
        }
        #endregion

        #region 点检项目 CheckItemId
        /// <summary>
        /// 点检项目
        /// </summary>
        [Label("点检项目")]
        public static readonly Property<double> CheckItemProperty = P<StationCheckResult>.Register(e => e.CheckItemId);

        /// <summary>
        /// 点检项目
        /// </summary>
        public double CheckItemId
        {
            get { return this.GetProperty(CheckItemProperty); }
            set { this.SetProperty(CheckItemProperty, value); }
        }
        #endregion 

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<StationCheckResult>.RegisterRefId(e => e.StationId, ReferenceType.Parent);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)this.GetRefId(StationIdProperty); }
            set { this.SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<StationCheckResult>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工位点检结果实体配置
    /// </summary>
    internal class StationCheckResultEntityConfig : EntityConfig<StationCheckResult>
    {
        /// <summary>
        /// 配置实体元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CHK_STATION_RESULT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 点检类型
    /// </summary>
    public enum CheckType
    {
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        Item,

        /// <summary>
        /// 设备
        /// </summary>
        [Label("设备")]
        Equipment,

        /// <summary>
        /// 工具
        /// </summary>
        [Label("工具")]
        Tool
    }
}