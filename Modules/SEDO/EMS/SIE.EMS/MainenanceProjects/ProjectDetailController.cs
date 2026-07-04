using SIE.Domain;
using SIE.EMS.Common.Entity;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 点检保养项目控制器
    /// </summary>
    public partial class ProjectDetailController : DomainController
    {
        /// <summary>
        /// 查询点检保养项目列表
        /// </summary>
        /// <param name="criteria">点检保养查询对象</param>
        /// <returns>点检保养项目列表</returns>
        public virtual EntityList<ProjectDetail> GetProjectDetails(ProjectDetailCriteria criteria)
        {
            var query = Query<ProjectDetail>();
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);

            if (criteria.ProjectType.HasValue)
            {
                query.Where(p => p.ProjectType == criteria.ProjectType);
            }

            if (criteria.CycleType.HasValue)
            {
                query.Where(p => p.CycleType == criteria.CycleType);
            }

            if (!criteria.Part.IsNullOrEmpty())
            {
                query.Where(p => p.Part.Contains(criteria.Part));
            }

            var list = query.OrderBy(criteria.OrderInfoList)
                .ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var item in list.Where(x => x.CycleType != null))
            {
                item.CycleTypeInfoId = ((int)item.CycleType.Value).ToString();
                item.ExtValues["CycleTypeInfoId_Display"] = item.CycleType.ToLabel().L10N();
            }

            return list;
        }

        /// <summary>
        /// 根据点检保养项目Id列表获取点检保养项目列表
        /// </summary>
        /// <param name="ids">点检保养项目Id列表</param>
        /// <returns>点检保养项目列表</returns>
        public virtual EntityList<ProjectDetail> GetProjectDetails(List<double> ids)
        {
            return Query<ProjectDetail>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 根据项目名称获取点检保养项目列表
        /// </summary>
        /// <param name="names">项目名称</param>
        /// <returns></returns>
        public virtual EntityList<ProjectDetail> GetProjectDetails(List<string> names)
        {
            return names.SplitContains(temps =>
            {
                return Query<ProjectDetail>().Where(p => temps.Contains(p.Name)).ToList();
            });
        }


        /// <summary>
        /// 根据点检保养项目Id列表获取点检保养项目备件列表
        /// </summary>
        /// <param name="ids">点检保养项目Id列表</param>
        /// <returns>点检保养项目列表</returns>
        public virtual EntityList<SparePartItem> GetSparePartItem(List<double> ids)
        {
            return Query<SparePartItem>().Where(p => ids.Contains(p.ProjectDetailId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据分类获取分类值
        /// </summary>
        /// <param name="upperLevel"></param>
        /// <param name="page"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<CycleTypeInfo> GetCycleTypeInfoList(ProjectType upperLevel, PagingInfo page, string keyword = null)
        {
            EntityList<CycleTypeInfo> sourceList = new EntityList<CycleTypeInfo>();
            CycleTypeInfo c = new CycleTypeInfo();
            List<EnumViewModel> list = EnumViewModel.GetByEnumType(typeof(CycleType));
            switch (upperLevel)
            {
                case ProjectType.Check:
                    list = FilterEnum("Check", list);
                    break;
                //保养项目周期类型不能选日和班，其他可选；
                case ProjectType.Maintain:
                    list = FilterEnum("Maintain", list);
                    break;
                default:
                    break;
            }
            if (page != null)
            {
                List<CycleTypeInfo> spList = null;

                //分页加载枚举类型数据并有值查询
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    spList = list.Where(p => p.Label.L10N().Contains(keyword) || p.EnumValue.ToString().Contains(keyword))
                       .Select(p => new CycleTypeInfo() { Id = Convert.ToInt32(p.EnumValue).ToString(), Value = p.Label.L10N() })
                       .ToList();
                }
                else
                {
                    spList = list.Select(p => new CycleTypeInfo() { Id = Convert.ToInt32(p.EnumValue).ToString(), Value = p.Label.L10N() })
                      .ToList();
                }
                var tmpList = spList.Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();
                sourceList.AddRange(tmpList);
                sourceList.SetTotalCount(spList.Count);
            }
            else
            {
                //正常加载枚举类型数据
                foreach (var item in list)
                {
                    c = new CycleTypeInfo()
                    {
                        Id = Convert.ToInt32(item.EnumValue).ToString(),
                        Value = item.Label.L10N()
                    };
                    sourceList.Add(c);
                }
            }
            //本地化
            //foreach (var item in sourceList)
            //{
            //    item.Value = item.Value.L10N();
            //}
            return sourceList;
        }



        /// <summary>
        /// 根据分类获取润滑项目
        /// </summary>
        /// <param name="proType"></param>
        /// <param name="page"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<ProjectDetail> GetProjectDetailByProjectType(ProjectType proType, PagingInfo page, string keyword = null)
        {
            var query = Query<ProjectDetail>().Where(p => p.ProjectType == proType);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(keyword));
            }
            return query.ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 过滤分类枚举
        /// </summary>
        /// <param name="FilterCategoery"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        private List<EnumViewModel> FilterEnum(string FilterCategoery, List<EnumViewModel> models)
        {
            if (FilterCategoery.IsNotEmpty())
            {
                models = models.Where(p => FilterCategoery == p.Category).ToList();
            }
            return models;
        }
    }
}
