using DevExpress.DataProcessing;
using DevExpress.Xpf.CodeView;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Wpf.MES.Controls;
using System;
using System.Linq;
using System.Text;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 工作单元信息选择
    /// </summary>
    public static class KZWorkstationSelector
    {

        /// <summary>
        /// 选择工作单元
        /// </summary>
        /// <param name="workstation">工作单元</param>
        /// <returns>是否切换成功</returns>
        public static bool SelectOperation(KZWorkstation workstation, Workstation _workstation)
        {
            bool isSelectSuccess = false;

            var employeeId = RT.IdentityId;

            var wipResourcesOfEmployee = RT.Service.Resolve<WipResourceController>().GetWipResources(employeeId);
            var processes = RT.Service.Resolve<ProcessController>().GetProcesssByUserId(
                employeeId, null, workstation.ProcessTypes);

            var editor = WorkCellSelectorControlFactory.CreateKZControl();

            //产线列表
            editor.WipResourceList.AddRange(wipResourcesOfEmployee.OrderBy(p => p.Name));

            //工序
            editor.ProcessList.AddRange(processes.OrderBy(p => p.Name));

            //切换时，带出原来选择的
            if (workstation.ResourceId.HasValue)
            {
                var wipResource = editor.WipResourceList.FirstOrDefault(x => x.Id == workstation.ResourceId.Value);
                if (wipResource != null)
                {
                    editor.SelectedWipResource = wipResource;
                }
            }

            if (workstation.ProcessId.HasValue)
            {
                var process = editor.ProcessList.FirstOrDefault(x => x.Id == workstation.ProcessId.Value);
                if (process != null)
                {
                    editor.SelectedProcess = process;
                }
            }

            if (workstation.StationId.HasValue)
            {
                var station = editor.StationList.FirstOrDefault(x => x.Id == workstation.StationId.Value);
                if (station != null)
                {
                    editor.SelectedStation = station;
                }
            }

            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor,
                w =>
                {
                    w.Title = "切换工作单元".L10N();
                    w.Height = 500;
                    w.Width = 500;
                    w.MinHeight = 350;
                    w.MinWidth = 400;
                    w.Commands.Clear();
                    w.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
                    {
                        if (editor.Result == 0)
                        {
                            bool hasError = false;
                            StringBuilder stringBuilder = new StringBuilder();

                            if (editor.SelectedWipResource == null)
                            {
                                hasError = true;
                                stringBuilder.Append("产线不能为空;".L10N());
                            }

                            //if (editor.SelectedProcess == null)
                            //{
                            //    hasError = true;
                            //    stringBuilder.Append("工序不能为空;".L10N());
                            //}

                            //if (editor.SelectedStation == null)
                            //{
                            //    hasError = true;
                            //    stringBuilder.Append("工位不能为空;".L10N());
                            //}
                            //if (editor.SelectedProcess != null
                            //    && workstation.EmployeeId.HasValue
                            //    && !RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(editor.SelectedProcess.Id, workstation.EmployeeId.Value))
                            //{
                            //    hasError = true;
                            //    stringBuilder.Append("员工[{0}]不具有工序[{1}]所要求的技能；"
                            //        .L10nFormat(workstation.Employee.Name, editor.SelectedProcess.Name));
                            //}

                            if (hasError)
                            {
                                CRT.MessageService.ShowMessage(stringBuilder.ToString().TrimEnd(';'));
                                e.Cancel = true;

                                //将工作单元选择弹出框的结果设置为默认
                                editor.Result = 1;
                            }
                            else
                            {
                                //赋值顺序不能更换，工位信息选择触发相应的事件
                                workstation.Resource = editor.SelectedWipResource;
                                workstation.Process = editor.SelectedProcess;
                                workstation.Station = editor.SelectedStation;

                                _workstation.Resource = editor.SelectedWipResource;
                                _workstation.Process = editor.SelectedProcess;
                                _workstation.Station = editor.SelectedStation;

                                //成功选择工作单元
                                isSelectSuccess = true;
                            }
                        }
                    };
                });

            return isSelectSuccess;
        }

    }
}
