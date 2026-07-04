using DevExpress.Xpf.Editors;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Domain.Validation;
using SIE.Wpf.Command;
using SIE.Wpf.Workbench;
using System;

namespace SIE.Wpf.Andon.Commands
{
    /// <summary>
    /// 安灯事件取消命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "取消", ToolTip = "取消", GroupType = CommandGroupType.Edit)]
    public class AndonManageCanelCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;
            return andonManage != null && andonManage.State == SIE.Andon.Andons.Enum.AndonManageState.Standby;
        }

        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            AndonManageOperateLog operateLog = new AndonManageOperateLog();

            var template = new DetailsUITemplate(typeof(AndonManageOperateLog),
                AndonManageOperateLogViewConfig.CancelViewGroup, view.ModuleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = operateLog;

            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "取消".L10N();
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
                            if (operateLog.Remark.IsNullOrEmpty())
                            {
                                ClientRuntime.MessageService.ShowError("【取消原因】必须输入！".L10N());
                                e.Cancel = true;
                                return;
                            }

                            var model = new AndonEmpViewModel();
                            var template = new DetailsUITemplate<AndonEmpViewModel>();
                            template.ViewGroup = ViewConfig.DetailsView;
                            var ui = template.CreateUI();
                            var textEdit = ui.MainView.LayoutControl.GetLogicalChild<TextEdit>();
                            ui.MainView.Data = model;
                            var result = CRT.Workbench.ShowDialog(ui, w =>
                            {
                                w.Title = "扫描取消人".L10N();
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
                                            if (employee.EmployeeStatus == SIE.Resources.Employees.EmployeeStatus.UnJob)
                                                throw new ValidationException("员工号已离职无法进行取消".L10N());


                                            var andonManage = view.Current as AndonManage;

                                            RT.Service.Resolve<AndonManageController>()
                                                .AndonManageCancel(andonManage.Id, AndonManageOperateType.Cancel, operateLog.Remark, isCs: true, employee: employee);



                                            //更新状态
                                            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Cancel;
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
