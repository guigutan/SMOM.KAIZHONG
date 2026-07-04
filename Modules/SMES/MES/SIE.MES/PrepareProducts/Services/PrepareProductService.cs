using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.PrepareProducts.Daos;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.PrepareProducts.Services
{
    /// <summary>
    /// 产品产前准备配置Service
    /// </summary>
    public class PrepareProductService : DomainService
    {
        private readonly PrepareProductDao _prepareProductDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prepareProductDao"></param>
        public PrepareProductService(PrepareProductDao prepareProductDao)
        {
            _prepareProductDao = prepareProductDao;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProduct> QueryPreProductEntityList(PrepareProductCriteria criteria)
        {
            return _prepareProductDao.QueryPreProductEntityList(criteria);
        }

        /// <summary>
        /// 导出查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProduct> QueryPreProductEntityList(ExportPrepareProductModel model)
        {
            return _prepareProductDao.QueryPreProductEntityList(model);
        }

        /// <summary>
        /// 根据id产前准备项目是否被引用
        /// </summary>
        /// <param name="preProductIds"></param>
        /// <returns></returns>
        public virtual int GetPrepareProductDetailCount(List<double> preProductIds)
        {
            return _prepareProductDao.GetPrepareProductDetailCount(preProductIds);
        }

        /// <summary>
        /// 根据主表id获取产品产前子表
        /// </summary>
        /// <param name="preProductId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProductDetail> GetPrepareProductDetailList(double preProductId, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            return _prepareProductDao.GetPrepareProductDetailList(preProductId, sortInfo, pagingInfo);
        }

        /// <summary>
        /// 根据主表ids获取产品产前子表
        /// </summary>
        /// <param name="preProductIds"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProductDetail> GetPrepareProductDetailList(List<double> preProductIds, PagingInfo pagingInfo = null)
        {
            return _prepareProductDao.GetPrepareProductDetailList(preProductIds);
        }

        /// <summary>
        /// 根据主表产品族获取工序
        /// </summary>
        /// <param name="preDetail"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcessListByFamilyId(PrepareProductDetail preDetail, PagingInfo pagingInfo, string keyword)
        {
            return _prepareProductDao.GetProcessListByFamilyId(preDetail, pagingInfo, keyword);
        }

        /// <summary>
        /// 根据产品ids获取产品产前准备
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        public virtual List<PrepareProduct> GetPrepareProductListByProductIds(List<double?> productIds, List<double> parentIds)
        {
            return _prepareProductDao.GetPrepareProductListByProductIds(productIds, parentIds);
        }

        /// <summary>
        /// 根据产品编码获取产品产前准备的产品编码
        /// </summary>
        /// <param name="productCodes"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProduct> GetDBPrepareProductList(List<string> productCodes)
        {
            return _prepareProductDao.GetDBPrepareProductList(productCodes);
        }

        /// <summary>
        /// 根据产品族编码获取产品产前准备的产品族编码
        /// </summary>
        /// <param name="familyCodes"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProduct> GetDBPrepareFamilyList(List< string> familyCodes)
        {
            return _prepareProductDao.GetDBPrepareFamilyList(familyCodes);
        }

        /// <summary>
        /// 根据产品族ids获取产品产前准备
        /// </summary>
        /// <param name="familyIds"></param>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        public virtual List<PrepareProduct> GetPrepareProductListByFamilyIds(List<double?> familyIds, List<double> parentIds)
        {
            return _prepareProductDao.GetPrepareProductListByFamilyIds(familyIds, parentIds);
        }

        /// <summary>
        /// 根据工序ids获取产品产前准备子表
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProductDetail> GetDetailByProcessIds(List<double?> processIds)
        {
            return _prepareProductDao.GetDetailByProcessIds(processIds);
        }

        /// <summary>
        /// 根据ids获取产品产前准备
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProduct> GetPrepareProductListByIds(List<double> ids)
        {
            return _prepareProductDao.GetPrepareProductListByIds(ids);
        }

        /// <summary>
        /// 根据ids获取产品产前准备子表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProductDetail> GetPrepareProductDetailListByIds(List<double> ids)
        {
            return _prepareProductDao.GetPrepareProductDetailListByIds(ids);
        }

        /// <summary>
        /// 根据工序ids获取工序
        /// </summary>
        /// <param name="processeIds"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcesseByIds(List<double?> processeIds)
        {
            return _prepareProductDao.GetProcesseByIds(processeIds);
        }

        /// <summary>
        /// 查询产品产前准备设置子表条数
        /// </summary>
        /// <param name="preProductIds"></param>
        /// <returns></returns>
        public virtual int GetDetailTotalCount(List<double> preProductIds)
        {
            return _prepareProductDao.GetDetailTotalCount(preProductIds);
        }

        /// <summary>
        /// 导入时根据产品编码获取产品
        /// </summary>
        /// <param name="proCodes"></param>
        /// <returns></returns>
        public virtual EntityList<Item> ImGetProductByCodes(List<string> proCodes)
        {
            return _prepareProductDao.ImGetProductByCodes(proCodes);
        }

        /// <summary>
        /// 编辑选择半成品、成品物料
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Item> EditGetProduct(PagingInfo pagingInfo, string keyword)
        {
            return _prepareProductDao.EditGetProduct(pagingInfo, keyword);
        }

        /// <summary>
        /// 导入时根据产品族编码获取产品族
        /// </summary>
        /// <param name="familyCodes"></param>
        /// <returns></returns>
        public virtual EntityList<ProductFamily> ImGetFamilyByCodes(List<string> familyCodes)
        {
            return _prepareProductDao.ImGetFamilyByCodes(familyCodes);
        }

        /// <summary>
        /// 导入时根据工序编码获取工序
        /// </summary>
        /// <param name="processCodes"></param>
        /// <returns></returns>
        public virtual EntityList<Process> ImGetProcessByCodes(List<string> processCodes)
        {
            return _prepareProductDao.ImGetProcessByCodes(processCodes);
        }

        /// <summary>
        /// 导入时根据产前项目编码获取产前项目
        /// </summary>
        /// <param name="proCodes"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProject> ImGetProjectByCodes(List<string> proCodes)
        {
            return _prepareProductDao.ImGetProjectByCodes(proCodes);
        }

        /// <summary>
        /// 产品产前准备设置保存命令
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void PrepareProductSave(EntityList data)
        {
            if (data == null || (data.Count == 0 && data.DeletedList.Count == 0))
            {
                throw new ValidationException("保存数据异常！".L10N());
            }
            // 主表数据
            var parentData = data as EntityList<PrepareProduct>;
            if (parentData.Any(p => (p.ProductId == 0 || p.ProductId == null) && (p.ProductFamilyId == 0 || p.ProductFamilyId == null)))
            {
                throw new ValidationException("产品编码和产品族编码必须维护其一！".L10N());
            }
            // 子表数据
            EntityList<PrepareProductDetail> childData = new EntityList<PrepareProductDetail>();

            parentData.ForEach(parent =>
            {
                childData.AddRange(parent.PrepareProjectDetailList.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).ToList());
            });
            if (childData.Any(p => p.PrepareProjectId == 0))
            {
                throw new ValidationException("产前准备项目编码不能为空！".L10N());
            }
            // 主表id
            var parentIds = parentData.Select(p => p.Id).ToList();
            // 主表产品id
            var parentProductIds = parentData.Where(p => p.ProductId != null && p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.ProductId).ToList();
            // 主表产品族id
            var parentFamilyIds = parentData.Where(p => p.ProductFamilyId != null && p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.ProductFamilyId).ToList();
            // 数据库产品ids
            var dbProductIds = GetPrepareProductListByProductIds(parentProductIds, parentIds).Select(p => p.ProductId).ToList();
            var dbFamilyIds = GetPrepareProductListByFamilyIds(parentFamilyIds, parentIds).Select(p => p.ProductFamilyId).ToList();
            List<double?> repeatProductList = new List<double?>();
            List<double?> repeatFamilyList = new List<double?>();
            repeatProductList.AddRange(parentProductIds ?? new List<double?>());
            repeatProductList.AddRange(dbProductIds ?? new List<double?>());
            repeatFamilyList.AddRange(parentFamilyIds ?? new List<double?>());
            repeatFamilyList.AddRange(dbFamilyIds ?? new List<double?>());
            if (repeatProductList.Count != repeatProductList.Distinct().Count())
            {
                throw new ValidationException("产品唯一！".L10N());
            }
            if (repeatFamilyList.Count != repeatFamilyList.Distinct().Count())
            {
                throw new ValidationException("产品族唯一！".L10N());
            }
            // 数据库子表
            var dbChildList = GetPrepareProductDetailList(parentIds);
            childData.AddRange(dbChildList);
            var groupby = childData.GroupBy(p => new { p.PrepareProductId, p.ProcessId, p.PrepareProjectId }).ToList();
            if (groupby.Exists(p => p.Count() > 1))
            {
                throw new ValidationException("工序+项目唯一！".L10N());
            }

            // 验证子表数据工序是否为产品族下的工序
            parentData.ForEach(parent =>
            {
                var parentfamilyId = parent.ProductFamilyId;
                if (parentfamilyId != null && parentfamilyId != 0)
                {
                    var childProcessIds = parent.PrepareProjectDetailList.Where(p => p.ProcessId != null).Select(p => p.ProcessId).ToList();
                    var childFamilyIds = GetProcesseByIds(childProcessIds).Select(p => p.ProductFamilyId).ToList();
                    if (childFamilyIds.Exists(id => id != parentfamilyId))
                    {
                        throw new ValidationException("工序不属于当前产品族！".L10N());
                    }
                }
            });
        }
    }
}
