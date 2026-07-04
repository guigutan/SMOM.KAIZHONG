using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息定义 推送升级设置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("推送升级设置")]
    public partial class DefinitionSenderSettings : DataEntity
    {
        #region 条件 ConditionType
        /// <summary>
        /// 条件
        /// </summary>
        [Label("条件")]
        public static readonly Property<string> ConditionTypeProperty = P<DefinitionSenderSettings>.Register(e => e.ConditionType);

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
        public static readonly Property<double> TimeTypeProperty = P<DefinitionSenderSettings>.Register(e => e.TimeType);

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
        public static readonly Property<UnitType> UnitTypeProperty = P<DefinitionSenderSettings>.Register(e => e.UnitType);

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
            P<DefinitionSenderSettings>.RegisterRefId(e => e.PusherId, ReferenceType.Normal);

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
            P<DefinitionSenderSettings>.RegisterRef(e => e.Pusher, PusherIdProperty);

        /// <summary>
        /// 推送方式
        /// </summary>
        public Pusher Pusher
        {
            get { return this.GetRefEntity(PusherProperty); }
            set { this.SetRefEntity(PusherProperty, value); }
        }
        #endregion

        #region 异常信息定义 AbnormalDefinition
        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        [Label("异常信息定义")]
        public static readonly IRefIdProperty AbnormalDefinitionIdProperty =
            P<DefinitionSenderSettings>.RegisterRefId(e => e.AbnormalDefinitionId, ReferenceType.Parent);

        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        public double AbnormalDefinitionId
        {
            get { return (double)this.GetRefId(AbnormalDefinitionIdProperty); }
            set { this.SetRefId(AbnormalDefinitionIdProperty, value); }
        }

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoDefinition> AbnormalDefinitionProperty =
            P<DefinitionSenderSettings>.RegisterRef(e => e.AbnormalDefinition, AbnormalDefinitionIdProperty);

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public AbnormalInfoDefinition AbnormalDefinition
        {
            get { return this.GetRefEntity(AbnormalDefinitionProperty); }
            set { this.SetRefEntity(AbnormalDefinitionProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 异常信息定义推送升级设置 实体配置
    /// </summary>
    internal class DefinitionSenderSettingsConfig : EntityConfig<DefinitionSenderSettings>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QMS_DefSenderSet").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
