using DevExpress.Xpf.Core;
using SIE.Andon.Andons;
using SIE.Andon.Andons.ForWinform.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.PackingQcs;
using SIE.Security;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ConnectorPacking.Commands
{
    /// <summary>
    /// 替换
    /// </summary>
    [Command(ImageName = "", Label = "替  换", ToolTip = "替  换", GroupType = CommandGroupType.Edit)]
    public class ConnectorReplaceCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return true;
        }

        public override void Execute(DetailLogicalView view)
        {
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(ConnectorSnPackingViewModel));

            //var andonManage = RT.Service.Resolve<AndonManageController>().GetAndonManage(andonManageId);

            var template = new DetailsUITemplate(typeof(ConnectorSnPackingReplaceViewModel), ViewConfig.DetailsView, moduleKey);
            var ui = template.CreateUI();
            //var orderByList = andonManage.MessageSendList.OrderByDescending(a => a.MessageSendTime).AsEntityList();
            //andonManage.MessageSendList.Clear();
            //andonManage.MessageSendList.AddRange(orderByList);
            ConnectorSnPackingReplaceViewModel viewModel = new ConnectorSnPackingReplaceViewModel();

            ui.MainView.Data = viewModel;
            viewModel.FocuseBarcode();

            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "替换".L10N();
                w.Width = 1000;
                w.Height = 600;
                //var dc = (w as DialogContent);
                //dc.Loaded += (s, e) => { WipLayoutHelper.ResizeChildrenStyle(dc); };
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        try
                        {
                            if (viewModel.OldBatchNo.IsNullOrEmpty())
                                throw new ValidationException("要扫描原刻码!".L10N());
                            if (viewModel.NewBatchNo.IsNullOrEmpty())
                                throw new ValidationException("要扫描替换刻码!".L10N());
                            RT.Service.Resolve<ConnectorSnPackingController>().EngraveReplace(viewModel.OldBatchNo, viewModel.NewBatchNo);
                            viewModel.Reset(ResetType.CollectRestart);
                        }
                        catch (Exception ex)
                        {
                            e.Cancel = true;
                            viewModel.ShowError(ex.GetBaseException().Message);
                            //throw new ValidationException(ex.GetBaseException().Message);
                        }
                    }
                };
            });
        }
    }
}
