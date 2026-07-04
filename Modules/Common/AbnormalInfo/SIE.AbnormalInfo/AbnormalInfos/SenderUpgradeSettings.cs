using SIE.Alert;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 推送升级设置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("推送升级设置")]
    public partial class SenderUpgradeSettings : DataEntity
    {
        #region 条件 ConditionType
        /// <summary>
        /// 条件
        /// </summary>
        [Label("条件")]
        public static readonly Property<string> ConditionTypeProperty = P<SenderUpgradeSettings>.Register(e => e.ConditionType);

        /// <summary>
        /// 条件
        /// </summary>
        public string ConditionType
        {
            get { return this.GetProperty(ConditionTypeProperty); }
            set { this.SetProperty(ConditionTypeProperty, value); }
        }
        #endregion

        #region 时间 TimeType
        /// <summary>
        /// 时间
        /// </summary>
        [Label("时间")]
        [Required]
        public static readonly Property<double> TimeTypeProperty = P<SenderUpgradeSettings>.Register(e => e.TimeType);

        /// <summary>
        /// 时间
        /// </summary>
        public double TimeType
        {
            get { return this.GetProperty(TimeTypeProperty); }
            set { this.SetProperty(TimeTypeProperty, value); }
        }
        #endregion

        #region 单位 UnitType
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        [Required]
        public static readonly Property<UnitType> UnitTypeProperty = P<SenderUpgradeSettings>.Register(e => e.UnitType);

        /// <summary>
        /// 单位
        /// </summary>
        public UnitType UnitType
        {
            get { return this.GetProperty(UnitTypeProperty); }
            set { this.SetProperty(UnitTypeProperty, value); }
        }
        #endregion

        #region 推送方式 Pusher
        /// <summary>
        /// 推送方式Id
        /// </summary>
        [Label("推送方式")]
        public static readonly IRefIdProperty PusherIdProperty =
            P<SenderUpgradeSettings>.RegisterRefId(e => e.PusherId, ReferenceType.Normal);

        /// <summary>
        /// 推送方式Id
        /// </summary>
        public double PusherId
        {
            get { return (double)this.GetRefId(PusherIdProperty); }
            set { this.SetRefId(PusherIdProperty, value); }
        }

        /// <summary>
        /// 推送方式
        /// </summary>
        public static readonly RefEntityProperty<Pusher> PusherProperty =
            P<SenderUpgradeSettings>.RegisterRef(e => e.Pusher, PusherIdProperty);

        /// <summary>
        /// 推送方式
        /// </summary>
        public Pusher Pusher
        {
            get { return this.GetRefEntity(PusherProperty); }
            set { this.SetRefEntity(PusherProperty, value); }
        }
        #endregion

        #region 异常信息分类 AbnormalInfoCategory
        /// <summary>
        /// 异常信息分类Id
        /// </summary>
        [Label("异常信息分类")]
        public static readonly IRefIdProperty AbnormalInfoCategoryIdProperty =
            P<SenderUpgradeSettings>.RegisterRefId(e => e.AbnormalInfoCategoryId, ReferenceType.Parent);

        /// <summary>
        /// 异常信息分类Id
        /// </summary>
        public double AbnormalInfoCategoryId
        {
            get { return (double)this.GetRefId(AbnormalInfoCategoryIdProperty); }
            set { this.SetRefId(AbnormalInfoCategoryIdProperty, value); }
        }

        /// <summary>
        /// 异常信息分类
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoCategory> AbnormalInfoCategoryProperty =
            P<SenderUpgradeSettings>.RegisterRef(e => e.AbnormalInfoCategory, AbnormalInfoCategoryIdProperty);

        /// <summary>
        /// 异常信息分类
        /// </summary>
        public AbnormalInfoCategory AbnormalInfoCategory
        {
            get { return this.GetRefEntity(AbnormalInfoCategoryProperty); }
            set { this.SetRefEntity(AbnormalInfoCategoryProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 推送升级设置 实体配置
    /// </summary>
    internal class SenderUpgradeSettingsConfig : EntityConfig<SenderUpgradeSettings>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QMS_SendUpgradeSet").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
