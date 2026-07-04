using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.ConnectorPacking.Commands
{
    /// <summary>
    /// 采集重新开始命令
    /// </summary>
    [Command(ImageName = "Refresh", Label = "重新开始", ToolTip = "重新开始", GroupType = CommandGroupType.Edit)]
    public class ConnectorSnNormalCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as DataCollectionViewModel) != null && (view.Current as ConnectorSnPackingViewModel) != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as DataCollectionViewModel;
            vm.WorkOrderId = null;
            var vm1 = view.Current as ConnectorSnPackingViewModel;
            vm1.PackageSnRecordList.Clear();
            vm1.blueInt = 0;
            vm1.blueZInt = 0;
            vm1.XtBlue = "";
            vm1.BoolBlue = 1;
            vm1.YXtBlue = "";
            vm1.BoxExChange = 0;
            vm1.currentCount = 0;
            vm1.currentLevelIndex = 0;

            vm1.blueBable = null;
            vm1.blueLableLevel = null;
            vm1.BoolBlueLine = false;
            vm1.SnIdent = 0;
            vm1.Tips = "请输入蓝标!";
            vm1.Barcode = "";
            vm1.BatchZInt = 0;
            vm1.BatchInt = 0; 
            vm1.BatchLable = "";
            vm1.DeleteState = "扫码中";
            vm1.DeleteIdent = 0;
            vm.Reset(resetType: ResetType.CollectRestart);
            vm.FocuseBarcode();
        }
    }
}
