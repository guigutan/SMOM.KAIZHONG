using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIE.Common;
using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Common.ImportHelper;
using SIE.Common.Sort;
using SIE.Common.Utils;
using SIE.Core.Logs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.Configs;
using SIE.EventMessages.MES.WipRecords;
using SIE.Items;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档集控制器
    /// </summary>
    public class DocumentCollectionController : DomainController, IWipSop
    {
        /// <summary>
        /// 查询文档集
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>文档集列表</returns>
        public virtual EntityList<DocumentCollection> GetList(DocumentCollectionCriteria criteria)
        {
            var query = Query<DocumentCollection>();
            if (criteria.Code.IsNotEmpty())
                query.Where(f => f.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(f => f.Name.Contains(criteria.Name));
           
            if (criteria.Source.HasValue)
                query.Where(f => f.Source == criteria.Source);
            if (criteria.UnProcess)
                query.Where(f => f.IsProcessed == !criteria.UnProcess);
            if (criteria.ItemId.HasValue)
                query.Exists<DocumentCollectionItem>((p, d) => d.Where(f => f.DocumentCollectionId == p.Id && f.ItemId == criteria.ItemId));
            if (criteria.WorkOrderId.HasValue)
                query.Exists<DocumentCollectionWorkOrder>((p, d) => d.Where(f => f.CollectionId == p.Id && f.WorkOrderId == criteria.WorkOrderId));

            if (criteria.FileName.IsNotEmpty())
                query.Exists<Document>((p, d) => d.Where(f => f.DocumentCollectionId == p.Id && f.FileName.Contains(criteria.FileName)));

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>文档列表</returns>
        public virtual EntityList<Document> GetList(DocumentCriteria criteria)
        {
            var query = Query<Document>().Join<DocumentCollection>((f, s) => f.DocumentCollectionId == s.Id && !s.GetProperty(PhantomEntityExtension.IS_PHANTOMProperty));
            if (criteria.DocumentCollectionId.HasValue)
                query.Where(f => f.DocumentCollectionId == criteria.DocumentCollectionId);

            if (criteria.Code.IsNotEmpty())
                query.Where(f => f.Code.Contains(criteria.Code));

            if (criteria.Name.IsNotEmpty())
                query.Where(f => f.Name.Contains(criteria.Name));

            if (criteria.FileName.IsNotEmpty())
                query.Where(f => f.FileName.Contains(criteria.FileName));

            if (criteria.FileExtesion.IsNotEmpty())
                query.Where(f => f.FileExtension == criteria.FileExtesion);

            if (criteria.Source.HasValue)
                query.Where(f => f.Source == criteria.Source);
            if (criteria.ProcessId.HasValue)
                query.Where(f => f.ProcessId == criteria.ProcessId);

            if (criteria.ItemId.HasValue)
                query.Exists<DocumentCollectionItem>((d, c) => c.Where(f => f.DocumentCollectionId == d.DocumentCollectionId && f.ItemId == criteria.ItemId));
            if (criteria.WorkOrderId.HasValue)
                query.Exists<DocumentCollectionWorkOrder>((p, d) => d.Where(f => f.CollectionId == p.DocumentCollectionId && f.WorkOrderId == criteria.WorkOrderId));

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWith(Document.ProcessProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 获取服务器的EXCEL工作表
        /// </summary>
        /// <param name="documentCollection">文档集对象</param>
        /// <returns>返回excel页签</returns>
        public virtual string[] GetOnServiceExcelSheetNames(DocumentCollection documentCollection)
        {
            var filePath = documentCollection.FilePath;
            var fileStream = DownLoadToTempPath(filePath);
            var sheetNames = ExcelHelper.ExcelSheetNames(fileStream);
            return sheetNames;
        }

        /// <summary>
        /// 下载文件到临时目录
        /// </summary>
        /// <param name="filePath">服务器文件路径  XXX/XXX/XX.xlxs</param>
        Stream DownLoadToTempPath(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(filePath, fileName);
            return new MemoryStream(fileBytes);
        }

        /// <summary>
        /// 根据物料ID获取文档集适用物料
        /// </summary>
        /// <param name="itemIds">物料ID</param>
        /// <returns>文档与适用物料对应关系列表</returns>
        public virtual EntityList<DocumentCollectionItem> GetDocumentCollectionItemsByItem(params double[] itemIds)
        {
            return Query<DocumentCollectionItem>().Join<DocumentCollection>((o, e) => o.DocumentCollectionId == e.Id).Where(f => itemIds.Contains(f.ItemId)).ToList();
        }

        /// <summary>
        /// 删除引用该物料的旧的文档物料关系
        /// </summary>
        /// <param name="documentId">新关联的文档集ID</param>
        /// <param name="itemId">物料ID</param>
        public virtual void DeleteOldRefCollectionItem(double documentId, double itemId)
        {
            var items = Query<DocumentCollectionItem>().Where(p => p.DocumentCollectionId != documentId && p.ItemId == itemId).ToList();
            if (items.Count == 0)
                return;
            items.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(items);
        }

        /// <summary>
        /// 判断物料是否被其他文档集关联
        /// </summary>
        /// <param name="documentId">当前待关联文档集ID</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>被关联返回true，否则返回false</returns>
        public virtual bool IsItemRefCollection(double documentId, double itemId)
        {
            return Query<DocumentCollectionItem>().Where(p => p.DocumentCollectionId != documentId && p.ItemId == itemId).Count() > 0;
        }

        /// <summary>
        /// 判断物料是否被其他文档集关联
        /// </summary>
        /// <param name="documentId">当前待关联文档集ID</param>
        /// <param name="itemIds">物料ID数组</param>
        /// <returns>结果字典，key 物料ID，value 是否存在</returns>
        public virtual Dictionary<double, bool> IsItemRefCollection(double documentId, double[] itemIds)
        {
            Dictionary<double, bool> dicReslut = new Dictionary<double, bool>();
            foreach (double itemId in itemIds)
            {
                if (dicReslut.ContainsKey(itemId))
                    continue;
                var isExist = IsItemRefCollection(documentId, itemId);
                dicReslut[itemId] = isExist;
            }
            return dicReslut;
        }

        /// <summary>
        /// 获取文档集关联的物料
        /// </summary>
        /// <param name="documentCollectionId">文档集ID</param>
        /// <returns>文档与适用物料对应关系列表</returns>
        public virtual EntityList<DocumentCollectionItem> GetItemsByDocumentCollection(double documentCollectionId)
        {
            return Query<DocumentCollectionItem>()
                 .Where(f => f.DocumentCollectionId == documentCollectionId)
                 .ToList(new PagingInfo
                 {
                     PageSize = int.MaxValue,
                     PageNumber = 1
                 },
                 new EagerLoadOptions().LoadWith(DocumentCollectionItem.ItemProperty)
                 .LoadWithViewProperty());
        }

        /// <summary>
        /// 保存文档集
        /// </summary>
        /// <param name="documentCollection">文档集(参数)</param>
        /// <param name="documentCollectionItems">文档与适用物料对应关系</param>
        /// <returns>文档集</returns>
        public virtual DocumentCollection SaveDocumentCollection(DocumentCollection documentCollection, EntityList<DocumentCollectionItem> documentCollectionItems)
        {
            using (var trans = DB.TransactionScope(ESopEntityDataProvider.ConnectionStringName))
            {
                RF.Save(documentCollection);
                RF.Save(documentCollection.DocumentList);
                RF.Save(documentCollectionItems);
                trans.Complete();
            }

            // 清空Content 减少网络传输量
            documentCollection.Content = null;
            foreach (Document doc in documentCollection.DocumentList)
            {
                doc.Content = null;
            }

            return documentCollection;
        }

        /// <summary>
        /// 获取文档类型
        /// </summary>
        /// <param name="fileExtension">文件扩展</param>
        /// <returns>文档类型</returns>
        [IgnoreProxy]
        public virtual DocumentType GetDocumentType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".xls":
                case ".xlsx":
                case ".pdf":
                case ".docx":
                return DocumentType.Document;
                case ".png":
                case ".bmp":
                case ".jpg":
                case ".gif":
                case ".jpeg":
                    return DocumentType.Img;
                case ".avi":
                case ".rmvb":
                case ".rm":
                case ".mp4":
                case ".wmv":
                case ".mpg":
                case ".flv":
                case ".mov":
                case ".swf":
                case ".vob":
                case ".mkv":
                case ".mpeg":
                case ".asf":
                case ".divx":
                    return DocumentType.Video;
                default:
                    return DocumentType.Document;
            }
        }

        /// <summary>
        /// 获取文档保存基地址
        /// </summary>
        /// <returns>基地址</returns>
        public virtual string GetUploadDocumentBaseDir()
        {
            return AppRuntime.Config.Get("path.attachment", "C:/MES/Attachment");
        }

        /// <summary>
        /// esop文件夹名称
        /// </summary>
        /// <returns>名称</returns>
        [IgnoreProxy]
        public virtual string GetESopDir()
        {
            return "ESop";
        }

        /// <summary>
        /// 判断工单是否被其他文档集关联
        /// </summary>
        /// <param name="documentId">当前待关联文档集ID</param>
        /// <param name="workOrderIds">工单ID</param>
        /// <returns>结果字典，key 工单ID，value 是否存在</returns>
        public virtual Dictionary<double, bool> IsWorkOrderRefCollection(double documentId, double[] workOrderIds)
        {
            Dictionary<double, bool> dicReslut = new Dictionary<double, bool>();
            foreach (double workOrderId in workOrderIds)
            {
                if (dicReslut.ContainsKey(workOrderId))
                    continue;
                var isExist = IsWorkOrderRefCollection(documentId, workOrderId);
                dicReslut[workOrderId] = isExist;
            }
            return dicReslut;
        }

        /// <summary>
        /// 判断工单是否被其他文档集关联
        /// </summary>
        /// <param name="documentId">当前待关联文档集ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>被关联返回true，否则返回false</returns>
        public virtual bool IsWorkOrderRefCollection(double documentId, double workOrderId)
        {
            return Query<DocumentCollectionWorkOrder>().Where(p => p.CollectionId != documentId && p.WorkOrderId == workOrderId).Count() > 0;
        }

        /// <summary>
        /// 删除引用该工单的旧的文档工单关系
        /// </summary>
        /// <param name="documentId">新关联的文档集ID</param>
        /// <param name="workOrderId">工单ID</param>
        public virtual void DeleteOldRefCollectionWorkOrder(double documentId, double workOrderId)
        {
            var workOrders = Query<DocumentCollectionWorkOrder>().Where(p => p.CollectionId != documentId && p.WorkOrderId == workOrderId).ToList();
            if (workOrders.Count == 0)
                return;
            workOrders.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(workOrders);
        }

        /// <summary>
        /// 获取文档集与工单关系(贪婪加载工单属性)
        /// </summary>
        /// <param name="documentCollectionId">文档集Id</param>
        /// <returns>文档集与工单关系</returns>
        public virtual EntityList<DocumentCollectionWorkOrder> GetDocumentCollectionWorkOrder(double documentCollectionId)
        {
            return Query<DocumentCollectionWorkOrder>().Where(p => p.CollectionId == documentCollectionId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(DocumentCollectionWorkOrder.WorkOrderProperty));
        }

        /// <summary>
        /// 文档列表
        /// </summary>
        /// <param name="workOrderId">工单</param>
        /// <returns>文档列表</returns>
        public virtual List<WipSopInfo> GetDocumentListByWoId(double workOrderId)
        {
            SaveGetDocumentListByWoIdLog(workOrderId);
            List<WipSopInfo> wipSopInfoList = new List<WipSopInfo>();
            var documentList = Query<Document>()
                .Join<DocumentCollectionWorkOrder>((x, y) => y.CollectionId == x.DocumentCollectionId && y.WorkOrderId == workOrderId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var document in documentList)
            {
                wipSopInfoList.Add(new WipSopInfo()
                {
                    Process = document.ProcessName,
                    EsopName = document.FileName,
                });
            }
            return wipSopInfoList;
        }

        /// <summary>
        /// 保存获取文档列表日志
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        private void SaveGetDocumentListByWoIdLog(double workOrderId)
        {
            using (var tran = DB.AutonomousTransactionScope(ESopEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "工单Id:{0}".L10nFormat(workOrderId);
                var log = new InterfaceLog()
                {
                    Name = "IWipSop",
                    Method = "GetDocumentListByWoId",
                    ControllerName = "DocumentCollectionController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 文档列表
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>文档列表</returns>
        public virtual List<WipSopInfo> GetDocumentListByItemId(double itemId)
        {
            SaveGetDocumentListByItemIdLog(itemId);
            List<WipSopInfo> wipSopInfoList = new List<WipSopInfo>();
            var documentList = Query<Document>()
                .Join<DocumentCollectionItem>((x, y) => y.DocumentCollectionId == x.DocumentCollectionId && y.ItemId == itemId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var document in documentList)
            {
                wipSopInfoList.Add(new WipSopInfo()
                {
                    Process = document.ProcessName,
                    EsopName = document.FileName,
                });
            }
            return wipSopInfoList;
        }

        /// <summary>
        /// 保存获取文档列表日志
        /// </summary>
        /// <param name="itemId">物料Id</param>
        private void SaveGetDocumentListByItemIdLog(double itemId)
        {
            using (var tran = DB.AutonomousTransactionScope(ESopEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "物料Id:{0}".L10nFormat(itemId);
                var log = new InterfaceLog()
                {
                    Name = "IWipSop",
                    Method = "GetDocumentListByItemId",
                    ControllerName = "DocumentCollectionController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 文档文件发生变更时候
        /// </summary>
        /// <param name="documentCollection"></param>
        public virtual void OnDocumentCollectionFilePathProperChanged(DocumentCollection documentCollection)
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
        ///分析附件信息
        /// </summary>
        /// <param name="documentCollection"></param>
        public virtual DocumentCollection  AnalysisAttachment(DocumentCollection documentCollection)
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
            documentCollection.FileSize = FileHelper.FormatFileSize(int.Parse(documentCollection.FileSize));
            documentCollection.Source = Source.Manual;
            documentCollection.DocumentList.Clear();
            if (!documentCollection.HasId)
            {
                documentCollection.GenerateId();
            }

            IList<string> sheetNames = new List<string>();
            var fileByte = RT.Service.Resolve<AttachmentController>().FileDownload(documentCollection.FilePath, documentCollection.FileName);
            IWorkbook wk = null;
            try
            {
                Stream stream = new MemoryStream(fileByte);
                if (info.Extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(stream);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(stream);
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
            if (wk != null)
            {
                //获取所有的sheetName
                for (int i = 0; i < wk.NumberOfSheets; i++)
                {
                    sheetNames.Add(wk.GetSheetName(i));
                }
            }
            EntityList<Document> docs = RegexProcessWithDocument(documentCollection, sheetNames, config);
            documentCollection.DocumentList.AddRange(docs);

            var documentCollectionItems = documentCollection.LocalContext.GetPropertyOrDefault<EntityList<DocumentCollectionItem>>(typeof(DocumentCollectionItem).Name, null);

            if (documentCollectionItems == null)
            {
                documentCollection.LocalContext.SetExtendedProperty(typeof(DocumentCollectionItem).Name, new EntityList<DocumentCollectionItem>());
            }

            documentCollectionItems = documentCollection.LocalContext.GetPropertyOrDefault<EntityList<DocumentCollectionItem>>(typeof(DocumentCollectionItem).Name, null);
            ReSetDocumentCollectionItemByFilePath(documentCollection, documentCollectionItems, config, wk);
            ReSetDocumentWorkOrderByFilePath(documentCollection, config, wk);
            return documentCollection;
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
        /// 读取页签的内容
        /// </summary>
        /// <param name="workbook">文件</param>
        /// <param name="sheetName">页签名称</param>
        /// <param name="startRowNum">开始行索引</param>
        /// <param name="startColNum">开始列索引</param>
        /// <param name="endRowNum">结束行索引</param>
        /// <param name="endColNum">结束列索引</param>
        /// <returns></returns>
        private IList<string> ReadSheetValues(IWorkbook workbook, string sheetName, int startRowNum, int startColNum, int? endRowNum, int? endColNum)
        {
            List<string> values = new List<string>();

            if (workbook != null)
            {
                var sheet = workbook.GetSheet(sheetName);
                if (sheet != null)
                {
                    for (int i = startRowNum - 1; i < (endRowNum ?? sheet.LastRowNum + 1); i++)
                    {
                        var row = sheet.GetRow(i);
                        for (int j = startColNum - 1; j < (endColNum ?? row.LastCellNum + 1); j++)
                        {
                            if (row.Cells[j].CellType == CellType.String) values.Add(row.Cells[j].StringCellValue);
                            if (row.Cells[j].CellType == CellType.Numeric) values.Add(row.Cells[j].NumericCellValue.ToString());
                        }
                    }
                }

                workbook.Close();

            }
            return values;
        }



        /// <summary>
        /// 物料重新关联文档集
        /// </summary>
        /// <param name="documentCollection"></param>
        /// <param name="documentCollectionItems"></param>
        /// <param name="config"></param>
        /// <param name="wk"></param>
        public virtual void ReSetDocumentCollectionItemByFilePath(DocumentCollection documentCollection,
            EntityList<DocumentCollectionItem> documentCollectionItems,
            AttachmentConfigValue config, IWorkbook wk)
        {
            if (config == null || documentCollection == null || documentCollectionItems == null)
            {
                return;
            }
            var startIndex = config.ItemSheet.IndexOf('[') + 1;
            var length = config.ItemSheet.IndexOf(']') - startIndex;
            var index = config.ItemSheet.Substring(startIndex, length).Split(',');
            var itemSheetName = config.ItemSheet.Substring(0, config.ItemSheet.IndexOf('['));

            var sheetItemCodes = this.ReadSheetValues(wk, itemSheetName,
                index[0].ConvertTo<int>(), index[1].ConvertTo<int>(), index[2].ConvertTo<int?>(null),
                index[3].ConvertTo<int?>(null));

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
                doc.SetIndex(index++);
                return doc;
            }).AsEntityList();
            return docs;
        }

        /// <summary>
        /// 工单重新关联文档集
        /// </summary>
        /// <param name="documentCollection"></param>
        /// <param name="config"></param>
        /// <param name="wk"></param>
        public virtual void ReSetDocumentWorkOrderByFilePath(DocumentCollection documentCollection, AttachmentConfigValue config, IWorkbook wk)
        {
            if (config == null || documentCollection == null)
            {
                return;
            }
            var startIndex = config.WorkOrderSheet.IndexOf('[') + 1;
            var length = config.WorkOrderSheet.IndexOf(']') - startIndex;
            var index = config.WorkOrderSheet.Substring(startIndex, length).Split(',');
            var workOrderSheetName = config.WorkOrderSheet.Substring(0, config.WorkOrderSheet.IndexOf('['));
            var workOrderSheetNos = this.ReadSheetValues(wk, workOrderSheetName, index[0].ConvertTo<int>(), index[1].ConvertTo<int>(), index[2].ConvertTo<int?>(null), index[3].ConvertTo<int?>(null));

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
    }
}