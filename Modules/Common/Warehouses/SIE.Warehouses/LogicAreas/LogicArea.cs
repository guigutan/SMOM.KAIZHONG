using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 逻辑分区
    /// </summary>
    [RootEntity, Serializable]
	[EntityWithConfig(typeof(NoConfig))]
	[ConditionQueryType(typeof(LogicAreaCriteria))]
	[Label("逻辑分区")]
	[DisplayMember(nameof(Code))]
	public partial class LogicArea : DataEntity, IStateEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[MaxLength(20)]
		[Label("编码")]
		[NotDuplicate]
		[Required]
		public static readonly Property<string> CodeProperty = P<LogicArea>.Register(e => e.Code);

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
		[MaxLength(80)]
		[Label("名称")]
		[NotDuplicate]
		[Required]
		public static readonly Property<string> NameProperty = P<LogicArea>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 描述 Description
		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(240)]
		[Label("描述")]
		public static readonly Property<string> DescriptionProperty = P<LogicArea>.Register(e => e.Description);

		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			get { return GetProperty(DescriptionProperty); }
			set { SetProperty(DescriptionProperty, value); }
		}
		#endregion

		#region 仓库 Warehouse
		/// <summary>
		/// 仓库Id
		/// </summary>
		[Label("仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<LogicArea>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 仓库Id
		/// </summary>
		
		public double WarehouseId
		{
			get { return (double)GetRefId(WarehouseIdProperty); }
			set { SetRefId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<LogicArea>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 启用/禁用
		/// <summary>
		/// 启用/禁用
		/// </summary>
		[Label("状态")]
		public static readonly Property<State> StateProperty = P<LogicArea>.Register(e => e.State);

		/// <summary>
		/// 启用/禁用
		/// </summary>
		public State State
		{
			get { return GetProperty(StateProperty); }

			set { SetProperty(StateProperty, value); }
		}
		#endregion

		#region 是否立库库区 IsAutomatedArea
		/// <summary>
		/// 是否立库库区
		/// </summary>
		[Label("是否立库库区")]
		public static readonly Property<bool> IsAutomatedAreaProperty = P<LogicArea>.Register(e => e.IsAutomatedArea);

		/// <summary>
		/// 是否立库库区
		/// </summary>
		public bool IsAutomatedArea
		{
			get { return this.GetProperty(IsAutomatedAreaProperty); }
			set { this.SetProperty(IsAutomatedAreaProperty, value); }
		}
		#endregion

		#region 仓库编码 WarehouseCode
		/// <summary>
		/// 仓库编码
		/// </summary>
		[Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<LogicArea>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

    }

	/// <summary>
	/// 逻辑分区 实体配置
	/// </summary>
	internal class LogicAreaConfig : EntityConfig<LogicArea>
	{
		protected override void ConfigMeta()
		{
			Meta.MapTable("WH_LOGIC_AREA").MapAllProperties();
			Meta.Property(LogicArea.CodeProperty).ColumnMeta.HasLength(40);
			Meta.Property(LogicArea.NameProperty).ColumnMeta.HasLength(160);
			Meta.Property(LogicArea.DescriptionProperty).ColumnMeta.HasLength(480);
			Meta.EnablePhantoms();
		}
	}
}