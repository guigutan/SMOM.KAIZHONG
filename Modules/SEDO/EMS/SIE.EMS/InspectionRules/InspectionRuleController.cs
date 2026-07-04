using SIE.Domain;
using SIE.EMS.Common.Entity;
using SIE.EMS.Enums;
using SIE.EMS.MainenanceProjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.InspectionRules
{
    /// <summary>
    /// 检验规程控制器
    /// </summary>
    public class InspectionRuleController : DomainController
    {
        /// <summary>
        /// 获取检验规程集合
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<InspectionRule> GetInspectionRuleList()
        {
            return Query<InspectionRule>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取检验规程集合
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<InspectionRule> GetInspectionRuleList(InspectionRuleType? type, string key, PagingInfo pageinfo)
        {
            var query = Query<InspectionRule>();
            if (type.HasValue)
            {
                query.Where(p => p.InspectionRuleType == type);
            }
            if (key.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(key) || p.Name.Contains(key));
            }
            return query.ToList(pageinfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        

        /// <summary>
        /// 获取检验规程集合
        /// </summary>
        public virtual EntityList<InspectionRule> GetInspectionRuleList(InspectionRuleCriteria criteria)
        {
            var query = Query<InspectionRule>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.InspectionRuleType.HasValue)
            {
                query.Where(p => p.InspectionRuleType == criteria.InspectionRuleType);
            }
            if (criteria.CheckCategory.HasValue)
            {
                query.Where(p => p.CheckCategory == criteria.CheckCategory);
            }
            //创建日期
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

       

        /// <summary>
        ///根据检测规程Id获取检验项目
        /// </summary>
        /// <param name="InspectionRuleId"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<InspectionProjectItem> GetInspectionProjectItemList(double InspectionRuleId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<InspectionProjectItem>()
                .Where(r => r.InspectionRuleId == InspectionRuleId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据检验规程获取检验规程与点检项目的数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<InspectionProjectItem> GetInspectionProjectItemList(List<double> ids)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(InspectionProjectItem.InspectionRuleProperty);
            elo.LoadWith(InspectionProjectItem.ProjectDetailProperty);
            elo.LoadWithViewProperty();
            return ids.SplitContains((tmpIds) =>
            {
                return Query<InspectionProjectItem>().Where(p => ids.Contains(p.InspectionRuleId)).ToList(null, elo);
            });

        }


        /// <summary>
        /// 保存选择的点检项目数据
        /// </summary>
        /// <param name="checkProjectInfos"></param>
        public virtual void SaveSelProjectCheck(List<CheckProjectInfo> checkProjectInfos)
        {
            if (checkProjectInfos == null)
            {
                return;
            }
            var projectDetailIds = checkProjectInfos.Select(m => m.ProjectDetailId).ToList();
            var projectDetails = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(projectDetailIds);
            EntityList<InspectionProjectItem> savedData = new EntityList<InspectionProjectItem>();
            foreach (var item in checkProjectInfos)
            {
                var detail = projectDetails.FirstOrDefault(m => m.Id == item.ProjectDetailId);
                var checkProject = new InspectionProjectItem();
                checkProject.ProjectDetailId = item.ProjectDetailId;
                checkProject.Standard = detail.Standard;
                checkProject.MaxValue = detail.MaxValue;
                checkProject.MinValue = detail.MinValue;
                checkProject.InspectionRuleId = item.SourceId;
                savedData.Add(checkProject);
            }
            RF.Save(savedData);
        }
    }
}
