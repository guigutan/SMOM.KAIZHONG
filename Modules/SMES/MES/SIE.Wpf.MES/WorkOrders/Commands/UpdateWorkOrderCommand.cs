using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 工单修改
    /// </summary>
    [Command(ImageName = "EditEntity", Label = "修改", Hierarchy = "工单生成", GroupType = CommandGroupType.Edit)]
    public class UpdateWorkOrderCommand : ListEditCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var current = view.Current as WorkOrder;
            return current != null && view.SelectedEntities.Count == 1 && current.IsPause == YesNo.Yes && current.State != Core.WorkOrders.WorkOrderState.CancelRelease;
        }

        /// <summary>
        /// 创建编辑工单
        /// </summary>
        /// <returns>工单</returns>
        protected override Entity GetEditEntity()
        {
            var wo = base.GetEditEntity() as WorkOrder;
            wo.PropertyChanged += RT.Service.Resolve<WorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
            return wo;
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <returns>控件结果</returns>
        protected override ControlResult CreateUI()
        {
            var template = new WorkOrderDetailTemplate(WorkOrderViewConfig.EditView);
            var ui = template.CreateUI();
            return ui;
        }
    }
}