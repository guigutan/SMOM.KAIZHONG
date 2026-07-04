using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.MES.Workbench.KeyPerformances
{
    [Label("车间目标设置")]
    [RootEntity, Serializable, CriteriaQuery]
    public class ShopTargetSetting : TargetSetting
    {
        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<ShopTargetSetting>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return (double)this.GetRefId(WorkShopIdProperty); }
            set { this.SetRefId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<ShopTargetSetting>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 黄色预警 YellowAlert
        /// <summary>
        /// 黄色预警
        /// </summary>
        [Label("黄色预警")]
        public static readonly Property<double?> YellowAlertProperty = P<ShopTargetSetting>.Register(e => e.YellowAlert);

        /// <summary>
        /// 黄色预警
        /// </summary>
        public double? YellowAlert
        {
            get { return this.GetProperty(YellowAlertProperty); }
            set { this.SetProperty(YellowAlertProperty, value); }
        }
        #endregion

        #region 红色预警 RedAlert
        /// <summary>
        /// 红色预警
        /// </summary>
        [Label("红色预警")]
        public static readonly Property<double?> RedAlertProperty = P<ShopTargetSetting>.Register(e => e.RedAlert);

        /// <summary>
        /// 红色预警
        /// </summary>
        public double? RedAlert
        {
            get { return this.GetProperty(RedAlertProperty); }
            set { this.SetProperty(RedAlertProperty, value); }
        }
        #endregion



        #region 产线目标值设置列表 LinePlanRPSettings
        /// <summary>
        /// 产线目标值设置列表
        /// </summary>
        [Label("产线目标值")]
        public static readonly ListProperty<EntityList<LineTargetSetting>> LinePlanRPSettingsProperty = P<ShopTargetSetting>.RegisterList(e => e.LinePlanRPSettings);

        /// <summary>
        /// 产线目标值设置列表
        /// </summary>
        public EntityList<LineTargetSetting> LinePlanRPSettings
        {
            get { return this.GetLazyList(LinePlanRPSettingsProperty); }
        }
        #endregion
    }

    internal class ShopPlanRPSettingConfig : EntityConfig<ShopTargetSetting>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("TG_SETTING_SHOP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
