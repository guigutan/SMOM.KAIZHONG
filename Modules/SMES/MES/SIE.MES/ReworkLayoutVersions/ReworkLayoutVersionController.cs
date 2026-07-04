using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions
{
    public class ReworkLayoutVersionController : DomainController
    {

        #region 返工信息

        /// <summary>
        /// 返工信息查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ReworkInfoRecord> CriteriaReworkInfoRecord(ReworkInfoRecordCriteria criteria)
        {
            var q = DB.Query<ReworkInfoRecord>("rir");
            if (criteria.State != null)
                q.Where(p => p.State == criteria.State);
            if (!criteria.Factory.IsNullOrEmpty())
                q.Where(p => p.Factory.Contains(criteria.Factory));
            if (criteria.ItemId != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.BeginDateTime.BeginValue != null)
                q.Where(p => p.BeginDateTime >= criteria.BeginDateTime.BeginValue.Value);
            if (criteria.BeginDateTime.EndValue != null)
                q.Where(p => p.BeginDateTime <= criteria.BeginDateTime.EndValue.Value);
            if (criteria.EndDateTime.BeginValue != null)
                q.Where(p => p.EndDateTime >= criteria.EndDateTime.BeginValue.Value);
            if (criteria.EndDateTime.EndValue != null)
                q.Where(p => p.EndDateTime <= criteria.EndDateTime.EndValue.Value);
            if (criteria.ReworkLayoutVersionId != null)
                q.Where(p => p.ReworkLayoutVersionId == criteria.ReworkLayoutVersionId);
            if (!criteria.UniqueCode.IsNullOrEmpty())
                q.Where(p => p.UniqueCode.Contains(criteria.UniqueCode));
            if (!criteria.Department.IsNullOrEmpty())
                q.Where(p => p.Department.Contains(criteria.Department));
            if (!criteria.ProductOrder.IsNullOrEmpty())
                q.Where(p => p.ProductOrder.Contains(criteria.ProductOrder));
            if (!criteria.Msg.IsNullOrEmpty())
                q.Where(p => p.Msg.Contains(criteria.Msg));
            if (!criteria.Sn.IsNullOrEmpty())
            {
                q.Where(p => p.SQL<bool>(@"exists(select 1 from REWORK_INFO_RECORD_DTL rird 
                                           inner join WIP_BATCH wb on wb.id = rird.Wip_Batch_Id and wb.is_phantom = 0
                                            where rir.id = rird.Rework_Info_Record_Id and rird.is_phantom = 0 and {0})".L10nFormat(criteria.Sn.Contains("%") ? $"wb.batch_no like '{criteria.Sn}'" : $"wb.batch_no = '{criteria.Sn}'")));
            }
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                q.Where(p => p.SQL<bool>(@"exists(select 1 from REWORK_INFO_RECORD_DTL rird 
                                           inner join WIP_BATCH wb on wb.id = rird.Wip_Batch_Id and wb.is_phantom = 0
                                            inner join wo wo on wo.id = wb.work_order_id and wo.is_phantom = 0
                                            where rir.id = rird.Rework_Info_Record_Id and rird.is_phantom = 0 and {0})".L10nFormat(criteria.WorkOrderNo.Contains("%") ? $"wo.no like '{criteria.WorkOrderNo}'" : $"wo.no = '{criteria.WorkOrderNo}'")));
            }
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据Id获取返工信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<ReworkInfoRecord> GetReworkInfoRecordsByIds(List<double> ids)
        {
            var list = ids.SplitContains(temp =>
            {
                return Query<ReworkInfoRecord>().Where(p => temp.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 根据标签号获取返工信息的标签信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public virtual ReworkInfoRecordDtl GetReworkInfoRecordDtl(string sn)
        {
            var first = Query<ReworkInfoRecordDtl>().Where(p => p.WipBatch.BatchNo == sn).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }

        /// <summary>
        /// 根据标签号获取返工信息的标签信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public virtual EntityList<ReworkInfoRecordDtl> GetReworkInfoRecordDtls(string sn)
        {
            var list = Query<ReworkInfoRecordDtl>().Where(p => p.WipBatch.BatchNo == sn).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        #region 返工工艺路线版

        /// <summary>
        /// 根据物料编码获取返工工艺路线版本
        /// </summary>
        /// <param name="itemCodes"></param>
        /// <returns></returns>
        public virtual EntityList<ReworkLayoutVersion>  GetReworkLayoutVersionsByItemCodes(List<string> itemCodes)
        {
            var list = itemCodes.SplitContains(codes =>
            {
                return Query<ReworkLayoutVersion>().Where(p => codes.Contains(p.Item.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        #endregion
    }
}
