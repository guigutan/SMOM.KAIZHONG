using SIE.Domain;
using SIE.MES.WIP.Pressure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Engrave
{
    public class EngraveLabelController : DomainController
    {
        #region 刻码替换记录

        /// <summary>
        /// 刻码替换记录查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<EngraveLabelSnReplaceLog> CriteriaEngraveLabelSnReplaceLog(EngraveLabelSnReplaceLogCriteria criteria)
        {
            var q = Query<EngraveLabelSnReplaceLog>();
            if (!criteria.OldSn.IsNullOrEmpty())
                q.Where(p => p.OldSn.Contains(criteria.OldSn));
            if (!criteria.NewSn.IsNullOrEmpty())
                q.Where(p => p.NewSn.Contains(criteria.NewSn));
            if (!criteria.ProductCode.IsNullOrEmpty())
                q.Where(p => p.EngraveSn.EngraveLabel.Product.Code.Contains(criteria.ProductCode));
            if (!criteria.ProductName.IsNullOrEmpty())
                q.Where(p => p.EngraveSn.EngraveLabel.Product.Name.Contains(criteria.ProductName));
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.EngraveSn.EngraveLabel.Product.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.BatchNo.IsNullOrEmpty())
                q.Where(p => p.EngraveSn.EngraveLabel.BatchNo.Contains(criteria.BatchNo));
            if (!criteria.BlueLabel.IsNullOrEmpty())
                q.Where(p => p.PackingDetail.PackingQc.BlueLabel.Contains(criteria.BlueLabel));

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        #endregion

        public virtual EntityList<EngraveLabel> CriteriaEngraveLabel(EngraveLabelCriteria criteria)
        {
            var q = Query<EngraveLabel>();
            if (criteria.WorkOrderId.HasValue)
                q.Where(p => p.WorkOrderId == criteria.WorkOrderId);
            if (criteria.ResourceId.HasValue)
                q.Where(p => p.ResourceId == criteria.ResourceId);
            if (!criteria.BatchNo.IsNullOrEmpty())
                q.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            if (criteria.ProductId.HasValue)
                q.Where(p => p.ProductId == criteria.ProductId);
            if (!criteria.Sn.IsNullOrEmpty())
                q.Exists<EngraveSn>((a, b) => b.Where(p => p.EngraveLabelId == a.Id && p.Sn.Contains(criteria.Sn)));
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 判断刻码是否存在
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public virtual EngraveLabel BoolEngraveLabel(string batchNo)
        {
            return Query<EngraveLabel>().Where(p => p.BatchNo == batchNo).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 报错刻码标签
        /// </summary>
        /// <param name="engrave"></param>
        /// <returns></returns>
        public virtual bool SaveEngraveLabel(EngraveLabel engrave)
        {
            try
            {
                RF.Save(engrave);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取刻码子表
        /// </summary>
        /// <param name="engraveId"></param>
        /// <returns></returns>
        public virtual EntityList<EngraveSn> GetEngraveSns(double engraveId)
        {
            return Query<EngraveSn>().Where(p => p.EngraveLabelId == engraveId).ToList();
        }

        /// <summary>
        /// 获取刻码标签
        /// </summary>
        /// <param name="Sn"></param>
        /// <returns></returns>
        public virtual EngraveSn GetEngraveSn(string Sn)
        {
            return Query<EngraveSn>().Where(p => p.Sn == Sn).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 刻码SN是否存在
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public virtual bool BoolEngraveSn(string sn)
        {
            var query = Query<EngraveSn>().Where(p => p.Sn == sn).ToList();
            if (query.FirstOrDefault() != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 根据刻码SN列表标签查询
        /// </summary>
        /// <param name="snList"></param>
        /// <returns></returns>
        public virtual EntityList<EngraveSn> GetEngraveSnSns(List<string> snList)
        {
            var list = snList.SplitContains(temp =>
            {
                return Query<EngraveSn>().Where(p => snList.Contains(p.Sn)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }
    }
}
