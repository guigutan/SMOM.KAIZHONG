using SIE.Common.Employees;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.Chatting
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("聊天消息")]
    public partial class ChatRecord : DataEntity
    {
        #region 发送时间 SendDate
        /// <summary>
        /// 发送时间
        /// </summary>
        [Label("发送时间")]
        public static readonly Property<DateTime> SendDateProperty = P<ChatRecord>.Register(e => e.SendDate);

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendDate
        {
            get { return GetProperty(SendDateProperty); }
            set { SetProperty(SendDateProperty, value); }
        }
        #endregion

        #region 消息内容 Content
        /// <summary>
        /// 消息内容
        /// </summary>
        [MaxLength(480)]
        [Label("消息内容")]
        public static readonly Property<string> ContentProperty = P<ChatRecord>.Register(e => e.Content);

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content
        {
            get { return GetProperty(ContentProperty); }
            set { SetProperty(ContentProperty, value); }
        }
        #endregion

        #region 接收时间 ReciveDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReciveDateProperty = P<ChatRecord>.Register(e => e.ReciveDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReciveDate
        {
            get { return GetProperty(ReciveDateProperty); }
            set { SetProperty(ReciveDateProperty, value); }
        }
        #endregion

        #region 发送人 From
        /// <summary>
        /// 发送人Id
        /// </summary>
        public static readonly IRefIdProperty FromIdProperty = P<ChatRecord>.RegisterRefId(e => e.FromId, ReferenceType.Normal);

        /// <summary>
        /// 发送人Id
        /// </summary>
        public double FromId
        {
            get { return (double)GetRefId(FromIdProperty); }
            set { SetRefId(FromIdProperty, value); }
        }

        /// <summary>
        /// 发送人
        /// </summary>
        public static readonly RefEntityProperty<Employee> FromProperty = P<ChatRecord>.RegisterRef(e => e.From, FromIdProperty);

        /// <summary>
        /// 发送人
        /// </summary>
        public Employee From
        {
            get { return GetRefEntity(FromProperty); }
            set { SetRefEntity(FromProperty, value); }
        }
        #endregion

        #region 发送人 FromName
        /// <summary>
        /// 发送人
        /// </summary>
        [Label("发送人")]
        public static readonly Property<string> FromNameProperty = P<ChatRecord>.RegisterView(e => e.FromName, p => p.From.Name);

        /// <summary>
        /// 发送人
        /// </summary>
        public string FromName
        {
            get { return this.GetProperty(FromNameProperty); }
        }
        #endregion
        
        #region To 接收人
        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly IRefIdProperty ToIdProperty = P<ChatRecord>.RegisterRefId(e => e.ToId, ReferenceType.Normal);

        /// <summary>
        /// 接收人
        /// </summary>
        public double ToId
        {
            get { return (double)GetRefId(ToIdProperty); }
            set { SetRefId(ToIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ToProperty = P<ChatRecord>.RegisterRef(e => e.To, ToIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee To
        {
            get { return GetRefEntity(ToProperty); }
            set { SetRefEntity(ToProperty, value); }
        }
        #endregion

        #region 接收人 ToName
        /// <summary>
        /// 接收人
        /// </summary>
        [Label("接收人")]
        public static readonly Property<string> ToNameProperty = P<ChatRecord>.RegisterView(e => e.ToName, p => p.To.Name);

        /// <summary>
        /// 接收人
        /// </summary>
        public string ToName
        {
            get { return this.GetProperty(ToNameProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 聊天消息 实体配置
    /// </summary>
    internal class ChatRecordConfig : EntityConfig<ChatRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WB_CHAT").MapAllProperties();
            Meta.Property(ChatRecord.ContentProperty).ColumnMeta.HasLength(960);
            Meta.EnablePhantoms();
        }
    }
}