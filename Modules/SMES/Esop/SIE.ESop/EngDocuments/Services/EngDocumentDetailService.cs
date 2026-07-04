using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.ESop.Documents;
using SIE.ESop.EngDocuments.Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Services
{
    /// <summary>
    /// Service
    /// </summary>
    public class EngDocumentDetailService : DomainService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly EngDocumentDetailDao _engDocumentDetailDao;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="engDocumentDetailDao"></param>
        public EngDocumentDetailService (EngDocumentDetailDao engDocumentDetailDao)
        {
            _engDocumentDetailDao = engDocumentDetailDao;
        }

        /// <summary>
        /// 根据文件编码获取数据
        /// </summary>
        /// <param name="docCodes"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<EngDocumentDetail> GetEngDocumentDetailByDocCodes(List<string> docCodes, List<double> ids)
        {
            return _engDocumentDetailDao.GetEngDocumentDetailByDocCodes(docCodes, ids);
        }


        /// <summary>
        /// ESOP读取工程文件
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual EntityList<Document> GetDocuments(double? woId, double? itemId)
        {
            return _engDocumentDetailDao.GetDocuments(woId, itemId);
        }

        /// <summary>
        /// 解析文件生成MD5
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual string CreateMD5(string fileName)
        {
            return _engDocumentDetailDao.CreateMD5(fileName);
        }
    }
}
