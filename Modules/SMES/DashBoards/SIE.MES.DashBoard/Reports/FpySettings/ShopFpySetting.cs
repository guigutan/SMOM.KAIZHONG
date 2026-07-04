using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 车间直通率设置
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ShopFpySettingCriteria))]
    [Label("车间直通率设置")]
    public partial class ShopFpySetting : FpySetting
    {
        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        public static readonly IRefIdProperty ShopIdProperty = P<ShopFpySetting>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double ShopId
        {
            get { return (double)GetRefId(ShopIdProperty); }
            set { SetRefId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty = P<ShopFpySetting>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return GetRefEntity(ShopProperty); }
            set { SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 资源名称 ShopName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ShopNameProperty = P<ShopFpySetting>.RegisterView(e => e.ShopName, p => p.Shop.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ShopName
        {
            get { return this.GetProperty(ShopNameProperty); }
        }
        #endregion

        #region 资源直通率设置 LineFpyList
        /// <summary>
        /// 资源直通率设置
        /// </summary>
        public static readonly ListProperty<EntityList<LineFpySetting>> LineFpyListProperty = P<ShopFpySetting>.RegisterList(e => e.LineFpyList);
        /// <summary>
        /// 资源直通率设置
        /// </summary>
        public EntityList<LineFpySetting> LineFpyList
        {
            get { return this.GetLazyList(LineFpyListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 车间直通率设置 实体配置
    /// </summary>
    internal class ShopFpySettingConfig : EntityConfig<ShopFpySetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_SHOP_FPY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}