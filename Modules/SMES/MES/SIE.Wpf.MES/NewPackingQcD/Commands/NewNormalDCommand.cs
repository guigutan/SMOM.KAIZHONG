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
    /// 提交
    /// </summary>
    [Command(ImageName = "", Label = "提  交", ToolTip = "提  交", GroupType = CommandGroupType.Edit)]
    public class NewNormalDCommand : DetailViewCommand
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

            //界面信息
            var vm = view.Current as DataCollectionViewModel;
            //包装采集信息
            var vm1 = view.Current as NewPackingQcDViewModel;

            //等于1的时候是 换箱
            if (vm1.BoxExChange == 1)
            {
                MessageBoxResult result = MessageBox.Show(
                "是否确认换箱!!!",
                "提示",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    var XtblueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(vm1.XtBlue);
                    var YXtblueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(vm1.YXtBlue);
                    if (XtblueBable.ProductionNo == YXtblueBable.ProductionNo)
                    {
                        //查找包装QC主表BlueLableController
                        var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(vm1.YXtBlue);
                        if (packingQc != null)
                        {
                            packingQc.BlueLabel = vm1.XtBlue;
                            packingQc.OldBlueLabel = vm1.YXtBlue;
                            packingQc.IsUploadSap = false;
                            packingQc.UploadResult = "";
                            packingQc.BlueLableNum = XtblueBable.PackageNum;
                            if (packingQc.PackingDetailList.Sum(p => p.PackingNum) == packingQc.BlueLableNum)
                            {
                                packingQc.PackIdent = PackIdentEnum.FullTank;
                            }
                            RF.Save(packingQc);
                            YXtblueBable.IsPack = false;
                            XtblueBable.IsPack = true;
                            RF.Save(YXtblueBable);
                            RF.Save(XtblueBable);

                        }
                        var XzpackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(vm1.XtBlue);


                        foreach (var item in vm1.PackageSnRecordList)
                        {
                            item.BlueLabel = vm1.XtBlue;
                        }
                        vm1.Tips = "换箱成功!!!";
                    }
                    else
                    {
                        vm1.Tips = "现在的蓝标和原蓝标的工单号不一致!!!";
                        return;
                    }
                }
            }
            else
            {
                //查找包装QC主表
                var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(vm1.XtBlue);
                //直接提交
                MessageBoxResult result = MessageBox.Show(
             "未达到蓝标的装箱数量,蓝标数[" + packingQc.BlueLableNum + "],已装箱数[" + packingQc.PackingNum + "]未装箱数[" + (packingQc.BlueLableNum - packingQc.PackingNum) + "],是否提交",
             "提示",
             MessageBoxButton.OKCancel,
             MessageBoxImage.Information);

                // 根据用户选择执行相应操作
                if (result == MessageBoxResult.OK)
                {

                    //查找包装QC主表
                    //var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(vm1.XtBlue);
                    if (packingQc != null)
                    {
                        packingQc.PackIdent = PackIdentEnum.NotFullTank;
                        packingQc.Confirm = ConfirmEnum.YES;
                        packingQc.BoxState = BoxStateEnum.NO;
                    }
                    RF.Save(packingQc);
                    vm1.PackageSnRecordList.Clear();
                    vm1.blueInt = 0;
                    vm1.blueZInt = 0;
                    vm1.XtBlue = "";
                    vm1.BoolBlue = true;

                    vm1.currentCount = 0;
                    vm1.currentLevelIndex = 0;

                    vm1.blueBable = null;
                    vm1.blueLableLevel = null;
                    vm1.BoolBlueLine = false;
                    vm1.SnIdent = 0;
                    vm.Tips = "请输入蓝标标签!";
                }
                else
                {

                }

            }
        }
    }
}
