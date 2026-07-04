using SIE.MES.WorkOrders;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联工单条码
    /// </summary>
    [Command(ImageName = "Link", Label = "关联条码", Hierarchy = "条码", GroupType = CommandGroupType.Edit)]
    public class UnionBarcodeCommand : ListViewCommand
    {
        /// <summary>
        /// 工单关联条码ViewModel
        /// </summary>
        protected WorkOrderUnionBarcode viewModel;

        /// <summary>
        /// 判断命令是否可执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view == null || view.Current == null || view.SelectedEntities.Count != 1)
                return false;
            var workOrder = view.Current as WorkOrder;
            return workOrder != null && workOrder.Type == Core.WorkOrders.WorkOrderType.Rework;
        }

        /// <summary>
        /// 执行打开关联条码操作界面
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var workOrder = View.Current as WorkOrder;
            var retrospectType = RT.Service.Resolve<WorkOrderController>().GetRetrospectType(workOrder.ProductId);
            if (retrospectType != null && retrospectType == Core.Items.RetrospectType.Batch)
            {
                CRT.MessageService.ShowError("工单[{0}]的产品[{1}]是批次类型，不能关联条码！".L10nFormat(workOrder.No, workOrder.Product.Name));
                return;
            }

            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(WorkOrderUnionBarcode), workOrder);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N() + " {0}".FormatArgs(workOrder.No);
                var template = new UnionBarcodeUITemplate(workOrder);
                template.ModuleKey = view.ModuleKey;
                var ui = template.CreateUI();
                ////退出时，数据已被修改且未保存时，提示用户
                v.Closing += (o, e) =>
                {
                    var vm = ui.MainView.Data as WorkOrderUnionBarcode;
                    if (vm != null && vm.BarcodeList.Any(p => p.CodeState == SIE.MES.WorkOrders.Reworks.CodeState.NotAssociated))
                    {
                        if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()) == true)
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                };
                return ui;
            });
        }
    }
}