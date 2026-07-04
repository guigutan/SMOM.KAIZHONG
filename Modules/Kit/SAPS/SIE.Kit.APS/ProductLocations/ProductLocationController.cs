using SIE.Domain;
using SIE.Kit.APS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.ProductLocations
{
    /// <summary>
    /// 产品定位逻辑处理
    /// </summary>
    public class ProductLocationController : DomainController
    {
        /// <summary>
        /// 查询产品定位列表
        /// </summary>
        /// <returns>返回产品定位列表</returns>
        public virtual EntityList<ProductLocation> GetAllProductLocation()
        {
            return Query<ProductLocation>().ToList();
        }

        /// <summary>
        /// 查询产品定位列表
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回产品定位列表</returns>
        public virtual EntityList<ProductLocation> GetProductLocationList(ProductLocationCriteria criteria)
        {
            var query = Query<ProductLocation>();
            if (criteria.EnterpriseId > 0)
            {
                query.Where(p => p.EnterpriseId == criteria.EnterpriseId);
            }
            if (criteria.Classification.HasValue)
            {
                query.Where(p => p.Classification == criteria.Classification);
            }
            if (criteria.TypeValue.IsNotEmpty())
            {
                query.Where(p => p.TypeValue == criteria.TypeValue);
            }
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();

            var list = query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, elo);
            foreach (var item in list)
            {
                switch (item.Classification)
                {
                    case Classification.Industry:
                        var Industry = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(Classification.Industry, null, null).Where(s => s.Key == item.TypeValue).FirstOrDefault();
                        if (Industry != null) item.TypeValue = Industry.Value;

                        break;
                    case Classification.Product:
                        var Product = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(Classification.Product, null, null).Where(s => s.Key == item.TypeValue).FirstOrDefault();
                        if (Product != null) item.TypeValue = Product.Value;

                        break;
                    //case Classification.SpecialProcess:
                    //    var SpecialProcess = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(Classification.SpecialProcess, null, null).Where(s => s.Key == item.TypeValue).FirstOrDefault();
                    //    if (SpecialProcess != null) item.TypeValue = SpecialProcess.Value;
                    //    break;
                }
            }
            return list;
        }

        /// <summary>
        /// 加载分类值字典
        /// </summary>
        /// <param name="TypeValueDic">值字典</param>
        /// <param name="c">分类</param>
        public static void InitValueDic(ref Dictionary<string, string> TypeValueDic, Classification c)
        {
            if (TypeValueDic == null)
            {
                TypeValueDic = new Dictionary<string, string>();
                EntityList<ClassificationInfo> list = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(c, null, null);
                foreach (var item in list)
                {
                    TypeValueDic.Add(item.Value, item.Key);
                }
            }
        }

        /// <summary>
        /// 验证产品定位数据是否有交互
        /// </summary>
        /// <param name="prod">产品定位对象</param>
        /// <returns></returns>
        public virtual bool ValidateProductLocation(ProductLocation prod)
        {
            TransformationTypeValue(prod);
            var query = Query<ProductLocation>().Where(p => p.Classification == prod.Classification && p.TypeValue == prod.TypeValue);
            if (prod.Id > 0)
            {
                query.Where(p => p.Id != prod.Id);
            }
            query.Where(p => p.EnterpriseId == prod.EnterpriseId);
            
            var list = query.ToList();

            //if (prod.Classification == Classification.SpecialProcess)
            //{
            //    //特殊工艺查询无数据则可直接通过，有值则需要判断最大最小值是否相交！
            //    if (!list.Any()) return true;
            //    var groupList = list.GroupBy(p => new { p.EnterpriseId, p.Classification, p.TypeValue }).Select(p => new
            //    {
            //        p.Key,
            //        toList = p.ToList()
            //    }).FirstOrDefault();
            //    foreach (var item in groupList.toList)
            //    {
            //        if (prod.MinValue < item.MaxValue && prod.MaxValue > item.MinValue)
            //            return false;
            //    }
            //}
            //else
            //{
                //非特殊工艺有查询到数据则表示重复
                if (list.Any()) return false;
            //}
            return true;
        }

        /// <summary>
        /// 根据分类的分类值value转换
        /// </summary>
        /// <param name="p">转换对象</param>
        /// <returns></returns>
        public virtual ProductLocation TransformationTypeValue(ProductLocation p)
        {
            switch (p.Classification)
            {
                case Classification.Industry:
                    var Industry = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(Classification.Industry, null, null).Where(s => s.Value == p.TypeValue).FirstOrDefault();
                    if (Industry != null) p.TypeValue = Industry.Value;

                    break;
                case Classification.Product:
                    var Product = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(Classification.Product, null, null).Where(s => s.Value == p.TypeValue).FirstOrDefault();
                    if (Product != null) p.TypeValue = Product.Value;

                    break;
                //case Classification.SpecialProcess:
                //    var SpecialProcess = RT.Service.Resolve<ClassificationInfoController>().GetClassificationInfoList(Classification.SpecialProcess, null, null).Where(s => s.Value == p.TypeValue).FirstOrDefault();
                //    if (SpecialProcess != null) p.TypeValue = SpecialProcess.Key;
                //    break;
            }
            return p;
        }
    }
}
