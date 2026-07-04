using SIE.Andon.Andons.Configs;
using SIE.Andon.Andons.Enum;
using SIE.Common.Configs;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯类型维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonTypeCriterial))]
    [EntityWithConfig(typeof(AndonTypePushPlugConfig))]
    [DisplayMember(nameof(AndonTypeName))]
    [Label("安灯类型维护")]
    public class AndonType : DataEntity, IStateEntity
    {
        #region 安灯类型编码 AndonTypeCode
        /// <summary>
        /// 安灯类型编码
        /// </summary>
        [Required]
        [Label("安灯类型编码")]
        public static readonly Property<string> AndonTypeCodeProperty = P<AndonType>.Register(e => e.AndonTypeCode);

        /// <summary>
        /// 安灯类型编码
        /// </summary>
        public string AndonTypeCode
        {
            get { return this.GetProperty(AndonTypeCodeProperty); }
            set { this.SetProperty(AndonTypeCodeProperty, value); }
        }
        #endregion

        #region 安灯类型名称 AndonTypeName
        /// <summary>
        /// 安灯类型名称
        /// </summary>
        [Required]
        [Label("安灯类型名称")]
        public static readonly Property<string> AndonTypeNameProperty = P<AndonType>.Register(e => e.AndonTypeName);

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName
        {
            get { return this.GetProperty(AndonTypeNameProperty); }
            set { this.SetProperty(AndonTypeNameProperty, value); }
        }
        #endregion

        #region 安灯大类 AndonTypeClass
        /// <summary>
        /// 安灯大类
        /// </summary>
        [Required]
        [Label("安灯大类")]
        public static readonly Property<AndonTypeClass> AndonTypeClassProperty = P<AndonType>.Register(e => e.AndonTypeClass);

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass AndonTypeClass
        {
            get { return this.GetProperty(AndonTypeClassProperty); }
            set { this.SetProperty(AndonTypeClassProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<AndonType>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        //#region 推送方式 PushPlug
        ///// <summary>
        ///// 推送方式Id
        ///// </summary>
        //[Label("推送模板")]
        //public static readonly IRefIdProperty PushPlugIdProperty =
        //    P<AndonType>.RegisterRefId(e => e.PushPlugId, ReferenceType.Normal);

        ///// <summary>
        ///// 推送方式Id
        ///// </summary>
        //public double? PushPlugId
        //{
        //    get { return (double?)this.GetRefNullableId(PushPlugIdProperty); }
        //    set { this.SetRefNullableId(PushPlugIdProperty, value); }
        //}

        ///// <summary>
        ///// 推送方式
        ///// </summary>
        //public static readonly RefEntityProperty<PushPlug> PushPlugProperty =
        //    P<AndonType>.RegisterRef(e => e.PushPlug, PushPlugIdProperty);

        ///// <summary>
        ///// 推送方式
        ///// </summary>
        //public PushPlug PushPlug
        //{
        //    get { return this.GetRefEntity(PushPlugProperty); }
        //    set { this.SetRefEntity(PushPlugProperty, value); }
        //}
        //#endregion

        //#region 信息模板 MessageTemplate
        ///// <summary>
        ///// 信息模板
        ///// </summary>
        //[MaxLength(2000)]
        //[Label("信息模板")]
        //public static readonly Property<string> MessageTemplateProperty = P<AndonType>.Register(e => e.MessageTemplate);

        ///// <summary>
        ///// 信息模板
        ///// </summary>
        //public string MessageTemplate
        //{
        //    get { return this.GetProperty(MessageTemplateProperty); }
        //    set { this.SetProperty(MessageTemplateProperty, value); }
        //}
        //#endregion

        #region 触发权限 TriggerPowerList
        /// <summary>
        /// 触发权限
        /// </summary>
        [Label("触发权限")]
        public static readonly ListProperty<EntityList<AndonTypeTriggerPower>> TriggerPowerListProperty = P<AndonType>.RegisterList(e => e.TriggerPowerList);

        /// <summary>
        /// 触发权限
        /// </summary>
        public EntityList<AndonTypeTriggerPower> TriggerPowerList
        {
            get { return this.GetLazyList(TriggerPowerListProperty); }
        }
        #endregion

        #region 消息推送 MessageSendList
        /// <summary>
        /// 消息推送
        /// </summary>
        [Label("消息推送")]
        public static readonly ListProperty<EntityList<AndonTypeMessageSend>> MessageSendListProperty = P<AndonType>.RegisterList(e => e.MessageSendList);

        /// <summary>
        /// 消息推送
        /// </summary>
        public EntityList<AndonTypeMessageSend> MessageSendList
        {
            get { return this.GetLazyList(MessageSendListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 安灯类型实体配置
    /// </summary>
    public class AndonTypeConfig : EntityConfig<AndonType>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONTYPE").MapAllProperties();
            //Meta.Property(AndonType.MessageTemplateProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
