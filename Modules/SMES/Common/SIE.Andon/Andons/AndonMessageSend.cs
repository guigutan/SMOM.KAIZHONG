using SIE.Andon.Andons.Enum;
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
    [Label("安灯维护消息推送")]
    public class AndonMessageSend : DataEntity
    {
        #region 安灯维护 Andon
        /// <summary>
        /// 安灯维护Id
        /// </summary>
        [Label("安灯维护")]
        public static readonly IRefIdProperty AndonIdProperty =
            P<AndonMessageSend>.RegisterRefId(e => e.AndonId, ReferenceType.Parent);

        /// <summary>
        /// 安灯维护Id
        /// </summary>
        public double AndonId
        {
            get { return (double)this.GetRefId(AndonIdProperty); }
            set { this.SetRefId(AndonIdProperty, value); }
        }

        /// <summary>
        /// 安灯维护
        /// </summary>
        public static readonly RefEntityProperty<Andon> AndonProperty =
            P<AndonMessageSend>.RegisterRef(e => e.Andon, AndonIdProperty);

        /// <summary>
        /// 安灯维护
        /// </summary>
        public Andon Andon
        {
            get { return this.GetRefEntity(AndonProperty); }
            set { this.SetRefEntity(AndonProperty, value); }
        }
        #endregion

        #region 节点 Node
        /// <summary>
        /// 节点
        /// </summary>
        [Label("节点")]
        public static readonly Property<AndonTypeMessageSendState> NodeProperty = P<AndonMessageSend>.Register(e => e.Node);

        /// <summary>
        /// 节点
        /// </summary>
        public AndonTypeMessageSendState Node
        {
            get { return this.GetProperty(NodeProperty); }
            set { this.SetProperty(NodeProperty, value); }
        }
        #endregion

        #region 时间(分钟) Minute
        /// <summary>
        /// 时间(分钟)
        /// </summary>
        [Label("时间(分钟)")]
        public static readonly Property<double> MinuteProperty = P<AndonMessageSend>.Register(e => e.Minute);

        /// <summary>
        /// 时间(分钟)
        /// </summary>
        public double Minute
        {
            get { return this.GetProperty(MinuteProperty); }
            set { this.SetProperty(MinuteProperty, value); }
        }
        #endregion

        #region 推送模块 PushPlug
        /// <summary>
        /// 推送模块Id
        /// </summary>
        [Label("推送模块")]
        public static readonly IRefIdProperty PushPlugIdProperty =
            P<AndonMessageSend>.RegisterRefId(e => e.PushPlugId, ReferenceType.Normal);

        /// <summary>
        /// 推送模块Id
        /// </summary>
        public double? PushPlugId
        {
            get { return (double?)this.GetRefNullableId(PushPlugIdProperty); }
            set { this.SetRefNullableId(PushPlugIdProperty, value); }
        }

        /// <summary>
        /// 推送模块
        /// </summary>
        public static readonly RefEntityProperty<PushPlug> PushPlugProperty =
            P<AndonMessageSend>.RegisterRef(e => e.PushPlug, PushPlugIdProperty);

        /// <summary>
        /// 推送模块
        /// </summary>
        public PushPlug PushPlug
        {
            get { return this.GetRefEntity(PushPlugProperty); }
            set { this.SetRefEntity(PushPlugProperty, value); }
        }
        #endregion

        #region 消息模板 MessageTemplate
        /// <summary>
        /// 消息模板
        /// </summary>
        [MaxLength(4000)]
        [Label("消息模板")]
        public static readonly Property<string> MessageTemplateProperty = P<AndonMessageSend>.Register(e => e.MessageTemplate);

        /// <summary>
        /// 消息模板
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
        public static readonly ListProperty<EntityList<AndonPushObject>> PushObjectListProperty = P<AndonMessageSend>.RegisterList(e => e.PushObjectList);

        /// <summary>
        /// 推送对象
        /// </summary>
        public EntityList<AndonPushObject> PushObjectList
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
        public static readonly Property<string> PushPlugNameProperty = P<AndonMessageSend>.RegisterView(e => e.PushPlugName, p => p.PushPlug.Name);

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
    public class AndonMessageSendConfig: EntityConfig<AndonMessageSend>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONMESSAGESEND").MapAllProperties();
            Meta.Property(AndonMessageSend.MessageTemplateProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
