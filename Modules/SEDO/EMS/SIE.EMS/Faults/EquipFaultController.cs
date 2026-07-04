using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Faults.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 设备故障控制器
    /// </summary>
    public partial class EquipFaultController : DomainController
    {
        #region 故障大类 
        /// <summary>
        /// 故障大类查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<EquipLargeFault> QueryEquipLargeFault(EquipLargeFaultCriteria criteria)
        {
            var q = Query<EquipLargeFault>();
            if (criteria.LargeCode.IsNotEmpty())
                q.Where(w => w.Code.Contains(criteria.LargeCode));
            if (criteria.LargeName.IsNotEmpty())
                q.Where(w => w.Name.Contains(criteria.LargeName));
            if (criteria.MiddleCode.IsNotEmpty() && criteria.MiddleName.IsNotEmpty())
                q.Exists<EquipMiddleFault>((i, p) => p.Where(x => x.LargeFaultId == i.Id && x.Code.Contains(criteria.MiddleCode) && criteria.MiddleName.Contains(criteria.MiddleName)));
            if (criteria.MiddleCode.IsNotEmpty() && criteria.MiddleName.IsNullOrEmpty())
                q.Exists<EquipMiddleFault>((i, p) => p.Where(x => x.LargeFaultId == i.Id && x.Code.Contains(criteria.MiddleCode)));
            if (criteria.MiddleCode.IsNullOrEmpty() && criteria.MiddleName.IsNotEmpty())
                q.Exists<EquipMiddleFault>((i, p) => p.Where(x => x.LargeFaultId == i.Id && x.Name.Contains(criteria.MiddleName)));
            if (criteria.SmallCode.IsNotEmpty() && criteria.SmallName.IsNotEmpty())
                q.Exists<EquipSmallFault>((i, p) => p.Where(x => x.MiddleFault.LargeFaultId == i.Id && x.Code.Contains(criteria.SmallCode) && criteria.SmallName.Contains(criteria.SmallName)));
            if (criteria.SmallCode.IsNotEmpty() && criteria.SmallName.IsNullOrEmpty())
                q.Exists<EquipSmallFault>((i, p) => p.Where(x => x.MiddleFault.LargeFaultId == i.Id && x.Code.Contains(criteria.SmallCode)));
            if (criteria.SmallCode.IsNullOrEmpty() && criteria.SmallName.IsNotEmpty())
                q.Exists<EquipSmallFault>((i, p) => p.Where(x => x.MiddleFault.LargeFaultId == i.Id && x.Name.Contains(criteria.SmallName)));
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备故障大类编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetEquipLargeFaultCode()
        {
            var config = ConfigService.GetConfig(new EquipLargeFaultCodeConfig(), typeof(EquipLargeFault));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到设备故障大类编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取故障大类列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>故障大类列表</returns>
        public virtual EntityList<EquipLargeFault> GetEquipLargeFaults(Expression<Func<EquipLargeFault, bool>> exp, PagingInfo pagingInfo = null)
        {
            var query = Query<EquipLargeFault>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取故障大类
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>故障大类</returns>
        public virtual EquipLargeFault GetEquipLargeFault(Expression<Func<EquipLargeFault, bool>> exp)
        {
            var query = Query<EquipLargeFault>();
            if (exp != null)
                query.Where(exp);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取故障类别
        /// </summary>
        /// <param name="codes">编码</param>
        /// <param name="names">名称</param>
        /// <returns></returns>
        public virtual List<BaseDataInfo> GetFaultsByCAN(List<string> codes, List<string> names)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            codes.SplitDataExecute(tempC =>
            {
                names.SplitDataExecute(tempN =>
                {
                    var list = Query<EquipLargeFault>().Where(p => tempC.Contains(p.Code) || tempN.Contains(p.Name)).Select(p => new
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                    }).ToList<BaseDataInfo>().ToList();
                    baseDataInfos.AddRange(list);
                });
            });
            return baseDataInfos;
        }
        #endregion

        #region 故障中类  
        /// <summary>
        /// 获取设备故障中类列表
        /// </summary>
        /// <param name="LargeFaultId">设备故障中类Id</param>
        /// <param name="Code">编码</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备故障中类列表</returns>
        public virtual EntityList<EquipMiddleFault> QueryEquipMiddleFault(double LargeFaultId, string Code, PagingInfo pagingInfo)
        {
            var q = Query<EquipMiddleFault>();
            q.Where(o => o.LargeFaultId == LargeFaultId);
            if (Code.IsNotEmpty())
            {
                q.Where(o => o.Code.Contains(Code));
            }

            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备故障中类编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetEquipMiddleFaultCode()
        {
            var config = ConfigService.GetConfig(new EquipMiddleFaultCodeConfig(), typeof(EquipMiddleFault));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到设备故障中类编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取故障中类列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>故障中类列表</returns>
        public virtual EntityList<EquipMiddleFault> GetEquipMiddleFaults(Expression<Func<EquipMiddleFault, bool>> exp, PagingInfo pagingInfo = null)
        {
            var query = Query<EquipMiddleFault>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取故障中类
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>故障中类</returns>
        public virtual EquipMiddleFault GetEquipMiddleFault(Expression<Func<EquipMiddleFault, bool>> exp)
        {
            var query = Query<EquipMiddleFault>();
            if (exp != null)
                query.Where(exp);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 故障小类
        /// <summary>
        /// 获取设备故障小类列表
        /// </summary>
        /// <param name="MiddleFaultId">设备故障中类Id</param>
        /// <param name="Code">编码</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备故障小类列表</returns>
        public virtual EntityList<EquipSmallFault> QueryEquipSmallFault(double MiddleFaultId, string Code, PagingInfo pagingInfo)
        {
            var q = Query<EquipSmallFault>();
            q.Where(o => o.MiddleFaultId == MiddleFaultId);
            if (Code.IsNotEmpty())
            {
                q.Where(o => o.Code.Contains(Code));
            }

            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备故障小类编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetEquipSmallFaultCode()
        {
            var config = ConfigService.GetConfig(new EquipSmallFaultCodeConfig(), typeof(EquipSmallFault));
            if (config == null || config.CodeRule == null)
                throw new ValidationException("未找到设备故障小类编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.CodeRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取故障小类列表
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>故障小类列表</returns>
        public virtual EntityList<EquipSmallFault> GetEquipSmallFaults(Expression<Func<EquipSmallFault, bool>> exp, PagingInfo pagingInfo = null)
        {
            var query = Query<EquipSmallFault>();
            if (exp != null)
                query.Where(exp);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取故障小类
        /// Expression不支持序列号，前端不要调用
        /// </summary>
        /// <param name="exp">过滤条件</param>
        /// <returns>故障小类</returns>
        public virtual EquipSmallFault GetEquipSmallFault(Expression<Func<EquipSmallFault, bool>> exp)
        {
            var query = Query<EquipSmallFault>();
            if (exp != null)
                query.Where(exp);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion 
    }
}