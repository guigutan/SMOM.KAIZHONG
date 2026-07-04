using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Resources.Employees
{
	/// <summary>
	/// 员工和签名
	/// </summary>
	[RootEntity, Serializable]	
	[Label("员工和签名")]
	public partial class EmployeeSign : DataEntity
	{
		#region 版本号 VersionNo
		/// <summary>
		/// 版本号
		/// </summary>
		[Required]
		[Label("版本号")]
		public static readonly Property<string> VersionNoProperty = P<EmployeeSign>.Register(e => e.VersionNo);

		/// <summary>
		/// 版本号
		/// </summary>
		public string VersionNo
		{
			get { return GetProperty(VersionNoProperty); }
			set { SetProperty(VersionNoProperty, value); }
		}
		#endregion

		#region 签名图片 SingPhoto
		/// <summary>
		/// 签名图片
		/// </summary>
		[Required]
		[Label("签名图片")]
		public static readonly Property<byte[]> SingPhotoProperty = P<EmployeeSign>.Register(e => e.SingPhoto);

		/// <summary>
		/// 签名图片
		/// </summary>
		public byte[] SingPhoto
		{
			get { return GetProperty(SingPhotoProperty); }
			set { SetProperty(SingPhotoProperty, value); }
		}
		#endregion

		#region 图片名称 PhotoName
		/// <summary>
		/// 图片名称
		/// </summary>
		[Required]
		[Label("图片名称")]
		public static readonly Property<string> PhotoNameyProperty = P<EmployeeSign>.Register(e => e.PhotoName);

		/// <summary>
		/// 图片名称
		/// </summary>
		public string PhotoName
		{
			get { return GetProperty(PhotoNameyProperty); }
			set { SetProperty(PhotoNameyProperty, value); }
		}
		#endregion

		#region 文件后缀名 FileSuffix
		/// <summary>
		/// 文件后缀名
		/// </summary>
		[Required]
		[Label("文件后缀名")]
		public static readonly Property<string> FileSuffixProperty = P<EmployeeSign>.Register(e => e.FileSuffix);

		/// <summary>
		/// 文件后缀名
		/// </summary>
		public string FileSuffix
		{
			get { return GetProperty(FileSuffixProperty); }
			set { SetProperty(FileSuffixProperty, value); }
		}
		#endregion

		#region 启用人 EnableBy
		/// <summary>
		/// 启用人
		/// </summary>
		[Label("启用人")]
		public static readonly Property<string> EnableByProperty = P<EmployeeSign>.Register(e => e.EnableBy);

		/// <summary>
		/// 启用人
		/// </summary>
		public string EnableBy
		{
			get { return GetProperty(EnableByProperty); }
			set { SetProperty(EnableByProperty, value); }
		}
		#endregion

		#region 启用时间 EnableDate
		/// <summary>
		/// 启用时间
		/// </summary>
		[Label("启用时间")]
		public static readonly Property<DateTime?> EnableDateProperty = P<EmployeeSign>.Register(e => e.EnableDate);

		/// <summary>
		/// 启用时间
		/// </summary>
		public DateTime? EnableDate
		{
			get { return GetProperty(EnableDateProperty); }
			set { SetProperty(EnableDateProperty, value); }
		}
		#endregion

		#region 停用人 StopBy
		/// <summary>
		/// 停用人
		/// </summary>
		[Label("停用人")]
		public static readonly Property<string> StopByProperty = P<EmployeeSign>.Register(e => e.StopBy);

		/// <summary>
		/// 停用人
		/// </summary>
		public string StopBy
		{
			get { return GetProperty(StopByProperty); }
			set { SetProperty(StopByProperty, value); }
		}
		#endregion

		#region 停用时间 StopDate
		/// <summary>
		/// 停用时间
		/// </summary>
		[Label("停用时间")]
		public static readonly Property<DateTime?> StopDateProperty = P<EmployeeSign>.Register(e => e.StopDate);

		/// <summary>
		/// 停用时间
		/// </summary>
		public DateTime? StopDate
		{
			get { return GetProperty(StopDateProperty); }
			set { SetProperty(StopDateProperty, value); }
		}
		#endregion

		#region 状态 State
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<bool> StateProperty = P<EmployeeSign>.Register(e => e.State);

		/// <summary>
		/// 状态 true-启用 false 禁用
		/// </summary>
		public bool State
		{
			get { return this.GetProperty(StateProperty); }
			set { this.SetProperty(StateProperty, value); }
		}
		#endregion

		#region 员工 Employee
		/// <summary>
		/// 员工Id
		/// </summary>
		public static readonly IRefIdProperty EmployeeIdProperty = P<EmployeeSign>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

		/// <summary>
		/// 员工Id
		/// </summary>
		public double EmployeeId
		{
			get { return (double)GetRefId(EmployeeIdProperty); }
			set { SetRefId(EmployeeIdProperty, value); }
		}

		/// <summary>
		/// 员工
		/// </summary>
		public static readonly RefEntityProperty<Employees.Employee> EmployeeProperty = P<EmployeeSign>.RegisterRef(e => e.Employee, EmployeeIdProperty);

		/// <summary>
		/// 员工
		/// </summary>
		public Employees.Employee Employee
		{
			get { return GetRefEntity(EmployeeProperty); }
			set { SetRefEntity(EmployeeProperty, value); }
		}
		#endregion

	}

	/// <summary>
	/// 员工和签名 实体配置
	/// </summary>
	internal class EmployeeSignConfig : EntityConfig<EmployeeSign>
	{
		protected override void ConfigMeta()
		{
			Meta.MapTable("RES_EMP_SIGN").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}