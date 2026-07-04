using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Warehouses.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses
{
    /// <summary>
    /// 事务原因控制器
    /// </summary>
    public class ReasonController : DomainController
    {
        /// <summary>
        /// 初始化功能
        /// </summary>
        public virtual void InitReason()
        {
            EntityList<Reason> reasonlist = new EntityList<Reason>();
            foreach (ReasonType type in Enum.GetValues(typeof(ReasonType)))
            {
                InitReasonData(type, reasonlist);
            }

            foreach (var reason in reasonlist)
            {
                var move_data = GetReasonData(reason.Name, reason.ReasonType);
                if (move_data == null)
                {
                    RF.Save(reason);
                }
            }
        }

        /// <summary>
        /// 初始化原因数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="reasonlist">原因列表</param>
        private void InitReasonData(ReasonType type, EntityList<Reason> reasonlist)
        {
            switch (type)
            {
                case ReasonType.MOVE_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "优化储存".L10N(), "区分状态".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.ALLOCATE_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "生产需求".L10N(), "销售需求".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.ADJUST_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "盘点差异".L10N(), "库存损耗".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.FREEZE_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "销售订单预留".L10N(), "生产订单预留".L10N(), "委外订单预留".L10N(), "超期冻结".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.UNFREEZE_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "预留取消".L10N(), "限制解除".L10N(), "超期复检解冻".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.TASK_EXCEPTION:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "找不到物料".L10N(), "物料质量异常".L10N(), "设备实施故障".L10N(), "安排不合理".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.IMPERFECT_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "库位无货".L10N(), "产品缺损".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.COUNT_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "日常自查".L10N(), "定期审查".L10N(), "专项盘查".L10N(), "随机抽查".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.ONHAND_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "修正错误".L10N(), "区分状态".L10N(), "业务需求".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.SUPPLIERRETURN_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "来料检验不合格".L10N(), "料废".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.OVERDUERECHECK_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "超期复检".L10N(), "库存临期".L10N()}, type));
                    break;
                case ReasonType.CANCELQUEUE_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "签到超时".L10N(), "临时调整".L10N(), "其他".L10N() }, type));
                    break;
                case ReasonType.OVERDUEBATCHRECHECK_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "超期批次复检".L10N() }, type));
                    break;
                case ReasonType.OTHERRECHECK_REASON:
                    reasonlist.AddRange(CreateReasonData(new List<string> { "其他复检原因".L10N() }, type));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 创建事务原因数据
        /// </summary>
        /// <param name="reasonNames"></param>
        /// <param name="type"></param>
        private EntityList<Reason> CreateReasonData(List<string> reasonNames, ReasonType type)
        {
            EntityList<Reason> reasons = new EntityList<Reason>();
            foreach (var reasonName in reasonNames)
            {
                reasons.Add(new Reason
                {
                    Code = GetReasonCode(),
                    Name = reasonName,
                    ReasonType = type,
                    State = State.Enable,
                });
            }

            return reasons;
        }

        /// <summary>
        /// 获取事务原因编码
        /// </summary>
        /// <returns>返回库存调拨编码</returns>
        public virtual string GetReasonCode()
        {
            var config = ConfigService.GetConfig(new ReasonNoConfig(), typeof(Reason));
            if (config == null || config.ReasonNoRule == null)
                throw new ValidationException("未找到事务原因编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.ReasonNoRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取事务原因数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">事务类型</param>
        /// <returns>事务原因数据</returns>
        public virtual Reason GetReasonData(string name, ReasonType type)
        {
            return Query<Reason>().Where(p => p.Name.Contains(name) && p.ReasonType == type).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据操作类型获取事物原因数据
        /// </summary>
        /// <param name="reasonTypeValueList">操作类型</param>
        /// <param name="state">状态</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pageinfo">分页信息</param>
        /// <returns>事物原因数据</returns>
        public virtual EntityList<Reason> GetReasonTypeData(List<int> reasonTypeValueList, State? state, string keyword, PagingInfo pageinfo = null)
        {
            var query = Query<Reason>().Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword)
            || p.Describe.Contains(keyword));
            if (reasonTypeValueList != null)
            {
                query = query.Where(p => reasonTypeValueList.Contains((int)p.ReasonType));
            }

            if (state != null)
            {
                query.Where(p => p.State == state);
            }

            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据类型获取原因
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>原因集合</returns>
        public virtual EntityList<Reason> GetReasons(ReasonType type)
        {
            var query = Query<Reason>();
            query.Where(p => p.State == State.Enable && p.ReasonType == type);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据类型获取原因
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>原因集合</returns>
        public virtual List<ReasonData> GetReasonDatas(ReasonType type)
        {
            List<ReasonData> results = new List<ReasonData>();
            var reasons = GetReasons(type);
            reasons.ForEach(e =>
            {
                var result = new ReasonData()
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name.L10N(),
                    Desc = e.Describe
                };
                results.Add(result);
            });

            return results;
        }
    }
}
