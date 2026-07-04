using SIE.Resources.Employees;
using SIE.Web.Resources.Employees.Commands;

namespace SIE.WPF.Resources.Employees
{
	/// <summary>
	/// 员工和签名视图配置
	/// </summary>
	internal class EmployeeSignViewConfig : WebViewConfig<EmployeeSign>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.AssignAuthorize(typeof(Employee));
		}
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
        {
            View.ClearCommands();
			View.UseCommands(typeof(EnableSignCommand).FullName);
			//View.UseCommands(typeof(DeleteSignCommand).FullName);
			View.UseCommands(typeof(SignShowImageCommand).FullName);
			View.UseCommands(typeof(EmployeeUploadImageCommand).FullName);
			View.Property(p => p.VersionNo).Readonly();
			View.Property(p => p.State).Readonly().HasLabel("当前使用");
			View.Property(p => p.EnableBy).Readonly();
			View.Property(p => p.EnableDate).Readonly();
			View.Property(p => p.StopBy).Readonly();
			View.Property(p => p.StopDate).Readonly();
		}
		
		///<summary>
		/// 配置明细视图
		/// </summary>
		//protected override void ConfigDetailsView()
		///{

		///}
		
		///<summary>
		/// 配置下拉视图
		/// </summary>
		///protected override void ConfigSelectionView()
		///{
			
		///} 
	}
}
