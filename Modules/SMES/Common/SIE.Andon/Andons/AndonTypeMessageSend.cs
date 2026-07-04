using SIE.Andon.Andons.Configs;
using SIE.Andon.Andons.Enum;
using SIE.Common.Configs;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯维护消息推送
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安灯类型消息推送")]
    public class AndonTypeMessageSend : DataEntity
    {
        #region 安灯类型维护 AndonType
        /// <summary>
        /// 安灯类型维护Id
        /// </summary>
        [Label("安灯类型维护")]
        public static readonly IRefIdProperty AndonTypeIdProperty =
            P<AndonTypeMessageSend>.RegisterRefId(e => e.AndonTypeId, ReferenceType.Parent);

        /// <summary>
        /// 安灯类型维护Id
        /// </summary>
        public double AndonTypeId
        {
            get { return (double)this.GetRefId(AndonTypeIdProperty); }
            set { this.SetRefId(AndonTypeIdProperty, value); }
        }

        /// <summary>
        /// 安灯类型维护
        /// </summary>
        public static readonly RefEntityProperty<AndonType> AndonTypeProperty =
            P<AndonTypeMessageSend>.RegisterRef(e => e.AndonType, AndonTypeIdProperty);

        /// <summary>
        /// 安灯类型维护
        /// </summary>
        public AndonType AndonType
        {
            get { return this.GetRefEntity(AndonTypeProperty); }
            set { this.SetRefEntity(AndonTypeProperty, value); }
        }
        #endregion

        #region 节点 Node
        /// <summary>
        /// 节点
        /// </summary>
        [Required]
        [Label("节点")]
        public static readonly Property<Enum.AndonTypeMessageSendState> NodeProperty = P<AndonTypeMessageSend>.Register(e => e.Node);

        /// <summary>
        /// 节点
        /// </summary>
        public Enum.AndonTypeMessageSendState Node
        {
            get { return this.GetProperty(NodeProperty); }
            set { this.SetProperty(NodeProperty, value); }
        }
        #endregion

        #region 时间(分钟) Minute
        /// <summary>
        /// 时间(分钟)
        /// </summary>
        [Required]
        [Label("时间(分钟)")]
        public static readonly Property<double> MinuteProperty = P<AndonTypeMessageSend>.Register(e => e.Minute);

        /// <summary>
        /// 时间(分钟)
        /// </summary>
        public double Minute
        {
            get { return this.GetProperty(MinuteProperty); }
            set { this.SetProperty(MinuteProperty, value); }
        }
        #endregion

        #region 推送方式 PushPlug
        /// <summary>
        /// 推送方式Id
        /// </summary>
        [Label("推送方式")]
        public static readonly IRefIdProperty PushPlugIdProperty =
            P<AndonTypeMessageSend>.RegisterRefId(e => e.PushPlugId, ReferenceType.Normal);

        /// <summary>
        /// 推送方式Id
        /// </summary>
        public double? PushPlugId
        {
            get { return (double?)this.GetRefNullableId(PushPlugIdProperty); }
            set { this.SetRefNullableId(PushPlugIdProperty, value); }
        }

        /// <summary>
        /// 推送方式
        /// </summary>
        public static readonly RefEntityProperty<PushPlug> PushPlugProperty =
            P<AndonTypeMessageSend>.RegisterRef(e => e.PushPlug, PushPlugIdProperty);

        /// <summary>
        /// 推送方式
        /// </summary>
        public PushPlug PushPlug
        {
            get { return this.GetRefEntity(PushPlugProperty); }
            set { this.SetRefEntity(PushPlugProperty, value); }
        }
        #endregion

        #region 信息模板 MessageTemplate
        /// <summary>
        /// 信息模板
        /// </summary>
        [MaxLength(2000)]
        [Label("信息模板")]
        public static readonly Property<string> MessageTemplateProperty = P<AndonTypeMessageSend>.Register(e => e.MessageTemplate);

        /// <summary>
        /// 信息模板
        /// </summary>
        public string MessageTemplate
        {
            get { return this.GetProperty(MessageTemplateProperty); }
            set { this.SetProperty(MessageTemplateProperty, value); }
        }
        #endregion

        #region 推送对象 PushObjectList
        /// <summary>
        /// 推送对象
        /// </summary>
        [Label("推送对象")]
        public static readonly ListProperty<EntityList<AndonTypePushObject>> PushObjectListProperty = P<AndonTypeMessageSend>.RegisterList(e => e.PushObjectList);

        /// <summary>
        /// 推送对象
        /// </summary>
        public EntityList<AndonTypePushObject> PushObjectList
        {
            get { return this.GetLazyList(PushObjectListProperty); }
        }
        #endregion

        #region 视图属性
        #region 推送模块名称 PushPlugName
        /// <summary>
        /// 推送模块名称
        /// </summary>
        [Label("推送模块名称")]
        public static readonly Property<string> PushPlugNameProperty = P<AndonTypeMessageSend>.RegisterView(e => e.PushPlugName, p => p.PushPlug.Name);

        /// <summary>
        /// 推送模块名称
        /// </summary>
        public string PushPlugName
        {
            get { return this.GetProperty(PushPlugNameProperty); }
            set { this.SetProperty(PushPlugNameProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 消息推送配置
    /// </summary>
    public class AndonTypeMessageSendConfig: EntityConfig<AndonTypeMessageSend>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONTYPEMESSAGESEND").MapAllProperties();
            Meta.Property(AndonTypeMessageSend.MessageTemplateProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
