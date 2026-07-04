using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.ViewModels;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯类型消息推送对象
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安灯类型消息推送对象")]
    public class AndonTypePushObject : DataEntity
    {
        #region 消息推送 MessageSend
        /// <summary>
        /// 消息推送Id
        /// </summary>
        [Label("消息推送")]
        public static readonly IRefIdProperty MessageSendIdProperty =
            P<AndonTypePushObject>.RegisterRefId(e => e.MessageSendId, ReferenceType.Parent);

        /// <summary>
        /// 消息推送Id
        /// </summary>
        public double MessageSendId
        {
            get { return (double)this.GetRefId(MessageSendIdProperty); }
            set { this.SetRefId(MessageSendIdProperty, value); }
        }

        /// <summary>
        /// 消息推送
        /// </summary>
        public static readonly RefEntityProperty<AndonTypeMessageSend> MessageSendProperty =
            P<AndonTypePushObject>.RegisterRef(e => e.MessageSend, MessageSendIdProperty);

        /// <summary>
        /// 消息推送
        /// </summary>
        public AndonTypeMessageSend MessageSend
        {
            get { return this.GetRefEntity(MessageSendProperty); }
            set { this.SetRefEntity(MessageSendProperty, value); }
        }
        #endregion

        #region 对象类型 Type
        /// <summary>
        /// 对象类型
        /// </summary>
        [Required]
        [Label("对象类型")]
        public static readonly Property<Enum.PushObjectType> TypeProperty = P<AndonTypePushObject>.Register(e => e.Type);

        /// <summary>
        /// 对象类型
        /// </summary>
        public Enum.PushObjectType Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 对象编码 Code
        /// <summary>
        /// 对象编码
        /// </summary>
        [Label("对象编码")]
        public static readonly Property<string> CodeProperty = P<AndonTypePushObject>.Register(e => e.Code);

        /// <summary>
        /// 对象编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 对象名称 Name
        /// <summary>
        /// 对象名称
        /// </summary>
        [Label("对象名称")]
        public static readonly Property<string> NameProperty = P<AndonTypePushObject>.Register(e => e.Name);

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class AndonTypePushObjectConfig : EntityConfig<AndonTypePushObject>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONTYPEPUSHOBJECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
