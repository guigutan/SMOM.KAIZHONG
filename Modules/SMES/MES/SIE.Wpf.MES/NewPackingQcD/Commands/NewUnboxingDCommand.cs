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

namespace SIE.Wpf.MES.NewPackingQcD.Commands
{
    /// <summary>
    /// 开箱
    /// </summary>
    [Command(ImageName = "", Label = "开  箱", ToolTip = "开  箱", GroupType = CommandGroupType.Edit)]
    public class NewUnboxingDCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as DataCollectionViewModel) != null && (view.Current as NewPackingQcDViewModel) != null;
        }
        public override void Execute(DetailLogicalView view)
        {
            //包装采集信息
            var vm1 = view.Current as NewPackingQcDViewModel;
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


                    var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                    vm1.PackageSnRecordList.Clear();
                    foreach (var item in packDetailList)
                    {
                        NewPackingQcDModel pqc = new NewPackingQcDModel();
                        pqc.BlueLabel = vm1.XtBlue;
                        pqc.Confirm = ConfirmEnum.YES;
                        pqc.PackIdent = PackIdentEnum.NotFullTank;
                        pqc.ProductLabel = item.ProductLabel;
                        pqc.ItemId = packingQc.ItemId;
                        pqc.ItemName = packingQc.Item.Name;
                        pqc.ResourceId = vm1.PackresourceId;
                        pqc.PackingNum = item.PackingNum;
                        vm1.PackageSnRecordList.Add(pqc);
                    }
                    packingQc.BoxState = BoxStateEnum.YES;
                    RF.Save(packingQc);
                    vm1.BoolBlue = false;
                    vm.Tips = "已开箱请输入工序标签!";
                    vm.Error = "";
                }
            }
        }
    }
}
