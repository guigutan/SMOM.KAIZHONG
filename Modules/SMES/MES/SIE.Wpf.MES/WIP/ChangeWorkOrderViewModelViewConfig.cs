using SIE.Domain;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 工单切换视图模型 视图配置
    /// </summary>
    public class ChangeWorkOrderViewModelViewConfig : WPFViewConfig<ChangeWorkOrderViewModel>
    {
        /// <summary>
        /// 详细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(columnCount: 1);

            View.Property(p => p.WorkOrder).HasLabel("工单号").UseDataSource((e, p, s) =>
                    {
                        var result = new EntityList<WorkOrder>();

                        ChangeWorkOrderViewModel vm = e as ChangeWorkOrderViewModel;

                        if (vm == null)
                        {
                            return result;
                        }

                        var workCell = vm.Workcell;
                        if (workCell == null || workCell.ResourceId == 0)
                        {
                            return result;
                        }

                        return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, s, workCell.ResourceId);
                    })
                .UsePagingLookUpEditor(e => e.ReloadDataOnPopping = true).ShowInDetail();

            View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail().Readonly();
            View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail().Readonly();
            View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail().Readonly();
        }
    }
}
