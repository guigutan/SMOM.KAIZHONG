using SIE;
using SIE.Core.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.YardMaintains
{
	/// <summary>
	/// 园区维护
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(YardMaintainCriteria))]
    [Label("园区维护")]
    [DisplayMember(nameof(Name))]
    public partial class YardMaintain : BaseRegionalInfo, IStateEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<YardMaintain>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 名称 Name
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<YardMaintain>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion
		
		#region 状态 State
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<State> StateProperty = P<YardMaintain>.Register(e => e.State);

		/// <summary>
		/// 状态
		/// </summary>
		public State State
		{
			get { return this.GetProperty(StateProperty); }
			set { this.SetProperty(StateProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<YardMaintain>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 园区维护 实体配置
	/// </summary>
	internal class YardMaintainConfig : EntityConfig<YardMaintain>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("YARD_MAINTAIN").MapAllProperties();
			Meta.Property(YardMaintain.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(YardMaintain.AddressProperty).ColumnMeta.HasLength(4000);
            Meta.Property(YardMaintain.FullAddressProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
		}
	}
}