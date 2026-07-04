using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Items;
using SIE.MES.PrepareProducts.Enums;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.PrepareProducts.Daos
{
    /// <summary>
    /// 产品产前准备DAO
    /// </summary>
    public class PrepareProductDao : BaseDao<PrepareProduct>
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EntityList<PrepareProduct> QueryPreProductEntityList(PrepareProductCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<PrepareProduct>();
            }
            var query = Query();
            if (criteria.ProductCode.IsNotEmpty())
            {
                query.Where(p => p.Product.Code.Contains(criteria.ProductCode));
            }
            if (criteria.ProductName.IsNotEmpty())
            {
                query.Where(p => p.Product.Name.Contains(criteria.ProductName));
            }
            if (criteria.ProductFamilyId != 0 && criteria.ProductFamilyId != null)
            {
                query.Where(p => p.ProductFamilyId == criteria.ProductFamilyId);
            }
            if ((criteria.ProcessId != 0 && criteria.ProcessId != null) || criteria.ProjectCode.IsNotEmpty() || criteria.ProjectName.IsNotEmpty() || criteria.ProjectType.HasValue)
            {
                query.Exists<PrepareProductDetail>((x, y) => y.Where(p => p.PrepareProductId == x.Id)
                .WhereIf(criteria.ProcessId != 0 && criteria.ProcessId != null, p => p.ProcessId == criteria.ProcessId)
                .WhereIf(criteria.ProjectCode.IsNotEmpty(), p => p.PrepareProject.ProCode.Contains(criteria.ProjectCode))
                .WhereIf(criteria.ProjectName.IsNotEmpty(), p => p.PrepareProject.ProName.Contains(criteria.ProjectName))
                .WhereIf(criteria.ProjectType.HasValue, p => p.PrepareProject.ProType == criteria.ProjectType));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 导出查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public EntityList<PrepareProduct> QueryPreProductEntityList(ExportPrepareProductModel model)
        {
            if (model == null)
            {
                return new EntityList<PrepareProduct>();
            }
            var query = Query();
            if (model.ProductCode.IsNotEmpty())
            {
                query.Where(p => p.Product.Code.Contains(model.ProductCode));
            }
            if (model.ProductCode.IsNotEmpty())
            {
                query.Where(p => p.Product.Name.Contains(model.ProductName));
            }
            if (model.ProFamilyCode.IsNotEmpty())
            {
                query.Where(p => p.ProductFamily.Code.Contains(model.ProFamilyCode));
            }
            if (model.ProFamilyName.IsNotEmpty())
            {
                query.Where(p => p.ProductFamily.Name.Contains(model.ProFamilyName));
            }
            if ((model.ProcessId != 0 && model.ProcessId != null) || model.ProjectCode.IsNotEmpty() || model.ProjectName.IsNotEmpty() || model.ProjectType.HasValue)
            {
                query.Exists<PrepareProductDetail>((x, y) => y.Where(p => p.PrepareProductId == x.Id)
                .WhereIf(model.ProcessId != 0 && model.ProcessId != null, p => p.ProcessId == model.ProcessId)
                .WhereIf(model.ProjectCode.IsNotEmpty(), p => p.PrepareProject.ProCode.Contains(model.ProjectCode))
                .WhereIf(model.ProjectName.IsNotEmpty(), p => p.PrepareProject.ProName.Contains(model.ProjectName))
                .WhereIf(model.ProjectType.HasValue, p => p.PrepareProject.ProType == model.ProjectType));
            }
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询产品产前准备设置子表条数
        /// </summary>
        /// <param name="preProductIds"></param>
        /// <returns></returns>
        public int GetDetailTotalCount(List<double> preProductIds)
        {
            var query = preProductIds.SplitContains(tempIds =>
            {
                return DB.Query<PrepareProductDetail>().Where(p => tempIds.Contains(p.PrepareProductId)).ToList();
            });
            return query.Count;
        }

        /// <summary>
        /// 根据id产前准备项目是否被引用
        /// </summary>
        /// <param name="preProductIds"></param>
        /// <returns></returns>
        public int GetPrepareProductDetailCount(List<double> preProductIds)
        {
            int count = 0;
            preProductIds.SplitDataExecute(tempIds =>
            {
                count += DB.Query<PrepareProductDetail>().Where(p => tempIds.Contains(p.PrepareProjectId)).Count();
            });
            return count;
        }

        /// <summary>
        /// 根据主表id获取产品产前子表
        /// </summary>
        /// <param name="preProductId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<PrepareProductDetail> GetPrepareProductDetailList(double preProductId, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            var query = DB.Query<PrepareProductDetail>().Where(p => p.PrepareProductId == preProductId)
                .OrderBy(p => p.Process.Name)
                .OrderBy(p => p.PrepareProject.ProType)
                .OrderBy(p => p.PrepareProject.ProCode)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }

        /// <summary>
        /// 根据主表ids获取产品产前子表
        /// </summary>
        /// <param name="preProductIds"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<PrepareProductDetail> GetPrepareProductDetailList(List<double> preProductIds, PagingInfo pagingInfo = null)
        {
            var query = preProductIds.SplitContains(tempIds =>
            {
                return DB.Query<PrepareProductDetail>().Where(p => tempIds.Contains(p.PrepareProductId)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            });
            return query;
        }

        /// <summary>
        /// 根据主表产品族获取工序
        /// </summary>
        /// <param name="preDetail"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public EntityList<Process> GetProcessListByFamilyId(PrepareProductDetail preDetail, PagingInfo pagingInfo, string keyword)
        {
            if (preDetail.ProFamiliyId != null && preDetail.ProFamiliyId != 0)
            {
                var query = DB.Query<Process>().Where(p => p.ProductFamilyId == preDetail.ProFamiliyId)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                return query;
            }
            else
            {
                var prepareProduct = RF.GetById<PrepareProduct>(preDetail.PrepareProductId);
                if (prepareProduct == null)
                {
                    return new EntityList<Process>();
                }
                var query = DB.Query<Process>().WhereIf(prepareProduct.ProductFamilyId !=0 && prepareProduct.ProductFamilyId != null, p => p.ProductFamilyId == prepareProduct.ProductFamilyId)
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                return query;
            }
        }

        /// <summary>
        /// 根据产品ids获取产品产前准备
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public List<PrepareProduct> GetPrepareProductListByProductIds(List<double?> productIds, List<double> parentIds)
        {
            var preProductList = productIds.SplitContains(tempIds =>
            {
                return Query().Where(p => tempIds.Contains(p.ProductId)).ToList();
            });
            var preProducts = preProductList.Where(p => !parentIds.Contains(p.Id)).ToList();
            return preProducts;
        }

        /// <summary>
        /// 根据产品族ids获取产品产前准备
        /// </summary>
        /// <param name="familyIds"></param>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        public List<PrepareProduct> GetPrepareProductListByFamilyIds(List<double?> familyIds, List<double> parentIds)
        {
            var preProductList = familyIds.SplitContains(tempIds =>
            {
                return Query().Where(p => tempIds.Contains(p.ProductFamilyId)).ToList();
            });
            var preProducts = preProductList.Where(p => !parentIds.Contains(p.Id)).ToList();
            return preProducts;
        }

        /// <summary>
        /// 根据工序ids获取产品产前准备子表
        /// </summary>
        /// <param name="processIds"></param>
        /// <returns></returns>
        public EntityList<PrepareProductDetail> GetDetailByProcessIds(List<double?> processIds)
        {
            var preProductDetailList = processIds.SplitContains(tempIds =>
            {
                return DB.Query<PrepareProductDetail>().Where(p => tempIds.Contains(p.ProcessId)).ToList();
            });
            return preProductDetailList;
        }

        /// <summary>
        /// 根据ids获取产品产前准备
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public EntityList<PrepareProduct> GetPrepareProductListByIds(List<double> ids)
        {
            var query = ids.SplitContains(tempIds =>
            {
                return Query().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return query;
        }

        /// <summary>
        /// 根据ids获取产品产前准备子表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public EntityList<PrepareProductDetail> GetPrepareProductDetailListByIds(List<double> ids)
        {
            var query = ids.SplitContains(tempIds =>
            {
                return DB.Query<PrepareProductDetail>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return query;
        }

        /// <summary>
        /// 根据工序ids获取工序
        /// </summary>
        /// <param name="processeIds"></param>
        /// <returns></returns>
        public EntityList<Process> GetProcesseByIds(List<double?> processeIds)
        {
            var query = processeIds.SplitContains(tempIds =>
            {
                return DB.Query<Process>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return query;
        }

        /// <summary>
        /// 导入时根据产品编码获取产品
        /// </summary>
        /// <param name="proCodes"></param>
        /// <returns></returns>
        public EntityList<Item> ImGetProductByCodes(List<string> proCodes)
        {
            var quety = proCodes.SplitContains(tempCodes =>
            {
                return DB.Query<Item>().Where(p => tempCodes.Contains(p.Code)).ToList();
            });
            return quety;
        }

        /// <summary>
        /// 导入时根据产品族编码获取产品族
        /// </summary>
        /// <param name="familyCodes"></param>
        /// <returns></returns>
        public EntityList<ProductFamily> ImGetFamilyByCodes(List<string> familyCodes)
        {
            var query = familyCodes.SplitContains(tempCodes =>
            {
                return DB.Query<ProductFamily>().Where(p => tempCodes.Contains(p.Code)).ToList();
            });
            return query;
        }

        /// <summary>
        /// 导入时根据工序编码获取工序
        /// </summary>
        /// <param name="processCodes"></param>
        /// <returns></returns>
        public EntityList<Process> ImGetProcessByCodes(List<string> processCodes)
        {
            var query = processCodes.SplitContains(tempCodes =>
            {
                return DB.Query<Process>().Where(p => tempCodes.Contains(p.Code)).ToList();
            });
            return query;
        }

        /// <summary>
        /// 导入时根据产前项目编码获取产前项目
        /// </summary>
        /// <param name="proCodes"></param>
        /// <returns></returns>
        public EntityList<PrepareProject> ImGetProjectByCodes(List<string> proCodes)
        {
            var query = proCodes.SplitContains(tempCodes =>
            {
                return DB.Query<PrepareProject>().Where(p => tempCodes.Contains(p.ProCode)).ToList();
            });
            return query;
        }

        /// <summary>
        /// 根据产品编码获取产品产前准备的产品编码
        /// </summary>
        /// <param name="productCodes"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EntityList<PrepareProduct> GetDBPrepareProductList(List<string> productCodes)
        {
            var query = productCodes.SplitContains(tempCodes =>
            {
                return Query().Where(p => tempCodes.Contains(p.Product.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return query;
        }

        /// <summary>
        /// 根据产品族编码获取产品产前准备的产品族编码
        /// </summary>
        /// <param name="familyCodes"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EntityList<PrepareProduct> GetDBPrepareFamilyList(List<string> familyCodes)
        {
            var query = familyCodes.SplitContains(tempCodes =>
            {
                return Query().Where(p => tempCodes.Contains(p.ProductFamily.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return query;
        }

        /// <summary>
        /// 编辑选择半成品、成品物料
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public EntityList<Item> EditGetProduct(PagingInfo pagingInfo, string keyword)
        {
            var query = DB.Query<Item>().Where(p => p.Type == ItemType.Product || p.Type == ItemType.SemiFinished).WhereIf(keyword.IsNotEmpty(),p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
    }

    /// <summary>
    /// 导出命令查询实体
    /// </summary>
    [Serializable]
    public class ExportPrepareProductModel
    {
        /// <summary>
        /// 产品族编码
        /// </summary>
        public string ProFamilyCode { get; set; }

        /// <summary>
        /// 产品族名称
        /// </summary>
        public string ProFamilyName { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType? ProjectType { get; set; }

    }
}
