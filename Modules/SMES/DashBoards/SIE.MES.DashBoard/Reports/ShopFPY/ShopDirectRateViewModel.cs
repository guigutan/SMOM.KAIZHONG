using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.ShopFPY
{
    /// <summary>
    /// 车间报表数据ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class ShopDirectRateViewModel : DirectRateBaseViewModel
    {
        #region 车间Id ShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double?> ShopIdProperty = P<ShopDirectRateViewModel>.Register(e => e.ShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? ShopId
        {
            get { return this.GetProperty(ShopIdProperty); }
            set { this.SetProperty(ShopIdProperty, value); }
        }
        #endregion

        #region 车间名称 ShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        [FieldSetting("车间", FieldArea.RowArea, 0)]
        public static readonly Property<string> ShopNameProperty = P<ShopDirectRateViewModel>.Register(e => e.ShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string ShopName
        {
            get { return this.GetProperty(ShopNameProperty); }
            set { this.SetProperty(ShopNameProperty, value); }
        }
        #endregion

        #region 车间直通率设置 ShopDirectRate
        /// <summary>
        /// 车间直通率设置Id
        /// </summary>
        [Label("车间直通率设置")]
        public static readonly IRefIdProperty ShopDirectRateIdProperty =
            P<ShopDirectRateViewModel>.RegisterRefId(e => e.ShopDirectRateId, ReferenceType.Normal);

        /// <summary>
        /// 车间直通率设置Id
        /// </summary>
        public double? ShopDirectRateId
        {
            get { return (double?)this.GetRefNullableId(ShopDirectRateIdProperty); }
            set { this.SetRefNullableId(ShopDirectRateIdProperty, value); }
        }

        /// <summary>
        /// 车间直通率设置
        /// </summary>
        public static readonly RefEntityProperty<ShopFpySetting> ShopDirectRateProperty =
            P<ShopDirectRateViewModel>.RegisterRef(e => e.ShopDirectRate, ShopDirectRateIdProperty);

        /// <summary>
        /// 车间直通率设置
        /// </summary>
        public ShopFpySetting ShopDirectRate
        {
            get { return this.GetRefEntity(ShopDirectRateProperty); }
            set { this.SetRefEntity(ShopDirectRateProperty, value); }
        }
        #endregion

        #region 资源Id LineId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double?> LineIdProperty = P<ShopDirectRateViewModel>.Register(e => e.LineId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? LineId
        {
            get { return this.GetProperty(LineIdProperty); }
            set { this.SetProperty(LineIdProperty, value); }
        }
        #endregion

        #region 资源名称 LineName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        [FieldSetting("资源", FieldArea.RowArea, 1, SummaryType = SummaryType.Custom)]
        public static readonly Property<string> LineNameProperty = P<ShopDirectRateViewModel>.Register(e => e.LineName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string LineName
        {
            get { return this.GetProperty(LineNameProperty); }
            set { this.SetProperty(LineNameProperty, value); }
        }
        #endregion

        #region 产品直通率设置 LineDirectRate
        /// <summary>
        /// 产品直通率设置Id
        /// </summary>
        [Label("产品直通率设置")]
        public static readonly IRefIdProperty LineDirectRateIdProperty =
            P<ShopDirectRateViewModel>.RegisterRefId(e => e.LineDirectRateId, ReferenceType.Normal);

        /// <summary>
        /// 产品直通率设置Id
        /// </summary>
        public double? LineDirectRateId
        {
            get { return (double?)this.GetRefNullableId(LineDirectRateIdProperty); }
            set { this.SetRefNullableId(LineDirectRateIdProperty, value); }
        }

        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public static readonly RefEntityProperty<LineFpySetting> LineDirectRateProperty =
            P<ShopDirectRateViewModel>.RegisterRef(e => e.LineDirectRate, LineDirectRateIdProperty);

        /// <summary>
        /// 产品直通率设置
        /// </summary>
        public LineFpySetting LineDirectRate
        {
            get { return this.GetRefEntity(LineDirectRateProperty); }
            set { this.SetRefEntity(LineDirectRateProperty, value); }
        }
        #endregion

    }
}
