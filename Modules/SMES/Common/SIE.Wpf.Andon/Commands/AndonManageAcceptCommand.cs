using DevExpress.Xpf.Editors;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Wpf.Command;
using SIE.Wpf.Workbench;
using System;
using System.Linq;

namespace SIE.Wpf.Andon.Commands
{
    /// <summary>
    /// 安灯事件验收命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "验收", ToolTip = "验收", GroupType = CommandGroupType.Edit)]
    public class AndonManageAcceptCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;
            return andonManage != null && andonManage.State == AndonManageState.ToAccepted;
        }

        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;

            //弹窗填写实际影响时间：大于0，保留1位小数，默认为【当前时间减去触发时间】
            andonManage.ActualTime = RT.Service.Resolve<AndonManageController>().ComputerActualTime(andonManage.Id);

            var template = new DetailsUITemplate(typeof(AndonManage),
                AndonManageViewConfig.AcceptViewGroup, view.ModuleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = andonManage;

            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "验收".L10N();
                w.Height = 200;
                w.Width = 500;
                var dc = (w as DialogContent);
                dc.Loaded += (s, e) => { WipLayoutHelper.ResizeChildrenStyle(dc); };
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        try
                        {

                            var model = new AndonEmpViewModel();
                            var template = new DetailsUITemplate<AndonEmpViewModel>();
                            template.ViewGroup = ViewConfig.DetailsView;
                            var ui = template.CreateUI();
                            var textEdit = ui.MainView.LayoutControl.GetLogicalChild<TextEdit>();
                            ui.MainView.Data = model;
                            var result = CRT.Workbench.ShowDialog(ui, w =>
                            {
                                w.Title = "扫描验收人".L10N();
                                w.Height = 200;
                                w.Width = 500;
                                var dc = (w as DialogContent);
                                dc.Loaded += (s, e) => { WipLayoutHelper.ResizeChildrenStyle(dc); };
                                w.Closing += (s, e) =>
                                {
                                    if (w.Result == 0)
                                    {
                                        try
                                        {
                                            var AndomData = ui.MainView.Data;
                                            var empNo = ((AndonEmpViewModel)AndomData).AndonEmpNo;
                                            var employee = RT.Service.Resolve<AndonManageController>().EmpId(empNo);
                                            if (employee == null)
                                            {
                                                throw new ValidationException("员工号输入错误".L10N());
                                                //CRT.MessageService.ShowError("员工号输入错误".L10N());
                                                //e.Cancel = true;
                                            }
                                            var am = view.Current as AndonManage;
                                            var andonGroupDetails = RT.Service.Resolve<AndonController>().GetAndonGroupDetailsByAndonManageId(am.Id, andonManage.WipResource.AndonUpholdId ?? 0);
                                            if (andonGroupDetails.All(p => p.UserCode != empNo))
                                            {
                                                throw new ValidationException("该员工不属于安灯责任组，无验收权限".L10N());
                                                //CRT.MessageService.ShowError("该员工不属于安灯责任组，无响应权限".L10N());
                                                //e.Cancel = true;
                                            }
                                            if (andonGroupDetails.Any(p => p.UserCode == empNo && (p.IsAcceptancer == false || p.IsAcceptancer == null)))
                                            {
                                                throw new ValidationException("该员工不属于安灯责任组验收人，无验收权限".L10N());
                                            }
                                            if (andonGroupDetails.Any(p => p.UserCode == empNo && p.UserState == Domain.State.Disable))
                                            {
                                                throw new ValidationException("该员工已离职，无法进行响应操作".L10N());
                                                //CRT.MessageService.ShowError("该员工已离职，无法进行响应操作".L10N());
                                                //e.Cancel = true;
                                            }
                                            //textEdit?.Focus();

                                            RT.Service.Resolve<AndonManageController>()
                                            .AndonManageCheck(andonManage.Id, AndonManageOperateType.Check, "验收成功", andonManage.ActualTime, isCs: true, employee.Id);

                                            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Closed;
                                        }
                                        catch (Exception ex)
                                        {
                                            CRT.MessageService.ShowError(ex.GetBaseException().Message.L10N());
                                            e.Cancel = true;
                                        }
                                        finally
                                        {
                                            textEdit?.Focus();
                                        }
                                    }
                                };
                                CRT.MainThread.InvokeIfRequired(() =>
                                {
                                    textEdit?.Focus();
                                });
                            });
                        }
                        catch (Exception ex)
                        {
                            ClientRuntime.MessageService.ShowException(ex);
                            e.Cancel = true;
                        }
                    }
                };
            });

        }
    }
}
