using SIE.MetaModel.View;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Helper;
using System;
using System.IO;

namespace SIE.Wpf.Items.ProductModels.Commands
{
    /// <summary>
    /// 下载导入模板
    /// </summary>
    [Command(ImageName = "Download", Label = "下载模板", Location = CommandLocation.All, GroupType = CommandGroupType.Business)]
    public class DownLoadImportTemplateCommand : DetailViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">列表视图</param>
        public override void Execute(DetailLogicalView view)
        {
            DownloadFileHelper.Download(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\产品机型导入模板.xlsx"));
        }
    }
}
