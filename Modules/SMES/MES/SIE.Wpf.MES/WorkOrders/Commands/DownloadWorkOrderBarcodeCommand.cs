using SIE.Wpf.Command;
using SIE.Wpf.Common.Helper;
using System;
using System.IO;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 下载条码模板
    /// </summary>
    [Command(ImageName = "Download", Label = "下载条码模板", Hierarchy = "条码", GroupType = CommandGroupType.Edit)]
    public class DownloadWorkOrderBarcodeCommand : ListViewCommand
    {
        /// <summary>
        /// 判断方法是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return true;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            DownloadFileHelper.Download(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\工单条码导入模板.xls"));
        }
    }
}
