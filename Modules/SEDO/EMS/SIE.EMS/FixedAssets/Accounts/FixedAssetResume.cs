using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 资产履历
    /// </summary>
    [ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("资产履历")]
	public partial class FixedAssetResume : DataEntity
	{
		#region  ResumeType
		/// <summary>
		/// 类型
		/// </summary>
		[Label("类型")]
		public static readonly Property<ResumeType> ResumeTypeProperty = P<FixedAssetResume>.Register(e => e.ResumeType);

		/// <summary>
		/// 类型
		/// </summary>
		public ResumeType ResumeType
		{
			get { return GetProperty(ResumeTypeProperty); }
			set { SetProperty(ResumeTypeProperty, value); }
		}
		#endregion

		#region 管理状态 UseState 
		/// <summary>
		/// 管理状态
		/// </summary>
		[Label("管理状态")]
		public static readonly Property<AccountUseState> UseStateProperty = P<FixedAssetResume>.Register(e => e.UseState);

		/// <summary>
		/// 管理状态（原使用状态）
		/// </summary>
		public AccountUseState UseState
		{
			get { return GetProperty(UseStateProperty); }
			set { SetProperty(UseStateProperty, value); }
		}
		#endregion

		#region 关联单号 No
		/// <summary>
		/// 关联单号
		/// </summary>
		[Label("关联单号")]
		public static readonly Property<string> NoProperty = P<FixedAssetResume>.Register(e => e.No);

		/// <summary>
		/// 关联单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<FixedAssetResume>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 固定资产台账 FixedAssetsAccount
		/// <summary>
		/// 固定资产台账Id
		/// </summary>
		[Label("固定资产台账")]
		public static readonly IRefIdProperty FixdAccountIdProperty =
			P<FixedAssetResume>.RegisterRefId(e => e.FixdAccountId, ReferenceType.Parent);

		/// <summary>
		/// 固定资产台账Id
		/// </summary>
		public double FixdAccountId
		{
			get { return (double)this.GetRefId(FixdAccountIdProperty); }
			set { this.SetRefId(FixdAccountIdProperty, value); }
		}

		/// <summary>
		/// 固定资产台账
		/// </summary>
		public static readonly RefEntityProperty<FixedAssetsAccount> FixdAccountProperty =
			P<FixedAssetResume>.RegisterRef(e => e.FixdAccount, FixdAccountIdProperty);

		/// <summary>
		/// 固定资产台账
		/// </summary>
		public FixedAssetsAccount FixdAccount
		{
			get { return this.GetRefEntity(FixdAccountProperty); }
			set { this.SetRefEntity(FixdAccountProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 资产履历 实体配置
	/// </summary>
	internal class FixedAssetResumeConfig : EntityConfig<FixedAssetResume>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("FIXED_ASSET_RESUME").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}