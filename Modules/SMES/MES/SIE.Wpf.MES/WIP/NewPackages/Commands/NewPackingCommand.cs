using DevExpress.Xpf.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using SIE.Common;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.MES.WIP.NewPackages;
using SIE.MES.WIP.Packings;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Enums;
using SIE.Resources.Employees;
using SIE.Security;
using SIE.Threading;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.NewPackages.Commands
{
    /// <summary>
    /// 打包命令
    /// 1、单个层级打包，未打包的单个层级可以进行打包
    /// 2、选择多个已打包的同一层级，可以打包成上一层级 (包装层级、工单必须一致)
    /// </summary>
    [Command(ImageName = "Package", Label = "打包", ToolTip = "打包", GroupType = CommandGroupType.Edit)]
    public class NewPackingCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var records = view.SelectedEntities.OfType<PackageSnRecord>();
            if (records.Count() > 1)
            {
                var relation = records.FirstOrDefault();
                var result = !records.Any(p => p.PackageUnitId != relation.PackageUnitId) && !records.Any(p => p.WorkOrderId != relation.WorkOrderId);
                return result;
            }

            return records.Count() == 1;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {

            var vm = (view.Relations[0].View.Current as NewPackageViewModel);
            try
            {
                if (vm == null)
                    throw new ValidationException("包装视图模型为空,请检查".L10N());
                var records = view.SelectedEntities.OfType<PackageSnRecord>().AsEntityList();
                if (vm.Printer.IsNullOrEmpty())
                    throw new ValidationException("打印机不允许为空".L10N());
                var wo = RF.GetById<WorkOrder>(records.FirstOrDefault().WorkOrderId);
                if (wo == null)
                    throw new ValidationException("数据异常，工单不存在".L10N());
                var rules = wo.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).ToArray();
                var curRule = rules.FirstOrDefault(p => p.PackageUnitId == records.FirstOrDefault().PackageUnitId);

                bool isNext = false;
                WorkOrderPackageRuleDetail nextRule = null;
                for (int i = 0; i < rules.Length; i++)
                {
                    if (isNext)
                    {
                        nextRule = rules[i];
                        break;
                    }
                    if (curRule.Id == rules[i].Id)
                        isNext = true;
                }
                if (nextRule == null)
                    throw new ValidationException("找不到包装单位[{0}]对应的下一层级".L10nFormat(curRule.PackageUnit.Name));
                if (records.Count > nextRule.LevelQty)
                    throw new ValidationException("最多选择[{0}]进行打包，当前选择的数量[{1}]".L10nFormat(nextRule.LevelQty, records.Count));
                if (nextRule.LevelQty > records.Count && !CRT.MessageService.AskQuestion("外包装最大包装数为[{0}]，当前选择包装数为[{1}]，是否生成未满层级包装？".L10nFormat(nextRule.LevelQty, records.Count)))
                {
                    return;
                }
                //执行打包逻辑
                var resultInfo = RT.Service.Resolve<NewPackageController>().DoPackageMuanual(records, rules, nextRule, wo.Id, vm.GetWorkcell());
                if (!resultInfo.Item1.IsNullOrEmpty())
                {
                    vm.Reset( ResetType.CollectRestart);
                    throw new ValidationException(resultInfo.Item1);
                }
                var pkgNo = resultInfo.Item2;
                if (pkgNo.IsNotEmpty())
                {
                    var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(pkgNo.Split(',').ToList());
                    RT.EventBus.Publish(new DoPackingEvent(DoPackingAction.Packed, "MesPacking1", printRelations.ToArray()));
                    var invOrg = RT.InvOrg.Value;
                    Task.Run(new Action(() =>
                    {
                        vm.Print(printRelations, invOrg);
                    }).WithCurrentThreadContext());
                }

                vm.ReloadPackingRelation();

                vm.ShowTips("手动打包成功，生成包装号【{0}】".L10nFormat(pkgNo));
            }
            catch (Exception ex)
            {
                vm.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 权限校验
        /// </summary>
        //protected void CheckPermission(out Employee employee)
        //{
        //    employee = OperationEmployeeValidateHelper.Validate(ValidateUser);
        //    if (employee == null) return;
        //}

        /// <summary>
        /// 验证用户方法
        /// </summary>
        /// <param name="user">用户模型</param>
        /// <param name="employee">关联员工</param>
        /// <returns>验证信息</returns>
        public string ValidateUser(LoginUser user, out Employee employee)
        {
            var tmp = RF.GetById<Employee>(user.EmployeeId);
            if (tmp?.EmployeeType != EmployeeType.Foreman)
            {
                employee = null;
                return "账号{0}关联的员工不是班组长".L10nFormat(user.Code);
            }

            employee = tmp;
            return string.Empty;
        }


    }
}
