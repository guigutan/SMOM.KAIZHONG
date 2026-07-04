using SIE.Core.Inspections;
using SIE.Defects;
using SIE.Defects.InspectionItems;
using SIE.Domain;

namespace SIE.xUnit.Defects.Defects
{
    /// <summary>
    /// 缺陷测试数据控制器
    /// </summary>
    public class DefectTestController : DomainController
    {
        /// <summary>
        /// 创建缺陷分类
        /// </summary>
        /// <param name="IsSave">是否保存</param>
        /// <returns></returns>
        public virtual DefectCategory CreateDefectCategory(bool IsSave = true)
        {

            var entity = new DefectCategory();
            entity.GenerateId();
            entity.Code = $"Code{entity.Id}";
            entity.Description = $"Description{entity.Id}";
            if (IsSave)
                RF.Save(entity);
            return entity;
        }

        /// <summary>
        /// 创建缺陷代码（根据缺陷分类 ）
        /// </summary>
        /// <param name="DefectCategoryId">缺陷分类ID</param>
        /// <param name="IsSave">是否保存</param>
        /// <returns></returns>
        public virtual Defect CreateDefectByCategory(double DefectCategoryId, bool IsSave = true)
        {
            var entity = new Defect();
            entity.GenerateId();
            entity.Code = $"Code{entity.Id}";
            entity.Description = $"Description{entity.Id}";
            entity.DefectGradeId = 1;
            entity.QualityType = QualityType.Common;
            entity.DefectCategoryId = DefectCategoryId;
            if (IsSave)
                RF.Save(entity);
            return entity;
        }

        /// <summary>
        /// 创建检验方式
        /// </summary>
        /// <param name="type">检验类型</param>
        /// <param name="name">检验方式名称</param>
        /// <param name="IsSave">是否保存</param>
        /// <returns></returns>
        public virtual InspectionMode CreateInspectionMode(InspectionType type, string name = "正常", bool IsSave = true)
        {
            var entity = new InspectionMode();
            entity.GenerateId();
            entity.Code = $"InspModeCode{entity.Id}";
            entity.Name = name;
            if (IsSave)
                RF.Save(entity);
            return entity;
        }
    }
}
