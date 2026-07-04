using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.Items.ProductFamilys
{
    /// <summary>
    /// 产品族控制器
    /// </summary>
    public class ProductFamilyController : DomainController
    {
        /// <summary>
        /// 根据名称获取产品族分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>产品族分类</returns>
        public virtual ProductFamilyCategory GetProductFamilyCateByName(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            return Query<ProductFamilyCategory>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 随机获取一个产品族
        /// </summary>
        /// <returns></returns>
        public virtual ProductFamily GetProductFamilyFirst()
        {
            var first = Query<ProductFamily>().FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }

        /// <summary>
        /// 根据名称获取产品族分类
        /// </summary>
        /// <param name="productFamily">产品族编码或名称</param>
        /// <returns>产品族分类</returns>
        public virtual ProductFamily GetProductFamilyByCodeOrName(string productFamily)
        {
            Check.NotNullOrEmpty(productFamily, nameof(productFamily));
            return Query<ProductFamily>().Where(p => p.Code == productFamily || p.Name == productFamily).FirstOrDefault();
        }

        /// <summary>
        /// 查询获取产品族
        /// </summary>
        /// <returns>列表</returns>
        public virtual EntityList<ProductFamily> GetProductFamily(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<ProductFamily>();
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pagingInfo);
        }

        /// <summary>
        /// 根据产品族Id列表获取产品族列表
        /// </summary>
        /// <param name="familyIds">产品族Id列表</param>
        /// <returns>产品族列表</returns>
        public virtual EntityList<ProductFamily> GetProductFamilyList(List<double> familyIds)
        {
            Check.NotNull(familyIds, "产品族ID集合".L10N());
            return Query<ProductFamily>().Where(p => familyIds.Contains(p.Id)).ToList();
        }
    }
}