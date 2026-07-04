using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.Specifications
{
    /// <summary>
    /// 产品规格控制器
    /// </summary>
    public partial class ProductSpecificationController : DomainController
    {
        public virtual EntityList<ProductSpecification> GetProductSpecificationList(ProductSpecificationCriteria criteria)
        {
            var query = Query<ProductSpecification>();

            if (criteria.Code.IsNotEmpty() || criteria.Name.IsNotEmpty() || criteria.SpecificationModel.IsNotEmpty() || criteria.Type.HasValue || criteria.SourceType.HasValue || criteria.State.HasValue || criteria.ItemSourceType.HasValue)
            {
                query.Exists<Item>((x, y) => y.Where(f => f.Id == x.ProductId)
                .WhereIf(criteria.Code.IsNotEmpty(), p => p.Code.Contains(criteria.Code))
                 .WhereIf(criteria.Name.IsNotEmpty(), p => p.Name.Contains(criteria.Name))
                 .WhereIf(criteria.SpecificationModel.IsNotEmpty(), p => p.SpecificationModel.Contains(criteria.SpecificationModel))
                .WhereIf(criteria.Type.HasValue, p => p.Type == criteria.Type)
                .WhereIf(criteria.ItemSourceType.HasValue, p => p.ItemSourceType == criteria.ItemSourceType)
                 .WhereIf(criteria.State.HasValue, p => p.State == criteria.State)
                  .WhereIf(criteria.SourceType.HasValue, p => p.SourceType == criteria.SourceType)
                 );
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品规格件Id列表获取产品规格件列表
        /// </summary>
        /// <param name="ids">产品规格件Id列表</param>
        /// <returns>产品规格件列表</returns>
        public virtual EntityList<ProductSpecification> GetProductSpecifications(List<double> ids)
        {
            return Query<ProductSpecification>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 通过产品规格件id列表获取产品规格件明细列表
        /// </summary>
        /// <param name="productSpecificationIds">产品规格件id列表</param>
        /// <returns>产品规格件明细列表</returns>
        public virtual EntityList<ProductSpecificationDetail> GetProductSpecificationDetails(List<double> productSpecificationIds)
        {
            return Query<ProductSpecificationDetail>().Where(p => productSpecificationIds.Contains(p.ProductSpecificationId)).ToList();
        }

        /// <summary>
        /// 通过规格件清单id列表获取规格件清单列表
        /// </summary>
        /// <param name="productSpecificationIds">规格件清单id列表</param>
        /// <returns>规格件清单列表</returns>
        public virtual EntityList<Specification> GetSpecifications(List<double> specificationIds)
        {
            return Query<Specification>().Where(p => specificationIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 删除产品规格件及其明细列表
        /// </summary>
        /// <param name="selectedIds">产品规格件Id列表</param>
        public virtual void DeletedProdSpecifications(List<double> selectedIds)
        {
            var productSpecifications = GetProductSpecifications(selectedIds);
            var productSpecificationDetails = GetProductSpecificationDetails(selectedIds);

            if (productSpecifications.Count > 0)
                productSpecifications.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            if (productSpecificationDetails.Count > 0)
                productSpecificationDetails.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (productSpecifications.Count > 0)
                    RF.Save(productSpecifications);
                if (productSpecificationDetails.Count > 0)
                    RF.Save(productSpecificationDetails);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取规格件
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>规格件</returns>
        public virtual ProductSpecification GetProductSpecification(double productId)
        {
            return Query<ProductSpecification>().Where(p => p.ProductId == productId).FirstOrDefault();
        }
    }
}
