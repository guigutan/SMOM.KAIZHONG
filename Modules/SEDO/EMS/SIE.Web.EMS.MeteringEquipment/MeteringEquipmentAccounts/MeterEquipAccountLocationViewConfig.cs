using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.MetaModel.View;
using SIE.Web.Common.Sort.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量设备台账位置视图配置
    /// </summary>
    public class MeterEquipAccountLocationViewConfig : WebViewConfig<MeterEquipAccountLocation>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DomainName("设备台账位置");
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.UseCommands(typeof(MoveTopCommand).FullName, typeof(MoveUpCommand).FullName, typeof(MoveDownCommand).FullName, typeof(MoveBottomCommand).FullName);
            View.Property(p => p.Subarea);
            View.Property(p => p.BigStance);
            View.Property(p => p.Stance);
            View.Property(p => p.StanceType);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
