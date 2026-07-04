using Castle.Core.Internal;
using SIE.Andon.Andons;
using SIE.Common.Attachments;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WIP.Repairs;
using System;
using System.IO;
using System.Linq;

namespace SIE.Wpf.Andon.Commands
{
    /// <summary>
    /// 安灯事件附件查看命令
    /// </summary>
    [Command(ImageName = "Image", Label = "附件查看", ToolTip = "附件查看", GroupType = CommandGroupType.Edit)]
    public class ViewAttachmentCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;
            return andonManage != null && !andonManage.PhotoFile.IsNullOrEmpty();
        }

        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var andonManage = view.Current as AndonManage;

            var fileName = andonManage.PhotoFile.Split('/').LastOrDefault();

            var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(andonManage.PhotoFile, fileName);
            var path = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("yyyyMMddHHmmssfff"))).FullName;
            var tmpFile = Path.Combine(path, fileName);
            FileStream pFileStream = null;
            try
            {
                using (pFileStream = new FileStream(tmpFile, FileMode.OpenOrCreate))
                {
                    pFileStream.Write(fileBytes, 0, fileBytes.Length);

                    pFileStream.Close();
                }
            }
            finally
            {
                if (pFileStream != null)
                {
                    pFileStream.Close();
                    pFileStream.Dispose();
                }
            }
            if (File.Exists(tmpFile))
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = tmpFile;
                process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll";
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
        }
    }
}
