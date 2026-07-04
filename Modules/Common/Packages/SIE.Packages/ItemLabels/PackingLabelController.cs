using SIE.Api;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.InvOrg;
using SIE.Core.Common;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.Packages.ItemLabels.Configs;
using SIE.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// WMS标签控制器
    /// </summary>
    /// <seealso cref="SIE.DomainController" />
    public class PackingLabelController : DomainController
    {
        /// <summary>
        /// 查询标签条码
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>盘点方案集合</returns>
        public virtual EntityList<PackingLabel> GetPackingLabels(PackingLabelCriteria criteria)
        {
            var query = Query<PackingLabel>();
            query.LeftJoin<Item>((x, y) => x.ItemId == y.Id);
            if (!criteria.PackageNo.IsNullOrEmpty())
            {
                query.Where(p => p.PackageNo.Contains(criteria.PackageNo));
            }
            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }
            if (!criteria.ItemCode.IsNullOrEmpty())
            {
                query.Where<Item>((x, y) => y.Code.Contains(criteria.ItemCode));
            }
            if (!criteria.ItemName.IsNullOrEmpty())
            {
                query.Where<Item>((x, y) => y.Name.Contains(criteria.ItemName));
            }
            if (!criteria.LotCode.IsNullOrEmpty())
            {
                query.Where(p => p.Lot.Contains(criteria.LotCode));
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            if (criteria.CreateById.HasValue)
            {
                query.Where(p => p.CreateBy == criteria.CreateById.Value);
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            var rst = query.ToList(criteria.PagingInfo, elo);

            return rst;
        }

        /// <summary>
        /// 根据标签号获取可用标签
        /// </summary>
        /// <param name="no">标签号</param>
        /// <returns>标签信息</returns>
        /// <remarks>MES、QMS有调用,不要删除</remarks>
        public virtual PackingLabel GetPackingLabel(string no)
        {
            var query = Query<PackingLabel>();
            query.Where(p => p.No == no && !p.IsScrapped);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="noList">标签号集合</param>
        /// <returns>标签信息</returns>
        /// <remarks>MES、QMS有调用,不要删除</remarks>
        public virtual EntityList<PackingLabel> GetLabels(List<string> noList)
        {
            EntityList<PackingLabel> packingLabelList = new EntityList<PackingLabel>();
            for (int i = 0; i < Math.Ceiling((double)noList.Count / 1000); i++)
            {
                var query = Query<PackingLabel>()
                    .Where(p => noList.Skip(i * 1000).Take(1000).Contains(p.No) && !p.IsScrapped);
                packingLabelList.AddRange(query.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }

            return packingLabelList;
        }

        /// <summary>
        /// 判断标签条码是否失效
        /// </summary>
        /// <param name="no">条码</param>
        /// <returns>失效返回true，否则返回false</returns>
        /// <remarks>MES、QMS有调用,不要删除</remarks>
        public virtual bool IsInvalid(string no)
        {
            var invalidDate = Query<PackingLabel>().Where(p => p.No == no).Select(p => p.InvalidDate).FirstOrDefault<DateTime?>();
            return invalidDate != null && invalidDate.Value > RF.Find<PackingLabel>().GetDbTime();
        }

        /// <summary>
        /// 更新标签的信息
        /// </summary>
        /// <param name="nos">标签</param>
        /// <param name="labelStatus">生产状态</param>
        /// <param name="woId">工单Id</param>
        /// <param name="factoryId">工厂Id</param>
        public virtual void UpdatePackLabelInfos(List<string> nos, LabelStatus? labelStatus = null, double? woId = null, double? factoryId = null)
        {
            if (labelStatus == null && woId == null && factoryId == null || nos.Count == 0)
                return;
            nos.SplitDataExecute(sons =>
            {
                var query = DB.Update<PackingLabel>().Where(p => sons.Contains(p.No));
                if (labelStatus.HasValue)
                    query.Set(p => p.LabelStatus, labelStatus);
                if (woId.HasValue)
                    query.Set(p => p.WorkOrderId, woId);
                if (factoryId.HasValue)
                    query.Execute();
            });
        }

        /// <summary>
        /// 获取是否非序列号控制生成数量配置项
        /// </summary>
        /// <returns></returns>
        public virtual bool GetIsControlCount()
        {
            var config = ConfigService.GetConfig(new NotSerLabelNumControlConfig());
            return config != null && config.IsControlCount;
        }

        /// <summary>
        /// 根据来源单号查询非报废的标签
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="lineNos"></param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<PackingLabel> GetPackingLabelsBySource(string billNo, List<string> lineNos = null, EagerLoadOptions elo = null)
        {
            var query = Query<PackingLabel>().Where(p => p.AsnNo == billNo && !p.IsScrapped);
            if (lineNos != null)
                query.Where(p => lineNos.Contains(p.SourceData));
            return query.ToList(null, elo);
        }
    }
}
