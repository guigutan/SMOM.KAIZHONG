using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 车间直通率设置查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public partial class ShopFpySettingCriteria : Criteria
    {
        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty = P<ShopFpySettingCriteria>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? ShopId
        {
            get { return (double?)GetRefNullableId(ShopIdProperty); }
            set { SetRefNullableId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty = P<ShopFpySettingCriteria>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return GetRefEntity(ShopProperty); }
            set { SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 资源 Line
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<ShopFpySettingCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<ShopFpySettingCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FpySettingController>().GetShopFpySettings(this);
        }
    }
}