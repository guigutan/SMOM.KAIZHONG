using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrderArchives;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.PrepareProducts.Daos
{

    /// <summary>
    /// 产前准备记录数据层
    /// </summary>
    public class PrepareRecordDao : BaseDao<PrepareRecord>
    {

        /// <summary>
        /// 查询产前准备记录
        /// </summary>
        /// <param name="prepareRecordCriteria"></param>
        /// <returns></returns>
        public EntityList<PrepareRecord> QueryPrepareRecordList(PrepareRecordCriteria prepareRecordCriteria)
        {
            var query = Query();
            if (prepareRecordCriteria == null)
            {
                throw new ValidationException("产前准备记录查询实体异常！".L10N());
            }
            if (prepareRecordCriteria.No.IsNotEmpty())
            {
                query.Where(p => p.No.Contains(prepareRecordCriteria.No));
            }
            if (prepareRecordCriteria.FactoryId != 0 && prepareRecordCriteria.FactoryId != null)
            {
                query.Where(p => p.FactoryId == prepareRecordCriteria.FactoryId);
            }
            if (prepareRecordCriteria.WorkShopId != 0 && prepareRecordCriteria.WorkShopId != null)
            {
                query.Where(p => p.WorkShopId == prepareRecordCriteria.WorkShopId);
            }
            if (prepareRecordCriteria.ResourceId != 0 && prepareRecordCriteria.ResourceId != null)
            {
                query.Where(p => p.ResourceId == prepareRecordCriteria.ResourceId);
            }
            if (prepareRecordCriteria.ProductName.IsNotEmpty())
            {
                query.Where(p => p.Product.Name.Contains(prepareRecordCriteria.ProductName));
            }
            if (prepareRecordCriteria.State.IsNotEmpty())
            {
                var criteriaState = new List<int>();
                prepareRecordCriteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.State));
            }
            if (prepareRecordCriteria.PreState.HasValue)
            {
                query.Where(p => p.PrepareState == prepareRecordCriteria.PreState);
            }
            if (prepareRecordCriteria.PlanBeginTime.BeginValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate >= prepareRecordCriteria.PlanBeginTime.BeginValue);
            }
            if (prepareRecordCriteria.PlanBeginTime.EndValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate <= prepareRecordCriteria.PlanBeginTime.EndValue);
            }
            if (prepareRecordCriteria.ConfirmTime.BeginValue.HasValue || prepareRecordCriteria.ConfirmTime.EndValue.HasValue)
            {
                query.Exists<PrepareRecordDetail>((x, y) => y.WhereIf(prepareRecordCriteria.ConfirmTime.BeginValue.HasValue, p => p.ConfirmTime >= prepareRecordCriteria.ConfirmTime.BeginValue)
                .WhereIf(prepareRecordCriteria.ConfirmTime.EndValue.HasValue, p => p.ConfirmTime <= prepareRecordCriteria.ConfirmTime.EndValue)
                .Where(p => p.PrepareRecordId == x.Id));
            }
            query.Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.FactoryId && p.EmployeeId == RT.IdentityId));
            return query.OrderBy(prepareRecordCriteria.OrderInfoList).ToList(prepareRecordCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取已有的记录
        /// </summary>
        /// <param name="prepareRecord"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareRecordDetail> GetHadRecord(PrepareRecord prepareRecord)
        {
            return DB.Query<PrepareRecordDetail>().Where(
                m => m.PrepareRecordId == prepareRecord.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品或产品族的已设置的项目明细
        /// </summary>
        /// <param name="prepareRecord"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareProductDetail> GetProductOrFamilyDetails(PrepareRecord prepareRecord)
        {
            var resultList = DB.Query<PrepareProductDetail>().Join<PrepareProduct>((x, y) => x.PrepareProductId == y.Id).
                    Where<PrepareProduct>((p, q) => (q.ProductId != null && q.ProductId == prepareRecord.ProductId))
                    .OrderBy(m => m.PrepareProjectType).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (resultList.Any())
            {
                return resultList;
            }
            else
            {
                return DB.Query<PrepareProductDetail>().Join<PrepareProduct>((x, y) => x.PrepareProductId == y.Id).
                Where<PrepareProduct>((p, q) => (q.ProductFamilyId != null && q.ProductFamilyId == prepareRecord.ProductFamilyId))
                .OrderBy(m => m.PrepareProjectType).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        /// <summary>
        /// 根据主表id获取明细子表
        /// </summary>
        /// <param name="preRecordId"></param>
        /// <returns></returns>
        public EntityList<PrepareRecordDetail> GetPrepareRecordDetailList(double preRecordId)
        {
            return DB.Query<PrepareRecordDetail>().Where(p => p.PrepareRecordId == preRecordId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
