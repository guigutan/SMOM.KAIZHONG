using SIE;
using SIE.Web.Common;
using SIE.Rbac.Users;
using System.Collections.Generic;
using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.MetaModel.View;
using SIE.EMS.Equipments.Accounts;
using SIE.Web.EMS.DevicePurs.Commands;

namespace SIE.Web.EMS.DevicePurs
{

	/// <summary>
	/// 设备清单视图配置
	/// </summary>
	internal class DeviceDepaViewConfig : WebViewConfig<DeviceDepa>
	{
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
		{
			View.UseCommands(typeof(SelDepaCommand).FullName, WebCommandNames.Delete);
			View.DisableEditing();
			View.Property(p => p.EnterpriseCode);
			View.Property(p => p.EnterpriseName);
			View.Property(p => p.CreateByName);
			View.Property(p => p.CreateDate);
			View.Property(p => p.UpdateByName);
			View.Property(p => p.UpdateDate);
		}
		
	}
}