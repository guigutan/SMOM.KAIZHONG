using SIE.Wpf.Command;
using SIE.Wpf.MES.PackingQC;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.ConnectorPacking.Commands
{
    /// <summary>
    /// 移除
    /// </summary>
    [Command(ImageName = "", Label = "移  除", ToolTip = "移  除", GroupType = CommandGroupType.Edit)]
    public class ConnectorDeleteCommand : DetailViewCommand
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

        public override void Execute(DetailLogicalView view)
        {
            //包装采集信息
            var vm1 = view.Current as ConnectorSnPackingViewModel;
            var vm = view.Current as DataCollectionViewModel;
            if (vm1.DeleteIdent == 0)
            {
                MessageBoxResult result = MessageBox.Show(
             "扫码移除是否开启!",
             "提示",
             MessageBoxButton.OKCancel,
             MessageBoxImage.Information);

                // 根据用户选择执行相应操作
                if (result == MessageBoxResult.OK)
                {
                    vm1.DeleteIdent = 1;
                    vm1.DeleteState = "移除中";
                }
                else
                {

                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("扫码移除是否关闭!", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                // 根据用户选择执行相应操作
                if (result == MessageBoxResult.OK)
                {
                    vm1.DeleteIdent = 0;
                    vm1.DeleteState = "扫码中";
                }
                else
                {

                }
            }
        }
    }
}
