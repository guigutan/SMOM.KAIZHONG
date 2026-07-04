using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.ImportHelper;
using SIE.Common.Sort;
using SIE.Common.Utils;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ESop.Configs;
using SIE.Items;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档集属性变更类
    /// </summary>
    [IgnoreProxy]
    public class DocumentPropertyChanged : DomainController
    {
        /// <summary>
        /// 文档集属性变更触发
        /// </summary>
        /// <param name="docSet">当前变更</param>
        /// <param name="e">属性变更参数</param>
        public virtual void OnDocumentCollectionPropertyChanged(object docSet, PropertyChangedEventArgs e)
        {
            var documentCollection = docSet as DocumentCollection;
            if (e.PropertyName == DocumentCollection.FilePathProperty.Name)
                OnDocumentCollectionFilePathPropertyChanged(documentCollection);
        }

        /// <summary>
        /// 文档集属性变更
        /// </summary>
        /// <param name="documentCollection">文档集</param>
        public virtual void OnDocumentCollectionFilePathPropertyChanged(DocumentCollection documentCollection)
        {
            if (documentCollection.FilePath.IsNotEmpty())
            {
                AnalysisAttachment(documentCollection); //// 文件路径不为空
            }
            else
            {
                RestForNotAttachment(documentCollection); //// 文件路径为空
            }
        }

        /// <summary>
        /// 重置文档集数据
        /// </summary>
        /// <param name="documentCollection">文档集对象</param>
        private void RestForNotAttachment(DocumentCollection documentCollection)
        {
            documentCollection.FileSize = documentCollection.FileName = documentCollection.FileExtension = documentCollection.Md5 = string.Empty;
            documentCollection.Source = Source.Manual;
            documentCollection.IsProcessed = true;
            documentCollection.DocumentList.Clear();
        }

        /// <summary>
        /// 解析附件
        /// </summary>
        /// <param name="documentCollection">文档集</param>
        public virtual void AnalysisAttachment(DocumentCollection documentCollection)
        {
            if (documentCollection == null)
            {
                throw new ArgumentNullException(nameof(documentCollection));
            }

            AttachmentConfigValue config = ConfigService.GetConfig(new AttachmentConfig(), typeof(DocumentCollection));
            FileInfo info = new FileInfo(documentCollection.FilePath);
            documentCollection.Md5 = FileHelper.ComputeHash(info);  // MD5加密选中的文件
            documentCollection.FileExtension = info.Extension;
            documentCollection.FileName = info.Name;
            documentCollection.IsProcessed = !config.UseCom;
            documentCollection.FileSize = FileHelper.FormatFileSize(info.Length);
            documentCollection.Source = Source.Manual;
            documentCollection.DocumentList.Clear();
            if (!documentCollection.HasId)
            {
                documentCollection.GenerateId();
            }

            ExcelHelper excel = new ExcelHelper(documentCollection.FilePath);
            var sheetNames = excel.ExcelSheetNames();

            EntityList<Document> docs = RegexProcessWithDocument(documentCollection, sheetNames, config);
            documentCollection.DocumentList.AddRange(docs);

            var documentCollectionItems = documentCollection.LocalContext.GetPropertyOrDefault<EntityList<DocumentCollectionItem>>(typeof(DocumentCollectionItem).Name, null);

            if (documentCollectionItems == null)
            {
                documentCollection.LocalContext.SetExtendedProperty(typeof(DocumentCollectionItem).Name, new EntityList<DocumentCollectionItem>());
            }

            documentCollectionItems = documentCollection.LocalContext.GetPropertyOrDefault<EntityList<DocumentCollectionItem>>(typeof(DocumentCollectionItem).Name, null);
            ReSetDocumentCollectionItemByFilePath(documentCollection, documentCollectionItems, config);
            ReSetDocumentWorkOrderByFilePath(documentCollection, config);
        }

        /// <summary>
        /// 正则表达是匹配工序工作表
        /// </summary>
        /// <param name="documentCollection">文档集对象</param>
        /// <param name="sheetNames">选中文件对应所有页签名称集合</param>
        /// <param name="config">配置信息对象</param>
        /// <returns>返回文档实体集合</returns>
        private EntityList<Document> RegexProcessWithDocument(DocumentCollection documentCollection, IList<string> sheetNames, AttachmentConfigValue config)
        {
            Regex reg = new Regex(config.MappingSheetRegular, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);
            var processIndex = reg.GroupNumberFromName("Process") - 1;
            var seqIndex = reg.GroupNumberFromName("Seq") - 1;
            List<Tuple<int, string, string>> sheets = new List<Tuple<int, string, string>>();  //// 序号,工序,页签名称
            foreach (var sheetName in sheetNames)
            {
                var results = reg.Matches(sheetName);
                var process = string.Empty;
                int seq = 0;
                for (int i = 0; i < results.Count; i++)
                {
                    if (processIndex == i)
                    {
                        process = results[processIndex].Value;
                    }
                    else if (seqIndex == i)
                    {
                        seq = results[seqIndex].Value.ConvertTo<int>(0);
                    }
                    else
                    {
                        //
                    }
                }

                sheets.Add(Tuple.Create(seq, process, sheetName));
            }

            var commonCriteria = new CommonQueryCriteria();
            commonCriteria.Add(Process.NameProperty, BinaryOp.In, sheets.Select(f => f.Item2).ToArray());
            var processes = RF.Find<Process>().GetBy(commonCriteria) as EntityList<Process>;

            double index = 1;
            var docs = sheets.OrderBy(f => f.Item2).ThenBy(f => f.Item1).Select(f =>
            {
                var doc = new Document
                {
                    Code = f.Item3,
                    Name = f.Item3,
                    FileName = f.Item3,
                    ProcessId = processes.FirstOrDefault(p => p.Name == f.Item2)?.Id ?? 0,
                    Source = config.UseCom ? Source.ByProgram : Source.Manual,
                    DocumentType = config.UseCom ? DocumentType.Img : DocumentType.Document,
                    DocumentCollection = documentCollection,
                    IsProcessed = !config.UseCom
                };
                doc.GenerateId();
                //SortExtension.SetIndex(doc, (int)doc.Id);
                doc.SetIndex(index++);
                return doc;
            }).AsEntityList();
            return docs;
        }

        /// <summary>
        /// 物料重新关联文档集
        /// </summary>
        /// <param name="documentCollection">文档集</param>
        /// <param name="documentCollectionItems">适用物料</param>
        /// <param name="config">配置项</param>
        public virtual void ReSetDocumentCollectionItemByFilePath(DocumentCollection documentCollection, EntityList<DocumentCollectionItem> documentCollectionItems, AttachmentConfigValue config)
        {
            if(config == null || documentCollection == null || documentCollectionItems == null)
            {
                return;
            }
            var startIndex = config.ItemSheet.IndexOf('[') + 1;
            var length = config.ItemSheet.IndexOf(']') - startIndex;
            var index = config.ItemSheet.Substring(startIndex, length).Split(',');
            var itemSheetName = config.ItemSheet.Substring(0, config.ItemSheet.IndexOf('['));
            var sheetItemCodes = ExcelHelper.ReadSheetValues(documentCollection.FilePath, itemSheetName, index[0].ConvertTo<int>(), index[1].ConvertTo<int>(), index[2].ConvertTo<int?>(null), index[3].ConvertTo<int?>(null));

            foreach (var dpocItem in documentCollectionItems)
            {
                if (sheetItemCodes.Any(f => f == dpocItem.Item.Code))
                {
                    sheetItemCodes.Remove(dpocItem.Item.Code);
                }
                else
                {
                    dpocItem.PersistenceStatus = PersistenceStatus.Deleted;
                }
            }

            foreach (var code in sheetItemCodes)
            {
                var item = RT.Service.Resolve<ItemController>().GetItem(code);
                if (item == null)
                {
                    continue;
                }

                var docsetItem = new DocumentCollectionItem
                {
                    DocumentCollectionId = documentCollection.Id,
                    ItemId = item.Id
                };
                documentCollectionItems.Add(docsetItem);
            }
        }

        /// <summary>
        /// 工单重新关联文档集
        /// </summary>
        /// <param name="documentCollection">文档集</param>
        /// <param name="config">配置项</param> 
        public virtual void ReSetDocumentWorkOrderByFilePath(DocumentCollection documentCollection, AttachmentConfigValue config)
        {
            if (config == null || documentCollection == null)
            {
                return;
            }
            var startIndex = config.WorkOrderSheet.IndexOf('[') + 1;
            var length = config.WorkOrderSheet.IndexOf(']') - startIndex;
            var index = config.WorkOrderSheet.Substring(startIndex, length).Split(',');
            var workOrderSheetName = config.WorkOrderSheet.Substring(0, config.WorkOrderSheet.IndexOf('['));
            var workOrderSheetNos = ExcelHelper.ReadSheetValues(documentCollection.FilePath, workOrderSheetName, index[0].ConvertTo<int>(), index[1].ConvertTo<int>(), index[2].ConvertTo<int?>(null), index[3].ConvertTo<int?>(null));
            var workOrderList = RT.Service.Resolve<DocumentCollectionController>().GetDocumentCollectionWorkOrder(documentCollection.Id);
            foreach (var dpocWorkOrder in workOrderList)
            {
                if (workOrderSheetNos.Any(f => f == dpocWorkOrder.WorkOrder.No))
                {
                    workOrderSheetNos.Remove(dpocWorkOrder.WorkOrder.No);
                }
                else
                {
                    dpocWorkOrder.PersistenceStatus = PersistenceStatus.Deleted;
                }
            }
            RF.Save(workOrderList);
            var workOrderIds = RT.Service.Resolve<WorkOrderController>().GetWorkOrderByNos(workOrderSheetNos);
            foreach (var workOrderId in workOrderIds)
            {
                var docsetWorkOrder = new DocumentCollectionWorkOrder
                {
                    CollectionId = documentCollection.Id,
                    WorkOrderId = workOrderId
                };
                RF.Save(docsetWorkOrder);
            }
        }

        /// <summary>
        /// 文档属性变更
        /// </summary>
        /// <param name="doc">文档实体对象</param>
        /// <param name="e">属性变更参数对象</param>
        public virtual void OnDocumentPropertyChanged(object doc, PropertyChangedEventArgs e)
        {
            var document = doc as Document;
            if (e != null && e.PropertyName == Document.FilePathProperty.Name)
            {
                OnDocumentFilePathPropertyChanged(document);    //// 文件路径变更
            }

            if (e != null && e.PropertyName == Document.FileNameProperty.Name)
            {
                OnDocumentSheetNamePropertyChanged(document);   //// 文件名称变更
            }
        }

        /// <summary>
        /// 文档的文件路径变更
        /// </summary>
        /// <param name="document">文档对象</param>
        protected virtual void OnDocumentFilePathPropertyChanged(Document document)
        {
            if (document == null)
            {
                return;
            }
            if (document.FilePath.IsNullOrWhiteSpace())
            {
                document.Md5 = string.Empty;
                document.FileExtension = string.Empty;
                document.FileName = string.Empty;
                document.FileSize = string.Empty;
            }
            else
            {
                var info = new FileInfo(document.FilePath);
                if (!info.Exists)
                {
                    return;
                }

                document.Md5 = FileHelper.ComputeHash(info);
                document.FileExtension = info.Extension;
                document.FileName = info.Name;
                document.FileSize = FileHelper.FormatFileSize(info.Length);
                document.DocumentType = RT.Service.Resolve<DocumentCollectionController>().GetDocumentType(info.Extension);
            }

            var config = ConfigService.GetConfig(new AttachmentConfig(), typeof(DocumentCollection));
            bool isProcess = !config.UseCom;
            if (config.UseCom)
            {
                isProcess = document.DocumentType != DocumentType.Document;
            }

            document.IsProcessed = isProcess;
        }

        /// <summary>
        /// 文档文件名变更
        /// </summary>
        /// <param name="document">文档对象</param>
        protected virtual void OnDocumentSheetNamePropertyChanged(Document document)
        {
            if (document != null && document.FileName.IsNullOrEmpty())
            {
                document.DocumentType = DocumentType.Img;
                document.FilePath = document.FileExtension = document.Md5 = document.FileSize = string.Empty;
            }
        }

        /// <summary>
        /// 生成新的文档，根据文档集中excel的页签个数生成对应的文档
        /// </summary>
        /// <param name="documentCollection">文档集对象</param>
        /// <returns>返回文档集合</returns>
        public virtual EntityList<Document> GetSheetNames(DocumentCollection documentCollection)
        {
            if (documentCollection != null && !documentCollection.FilePath.IsNullOrEmpty())
            {
                string[] sheetNames = null;
                try
                {
                    ExcelHelper excel = new ExcelHelper(documentCollection.FilePath);
                    sheetNames = excel.ExcelSheetNames();
                }
                catch (Exception)
                {
                    sheetNames = RT.Service.Resolve<DocumentCollectionController>().GetOnServiceExcelSheetNames(documentCollection);
                }

                return sheetNames.Select(f => new Document { FileName = f, Code = f, Name = f }).AsEntityList();
            }

            return new EntityList<Document>();
        }
    }
}