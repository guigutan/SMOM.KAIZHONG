using DevExpress.CodeParser;
using DevExpress.Xpf.Editors;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Reflection;
using SIE.Resources.Employees;
using SIE.Security;
using SIE.Wpf.Command;
using SIE.Wpf.Workbench;
using System;
using System.Linq;

namespace SIE.Wpf.Andon.Commands
{

    /// <summary>
    /// 安灯事件响应命令
    /// </summary>
    [Command(ImageName = "ArrowRightDropCircleOutline", Label = "响应", ToolTip = "响应", GroupType = CommandGroupType.Edit)]
    public class AndonManageResponseCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;
            return andonManage != null && andonManage.State == AndonManageState.Standby;
        }

        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            Employee employee = view.Current as Employee;
            var model = new AndonEmpViewModel();
            var template = new DetailsUITemplate<AndonEmpViewModel>();
            template.ViewGroup = ViewConfig.DetailsView;
            var ui = template.CreateUI();
            var textEdit = ui.MainView.LayoutControl.GetLogicalChild<TextEdit>();
            ui.MainView.Data = model;
            var result = CRT.Workbench.ShowDialog(ui, w =>
             {
                 w.Title = "扫描响应人".L10N();
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
                            employee = RT.Service.Resolve<AndonManageController>().EmpId(empNo);
                            if (employee == null)
                            {
                                throw new ValidationException("员工号输入错误".L10N());
                            }
                            var am = view.Current as AndonManage;
                            var andonGroupDetails = RT.Service.Resolve<AndonController>().GetAndonGroupDetailsByAndonManageId(am.Id, am.WipResource?.AndonUpholdId ?? 0);
                            if (andonGroupDetails.All(p => p.UserCode != empNo))
                            {
                                throw new ValidationException("该员工不属于安灯责任组，无响应权限".L10N());
                            }
                            if (andonGroupDetails.Any(p => p.UserCode == empNo && p.UserState == Domain.State.Disable))
                            {
                                throw new ValidationException("该员工已离职，无法进行响应操作".L10N());
                            }
                        }
                        catch (Exception ex)
                        {
                            CRT.MessageService.ShowError(ex.GetBaseException().Message);
                            e.Cancel = true;
                            //设置为空，后面就不会继续运行
                            employee = null;
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

            if (employee == null)
            {
                return;
                //throw new ValidationException("员工号输入错误！".L10N());
            }
            #region
            var andonManage = view.Current as AndonManage;

            var andonManageId = andonManage.Id;
            var nowHandler = RT.Identity;
            var oldHandler = andonManage.Handler;
            var reason = "";
            if (oldHandler != null)
            {
                reason = "处理人由" + andonManage.Handler.Name + "变更为" + nowHandler.Name;
            }
            else
            {
                reason = "处理人更新为" + nowHandler.Name;
            }
            RT.Service.Resolve<AndonManageController>().AndonManageResponse(andonManageId, AndonManageOperateType.Response, reason.L10N(), employee.Id, isCs: true);
            ClientRuntime.MessageService.ShowMessage("响应成功".L10N());
            andonManage.State = AndonManageState.Processing;
            #endregion
        }
    }
}
