using SIE.Core.UserAgreements;
using SIE.Web.Core.UserAgreements.Commands;
using System;

namespace SIE.Web.Core.UserAgreements
{
	/// <summary>
	/// 用户协议表视图配置
	/// </summary>
	internal class UserAgreementViewConfig : WebViewConfig<UserAgreement>
	{
		/// <summary>
		/// 自定义命令视图
		/// </summary>
		const string CustomCmdViewGroup = "CustomCmdViewGroup";

        protected override void ConfigView()
        {
			View.DeclareExtendViewGroup(CustomCmdViewGroup);
			if (ViewGroup == CustomCmdViewGroup)
				ConfigCustomCmdView();

		}
		/// <summary>
		/// 配置自定义命令权限
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
        private void ConfigCustomCmdView()
        {
			View.UseCommands(typeof(EnableAgreementCommand).FullName, typeof(UploadAgreementCommand).FullName);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
		{ 	  
			View.UseDefaultCommands();
			View.ClearCommands();
			View.Property(p => p.VersionNoDisplay);
			View.Property(p => p.AgreementType);
			View.Property(p => p.IsUse);
			View.Property(p => p.FileName);

		}
	}
}
