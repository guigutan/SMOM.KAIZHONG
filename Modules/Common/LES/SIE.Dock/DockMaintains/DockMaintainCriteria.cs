using SIE.Dock.DockMaintains.Service;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockMaintains
{
	/// <summary>
	/// 月台维护查询实体
	/// </summary>
	[QueryEntity, Serializable]
	[Label("月台维护查询实体")]
	public partial class DockMaintainCriteria : Criteria
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<DockMaintainCriteria>.Register(e => e.Code);

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
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<DockMaintainCriteria>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
        #endregion

        #region 是否收货月台 IsReceive
        /// <summary>
        /// 是否收货月台
        /// </summary>
        [Label("是否收货月台")]
		public static readonly Property<bool?> IsReceiveProperty = P<DockMaintainCriteria>.Register(e => e.IsReceive);

		/// <summary>
		/// 是否收货月台
		/// </summary>
		public bool? IsReceive
		{
			get { return GetProperty(IsReceiveProperty); }
			set { SetProperty(IsReceiveProperty, value); }
		}
        #endregion

        #region 是否发货月台 IsShip
        /// <summary>
        /// 是否发货月台
        /// </summary>
        [Label("是否发货月台")]
		public static readonly Property<bool?> IsShipProperty = P<DockMaintainCriteria>.Register(e => e.IsShip);

		/// <summary>
		/// 是否发货月台
		/// </summary>
		public bool? IsShip
		{
			get { return GetProperty(IsShipProperty); }
			set { SetProperty(IsShipProperty, value); }
		}
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<DockMaintainCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        /// <summary>
		/// 查询逻辑
		/// </summary>
		/// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
		{
			return RT.Service.Resolve<DockMaintainService>().GetDockMaintains(this);
		}
	}
}
