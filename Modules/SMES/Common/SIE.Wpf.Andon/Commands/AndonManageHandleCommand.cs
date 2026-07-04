using DevExpress.Xpf.Editors;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.IOT;
using SIE.Domain.Validation;
using SIE.Resources.Employees;
using SIE.Wpf.Command;
using SIE.Wpf.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.Andon.Commands
{
    /// <summary>
    /// 安灯事件处理命令
    /// </summary>
    [Command(ImageName = "CheckmarkThick", Label = "处理完成", ToolTip = "处理完成", GroupType = CommandGroupType.Edit)]
    public class AndonManageHandleCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;
            return andonManage != null && andonManage.State == AndonManageState.Processing;
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
                w.Title = "扫描处理人".L10N();
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
                                throw new ValidationException("该员工不属于安灯责任组，无处理权限".L10N());
                            }
                            if (andonGroupDetails.Any(p => p.UserCode == empNo && p.UserState == Domain.State.Disable))
                            {
                                throw new ValidationException("该员工已离职，无法进行处理操作".L10N());
                            }
                        }
                        catch (Exception ex)
                        {
                            CRT.MessageService.ShowError(ex.GetBaseException().Message.L10N());
                            e.Cancel = true;
                            employee = null;
                        }
                        textEdit?.Focus();
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
            RT.Service.Resolve<AndonManageController>().AndonManageHandleAsync(andonManageId, AndonManageOperateType.Handle, "", "", "", "", employee.Id, isCs: true);
            
            ClientRuntime.MessageService.ShowMessage("处理完成成功".L10N());
            andonManage.State = AndonManageState.ToAccepted;
            #endregion
        }
    }
}
