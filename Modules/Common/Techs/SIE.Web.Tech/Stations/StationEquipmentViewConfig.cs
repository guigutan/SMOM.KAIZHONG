using SIE.MetaModel.View;
using SIE.Tech.Stations;
using SIE.Web.Tech.Stations.Commands;
using System.Collections.Generic;

namespace SIE.Web.Tech.Stations
{
    /// <summary>
    /// 工位设备视图配置
    /// </summary>
    internal class StationEquipmentViewConfig : WebViewConfig<StationEquipment>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(Station));
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(StationEquipmentSetMasterCommand).FullName);

            View.Property(p => p.EquipAccountId).HasLabel("设备编码").ShowInList(150)
                .UsePagingLookUpEditor((m, e) =>
                {
                    m.ReloadDataOnPopping = true;
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipAccountCode), nameof(e.EquipAccount.Code));
                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    m.DicLinkField = keyValues;
                });
            View.Property(p => p.EquipAccountName).ShowInList(150).Readonly();
            View.Property(p => p.IsMaster).ShowInList().Readonly();

        }

    }
}
