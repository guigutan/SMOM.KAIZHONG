using SIE.Common.Sort;
using SIE.Common;
using SIE.Domain;
using SIE.MES.WIP.NewPackages;
using SIE.MES.WorkOrders;
using SIE.Packages.Packings;
using SIE.Packages;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP.NewPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Domain.Validation;
using SIE.MES.WIP.Packings;
using SIE.Threading;

namespace SIE.Wpf.MES.WIP.Packings.Commands
{
    /// <summary>
    /// 打包命令
    /// 1、单个层级打包，未打包的单个层级可以进行打包
    /// 2、选择多个已打包的同一层级，可以打包成上一层级 (包装层级、工单必须一致)
    /// </summary>
    [Command(ImageName = "Package", Label = "打包", ToolTip = "打包", GroupType = CommandGroupType.Edit)]
    public class DirectPackingCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var records = view.SelectedEntities.OfType<DirectPackageSnRecord>().ToList();
            //if (records.Count() > 1)
            //{
            //    var relation = records.FirstOrDefault();
            //    var result = !records.Any(p => p.PackageUnitId != relation.PackageUnitId) && !records.Any(p => p.WorkOrderId != relation.WorkOrderId);
            //    return result;
            //}
            if (records.Count != 0)
            {
                return records[0].Sn.IsNullOrEmpty();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = (view.Relations[0].View.Current as DirectPackingViewModel);
            try
            {
                if (vm == null)
                    throw new ValidationException("包装视图模型为空,请检查".L10N());
                var record = view.SelectedEntities.OfType<DirectPackageSnRecord>().AsEntityList();
                if (vm.Printer.IsNullOrEmpty())
                    throw new ValidationException("打印机不允许为空".L10N());
                var wo = RF.GetById<WorkOrder>(record.FirstOrDefault().WorkOrderId);
                if (wo == null)
                    throw new ValidationException("数据异常，工单不存在".L10N());
                var rules = wo.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).ToList();
                var curRule = rules.FirstOrDefault(p => p.PackageUnitId == record.FirstOrDefault().PackageUnitId);

                //执行打包逻辑
                var pkgNo = RT.Service.Resolve<DirectPackingController>().GivePackageNo(rules, curRule, record.FirstOrDefault());
                if (pkgNo.IsNotEmpty())
                {
                    var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(pkgNo.Split(',').ToList());
                    var invOrg = RT.InvOrg.Value;
                    Task.Run(new Action(() =>
                    {
                        vm.Print(printRelations, invOrg);
                    }).WithCurrentThreadContext());
                }
                vm.ResetOuterRelation();
                vm.ReloadDirectPackingSnRelation();
                vm.ShowTips("手动打包成功，生成包装号【{0}】".L10nFormat(pkgNo));
            }
            catch (Exception ex)
            {
                vm.ShowError(ex.Message);
            }
        }
    }
}
