using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.MES.Workbench.KeyPerformances
{
    [Label("产线目标设置")]
    [ChildEntity, Serializable]
    public class LineTargetSetting : TargetSetting
    {
        #region 车间达成率设置 ShopPlanRPSetting
        /// <summary>
        /// 车间达成率设置Id
        /// </summary>
        [Label("车间达成率设置")]
        public static readonly IRefIdProperty ShopPlanRPSettingIdProperty =
            P<LineTargetSetting>.RegisterRefId(e => e.ShopPlanRPSettingId, ReferenceType.Parent);

        /// <summary>
        /// 车间达成率设置Id
        /// </summary>
        public double ShopPlanRPSettingId
        {
            get { return (double)this.GetRefId(ShopPlanRPSettingIdProperty); }
            set { this.SetRefId(ShopPlanRPSettingIdProperty, value); }
        }

        /// <summary>
        /// 车间达成率设置
        /// </summary>
        public static readonly RefEntityProperty<ShopTargetSetting> ShopPlanRPSettingProperty =
            P<LineTargetSetting>.RegisterRef(e => e.ShopPlanRPSetting, ShopPlanRPSettingIdProperty);

        /// <summary>
        /// 车间达成率设置
        /// </summary>
        public ShopTargetSetting ShopPlanRPSetting
        {
            get { return this.GetRefEntity(ShopPlanRPSettingProperty); }
            set { this.SetRefEntity(ShopPlanRPSettingProperty, value); }
        }
        #endregion

        #region 产线 Line
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty LineIdProperty =
            P<LineTargetSetting>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double LineId
        {
            get { return (double)this.GetRefId(LineIdProperty); }
            set { this.SetRefId(LineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Resource> LineProperty =
            P<LineTargetSetting>.RegisterRef(e => e.Line, LineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Resource Line
        {
            get { return this.GetRefEntity(LineProperty); }
            set { this.SetRefEntity(LineProperty, value); }
        }
        #endregion
    }

    internal class LinePlanRPSettingConfig : EntityConfig<LineTargetSetting>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("TG_SETTING_LINE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
