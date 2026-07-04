using SIE.Domain;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.WipResources;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 添加生产资源命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加自定义", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class WipResourceAddCommand : ListAddCommand
    {
        /// <summary>
        /// 创建编辑项--生成ResourceNo、Scheme
        /// </summary>
        /// <returns>生产资源</returns>
        protected override Entity CreateNewItem()
        {
            var item = base.CreateNewItem() as WipResource;
            item.Code = RT.Service.Resolve<WipResourceController>().GetResourceNo();
            item.ResourceState = ResourceState.Unused;
            item.Scheme = RT.Service.Resolve<CalendarSchemeController>().GetDefaultCalendar();
            //item.ResourceType = ResourceType.ProcessingCell;
            item.Qty = 1;
            item.TaktTime = 1;
            //item.SourceType = SyncSourceType.Custom;
            return item;
        }
    }
}
