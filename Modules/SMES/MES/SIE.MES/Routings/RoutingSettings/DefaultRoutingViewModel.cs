using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 默认版本工艺路线ViewModel类
    /// </summary>
    [RootEntity, Serializable]
    [Label("默认版本工艺路线ViewModel")]
    public partial class DefaultRoutingViewModel : ViewModel
    {
        #region 默认工艺路线名称 Name
        /// <summary>
        /// 默认工艺路线名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("默认工艺路线名称")]
        public static readonly Property<string> NameProperty = P<DefaultRoutingViewModel>.Register(e => e.Name);

        /// <summary>
        /// 默认工艺路线名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 默认工艺路线描述 Description
        /// <summary>
        /// 默认工艺路线描述
        /// </summary>
        [Label("默认工艺路线描述")]
        public static readonly Property<string> DescriptionProperty = P<DefaultRoutingViewModel>.Register(e => e.Description);

        /// <summary>
        /// 默认工艺路线描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 默认版本 DefaultVersion
        /// <summary>
        /// 默认工艺路线版本Id
        /// </summary>
        public static readonly IRefIdProperty DefaultVersionIdProperty = P<DefaultRoutingViewModel>.RegisterRefId(e => e.DefaultVersionId, ReferenceType.Normal);

        /// <summary>
        /// 默认工艺路线版本Id
        /// </summary>
        public double? DefaultVersionId
        {
            get { return (double?)GetRefNullableId(DefaultVersionIdProperty); }
            set { SetRefNullableId(DefaultVersionIdProperty, value); }
        }

        /// <summary>
        /// 默认工艺路线版本
        /// </summary>
        public static readonly RefEntityProperty<RoutingVersion> DefaultVersionProperty = P<DefaultRoutingViewModel>.RegisterRef(e => e.DefaultVersion, DefaultVersionIdProperty);

        /// <summary>
        /// 默认工艺路线版本
        /// </summary>
        public RoutingVersion DefaultVersion
        {
            get { return GetRefEntity(DefaultVersionProperty); }
            set { SetRefEntity(DefaultVersionProperty, value); }
        }
        #endregion

        #region 布局Id LayoutId
        /// <summary>
        /// 布局Id
        /// </summary>
        [Label("布局Id")]
        public static readonly Property<double?> LayoutIdProperty = P<DefaultRoutingViewModel>.Register(e => e.LayoutId);

        /// <summary>
        /// 布局Id
        /// </summary>
        public double? LayoutId
        {
            get { return this.GetProperty(LayoutIdProperty); }
            set { this.SetProperty(LayoutIdProperty, value); }
        }
        #endregion
    }
}