using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.ESop.EngDocuments.Daos;
using SIE.ESop.EngDocuments.Handles;
using System;
using System.Collections.Generic;

namespace SIE.ESop.EngDocuments.Services
{
    /// <summary>
    /// Service层
    /// </summary>
    public class EngDocumentService : DomainService
    {
        private readonly EngDocumentDao _engDocumentDao;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engDocumentDao"></param>
        public EngDocumentService(EngDocumentDao engDocumentDao)
        {
            _engDocumentDao = engDocumentDao;
        }

        /// <summary>
        /// 工程文件保存命令
        /// </summary>
        /// <param name="data"></param>
        public virtual void EngDocSave(EntityList data)
        {
            var saveHandle = new EngDocSaveHandle(data);
            saveHandle.Init();
            // 主表必填校验
            saveHandle.ParentNotRequired();
            // 主表前端产品或工单唯一验证
            saveHandle.WebProductIsRepeat();
            // 主表后端产品或工单唯一验证
            saveHandle.DBProductIsRepeat();
            // 子表必填校验
            saveHandle.ChildrenNotRequired();
            // 子表前端产品+文件编码+工序+使用类型唯一验证
            saveHandle.WebChildIsRepeat();
            // 子表后端产品+文件编码+工序+使用类型唯一验证
            saveHandle.DBChildIsRepeat();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="engDocCriteria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList EngDocCriteriaFetch(EngDocCriteria engDocCriteria)
        {
            return _engDocumentDao.EngDocCriteriaFetch(engDocCriteria);
        }

        /// <summary>
        /// 根据产品Ids获取数据
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<EngDocument> GetEngDocumentByProductIds(List<double> productIds, List<double> ids)
        {
            return _engDocumentDao.GetEngDocumentByProductIds(productIds, ids);
        }

        /// <summary>
        /// 根据工单Ids获取数据
        /// </summary>
        /// <param name="woIds"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<EngDocument> GetEngDocumentByWoIds(List<double> woIds, List<double> ids)
        {
            return _engDocumentDao.GetEngDocumentByWoIds(woIds, ids);
        }
    }
}
