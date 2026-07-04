using Newtonsoft.Json;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.UserAgreements
{
	/// <summary>
	/// 用户协议表
	/// </summary>
	[RootEntity, Serializable]
	[CriteriaQuery]
	[Label("用户协议表")]
	public partial class UserAgreement : DataEntity
	{
		#region 版本 Version
		/// <summary>
		/// 版本
		/// </summary>
		[Required]
		[Label("版本")]
        public static readonly Property<int> VersionProperty = P<UserAgreement>.Register(e => e.Version);

        /// <summary>
        /// 版本
        /// </summary>
        public int Version
        {
            get { return this.GetProperty(VersionProperty); }
            set { this.SetProperty(VersionProperty, value); }
        }
		#endregion

        #region 版本号 VersionNoDisplay
        /// <summary>
        /// 版本号
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> VersionNoDisplayProperty = P<UserAgreement>.RegisterExtensionReadOnly("VersionNoDisplay", typeof(UserAgreement),
            GetVersionNoDisplay, UserAgreement.VersionProperty);

		/// <summary>
		/// 版本号
		/// </summary>
		public string VersionNoDisplay
		{
			get { return this.GetProperty(VersionNoDisplayProperty); }
			set { this.SetProperty(VersionNoDisplayProperty, value); }
		}

		/// <summary>
		/// 版本号
		/// </summary>
		public static string GetVersionNoDisplay(UserAgreement me)
        {
			return String.Format("V{0:D4}", me.Version);
        }

        #endregion

        #region 协议类型 AgreementType
        /// <summary>
        /// 协议类型
        /// </summary>
        [Label("协议类型")]
		public static readonly Property<AgreementType> AgreementTypeProperty = P<UserAgreement>.Register(e => e.AgreementType);

		/// <summary>
		/// 协议类型
		/// </summary>
		public AgreementType AgreementType
		{
			get { return GetProperty(AgreementTypeProperty); }
			set { SetProperty(AgreementTypeProperty, value); }
		}
		#endregion

		#region 是否启用 IsUse
		/// <summary>
		/// 是否启用
		/// </summary>
		[Label("是否启用")]
		public static readonly Property<bool> IsUseProperty = P<UserAgreement>.Register(e => e.IsUse);

		/// <summary>
		/// 是否启用
		/// </summary>
		public bool IsUse
		{
			get { return GetProperty(IsUseProperty); }
			set { SetProperty(IsUseProperty, value); }
		}
		#endregion

		#region 文件名称 FileName
		/// <summary>
		/// 文件名称
		/// </summary>
		[Required]
		[Label("文件名称")]
		public static readonly Property<string> FileNameProperty = P<UserAgreement>.Register(e => e.FileName);

		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName
		{
			get { return GetProperty(FileNameProperty); }
			set { SetProperty(FileNameProperty, value); }
		}
        #endregion

        #region 附件 UserAgreementDoc
        /// <summary>
        /// 附件
        /// </summary>
        [Label("附件")]
        [JsonIgnore]
        public static readonly ListProperty<EntityList<UserAgreementAttachment>> UserAgreementDocProperty = P<UserAgreement>.RegisterList(e => e.UserAgreementDoc);

        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<UserAgreementAttachment> UserAgreementDoc
        {
            get { return this.GetLazyList(UserAgreementDocProperty); }
        }
        #endregion
    }

	/// <summary>
	/// 用户协议表 实体配置
	/// </summary>
	internal class UserAgreementConfig : EntityConfig<UserAgreement>
	{
		protected override void ConfigMeta()
		{
			Meta.MapTable("User_Agreement").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}