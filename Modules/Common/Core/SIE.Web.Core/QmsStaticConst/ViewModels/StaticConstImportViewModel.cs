using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Core.QmsStaticConst.ViewModels
{
	/// <summary>
	/// Static常用参数导入ViewModel
	/// </summary>
	[RootEntity, Serializable]
	public class StaticConstImportViewModel : ViewModel
    {
		#region ErrorMessage 导入失败原因

		/// <summary>
		/// 导入失败原因
		/// </summary>
		[Label("导入失败原因")]
		public static readonly Property<string> ErrorMessageProperty = P<StaticConstImportViewModel>.Register(e => e.ErrorMessage);

		/// <summary>
		/// 导入失败原因
		/// </summary>
		public string ErrorMessage
		{
			get { return this.GetProperty(ErrorMessageProperty); }
			set { this.SetProperty(ErrorMessageProperty, value); }
		}

		#endregion

		#region SheetName Sheet

		/// <summary>
		/// Sheet
		/// </summary>
		[Label("Sheet")]
		public static readonly Property<string> SheetNameProperty = P<StaticConstImportViewModel>.Register(e => e.SheetName);

		/// <summary>
		/// Sheet
		/// </summary>
		public string SheetName
		{
			get { return this.GetProperty(SheetNameProperty); }
			set { this.SetProperty(SheetNameProperty, value); }
		}

		#endregion

		#region 主表

		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Label("编码")]
		[Required]
		[NotDuplicate]
		public static readonly Property<string> CodeProperty = P<StaticConstImportViewModel>.Register(e => e.Code);

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
		[Required]
		[NotDuplicate]
		public static readonly Property<string> NameProperty = P<StaticConstImportViewModel>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#endregion


		#region SPC常数表

		#region 样本数 SampleQty
		/// <summary>
		/// n
		/// </summary>
		[Label("n")]
		public static readonly Property<int> SampleQtyProperty = P<StaticConstImportViewModel>.Register(e => e.SampleQty);

		/// <summary>
		/// n
		/// </summary>
		public int SampleQty
		{
			get { return GetProperty(SampleQtyProperty); }
			set { SetProperty(SampleQtyProperty, value); }
		}
		#endregion

		#region A2 A2
		/// <summary>
		/// A2
		/// </summary>
		[Label("A2")]
		public static readonly Property<double?> A2Property = P<StaticConstImportViewModel>.Register(e => e.A2);

		/// <summary>
		/// A2
		/// </summary>
		public double? A2
		{
			get { return GetProperty(A2Property); }
			set { SetProperty(A2Property, value); }
		}
		#endregion

		#region D2 D2
		/// <summary>
		/// D2
		/// </summary>
		[Label("D2")]
		public static readonly Property<double?> D2Property = P<StaticConstImportViewModel>.Register(e => e.D2);

		/// <summary>
		/// D2
		/// </summary>
		public double? D2
		{
			get { return GetProperty(D2Property); }
			set { SetProperty(D2Property, value); }
		}
		#endregion

		#region D3 D3
		/// <summary>
		/// D3
		/// </summary>
		[Label("D3")]
		public static readonly Property<double?> D3Property = P<StaticConstImportViewModel>.Register(e => e.D3);

		/// <summary>
		/// D3
		/// </summary>
		public double? D3
		{
			get { return GetProperty(D3Property); }
			set { SetProperty(D3Property, value); }
		}
		#endregion

		#region D4 D4
		/// <summary>
		/// D4
		/// </summary>
		[Label("D4")]
		public static readonly Property<double?> D4Property = P<StaticConstImportViewModel>.Register(e => e.D4);

		/// <summary>
		/// D4
		/// </summary>
		public double? D4
		{
			get { return GetProperty(D4Property); }
			set { SetProperty(D4Property, value); }
		}
		#endregion

		#region A3 A3
		/// <summary>
		/// A3
		/// </summary>
		[Label("A3")]
		public static readonly Property<double?> A3Property = P<StaticConstImportViewModel>.Register(e => e.A3);

		/// <summary>
		/// A3
		/// </summary>
		public double? A3
		{
			get { return GetProperty(A3Property); }
			set { SetProperty(A3Property, value); }
		}
		#endregion

		#region C4 C4
		/// <summary>
		/// C4
		/// </summary>
		[Label("C4")]
		public static readonly Property<double?> C4Property = P<StaticConstImportViewModel>.Register(e => e.C4);

		/// <summary>
		/// C4
		/// </summary>
		public double? C4
		{
			get { return GetProperty(C4Property); }
			set { SetProperty(C4Property, value); }
		}
		#endregion

		#region B3 B3
		/// <summary>
		/// B3
		/// </summary>
		[Label("B3")]
		public static readonly Property<double?> B3Property = P<StaticConstImportViewModel>.Register(e => e.B3);

		/// <summary>
		/// B3
		/// </summary>
		public double? B3
		{
			get { return GetProperty(B3Property); }
			set { SetProperty(B3Property, value); }
		}
		#endregion

		#region B4 B4
		/// <summary>
		/// B4
		/// </summary>
		[Label("B4")]
		public static readonly Property<double?> B4Property = P<StaticConstImportViewModel>.Register(e => e.B4);

		/// <summary>
		/// B4
		/// </summary>
		public double? B4
		{
			get { return GetProperty(B4Property); }
			set { SetProperty(B4Property, value); }
		}
		#endregion

		#endregion
	}
}
