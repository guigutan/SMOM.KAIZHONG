using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.Tech.Stations.Commands;
using System;

namespace SIE.Wpf.Tech.Stations
{
    /// <summary>
    /// 导入工位物料实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("导入失败数据")]
    public class StationItemCheckDataViewModel : ViewModel
    {
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StationItemCheckDataViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<StationItemCheckDataViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 容量值 Capacity
        /// <summary>
        /// 容量值
        /// </summary>
        [Label("容量值")]
        public static readonly Property<string> CapacityProperty = P<StationItemCheckDataViewModel>.Register(e => e.Capacity);

        /// <summary>
        /// 容量值
        /// </summary>
        public string Capacity
        {
            get { return GetProperty(CapacityProperty); }
            set { SetProperty(CapacityProperty, value); }
        }
        #endregion

        #region 预警值 Warning
        /// <summary>
        /// 预警值
        /// </summary>
        [Label("预警值")]
        public static readonly Property<string> WarningProperty = P<StationItemCheckDataViewModel>.Register(e => e.Warning);

        /// <summary>
        /// 预警值
        /// </summary>
        public string Warning
        {
            get { return GetProperty(WarningProperty); }
            set { SetProperty(WarningProperty, value); }
        }
        #endregion

        #region ErrorMessage 导入失败原因
        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<StationItemCheckDataViewModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 配置视图
    /// </summary>
    class StationItemCheckDataViewModelConfig : WPFViewConfig<StationItemCheckDataViewModel>
    {
        /// <summary>
        /// 默认视图授权给父MODEL
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ImportStationItemViewModel));
        }

        /// <summary>
        /// 列表实体
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(ExportFailedDataCommand));
            View.Property(p => p.ErrorMessage);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.Warning);
            View.Property(p => p.Capacity);
        }
    }
}
