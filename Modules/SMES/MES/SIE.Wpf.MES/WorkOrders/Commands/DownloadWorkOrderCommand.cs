using SIE.Wpf.Command;
using SIE.Wpf.Common.Helper;
using System;
using System.IO;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 下载条码模板
    /// </summary>
    [Command(ImageName = "Download", Label = "下载导入模板", GroupType = CommandGroupType.Edit)]
    public class DownloadWorkOrderCommand : DetailViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            DownloadFileHelper.Download(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\工单导入模板.xlsx"));
        }
    }
}