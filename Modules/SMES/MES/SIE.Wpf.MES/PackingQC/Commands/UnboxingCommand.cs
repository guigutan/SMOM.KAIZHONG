using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.PackingQC.Commands
{
    /// <summary>
    /// 开箱
    /// </summary>
    [Command(ImageName = "", Label = "开  箱", ToolTip = "开  箱", GroupType = CommandGroupType.Edit)]
    public class UnboxingCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as DataCollectionViewModel) != null && (view.Current as PackingQcViewModel) != null;
        }
        public override void Execute(DetailLogicalView view)
        {
            //包装采集信息
            var vm1 = view.Current as PackingQcViewModel;
            var vm = view.Current as DataCollectionViewModel;
            MessageBoxResult result = MessageBox.Show(
         "开箱",
         "提示",
         MessageBoxButton.OKCancel,
         MessageBoxImage.Information);

            // 根据用户选择执行相应操作
            if (result == MessageBoxResult.OK)
            {

                //界面信息
                if (vm1.XtBlue.IsNotEmpty())
                {


                    var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(vm1.XtBlue);
                    if (packingQc.BoxState == BoxStateEnum.YES)
                    {
                        vm1.Error = "该蓝标是开箱状态,无需再次开箱!";
                        return;
                    }
                    vm1.LevelString(vm1.blueLableLevel.LevelName);
                    //临时计算当前行
                    var lsCount = vm1.blueInt;
                    //当前行数
                    var dqCount = 0;
                    for (int i = 0; i < vm1.levelTargets.Length; i++)
                    {
                        lsCount = lsCount - vm1.levelTargets[i];
                        if (i == 0)
                        {
                            if (lsCount < 0)
                            {
                                dqCount = vm1.blueInt;
                            }
                        }
                        //如果等于0 就直接去下一层
                        if (lsCount == 0)
                        {
                            vm1.currentCount = 0;
                            vm1.currentLevelIndex = i + 1;
                            break;
                        }
                        //如果小于0 就去当前数，做当前层，和当前累计值
                        if (lsCount < 0)
                        {
                            vm1.currentCount = dqCount;
                            vm1.currentLevelIndex = i;
                            break;
                        }
                        if (lsCount > 0)
                            dqCount = lsCount;
                    }

                    if (vm1.blueBable.PackageNum == vm1.blueInt)
                    {
                        vm1.Error = "该蓝标已经是满箱状态!";
                        vm1.Barcode = "";
                        vm1.blueInt = 0;
                        vm1.WorkOrderId = 0;
                        vm1.currentCount = 0;
                        vm1.currentLevelIndex = 0;
                        return;
                    }

                    var packingQcData = RT.Service.Resolve<PackingQcController>().GetPackingQcByState(vm1.PackresourceId);
                    foreach (var item in packingQcData)
                    {
                        if (item.BlueLabel != vm1.XtBlue)
                        {

                            if (item.Confirm == ConfirmEnum.NO)
                            {
                                vm1.Error = "QC未确认,不允许使用其它蓝标标签!";
                                vm1.Barcode = "";
                                vm1.BoolBlue = true;

                            }

                        }
                    }
                    var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                    vm1.PackageSnRecordList.Clear();
                    foreach (var item in packDetailList)
                    {
                        PackingQcModel pqc = new PackingQcModel();
                        pqc.BlueLabel = vm1.XtBlue;
                        pqc.Confirm = ConfirmEnum.YES;
                        pqc.PackIdent = PackIdentEnum.NotFullTank;
                        pqc.ProductLabel = item.ProductLabel;
                        pqc.ItemId = packingQc.ItemId;
                        pqc.ItemName = packingQc.Item.Name;
                        pqc.ResourceId = vm1.PackresourceId;
                        vm1.PackageSnRecordList.Add(pqc);
                    }
                    packingQc.BoxState = BoxStateEnum.YES;
                    packingQc.ReportsType = ReportsTypeEnum.NO;
                    
                    RF.Save(packingQc);
                    vm1.BoolBlue = false;
                    vm.Tips = "已开箱请输入工序标签!";
                    vm.Error = "";
                }
            }
        }
    }
}
