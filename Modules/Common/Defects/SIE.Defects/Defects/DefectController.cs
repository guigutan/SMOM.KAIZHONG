using SIE.Api;
using SIE.Core.Common;
using SIE.Data;
using SIE.Defects.Defects;
using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Defects
{
    /// <summary>
    /// 缺陷控制器
    /// </summary>
    public partial class DefectController : DomainController
    {
        /// <summary>
        /// 获取所有缺陷责任分类
        /// </summary>
        /// <returns>缺陷责任分类列表</returns>
        public virtual EntityList<DefectResponsibilityCategory> GetAllDefectResponsibilityCategory()
        {
            return Query<DefectResponsibilityCategory>().ToList();
        }

        /// <summary>
        /// 获取所有缺陷分类
        /// </summary>
        /// <returns>缺陷分类列表</returns>
        public virtual EntityList<DefectCategory> GetAllDefectCategory()
        {
            return Query<DefectCategory>().ToList();
        }

        /// <summary>
        /// 根据缺陷分类编码获取缺陷分类信息
        /// </summary>
        /// <param name="pageinfo">分页</param>
        /// <param name="code">编码</param>
        /// <returns>缺陷分类列表</returns>
        public virtual EntityList<DefectCategory> GetAllDefectCategory(PagingInfo pageinfo, string code)
        {
            return Query<DefectCategory>().Where(p => p.Code.Contains(code)).ToList(pageinfo);
        }

        /// <summary>
        /// 获取所有缺陷代码
        /// </summary>
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<Defect> GetAllDefect()
        {
            return Query<Defect>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取缺陷分类
        /// </summary>
        /// <returns>缺陷分类列表</returns>
        public virtual EntityList<DefectCategory> GetDefectCategoryByCodes(List<string> codeList)
        {
            EntityList<DefectCategory> result = new EntityList<DefectCategory>();
            DataProcessEx.SplitDataExecute(codeList, codes =>
            {
                var query = Query<DefectCategory>();
                var list = query.Where(p => codes.Contains(p.Code)).ToList();
                result.AddRange(list);
            });
            result.SetTotalCount(result.Count);
            return result;
        }

        /// <summary>
        /// 获取所有缺陷等级
        /// </summary>
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<DefectGrade> GetAllDefectGrade()
        {
            return Query<DefectGrade>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取缺陷等级
        /// </summary>
        /// <returns>缺陷等级列表</returns>
        public virtual EntityList<DefectGrade> GetDefectGradeByNames(List<string> nameList)
        {
            EntityList<DefectGrade> result = new EntityList<DefectGrade>();
            DataProcessEx.SplitDataExecute(nameList, names =>
            {
                var query = Query<DefectGrade>();
                var list = query.Where(p => names.Contains(p.Name)).ToList();
                result.AddRange(list);
            });
            result.SetTotalCount(result.Count);
            return result;
        }

        /// <summary>
        /// 根据缺陷ID列表 获取缺陷代码列表
        /// </summary> 
        /// <param name="defectIds">缺陷ID列表</param>       
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<Defect> GetDefectList(List<double> defectIds)
        {
            if (defectIds.IsNullOrEmpty())
                return new EntityList<Defect>();
            return Query<Defect>().Where(defectIds.CreateContainsExpression<Defect>("x", nameof(Defect.Id))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询缺陷代码
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="paging">分页信息</param> 
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<Defect> GetDefectList(string keyword, PagingInfo paging)
        {
            return Query<Defect>()
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Description.Contains(keyword))
                .ToList(paging, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取缺陷代码集合
        /// </summary>
        /// <param name="categoryId">缺陷代码分类ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>缺陷代码集合</returns>
        public virtual EntityList<Defect> GetDefects(double categoryId, string keyword, PagingInfo pagingInfo = null)
        {
            var query = Query<Defect>().Where(p => p.DefectCategoryId == categoryId);
            if (!string.IsNullOrEmpty(keyword))
                query.Where(p => p.Code.Contains(keyword) || p.Description.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 按查询条件查询缺陷代码
        /// </summary>
        /// <param name="criteria"></param>        
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual EntityList GetDefects(DefectCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            var query = DB.Query<Defect>("i");
            if (!criteria.Code.IsNullOrEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }

            if (!criteria.Description.IsNullOrEmpty())
            {
                query.Where(p => p.Description.Contains(criteria.Description));
            }

            if (criteria.DefectCategoryId.HasValue)
            {
                query.Where(p => p.DefectCategoryId == criteria.DefectCategoryId);
            }

            if (criteria.QualityType.HasValue)
            {
                query.Where(p => p.QualityType == criteria.QualityType);
            }

            if (criteria.DefectGradeId.HasValue)
            {
                query.Where(p => p.DefectGradeId == criteria.DefectGradeId);
            }

            if (criteria.FilterId != null && criteria.FilterId.Any())
            {
                query.Where(p => !criteria.FilterId.Contains(p.Id));
            }

            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据缺陷描述获取缺陷
        /// </summary>
        /// <param name="defectName">缺陷描述</param>
        /// <returns>缺陷</returns>
        public virtual Defect GetDefect(string defectName)
        {
            return Query<Defect>().Where(p => p.Code == defectName).FirstOrDefault(new EagerLoadOptions().LoadWith(Defect.DefectCategoryProperty));
        }

        /// <summary>
        /// 获取缺陷等级
        /// </summary>
        /// <returns>缺陷等级列表</returns>
        public virtual EntityList<Defect> GetDefectsByCodes(List<string> codeList)
        {
            EntityList<Defect> result = new EntityList<Defect>();
            DataProcessEx.SplitDataExecute(codeList, codes =>
            {
                var query = Query<Defect>();
                var list = query.Where(p => codes.Contains(p.Code)).ToList();
                result.AddRange(list);
            });
            result.SetTotalCount(result.Count);
            return result;
        }

        /// <summary>
        /// 获取缺陷信息
        /// </summary>
        /// <returns>缺陷信息列表</returns>
        [ApiService("获取缺陷信息")]
        [return: ApiReturn("缺陷信息列表. 参数类型: List<DefectCategoryInfo>")]
        public virtual List<DefectCategoryInfo> GetDefectInfos()
        {
            List<DefectCategoryInfo> infos = new List<DefectCategoryInfo>();
            var defects = GetAllDefect();
            var defectc = GetAllDefectCategory();
            foreach (var item in defectc)
            {
                var defectList = defects.Where(p => p.DefectCategoryId == item.Id);
                var categoryInfo = new DefectCategoryInfo()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Description
                };
                defectList.ForEach(defect =>
                {
                    categoryInfo.Defects.Add(new DefectInfo()
                    {
                        Id = defect.Id,
                        Code = defect.Code,
                        Name = defect.Description
                    });
                });
                infos.Add(categoryInfo);
            }
            return infos;
        }

        /// <summary>
        /// 获取缺陷等级
        /// </summary>
        /// <param name="name">缺陷等级 不可空</param>
        public virtual DefectGrade GetDefectGradeName(string name)
        {
            return Query<DefectGrade>().Where(p => p.Name == name).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取缺陷代码分类
        /// </summary>
        /// <param name="code">缺陷代码分类 不可空</param>
        public virtual DefectCategory GetDefectCategoryCode(string code)
        {
            return Query<DefectCategory>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 缺陷分类信息
        /// </summary>
        [Serializable]
        public class DefectCategoryInfo
        {
            /// <summary>
            /// 缺陷分类ID
            /// </summary>
            public double Id { get; set; }

            /// <summary>
            /// 缺陷分类编码
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// 缺陷分类名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 缺陷列表
            /// </summary>
            public List<DefectInfo> Defects { get; } = new List<DefectInfo>();
        }

        /// <summary>
        /// 缺陷信息
        /// </summary>
        [Serializable]
        public class DefectInfo
        {
            /// <summary>
            /// 缺陷ID
            /// </summary>
            public double Id { get; set; }

            /// <summary>
            /// 缺陷编码
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// 缺陷名称
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// 根据编码缺陷分类
        /// </summary>
        /// <param name="code">缺陷分类编码</param>
        /// <returns>返回</returns>
        public virtual DefectCategory GetDefectCategory(string code)
        {
            var itemEntity = Query<DefectCategory>().Where(p => p.Code == code).FirstOrDefault();

            return itemEntity;
        }

        /// <summary>
        /// 获取缺陷等级
        /// </summary>
        /// <param name="name">缺陷等级</param>
        /// <returns>返回</returns>
        public virtual DefectGrade GetDefectGrade(string name)
        {
            var itemEntity = Query<DefectGrade>().Where(p => p.Name == name).FirstOrDefault();

            return itemEntity;
        }
        /// <summary>
        /// 根据缺陷编码获取缺陷
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public virtual Defect GetDefectByCode(string code)
        {
            return Query<Defect>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWith(Defect.DefectCategoryProperty));
        }

        /// <summary>
        /// 根据缺陷编码列表 获取缺陷代码列表
        /// </summary> 
        /// <param name="defectCodelist">缺陷描述列表</param>       
        /// <returns>缺陷代码列表</returns>
        public virtual EntityList<Defect> GetDefectList(List<string> defectCodelist)
        {
            if (defectCodelist.IsNullOrEmpty())
                return new EntityList<Defect>();
            return Query<Defect>().Where(defectCodelist.CreateContainsExpression<Defect>("x", nameof(Defect.Code))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}