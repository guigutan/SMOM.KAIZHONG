using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息自动推送记录
    /// </summary>
    [RootEntity, Serializable]
	[Label("异常信息自动推送记录")]
	public partial class SendLog : DataEntity
	{
		#region 异常信息 AbnormalInfor
		/// <summary>
		/// 异常信息Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalInforIdProperty = P<SendLog>.RegisterRefId(e => e.AbnormalInforId, ReferenceType.Normal);

		/// <summary>
		/// 异常信息Id
		/// </summary>
		public double AbnormalInforId
		{
			get { return (double)GetRefId(AbnormalInforIdProperty); }
			set { SetRefId(AbnormalInforIdProperty, value); }
		}

		/// <summary>
		/// 异常信息
		/// </summary>
		public static readonly RefEntityProperty<AbnormalInfor> AbnormalInforProperty = P<SendLog>.RegisterRef(e => e.AbnormalInfor, AbnormalInforIdProperty);

		/// <summary>
		/// 异常信息
		/// </summary>
		public AbnormalInfor AbnormalInfor
		{
			get { return GetRefEntity(AbnormalInforProperty); }
			set { SetRefEntity(AbnormalInforProperty, value); }
		}
		#endregion

		#region 推送升级设置 SenderSetting
		/// <summary>
		/// 推送升级设置Id
		/// </summary>
		public static readonly IRefIdProperty SenderSettingIdProperty = P<SendLog>.RegisterRefId(e => e.SenderSettingId, ReferenceType.Normal);

		/// <summary>
		/// 推送升级设置Id
		/// </summary>
		public double SenderSettingId
		{
			get { return (double)GetRefId(SenderSettingIdProperty); }
			set { SetRefId(SenderSettingIdProperty, value); }
		}

		/// <summary>
		/// 推送升级设置
		/// </summary>
		public static readonly RefEntityProperty<DefinitionSenderSettings> SenderSettingProperty = P<SendLog>.RegisterRef(e => e.SenderSetting, SenderSettingIdProperty);

		/// <summary>
		/// 推送升级设置
		/// </summary>
		public DefinitionSenderSettings SenderSetting
		{
			get { return GetRefEntity(SenderSettingProperty); }
			set { SetRefEntity(SenderSettingProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 异常信息自动推送记录 实体配置
	/// </summary>
	internal class SendLogConfig : EntityConfig<SendLog>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("QMS_SENDLOG").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}