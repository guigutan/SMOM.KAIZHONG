using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Kit.APS.ProductLocations;
using SIE.SO.SaleOrders;
using SIE.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.Common
{
    /// <summary>
    /// 分类值控制器
    /// </summary>
    public class ClassificationInfoController : DomainController
    {
        /// <summary>
        /// 根据分类获取分类值
        /// </summary>
        /// <param name="upperLevel"></param>
        /// <param name="page"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<ClassificationInfo> GetClassificationInfoList(Classification upperLevel, PagingInfo page, string keyword = null)
        {
            EntityList<ClassificationInfo> sourceList = new EntityList<ClassificationInfo>();
            ClassificationInfo c = new ClassificationInfo();
            //switch (upperLevel)
            //{
                //case Classification.SpecialProcess:
                //    List<EnumViewModel> list = EnumViewModel.GetByEnumType(typeof(System.Diagnostics.Process));
                //    if (page != null)
                //    {
                //        //分页加载枚举类型数据并有值查询
                //        if (!string.IsNullOrWhiteSpace(keyword))
                //        {
                //            var spList = list.Where(p => p.Label.Contains(keyword) || p.EnumValue.ToString().Contains(keyword))
                //                .Select(p => new ClassificationInfo() { Key = p.EnumValue.ToString(), Value = p.Label })
                //                .ToList();
                //            var tmpList = spList.Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();
                //            sourceList.AddRange(tmpList);
                //            sourceList.SetTotalCount(spList.Count);
                //        }
                //        else
                //        {
                //            var spList = list.Select(p => new ClassificationInfo() { Key = p.EnumValue.ToString(), Value = p.Label })
                //               .ToList();
                //            var tmpList = spList.Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();
                //            sourceList.AddRange(tmpList);
                //            sourceList.SetTotalCount(spList.Count);
                //        }
                //    }
                //    else
                //    {
                //        //正常加载枚举类型数据
                //        foreach (var item in list)
                //        {
                //            c = new ClassificationInfo()
                //            {
                //                Key = item.EnumValue.ToString(),
                //                Value = item.Label
                //            };
                //            sourceList.Add(c);
                //        }
                //    }
                //    break;
                //default:
                    string KM = string.Empty;
                    if (upperLevel == Classification.Industry)
                    {
                        KM = SaleOrderDetail.INDUSTRYTYPE;
                    }
                    else if (upperLevel == Classification.Product)
                    {
                        KM = SaleOrderDetail.PRODUCTTYPE;
                    }
                    EntityList<Catalog> categoryList = RT.Service.Resolve<CatalogController>().GetCatalogList(KM, page, keyword);
                    foreach (Catalog catalogItem in categoryList)
                    {
                        c = new ClassificationInfo()
                        {
                            Key = catalogItem.Code,
                            Value = catalogItem.Name
                        };
                        sourceList.Add(c);
                    }
                    if (categoryList.TotalCount > 0)
                        sourceList.SetTotalCount(categoryList.TotalCount);
                    else
                        sourceList.SetTotalCount(categoryList.Count);
                  //  break;
           // }
            return sourceList;
        }
    }
}
