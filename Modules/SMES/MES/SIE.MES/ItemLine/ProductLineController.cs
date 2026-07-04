using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LoadItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules.CTypes;

namespace SIE.MES.ItemLine
{
    /// <summary>
    /// 产品与产线关系控制器
    /// </summary>
    public class ProductLineController : DomainController
    {
        #region 查询产品与产线关系

        /// <summary>
        /// 根据资源、产品获取产品与产线关系
        /// </summary>
        /// <param name="wipResourceCodes"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public virtual EntityList<ProductLine> GetProductLinesByWipResourceCodesItemCodes(List<string> wipResourceCodes, List<string> itemCodes)
        {
            var list = wipResourceCodes.SplitContains(wrcs =>
            {
                return itemCodes.SplitContains(ics =>
                {
                    return Query<ProductLine>().Where(p => wrcs.Contains(p.WipResource.Code) && ics.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });
            return list;
        }

        /// <summary>
        /// 根据工序、产品获取产品与产线关系
        /// </summary>
        /// <param name="processCodes"></param>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public virtual EntityList<ProductLine> GetProductLinesByProcessCodesItemCodes(List<string> processCodes, List<string> itemCodes)
        {
            var list = processCodes.SplitContains(pcs =>
            {
                return itemCodes.SplitContains(ics =>
                {
                    return Query<ProductLine>().Where(p => pcs.Contains(p.Process.Code) && ics.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });
            return list;
        }

        /// <summary>
        /// 根据工序、资源获取产品与产线关系
        /// </summary>
        /// <param name="processCodes"></param>
        /// <param name="wipResourceCodes"></param>
        /// <returns></returns>
        public virtual EntityList<ProductLine> GetProductLinesByProcessCodesWipResourceCodes(List<string> processCodes, List<string> wipResourceCodes)
        {
            var list = processCodes.SplitContains(pcs =>
            {
                return wipResourceCodes.SplitContains(wrcs =>
                {
                    return Query<ProductLine>().Where(p => pcs.Contains(p.Process.Code) && wrcs.Contains(p.WipResource.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            });
            return list;
        }

        /// <summary>
        /// 根据物料编码和工序编码获取产品与产线关系
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="processCode"></param>
        /// <returns></returns>
        public virtual EntityList<ProductLine> GetProductLine(string itemCode, string processCode)
        {
            var list = Query<ProductLine>().Where(p => p.Item.Code == itemCode && p.Process.Code == processCode).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 查询产品与产线关系
        /// </summary>
        /// <param name="criterial"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<ProductLine> CriterialProductLine(ProductLineCriterial criterial)
        {
            if (criterial == null)
            {
                throw new ValidationException("产品与产线关系查询实体异常！".L10N());
            }
            var query = Query<ProductLine>();
            //if (criterial.State.HasValue)
            //{
            //    query.Where(p => p.State == criterial.State.Value);
            //}
            if (criterial.ItemId.HasValue)
            {
                if (criterial.ItemId > 0)
                {
                    query.Where(p => p.ItemId == criterial.ItemId);
                }
            }
            if (criterial.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criterial.ProcessId);
            }
            if (criterial.WipResourceId.HasValue)
            {
                query.Where(p => p.WipResourceId == criterial.WipResourceId);
            }
            if (!criterial.ItemName.IsNullOrEmpty())
            {
                query.Where(m => m.Item.Name.Contains("%"+criterial.ItemName+"%"));
            }
            if (!criterial.LineName.IsNullOrEmpty())
            {
                query.Where(m => m.WipResource.Name.Contains("%"+criterial.LineName+"%"));
            }
            if (!criterial.ProcessCode.IsNullOrEmpty())
            {
                query.Where(p => p.Process.Code .Contains( criterial.ProcessCode));
            }
            return query.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 启用产品与产线关系
        /// <summary>
        /// 启用产品与产线关系
        /// </summary>
        /// <returns></returns>
        public virtual void EnableProductLine(List<double> LineIds)
        {
            //var lineList = LineIds.SplitContains(tempIds =>
            //{
            //    return Query<ProductLine>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //});
            //lineList.ForEach(p =>
            //{
            //    p.State = State.Enable;
            //});
            //RF.Save(lineList);
        }
        #endregion

        #region 禁用产品与产线关系
        /// <summary>
        /// 禁用产品与产线关系
        /// </summary>
        /// <param name="LineIds"></param>
        public virtual void DisableProductLine(List<double> LineIds)
        {
            //var lineList = LineIds.SplitContains(tempIds =>
            //{
            //    return Query<ProductLine>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //});
            //lineList.ForEach(p =>
            //{
            //    p.State = State.Disable;
            //});
            //RF.Save(lineList);
        }
        #endregion

        #region 是否有相同的数据
        /// <summary>
        /// 是否有相同的数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="processId"></param>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual bool GetProductLineBool(double itemId, double processId, double wipId)
        {
            var query = Query<ProductLine>().Where(p => p.ItemId == itemId && p.ProcessId == processId && p.WipResourceId == wipId).ToList();
            if (query.Count > 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}
