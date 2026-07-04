using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 工艺路线版本ViewModel类
    /// </summary>
    [RootEntity, Serializable]
    [Label("工艺路线版本")]
    public partial class RoutingVersionViewModel : ViewModel
    {
        #region 工艺路线版本 VersionName
        /// <summary>
        /// 工艺路线版本
        /// </summary>
        [Label("工艺路线版本")]
        public static readonly Property<string> VersionNameProperty = P<RoutingVersionViewModel>.Register(e => e.VersionName);

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string VersionName
        {
            get { return this.GetProperty(VersionNameProperty); }
            set { this.SetProperty(VersionNameProperty, value); }
        }
        #endregion

        #region 是否默认 IsDefault
        /// <summary>
        /// 是否默认
        /// </summary>
        [Label("是否默认")]
        public static readonly Property<YesNo> IsDefaultProperty = P<RoutingVersionViewModel>.Register(e => e.IsDefault);

        /// <summary>
        /// 是否默认
        /// </summary>
        public YesNo IsDefault
        {
            get { return this.GetProperty(IsDefaultProperty); }
            set { this.SetProperty(IsDefaultProperty, value); }
        }
        #endregion
    }
}