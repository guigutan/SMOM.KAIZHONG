using SIE.Wpf.Command;
using SIE.Wpf.Common.Helper;
using System;
using System.IO;

namespace SIE.Wpf.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺 导入模板
    /// </summary>
    [Command(ImageName = "Download", Label = "下载模板", GroupType = CommandGroupType.Edit)]
    public class DownloadProcessTechViewModelCommand : DetailViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            DownloadFileHelper.Download(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\制程工艺导入模板.xlsx"));
        }
    }
}
