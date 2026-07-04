using SIE.Items;
using SIE.MES.ProjectDesigns.ViewModels;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 工艺卡命令
    /// </summary>
    [Command(Label = "工艺卡", GroupType = 10, ImageName = "FileEye", ToolTip = "显示工艺资料", Location = CommandLocation.All)]
    public class CollectProjectCardCommand : DetailViewCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            DataCollectionViewModel collectViewModel = view.Current as DataCollectionViewModel;
            if (collectViewModel == null)
            {
                return false;
            }
            if (collectViewModel.WorkOrderId == null || collectViewModel.Workstation.ProcessId == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Execute(DetailLogicalView view)
        {
            DataCollectionViewModel collectViewModel = view.Current as DataCollectionViewModel;
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(DataCollectionViewModel), null);
            CRT.Workbench.ShowView(key, v => 
            {
                v.Title = "工艺资料".L10N();
                var template = new DetailsUITemplate<ProjectDesignCard>(view.ModuleKey);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                ProjectDesignCard card = new ProjectDesignCard
                {
                    WoNo = collectViewModel.WorkOrder.No,
                    ProductCode = collectViewModel.WorkOrder.ProductCode,
                    ProductName = collectViewModel.WorkOrder.ProductName,
                    ProductId = collectViewModel.WorkOrder.ProductId,
                    PlanQty = collectViewModel.WorkOrder.PlanQty,
                    WipResource = collectViewModel.WorkOrder.ResourceCode,
                    ProcessId = collectViewModel.Workstation.ProcessId ?? 0,
                    Process = collectViewModel.Workstation.ProcessId != null ? collectViewModel.Workstation.Process.Name : "",
                    ProjectId = collectViewModel.WorkOrder.ProjectMaintainId,

                    ProjectNo = collectViewModel.WorkOrder.ProjectMaintainId != null ? collectViewModel.WorkOrder.ProjectMaintainCode : "",
                };
                detailView.Data = card;
                return ui;
            });
        }
    }
}
