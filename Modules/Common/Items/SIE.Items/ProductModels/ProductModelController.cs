using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Resources.Skills;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items.ProductModels
{
    /// <summary>
    /// 产品机型控制器
    /// </summary>
    public partial class ProductModelController : DomainController
    {
        /// <summary>
        /// 获得产品机型
        /// </summary>
        /// <param name="productModelId">机型ID</param>
        /// <returns>产品机型</returns>
        public virtual ProductModel GetProductModel(double productModelId)
        {
            if (productModelId <= 0)
                throw new ArgumentException("产品机型ID不合法".L10N());
            return RF.GetById<ProductModel>(productModelId);
        }

        /// <summary>
        /// 通过产品机型编码/名称获取产品机型
        /// </summary>
        /// <param name="keyword">产品机型编码或者名称</param>
        /// <returns>产品机型</returns>
        public virtual ProductModel GetProductModel(string keyword)
        {
            Check.NotNullOrEmpty(keyword, nameof(keyword));
            return Query<ProductModel>().Where(p => p.Code == keyword || p.Name == keyword).FirstOrDefault();
        }

        /// <summary>
        /// 查询全部产品机型
        /// </summary>
        /// <returns>产品机型列表</returns>
        public virtual EntityList<ProductModel> GetProductModels()
        {
            return Query<ProductModel>().ToList();
        }

        /// <summary>
        /// 获取产品机型集合
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>产品机型集合</returns>
        public virtual EntityList<ProductModel> GetProductModels(string keyword, PagingInfo pagingInfo)
        {
            var query = Query<ProductModel>();
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            return query.Distinct().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 设置产品机型技能视图数据
        /// </summary>      
        /// <param name="empIdstr">出勤员工集合</param>
        /// <param name="modelId">机型Id</param>
        /// <returns>机型技能集合</returns>
        public virtual EntityList<ModelSkillViewModel> GetModelSkill(string empIdstr, double modelId)
        {
            EntityList<ModelSkillViewModel> rst = new EntityList<ModelSkillViewModel>();
            var productModel = RF.GetById<ProductModel>(modelId);
            if (productModel == null || productModel.SkillList.Count == 0)
                return rst;
            List<double> empIds = new List<double>();
            empIdstr.Split(',').ForEach(e =>
            {
                empIds.Add(double.Parse(e));
            });
            var empSkills = RT.Service.Resolve<SkillController>().GetEmployeeSkill(empIds);
            List<double> occupyEmpIds = new List<double>();

            productModel.SkillList.ForEach(p =>
            {
                //同一计划任务下，相同机型不同技能的技能人员不能重复占用
                var tmpEmpSkills = empSkills.Where(e => e.SkillId == p.SkillId && !occupyEmpIds.Contains(e.EmployeeId));
                var skillCount = tmpEmpSkills.Count();
                if (skillCount > 0)
                    occupyEmpIds.AddRange(tmpEmpSkills.Select(e => e.EmployeeId).Distinct());
                ModelSkillViewModel item = new ModelSkillViewModel();
                item.SkillCode = p.Skill.Code;
                item.SkillName = p.Skill.Name;
                item.DemandQty = p.DemandQty;
                item.ActualQty = tmpEmpSkills == null ? 0 : skillCount;
                if (item.DemandQty == null)
                {
                    item.LackQty = null;
                }
                else
                {
                    var lackQty = 0;
                    lackQty = item.DemandQty.Value - item.ActualQty.Value;
                    if (lackQty < 0)
                    {
                        lackQty = 0;
                    }
                    item.LackQty = lackQty;
                }
                rst.Add(item);
            });
            return rst;
        }

        /// <summary>
        /// 根据产品机型ID获得产线产能
        /// </summary>
        /// <param name="productModelId">产品ID</param>
        /// <param name="orderInfos">排序条件</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>产线产能列表</returns>
        public virtual EntityList<ProductModelLineCapacity> GetProductModelLineCapacities(double productModelId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            return Query<ProductModelLineCapacity>()
                .Where(p => p.ProductModelId == productModelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据产品机型ID列表获得产线产能
        /// </summary>
        /// <param name="productModelId">产品机型ID</param>
        /// <returns>产线产能列表</returns>
        public virtual EntityList<ProductModelLineCapacity> GetProductModelLineCapacities(List<double> productModelId)
        {
            return Query<ProductModelLineCapacity>().Where(p => productModelId.Contains(p.ProductModelId)).ToList();
        }

        /// <summary>
        /// 获取产品机型产线产能
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="productModelId">机型Id</param>
        /// <returns>产线产能</returns>
        public virtual ProductModelLineCapacity GetProductModelLine(double resourceId, double productModelId)
        {
            return Query<ProductModelLineCapacity>().Where(p => p.ProductModelId == productModelId && p.ResourceId == resourceId).FirstOrDefault();
        }

        /// <summary>
        /// 根据产品机型名称获取产品机型名称Id字典
        /// </summary>
        /// <param name="names">产品机型名称</param>
        /// <returns></returns>
        public virtual Dictionary<string, double> GetProductModelNameIdDic(IEnumerable<string> names)
        {
            List<BaseDataInfo> baseDataInfos = new List<BaseDataInfo>();
            names.SplitDataExecute(temps =>
            {
                var list = Query<ProductModel>().Where(p => temps.Contains(p.Name)).Select(p => new
                {
                    Name = p.Name,
                    Id = p.Id,
                }).ToList<BaseDataInfo>();
                baseDataInfos.AddRange(list);
            });
            return baseDataInfos.ToDictionary(p => p.Name, p => p.Id);
        }
    }
}