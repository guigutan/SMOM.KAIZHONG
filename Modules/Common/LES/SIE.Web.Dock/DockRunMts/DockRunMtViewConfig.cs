using SIE.Dock.DockRunMts;
using SIE.MetaModel.View;
using SIE.Web.Dock.DockRunMts.Commands;
using System.Collections.Generic;

namespace SIE.Web.Dock.DockRunMts
{
    /// <summary>
    /// 月台运行维护视图配置
    /// </summary>
    internal class DockRunMtViewConfig : WebViewConfig<DockRunMt>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(SaveDockRunMtCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.DockMaintainId).UsePagingLookUpEditor((c, p) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(p.DockMaintainName), nameof(p.DockMaintain.Name));
                    c.DicLinkField = dic;
                    c.ReloadDataOnPopping = true;
                });
                View.Property(p => p.DockMaintainName).Readonly();
                View.Property(p => p.Remark);
                View.ChildrenProperty(p => p.WorkTimeList).OrderNo = 1;
                View.ChildrenProperty(p => p.ExcepTimeList).OrderNo = 2;
            }
        }
    }
}