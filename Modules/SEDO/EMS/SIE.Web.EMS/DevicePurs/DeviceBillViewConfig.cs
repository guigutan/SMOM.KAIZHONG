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
    internal class DeviceBillViewConfig : WebViewConfig<DeviceBill>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.DevicePurs.Behaviors.DevicePurToolBarBehavior");
            View.UseCommands(typeof(SelBillCommand).FullName, WebCommandNames.Delete);
            View.UseCommand("SIE.Web.EMS.DevicePurs.Commands.EquipSearchRepairCommand");
            View.DisableEditing();
            View.Property(p => p.EquipAccountCode);
            View.Property(p => p.EquipAccountName);
            View.Property(p => p.ModelCode);
            View.Property(p => p.ModelName);
            View.Property(p => p.EquipTypeCode);
            View.Property(p => p.EquipTypeName);
            View.Property(p => p.Resource);
            View.Property(p => p.Process);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
        }

    }
}