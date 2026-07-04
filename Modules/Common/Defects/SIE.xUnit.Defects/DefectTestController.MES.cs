using SIE.Core.Common.Controllers;
using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.Domain.Validation;

namespace SIE.xUnit.Defects
{
    /// <summary>
    /// 测试缺陷数据控制器
    /// </summary>
    public partial class DefectTestController : DomainController
    {
        #region 缺陷代码
        public virtual EntityList<Defect> GetOrCreateDefects(int count)
        {
            var category = CreateDefectCategory("MES缺陷分类");
            var defects = RT.Service.Resolve<DefectController>().GetDefects(category.Id, "");
            if (defects.Count == 0)
            {
                defects = new EntityList<Defect>();
                for (int i = 0; i < count; i++)
                {
                    var defect = new Defect();
                    defect.GenerateId();
                    defect.Code = $"DefectCode{defect.Id}";
                    defect.Description = $"DefectDescription{defect.Id}";
                    defect.DefectGradeId = 1;
                    defect.DefectCategory = category;
                    defects.Add(defect);
                }
                RF.Save(defects);
            }
            return defects;
        }

        public virtual DefectCategory CreateDefectCategory(string code)
        {
            var category = RT.Service.Resolve<CommonController>().GetData<DefectCategory>(p => p.Code == code);
            if (category == null)
            {
                category = new DefectCategory()
                {
                    Code = code,
                    Description = $"{code}描述"
                };
                category.GenerateId();
                RF.Save(category);
            }
            return category;
        }
        #endregion

        #region 缺陷责任
        /// <summary>
        /// 获取缺陷责任
        /// </summary>
        /// <param name="count">获取条数</param>
        /// <returns>缺陷责任</returns>
        public virtual EntityList<DefectResponsibility> GetDefectRespons(int count)
        {
            if (count < 1)
                throw new ValidationException("参数不能小于1");
            var defectRespons = Query<DefectResponsibility>().ToList(new PagingInfo(1, count));
            if (defectRespons.Count < count)
            {
                var category = GetResponCategoryByCode("MES缺陷责任分类");
                defectRespons = new EntityList<DefectResponsibility>();
                for (int i = 0; i < count; i++)
                {
                    var defectRespon = new DefectResponsibility();
                    defectRespon.GenerateId();
                    defectRespon.Code = $"DefectCode{defectRespon.Id}";
                    defectRespon.Description = $"DefectDescription{defectRespon.Id}";
                    defectRespon.CategoryId = category.Id;
                    defectRespons.Add(defectRespon);
                }
                RF.Save(defectRespons);
            }
            return defectRespons;
        }

        /// <summary>
        /// 根据编码获取缺陷责任分类
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>缺陷责任分类</returns>
        public virtual DefectResponsibilityCategory GetResponCategoryByCode(string code)
        {
            var responCategory = RT.Service.Resolve<CommonController>().GetData<DefectResponsibilityCategory>(p => p.Code == code);
            if (responCategory == null)
            {
                responCategory = new DefectResponsibilityCategory()
                {
                    Code = code,
                    Description = $"{code}描述"
                };
                responCategory.GenerateId();
                RF.Save(responCategory);
            }
            return responCategory;
        }
        #endregion

        #region 维修措施
        /// <summary>
        /// 获取维修措施
        /// </summary>
        /// <param name="count">条数</param>
        /// <returns>维修措施</returns>
        public virtual EntityList<RepairMeasure> GetRepairMeasures(int count)
        {
            if (count < 1)
                throw new ValidationException("参数不能小于1");
            var repairMeasures = Query<RepairMeasure>().ToList(new PagingInfo(1, count));
            if (repairMeasures.Count < count)
            {
                repairMeasures = new EntityList<RepairMeasure>();
                for (int i = 0; i < count; i++)
                {
                    var repairMeasure = new RepairMeasure();
                    repairMeasure.GenerateId();
                    repairMeasure.Code = $"MeasureCode{repairMeasure.Id}";
                    repairMeasure.Name = $"MeasureName{repairMeasure.Id}";
                    repairMeasure.Description = $"MeasureDescription{repairMeasure.Id}";
                    repairMeasures.Add(repairMeasure);
                }
                RF.Save(repairMeasures);
            }
            return repairMeasures;
        }
        #endregion
    }
}