using SIE.Domain;
using SIE.ESop.Displays;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using System;

namespace SIE.Wpf.ESop
{
    /// <summary>
    /// 工作站信息
    /// </summary>
    [RootEntity, Serializable]
    public class EsopWorkstation : ViewModel
    {
        #region 用户 User 
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("人员")]
        [Required]
        public static readonly IRefIdProperty UserIdProperty = P<EsopWorkstation>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)this.GetRefNullableId(UserIdProperty); }
            set { this.SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<Employee> UserProperty = P<EsopWorkstation>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public Employee User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<EsopWorkstation>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<EsopWorkstation>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 显示点 DisplayPoint 
        /// <summary>
        /// 显示点Id
        /// </summary>
        [Label("显示点")]
        [Required]
        public static readonly IRefIdProperty DisplayPointIdProperty = P<EsopWorkstation>.RegisterRefId(e => e.DisplayPointId, ReferenceType.Normal);

        /// <summary>
        /// 显示点Id
        /// </summary>
        public double? DisplayPointId
        {
            get { return (double?)this.GetRefNullableId(DisplayPointIdProperty); }
            set { this.SetRefNullableId(DisplayPointIdProperty, value); }
        }

        /// <summary>
        /// 显示点
        /// </summary>
        public static readonly RefEntityProperty<DisplayPoint> DisplayPointProperty = P<EsopWorkstation>.RegisterRef(e => e.DisplayPoint, DisplayPointIdProperty);

        /// <summary>
        /// 显示点
        /// </summary>
        public DisplayPoint DisplayPoint
        {
            get { return this.GetRefEntity(DisplayPointProperty); }
            set { this.SetRefEntity(DisplayPointProperty, value); }
        }
        #endregion

        #region 工位Id StationId 
        /// <summary>
        /// 工位Id
        /// </summary>
        public static readonly Property<double?> StationIdProperty = P<EsopWorkstation>.Register(e => e.StationId);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return this.GetProperty(StationIdProperty); }
            set { this.SetProperty(StationIdProperty, value); }
        }
        #endregion
    }
}