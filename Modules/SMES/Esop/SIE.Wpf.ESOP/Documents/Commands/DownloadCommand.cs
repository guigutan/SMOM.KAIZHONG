using SIE.Common.Utils;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Wpf.Command;
using SIE.Wpf.ESOP.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SIE.Wpf.ESop.Documents.Commands
{
    /// <summary>
    /// 文档集--查看文件命令
    /// </summary>
    [Command(ImageName = "FileEye", Label = "查看文件", ToolTip = "查看文件", GroupType = CommandGroupType.Business)]
    public class DownloadCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">此命令对应的视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Current == null) return false;
            Type type = view.Current.GetType();
            PropertyInfo propertyInfo = type.GetProperty("FilePath");
            var filePath = (string)propertyInfo.GetValue(view.Current, null);
            return view.Current.PersistenceStatus != PersistenceStatus.New && !filePath.IsNullOrEmpty();
        }

        /// <summary>
        /// 执行具体的逻辑
        /// </summary>
        /// <param name="view">命令对应的视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            Type type = view.Current.GetType();
            PropertyInfo propertyInfo = type.GetProperty("FilePath");
            var filePath = (string)propertyInfo.GetValue(view.Current, null);
            if (!filePath.IsNullOrEmpty())
            {
                var fileName = Path.GetFileName(filePath);
                //创建临时目录
                var esopPath = RT.Service.Resolve<SIE.ESop.Documents.DocumentCollectionController>().GetESopDir();
                var tempPath = Path.Combine(Path.GetTempPath(), esopPath);
                Directory.CreateDirectory(tempPath);
                var tempFile = Path.Combine(tempPath, fileName);
                //判断文件是否已经下载 MD5对比
                if (File.Exists(tempFile))
                {
                    string localMd5; 
                    try
                    {
                        localMd5 = FileHelper.ComputeHash(new FileInfo(tempFile));
                    }
                    catch
                    {
                        throw new ValidationException("文件访问冲突或该文件被其他进程占用！".L10N());
                    }
                    PropertyInfo md5Property = type.GetProperty("Md5");
                    var fileMd5 = (string)md5Property.GetValue(view.Current, null);
                    if (localMd5 != string.Empty && fileMd5 == localMd5)
                    {
                        try
                        {
                            var pro = new Process();
                            pro.StartInfo = new ProcessStartInfo(tempFile)
                            {
                                UseShellExecute = true
                            };
                            pro.Start();
                        }
                        catch (Exception ex)
                        {

                            throw new ValidationException(ex.Message);
                        }
                        //System.Diagnostics.Process.Start(tempFile);
                        return;
                    }
                }
                //下载文件存储到临时目录 
                DownloadHelper.FileDownload(filePath, tempPath);

                try
                {
                    //System.Diagnostics.Process.Start(tempFile);
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(tempFile)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
                catch (Exception ex)
                {

                    throw new ValidationException(ex.Message);
                }
            }
        }
    }
}