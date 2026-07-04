using SIE.Barcodes;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Wpf.Barcodes.ViewModels;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Barcodes.Commonds
{
    /// <summary>
    /// 条码领用
    /// </summary>
    [Command(ImageName = "Receive", Label = "领用", GroupType = CommandGroupType.View)]
    public class ReceiveBarcodeCommand : ListSaveCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条码状态非已领用状态返回true,否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var barcodeRange = view?.Current as BarcodeRange;
            return barcodeRange != null && barcodeRange.State != ReceiveState.Received;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var range = view?.Current as BarcodeRange;
            if (range == null)
            {
                return;
            }

            var vm = new ReceiveBarcodeViewModel();
            var template = new DetailsUITemplate<ReceiveBarcodeViewModel>();
            var ui = template.CreateUI();
            ui.MainView.Data = vm;
            var result = CRT.Workbench.ShowDialog(ui, (v) =>
            {
                v.Title = "条码领用".L10N();
                v.Width = 300;
                v.Height = 200;
                v.Closing += (o, e) =>
                {
                    if (v.Result == 0)
                    {
                        try
                        {
                            var broken = vm.Validate();
                            if (broken.Count > 0)
                                throw new ValidationException(broken.ToString());
                            RT.Service.Resolve<BarcodeController>().BarcodeReceive(range.Id, vm.UserName, vm.Password);
                        }
                        catch (Exception ex)
                        {
                            var baseEx = ex.GetBaseException();
                            if (baseEx is AuthenticationException)
                            {
                                CRT.MessageService.ShowMessage((baseEx as AuthenticationException).Message);
                            }
                            else if (baseEx is ValidationException)
                            {
                                CRT.MessageService.ShowMessage((baseEx as ValidationException).Message);
                            }
                            else
                            {
                                CRT.MessageService.ShowMessage(ex.Message);
                            }

                            e.Cancel = true;
                        }
                    }
                };
            });
            if (result == 0)
            {
                view?.QueryView?.TryExecuteQuery();
                CRT.MessageService.ShowMessage("条码领用成功".L10N());
            }
        }
    }
}