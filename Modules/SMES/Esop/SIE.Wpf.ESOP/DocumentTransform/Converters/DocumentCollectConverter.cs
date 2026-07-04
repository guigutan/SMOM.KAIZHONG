using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop;
using SIE.ESop.Documents;
using SIE.Wpf.ESop.DocumentTransform.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Topshelf.Logging;

namespace SIE.Wpf.ESop.DocumentTransform
{
    /// <summary>
    /// 文档集转换类
    /// </summary>
    public class DocumentCollectConverter : IDocumentConvert, IDisposable
    {
        /// <summary>
        /// 日记对象
        /// </summary>
        private static readonly LogWriter Logwriter = HostLogger.Get("DocumentTransformService");

        /// <summary>
        /// 基本目录路径（系统配置）
        /// </summary>
        private static readonly string BaseDir;

        /// <summary>
        /// 是否结束标记
        /// </summary>
        private bool cancel;

        /// <summary>
        /// Excel转换图片对象
        /// </summary>
        private ExcelSheetConvertToImg DocumentConverter { get; set; }

        /// <summary>
        /// 文档集转换实例
        /// </summary>
        public static readonly DocumentCollectConverter Instance = new DocumentCollectConverter();

        /// <summary>
        /// 静态初始化基本目录路径
        /// </summary>
        static DocumentCollectConverter()
        {
            BaseDir = RT.Service.Resolve<DocumentCollectionController>().GetUploadDocumentBaseDir();
        }

        /// <summary>
        /// 处理未处理
        /// </summary>
        public void Convert()
        {
            //// 循环所有库存组织
            Logwriter.Info("DocumentConvert Started...");
            int convertCount = 0;
            DocumentConverter = new ExcelSheetConvertToImg();
            var ctl = RT.Service.Resolve<IInvOrgCodeService>();
            List<int> orgCodeList = ctl.GetAllInvOrgCode();
            foreach (int orgCode in orgCodeList)
            {
                //// 获取库存组织下所有未处理的文档集
                AppRuntime.InvOrg = orgCode;
                DocumentCollectionCriteria documentCollectionCriteria = new DocumentCollectionCriteria { UnProcess = true, PagingInfo = new PagingInfo { PageNumber = 1, PageSize = int.MaxValue } };
                var unprocessDocumentList = RT.Service.Resolve<DocumentCollectionController>().GetList(documentCollectionCriteria);
                using (var trans = DB.TransactionScope(ESopEntityDataProvider.ConnectionStringName))
                {
                    //// 循环处理所有未处理的文档集
                    foreach (var docSet in unprocessDocumentList)
                    {
                        if (cancel) break;
                        try
                        {
                            convertCount++;
                            ConvertDocumentCollectionFile(docSet);
                            trans.Complete();
                        }
                        catch (Exception e)
                        {
                            Logwriter.Error(e);
                            break;
                        }
                    }
                }

                if (cancel) break;
            }

            DocumentConverter.Dispose();
            DocumentConverter = null;
            Logwriter.Info("DocumentConvert End,Convert File Count:" + convertCount);

            Thread.Sleep(60000);
        }

        /// <summary>
        /// 处理文档集下的文档
        /// </summary>
        /// <param name="docSet">文档集对象</param>
        public void ConvertDocumentCollectionFile(DocumentCollection docSet)
        {
            if (docSet == null) throw new ArgumentNullException(nameof(docSet));

            docSet.IsProcessed = true;
            docSet.ProcessedDate = DateTime.Now;
            RF.Save(docSet);

            var excelPath = Path.Combine(BaseDir, docSet.FilePath);
            Logwriter.Info("Begin Convert " + excelPath);

            //// 转换文档集对应的excel中所有页签为图片
            Dictionary<string, byte[]> imgByte;
            DocumentConverter.ConvertSheetsToMs(excelPath, out imgByte);

            //// 获取文档集下属于外部程序的文档
            DocumentCriteria documentCriteria = new DocumentCriteria { DocumentCollectionId = docSet.Id, Source = Source.ByProgram, PagingInfo = new PagingInfo { PageNumber = 1, PageSize = int.MaxValue } };
            var docList = RT.Service.Resolve<DocumentCollectionController>().GetList(documentCriteria).OrderBy(f => f.FileName).ToList();
            for (int i = 0; i < docList.Count(); i++)
            {
                if (i == 0 || docList[i].FileName != docList[i - 1].FileName)
                {
                    docList[i].IsProcessed = true;
                    docList[i].ProcessedDate = DateTime.Now;
                    if (!imgByte.ContainsKey(docList[i].FileName))
                        throw new ValidationException("文档记录编码:{0},\n无法关联相应的文件".L10nFormat(docList[i].Code));
                    docList[i].Content = imgByte[docList[i].FileName];
                    docList[i].FileName = $"{docList[i].FileName}.bmp";
                    docList[i].Source = Source.ByProgram;
                    docList[i].PropertyChanged += RT.Service.Resolve<DocumentPropertyChanged>().OnDocumentPropertyChanged;
                    RF.Save(docList[i]);
                }
                else
                {
                    docList[i].IsProcessed = true;
                    docList[i].ProcessedDate = DateTime.Now;
                    docList[i].FileName = docList[i - 1].FileName;
                    docList[i].Source = docList[i - 1].Source;
                    docList[i].FilePath = docList[i - 1].FilePath;
                    docList[i].Md5 = docList[i - 1].Md5;
                    docList[i].FileExtension = docList[i - 1].FileExtension;
                    docList[i].FileName = docList[i - 1].FileName;
                    docList[i].FileSize = docList[i - 1].FileSize;
                    docList[i].DocumentType = docList[i - 1].DocumentType;
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            cancel = true;
        }

        /// <summary>
        /// 是否资源
        /// </summary>
        public void Dispose()
        {
            //未实现
        }
    }
}
