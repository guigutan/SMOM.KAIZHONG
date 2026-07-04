using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯管理消息推送子表
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安灯管理消息推送子表")]
    public  class AndonManageMessageSend : DataEntity
    {
        #region 安灯管理 AndonManage
        /// <summary>
        /// 安灯管理Id
        /// </summary>
        [Label("安灯管理")]
        public static readonly IRefIdProperty AndonManageIdProperty =
            P<AndonManageMessageSend>.RegisterRefId(e => e.AndonManageId, ReferenceType.Parent);

        /// <summary>
        /// 安灯管理Id
        /// </summary>
        public double AndonManageId
        {
            get { return (double)this.GetRefId(AndonManageIdProperty); }
            set { this.SetRefId(AndonManageIdProperty, value); }
        }

        /// <summary>
        /// 安灯管理
        /// </summary>
        public static readonly RefEntityProperty<AndonManage> AndonManageProperty =
            P<AndonManageMessageSend>.RegisterRef(e => e.AndonManage, AndonManageIdProperty);

        /// <summary>
        /// 安灯管理
        /// </summary>
        public AndonManage AndonManage
        {
            get { return this.GetRefEntity(AndonManageProperty); }
            set { this.SetRefEntity(AndonManageProperty, value); }
        }
        #endregion

        #region 安灯维护消息推送 AndonMessageSend
        /// <summary>
        /// 安灯维护消息推送Id
        /// </summary>
        [Label("安灯维护消息推送")]
        public static readonly IRefIdProperty AndonMessageSendIdProperty =
            P<AndonManageMessageSend>.RegisterRefId(e => e.AndonMessageSendId, ReferenceType.Normal);

        /// <summary>
        /// 安灯维护消息推送Id
        /// </summary>
        public double AndonMessageSendId
        {
            get { return (double)this.GetRefId(AndonMessageSendIdProperty); }
            set { this.SetRefId(AndonMessageSendIdProperty, value); }
        }

        /// <summary>
        /// 安灯维护消息推送
        /// </summary>
        public static readonly RefEntityProperty<AndonMessageSend> AndonMessageSendProperty =
            P<AndonManageMessageSend>.RegisterRef(e => e.AndonMessageSend, AndonMessageSendIdProperty);

        /// <summary>
        /// 安灯维护消息推送
        /// </summary>
        public AndonMessageSend AndonMessageSend
        {
            get { return this.GetRefEntity(AndonMessageSendProperty); }
            set { this.SetRefEntity(AndonMessageSendProperty, value); }
        }
        #endregion


        #region 推送时间 MessageSendTime
        /// <summary>
        /// 推送时间
        /// </summary>
        [Label("推送时间")]
        public static readonly Property<DateTime> MessageSendTimeProperty = P<AndonManageMessageSend>.Register(e => e.MessageSendTime);

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime MessageSendTime
        {
            get { return this.GetProperty(MessageSendTimeProperty); }
            set { this.SetProperty(MessageSendTimeProperty, value); }
        }
        #endregion

        #region 推送人 MessageSendPerson
        /// <summary>
        /// 推送人Id
        /// </summary>
        [Label("推送人")]
        public static readonly IRefIdProperty MessageSendPersonIdProperty =
            P<AndonManageMessageSend>.RegisterRefId(e => e.MessageSendPersonId, ReferenceType.Normal);

        /// <summary>
        /// 推送人Id
        /// </summary>
        public double? MessageSendPersonId
        {
            get { return (double?)this.GetRefId(MessageSendPersonIdProperty); }
            set { this.SetRefId(MessageSendPersonIdProperty, value); }
        }

        /// <summary>
        /// 推送人
        /// </summary>
        public static readonly RefEntityProperty<Employee> MessageSendPersonProperty =
            P<AndonManageMessageSend>.RegisterRef(e => e.MessageSendPerson, MessageSendPersonIdProperty);

        /// <summary>
        /// 推送人
        /// </summary>
        public Employee MessageSendPerson
        {
            get { return this.GetRefEntity(MessageSendPersonProperty); }
            set { this.SetRefEntity(MessageSendPersonProperty, value); }
        }
        #endregion

        #region 推送地址 MessageSendAddress
        /// <summary>
        /// 推送地址
        /// </summary>
        [MaxLength(2000)]
        [Label("推送地址")]
        public static readonly Property<string> MessageSendAddressProperty = P<AndonManageMessageSend>.Register(e => e.MessageSendAddress);

        /// <summary>
        /// 推送地址
        /// </summary>
        public string MessageSendAddress
        {
            get { return this.GetProperty(MessageSendAddressProperty); }
            set { this.SetProperty(MessageSendAddressProperty, value); }
        }
        #endregion

        #region 消息内容 MessageSendTemplate
        /// <summary>
        /// 消息内容
        /// </summary>
        [MaxLength(2000)]
        [Label("消息内容")]
        public static readonly Property<string> MessageSendTemplateProperty = P<AndonManageMessageSend>.Register(e => e.MessageSendTemplate);

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageSendTemplate
        {
            get { return this.GetProperty(MessageSendTemplateProperty); }
            set { this.SetProperty(MessageSendTemplateProperty, value); }
        }
        #endregion

        #region 异常发生时间 AbnormalTime
        /// <summary>
        /// 异常发生时间
        /// </summary>
        [Label("异常发生时间")]
        public static readonly Property<DateTime> AbnormalTimeProperty = P<AndonManageMessageSend>.Register(e => e.AbnormalTime);

        /// <summary>
        /// 异常发生时间
        /// </summary>
        public DateTime AbnormalTime
        {
            get { return this.GetProperty(AbnormalTimeProperty); }
            set { this.SetProperty(AbnormalTimeProperty, value); }
        }
        #endregion

        #region 等待时长/分钟 WaitinglTime
        /// <summary>
        /// 等待时长/分钟
        /// </summary>
        [Label("等待时长/分钟")]
        public static readonly Property<double> WaitinglTimeProperty = P<AndonManageMessageSend>.Register(e => e.WaitinglTime);

        /// <summary>
        /// 等待时长/分钟
        /// </summary>
        public double WaitinglTime
        {
            get { return this.GetProperty(WaitinglTimeProperty); }
            set { this.SetProperty(WaitinglTimeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体元配置
    /// </summary>
    public class AndonManageMessageSendConfig: EntityConfig<AndonManageMessageSend>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_ANDONMANAGEMESSAGESEND").MapAllProperties();
            Meta.Property(AndonManageMessageSend.MessageSendAddressProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AndonManageMessageSend.MessageSendTemplateProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
