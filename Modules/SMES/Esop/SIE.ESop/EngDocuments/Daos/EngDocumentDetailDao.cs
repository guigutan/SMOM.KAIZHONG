using NPOI.HSSF.Record.PivotTable;
using SIE.Common.Configs;
using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.ESop.Documents;
using SIE.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Daos
{
    /// <summary>
    /// Dao
    /// </summary>
    public class EngDocumentDetailDao : BaseDao<EngDocumentDetail>
    {
        /// <summary>
        /// 根据文件编码获取数据
        /// </summary>
        /// <param name="docCodes"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public EntityList<EngDocumentDetail> GetEngDocumentDetailByDocCodes(List<string> docCodes, List<double> ids)
        {
            return docCodes.SplitContains(tempCodes =>
            {
                return Query().Where(p => tempCodes.Contains(p.DocCode) && !ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 临时生成MD5
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateMD5(string fileName)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(fileName);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 将哈希字节数组转换为字符串表示
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // 转换为16进制并补0
                }

                string md5Hash = sb.ToString();
                return md5Hash;
            }
        }

        /// <summary>
        /// 工程文件转化为文档集数据类型
        /// </summary>
        /// <param name="docs"></param>
        /// <param name="engDocDetails"></param>
        private void ChangeToDocument(EntityList<Document> docs, EntityList<EngDocumentDetail> engDocDetails)
        {
            foreach (var eng in engDocDetails)
            {
                var doc = new Document
                {
                    Id = eng.Id,
                    Code = eng.DocCode,
                    Name = eng.DocName,
                    FilePath = eng.SavePath,
                    FileSize = eng.FileSize,
                    IsProcessed = true,
                    Process = eng.Process,
                    FileExtension = eng.ServerFileName.Split('.')[1],
                    DocumentType = eng.DocumentType,
                    Md5 = eng.MD5,
                    SheetName = eng.SheetPage,
                };
                docs.Add(doc);
            }
        }

        /// <summary>
        /// ESOP读取工程文件
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EntityList<Document> GetDocuments(double? woId, double? itemId)
        {
            EntityList<Document> docs = new EntityList<Document>();
            EntityList<EngDocumentDetail> engDetailWo = Query().Join<EngDocument>((x, y) => x.EngDocumentId == y.Id && y.WorkOrderId != null && y.WorkOrderId == woId).ToList();
            EntityList<EngDocumentDetail> engDetailItem = Query().Join<EngDocument>((x, y) => x.EngDocumentId == y.Id && y.ProductId != null && y.ProductId == itemId).ToList();
            if (engDetailWo.Any())
            {
                ChangeToDocument(docs, engDetailWo);
            }
            else if (engDetailItem.Any())
            {
                ChangeToDocument(docs, engDetailItem);
            }
            else
            {
                //
            }
            return docs;
        }
    }
}
