using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 设置
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(VersionHead))]
    [Label("设置")]     
    public class FileSetting : DataEntity
    {
        #region 启用OA流程审批 IsOA
        /// <summary>
        /// 启用OA流程审批
        /// </summary>
        [Label("启用OA流程审批")]
        public static readonly Property<bool> IsOAProperty = P<FileSetting>.Register(e => e.IsOA);

        /// <summary>
        /// 启用OA流程审批
        /// </summary>
        public bool IsOA
        {
            get { return this.GetProperty(IsOAProperty); }
            set { this.SetProperty(IsOAProperty, value); }
        }
        #endregion

        #region 版本前缀 VersionHead
        /// <summary>
        /// 版本前缀
        /// </summary>
        [Label("版本前缀")]
        public static readonly Property<string> VersionHeadProperty = P<FileSetting>.Register(e => e.VersionHead);

        /// <summary>
        /// 版本前缀
        /// </summary>
        public string VersionHead
        {
            get { return this.GetProperty(VersionHeadProperty); }
            set { this.SetProperty(VersionHeadProperty, value); }
        }
        #endregion

        #region 系统内审核推送方式 Pusher
        /// <summary>
        /// 推送方法Id
        /// </summary>
        [Label("审核推送方式")]
        public static readonly IRefIdProperty PusherIdProperty =
            P<FileSetting>.RegisterRefId(e => e.PusherId, ReferenceType.Normal);

        /// <summary>
        /// 推送方法Id
        /// </summary>
        public double PusherId
        {
            get { return (double)this.GetRefId(PusherIdProperty); }
            set { this.SetRefId(PusherIdProperty, value); }
        }

        /// <summary>
        /// 推送方法
        /// </summary>
        public static readonly RefEntityProperty<Pusher> PusherProperty =
            P<FileSetting>.RegisterRef(e => e.Pusher, PusherIdProperty);

        /// <summary>
        /// 推送方法
        /// </summary>
        public Pusher Pusher
        {
            get { return this.GetRefEntity(PusherProperty); }
            set { this.SetRefEntity(PusherProperty, value); }
        }
        #endregion

        #region 审核人 AuditMans
        /// <summary>
        /// 审核人
        /// </summary>
        [Label("审核人")]
        public static readonly Property<string> AuditMansProperty = P<FileSetting>.Register(e => e.AuditMans);

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditMans
        {
            get { return this.GetProperty(AuditMansProperty); }
            set { this.SetProperty(AuditMansProperty, value); }
        }
        #endregion

        #region 推送方式名称 PusherName
        /// <summary>
        /// 推送方式名称
        /// </summary>        
        public static readonly Property<string> PusherNameProperty = P<FileSetting>.RegisterView(e => e.PusherName, p => p.Pusher.Name);

        /// <summary>
        /// 推送方式名称
        /// </summary>
        public string PusherName
        {
            get { return this.GetProperty(PusherNameProperty); }
        }
        #endregion


    }

    /// <summary>
    /// 文件管理 实体配置
    /// </summary>
    internal class FileSettingConfig : EntityConfig<FileSetting>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。     
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("FILE_SETTING").MapAllProperties();
            Meta.Property(FileSetting.AuditMansProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
