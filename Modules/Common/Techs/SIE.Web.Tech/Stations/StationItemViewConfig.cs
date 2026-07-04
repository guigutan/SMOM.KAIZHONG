using SIE.MetaModel.View;
using SIE.Tech.Stations;
using System.Collections.Generic;

namespace SIE.Web.Tech.Stations
{
    /// <summary>
    /// 工位物料视图配置
    /// </summary>
    internal class StationItemViewConfig : WebViewConfig<StationItem>
    {

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(Station));
            View.RemoveCommands();
            View.UseCommands("SIE.Web.Tech.Stations.Commands.StationItemAddCommand", WebCommandNames.Edit, WebCommandNames.Delete, 
                "SIE.Web.Tech.Stations.Commands.StationItemImportCommand",
                 WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                m.DicLinkField = dic;
            }).ShowInList(150);
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.Warning).UseSpinEditor(e => { e.MinValue = 1; });
            View.Property(p => p.Capacity).UseSpinEditor(e => { e.MinValue = 1; });
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

        /// <summary>
        /// 导入模板
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.Warning);
            View.Property(p => p.Capacity);
        }
    }
}
