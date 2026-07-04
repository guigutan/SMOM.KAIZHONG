using SIE.Common.Configs;
using SIE.Common.InvOrg;
using SIE.Common.Utils;
using SIE.Domain;
using SIE.ESop.Configs;
using SIE.ESop.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Topshelf.Logging;

namespace SIE.Wpf.ESop.DocumentTransform.ProcessDocuments
{
    /// <summary>
    /// 处理Excel文档
    /// </summary>
    public class ProcessExcelDocumenter : IProcessDocument
    {
        /// <summary>
        /// 日记对象
        /// </summary>
        private static readonly LogWriter Logwriter = HostLogger.Get("DocumentTransformService");

        /// <summary>
        /// 扩展名
        /// </summary>
        private string[] ExcelExtension
        {
            get
            {
                return RT.Config.Get("docTool.fileExtension", ".xls;.xlsx").Split(';');
            }
        }

        /// <summary>
        /// 需要删除的路径
        /// </summary>
        private readonly List<string> deletePaths = new List<string>();

        /// <summary>
        /// 是否结束
        /// </summary>
        private bool cancel;

        /// <summary>
        /// 处理目录下符合扩展名的文件
        /// </summary>
        /// <param name="dir">目录对象</param>
        private void ProcessDocumentCollection(DirectoryInfo dir)
        {
            /* 遍历指定目录每个EXCEL文件
             * 创建文档集或修改
             * 创建完成删除文件
             */
            AttachmentConfigValue config = ConfigService.GetConfig(new AttachmentConfig(), typeof(DocumentCollection));
            using (var trans = DB.TransactionScope(SIE.ESop.ESopEntityDataProvider.ConnectionStringName))
            {
                foreach (FileInfo nextFile in dir.GetFiles())
                {
                    if (cancel)
                    {
                        break;
                    }

                    try
                    {
                        if (ExcelExtension.Contains(nextFile.Extension))
                        {
                            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(nextFile.FullName);
                            if (fileNameWithoutExtension.StartsWith("~$"))
                            {
                                continue;
                            }

                            var documentCollectionCriteria = new DocumentCollectionCriteria { Name = fileNameWithoutExtension };
                            var existsDocSet = RT.Service.Resolve<DocumentCollectionController>().GetList(documentCollectionCriteria).FirstOrDefault();
                            if (existsDocSet == null)
                            {
                                existsDocSet = new DocumentCollection
                                {
                                    Code = fileNameWithoutExtension,
                                    Name = fileNameWithoutExtension,
                                };
                            }
                            else if (existsDocSet.Md5 == FileHelper.ComputeHash(nextFile))
                            {
                                deletePaths.Add(nextFile.FullName);
                                continue;
                            }
                            else
                            {
                                //
                            }

                            existsDocSet.FileExtension = nextFile.Extension;
                            existsDocSet.FileSize = FileHelper.FormatFileSize(nextFile.Length);
                            existsDocSet.IsProcessed = false;
                            existsDocSet.Md5 = FileHelper.ComputeHash(nextFile);
                            existsDocSet.FileName = nextFile.Name;
                            existsDocSet.Source = Source.ByProgram;
                            existsDocSet.FilePath = nextFile.FullName;
                            using (FileStream stream = new FileStream(nextFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                BinaryReader r = new BinaryReader(stream);
                                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                                existsDocSet.Content = r.ReadBytes((int)r.BaseStream.Length);
                                r.Close();
                            }

                            var documentPropertyChanged = RT.Service.Resolve<DocumentPropertyChanged>();
                            var documentCollectionController = RT.Service.Resolve<DocumentCollectionController>();
                            documentPropertyChanged.AnalysisAttachment(existsDocSet);

                            var docsetItems = documentCollectionController.GetItemsByDocumentCollection(existsDocSet.Id);
                            documentPropertyChanged.ReSetDocumentCollectionItemByFilePath(existsDocSet, docsetItems, config);
                            documentCollectionController.SaveDocumentCollection(existsDocSet, docsetItems);

                            trans.Complete();

                            deletePaths.Add(nextFile.FullName);
                        }
                    }
                    catch (Exception e)
                    {
                        var path = Path.Combine(nextFile.Directory.FullName, "Error");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        File.Move(nextFile.FullName, Path.Combine(path, nextFile.Name));
                        Logwriter.Error(e);
                    }
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        private void ClearFiles()
        {
            var errorPaths = new List<string>();
            foreach (var p in deletePaths)
            {
                try
                {
                    File.Delete(p);
                }
                catch (Exception e)
                {
                    errorPaths.Add(p);
                    Logwriter.Error(e);
                }
            }

            deletePaths.Clear();
            deletePaths.AddRange(errorPaths);
        }

        /// <summary>
        /// 处理用户直接上次上传的文件
        /// </summary>
        public void Process()
        {
            var documentCollectionController = RT.Service.Resolve<DocumentCollectionController>();
            string esopDir = documentCollectionController.GetESopDir();
            string esopFullDir = documentCollectionController.GetUploadDocumentBaseDir();
            var fullPath = Path.Combine(esopFullDir, esopDir);
            Directory.CreateDirectory(fullPath);
            //// 获取当前用户的库存组织
            List<int> orgCodeList = RT.Service.Resolve<IInvOrgCodeService>().GetInvOrgCode(AppRuntime.IdentityId);
            foreach (var orgCode in orgCodeList)
            {
                // ESOP/INV_ORG_ID
                var invOrgDir = Directory.CreateDirectory(Path.Combine(fullPath, orgCode.ToString()));
                if (cancel)
                {
                    break;
                }

                AppRuntime.InvOrg = orgCode;
                ProcessDocumentCollection(invOrgDir);
            }

            Thread.Sleep(60000);
            ClearFiles();
        }

        /// <summary>
        /// 停止处理
        /// </summary>
        public void Stop()
        {
            cancel = true;
            ClearFiles();
        }
    }
}
