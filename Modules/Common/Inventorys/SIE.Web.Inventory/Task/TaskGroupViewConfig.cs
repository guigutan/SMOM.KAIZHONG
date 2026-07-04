using SIE.Core.Enums;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.Inventory.Task;
using SIE.MetaModel.View;

namespace SIE.Web.Inventory.Task
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class TaskGroupViewConfig : WebViewConfig<TaskGroup>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.No).UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}组号", "单号生成规则", "任务组"); }).ShowInList(150);
            View.Property(p => p.BillNo).ShowInList(150);
            View.Property(p => p.CountDimension);
            View.Property(p => p.Warehouse);
            View.Property(p => p.OrderType);
            View.Property(p => p.StorageArea);
            View.Property(p => p.StorageLocation);
            View.AttachChildrenProperty(typeof(TaskManagement), w =>
            {
                var args = w as ChildPagingDataArgs;
                var taskGroup = args.Parent as TaskGroup;
                if (taskGroup == null) return new EntityList<TaskManagement>();
                return RT.Service.Resolve<TaskGroupController>().GetTaskDatas(null, taskGroup?.Id, null, args.PagingInfo, args.SortInfo);
            }, TaskManagementViewConfig.TaskManagementAttachView).HasLabel("任务明细");
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.BillNo);
            View.Property(p => p.CountDimension);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            View.Property(p => p.CreateBy);
            View.Property(p => p.Warehouse);
            View.Property(p => p.OrderType).UseSelectEnumEditor(p =>
            {
                p.AllowBlank = true;
                p.ValuesList.Add((int)OrderType.StandardCount);
                p.ValuesList.Add((int)OrderType.AccountCount);
                p.ValuesList.Add((int)OrderType.RandomCount);
                p.ValuesList.Add((int)OrderType.DifferenceCount);
                p.ValuesList.Add((int)OrderType.WavePick);
            });
            View.Property(p => p.StorageArea);
            View.Property(p => p.StorageLocation);
        }
    }
}
