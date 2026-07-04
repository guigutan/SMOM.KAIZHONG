using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessTechs.Configs;
using SIE.Resources.ProcessTechs.Enums;
using SIE.Resources.ProcessTechs.ViewModels;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Resources.ProcessTechs
{
    /// <summary>
    /// 制程控制器
    /// </summary>
    public partial class ProcessTechBaseController : DomainController
    {
        /// <summary>
        /// 通过查询条件获取制程工艺列表
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTechList(ProcessTechCriteria criteria)
        {
            var query = Query<ProcessTech>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }

            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }

            if (criteria.ProcessTechTypeId.HasValue)
            {
                query.Where(p => p.ProcessTechTypeId == criteria.ProcessTechTypeId);
            }

            if (criteria.ProcessTechState.HasValue)
            {
                if (criteria.ProcessTechState == ProcessTechState.ARRANGE)
                {
                    query.Where(p => p.IsScheduling);
                }
                else
                {
                    query.Where(p => !p.IsScheduling);
                }
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有制程工艺
        /// </summary>
        /// <returns>返回所有制程工艺</returns>
        public virtual EntityList<ProcessTech> GetProcessTechList()
        {
            return Query<ProcessTech>().ToList();
        }

        /// <summary>
        /// 根据指定id获取对应的制程工艺
        /// </summary>
        /// <returns>返回指定的制程工艺</returns>
        public virtual EntityList<ProcessTech> GetProcessTechList(double processTechId)
        {
            return Query<ProcessTech>().Where(p => p.Id == processTechId).ToList();
        }

        /// <summary>
        /// 通过制程工艺Id列表获取制程工艺列表
        /// </summary>
        /// <param name="processTechIds">制程工艺Id列表</param>
        /// <returns>制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTechList(List<double> processTechIds)
        {
            return processTechIds.SplitContains((tempProcessTechIds) =>
            {
                return Query<ProcessTech>().Where(p => tempProcessTechIds.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 通过制程工艺Id列表获取制程工艺列表
        /// </summary>
        /// <param name="processTechIds">制程工艺Id列表</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTech(List<double> processTechIds, PagingInfo pagingInfo, string keyword)
        {
            return Query<ProcessTech>().Where(p => processTechIds.Contains(p.Id)).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取制程工艺
        /// </summary>
        /// <param name="tech">制程</param>
        /// <returns>制程工艺</returns>
        public virtual ProcessTech GetProcessTech(string tech)
        {
            return Query<ProcessTech>().Where(p => p.Code == tech || p.Name == tech).FirstOrDefault();
        }

        /// <summary>
        /// 根据制程工艺名称获取制程工艺
        /// </summary>
        /// <param name="techName">制程</param>
        /// <returns>制程工艺</returns>
        public virtual ProcessTech GetProcessTechName(string techName)
        {
            return Query<ProcessTech>().Where(p => p.Name == techName).FirstOrDefault();
        }

        /// <summary>
        /// 通过工段Id获取制程工艺列表
        /// </summary>
        /// <param name="processSegmentId">工段Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTechsBySegment(double processSegmentId, PagingInfo pagingInfo, string keyword)
        {
            return Query<ProcessTech>().Where(p => p.ProcessSegmentId == processSegmentId).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo);
        }

        /// <summary>
        /// 获取所有制程工艺列表
        /// </summary>
        /// <returns>制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTechByNameList(List<string> processTechNameList)
        {
            return Query<ProcessTech>().Where(p => processTechNameList.Contains(p.Name)).ToList();
        }

        /// <summary>
        /// 获取制程工艺的No
        /// </summary>
        /// <returns>制程工艺No字符串</returns>
        public virtual string GetProcessTechNo()
        {
            var config = ConfigService.GetConfig(new ProcessTechNoConfig(), typeof(ProcessTech));
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到制程工艺编码配置规则，请检查规则配置".L10N());
            }

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 通过制程编码获取制程工艺
        /// </summary>
        /// <param name="proCode">制程编码</param>
        /// <returns>制程工艺</returns>
        public virtual ProcessTech GetProcessTechByproCode(string proCode)
        {
            return Query<ProcessTech>().Where(p => p.Code == proCode).FirstOrDefault();
        }

        /// <summary>
        /// 通过制程编码获取制程工艺
        /// </summary>
        /// <param name="proTypeId">制程类型ID</param>
        /// <returns></returns>
        public virtual ProcessTech GetProcessTechTypeId(double proTypeId)
        {
            return Query<ProcessTech>().Where(p => p.ProcessTechTypeId == proTypeId).FirstOrDefault();
        }

        /// <summary>
        /// 根据制程Id获取制程工艺的转款时间
        /// </summary>
        /// <param name="processTechId">制程Id</param>
        /// <returns>转款时间</returns>
        public virtual decimal? GetTransferTime(double processTechId)
        {
            var processTech = Query<ProcessTech>().Where(p => p.Id == processTechId).FirstOrDefault();
            if (processTech != null)
            {
                return processTech.TransferTime;
            }
            return null;
        }

        /// <summary>
        /// 判断是否使用过制程工艺类型
        /// </summary>
        /// <param name="proTechTypeId">制程工艺类型Id</param>
        /// <returns>true/false</returns>
        public virtual bool IsExistsProcessTechType(double proTechTypeId)
        {
            return Query<ProcessTech>().Where(p => p.ProcessTechTypeId == proTechTypeId).Count() > 0;
        }

        /// <summary>
        /// 根据制程工艺类型获取对应制程工艺列表
        /// </summary>
        /// <param name="processTechTypeId">制程工艺类型Id</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>返回制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTechs(double processTechTypeId, PagingInfo pageInfo, string keyword)
        {
            var query = Query<ProcessTech>().Where(p => p.ProcessTechTypeId == processTechTypeId);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            return query.ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过制程工艺Id列表获取制程工艺列表
        /// </summary>
        /// <param name="processTechIds">制程工艺Id列表</param>        
        /// <returns>制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTechs(List<double> processTechIds)
        {
            return processTechIds.SplitContains(tempIds =>
            {
                return Query<ProcessTech>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 根据编码获取制程工艺列表
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>制程工艺列表</returns>
        public virtual EntityList<ProcessTech> GetProcessTechsFromCode(List<string> codes)
        {
            return Query<ProcessTech>().Where(m => codes.Contains(m.Code)).ToList();
        }

        /// <summary>
        /// 通过工段Id列表获取制程工艺Id列表
        /// </summary>
        /// <param name="processSegmentId">工段Id</param>
        /// <returns>制程工艺Id列表</returns>
        public virtual List<double?> GetProcessTechIds(double? processSegmentId)
        {
            return Query<ProcessTech>().Where(p => p.ProcessSegmentId == processSegmentId).ToList().Select(p => p.Id).Distinct().Cast<double?>().ToList();
        }

        /// <summary>
        /// 获取瓶颈制程工艺
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ProcessTech> GetProcessTechBottleneck(PagingInfo pagingInfo, string keyword)
        {
            return Query<ProcessTech>().Where(p => p.IsBottleneck).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有工艺信息
        /// </summary>
        /// <returns>返回所有工艺信息</returns>
        public virtual List<ProcessTechInfo> GetProcessTechInfos()
        {
            return Query<ProcessTech>().Select(p => new
            {
                ProcessTechId = p.Id,
                Code = p.Code,
                Name = p.Name,
                ProcessTechTypeId = p.ProcessTechTypeId,
                SAM = p.SAM,
                WorkingHours = p.WorkingHours,
                IsBottleneck = p.IsBottleneck,
                IsScheduling = p.IsScheduling,
                TransferTime = p.TransferTime,
                OutAssistDay = p.OutAssistDay

            }).ToList<ProcessTechInfo>().ToList();
        }

        /// <summary>
        /// 根据名称或编号获取制程类型id列表
        /// </summary>
        /// <param name="nameOrCode"></param>
        /// <returns></returns>
        public virtual List<double> GetProcessTechTypeIdsByNameOrCode(string nameOrCode)
        {
            var query = Query<ProcessTechType>();
            if (nameOrCode.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(nameOrCode) || p.Code.Contains(nameOrCode));
            }
            return query.Select(p => p.Id).ToList<double>().ToList();
        }

        /// <summary>
        /// 根据制程工艺Id获取产线ID集合
        /// </summary>
        /// <param name="processTechIds">制程工艺ID集合</param>
        /// <returns>返回产线ID集合</returns>
        public virtual List<double> GetWipResourceIdByProcessTechId(List<double> processTechIds)
        {
            List<double> resIds = Query<ProcessTech>().Join<ProcessTechType>((pt, type) => pt.ProcessTechTypeId == type.Id)
                .Join<ProcessTechType, WipResource>((type, res) => type.Id == res.ProcessTechTypeId)
                .Select<WipResource>((pt, res) => res.Id).Distinct().ToList<double>().ToList();

            return resIds;
        }

        /// <summary>
        /// 根据id列表获取相对应的编码和名称
        /// </summary>
        /// <param name="processTechIds">id列表</param>
        /// <returns>字典key：id；value：编码、名称</returns>
        public virtual Dictionary<double, Tuple<string, string>> GetProcessTechIdCodeAndNameById(List<double> processTechIds)
        {
            List<ProcessTechSimpleInfo> infos = new List<ProcessTechSimpleInfo>();
            processTechIds.SplitDataExecute(ids =>
            {
                var tmpList = Query<ProcessTech>().Where(p => ids.Contains(p.Id)).Select(p => new
                {
                    ProcessTechId = p.Id,
                    ProcessTechCode = p.Code,
                    ProcessTechName = p.Name
                }).ToList<ProcessTechSimpleInfo>().ToList();
                infos.AddRange(tmpList);
            });
            return infos.GroupBy(p => p.ProcessTechId).ToDictionary(p => p.Key, p => new Tuple<string, string>(p.First().ProcessTechCode, p.First().ProcessTechName));
        }
    }
}
