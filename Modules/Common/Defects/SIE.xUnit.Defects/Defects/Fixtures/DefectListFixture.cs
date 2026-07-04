using SIE.Defects;
using SIE.Domain;
using SIE.xUnit.Core;

namespace SIE.xUnit.Defects.Defects.Fixtures
{
    /// <summary>
    /// 缺陷集合固件（缺陷分类、缺陷代码）
    /// </summary>
    public class DefectListFixture : FixtureBase
    {
        /// <summary>
        /// 缺陷代码集合
        /// </summary>
        public EntityList<Defect> DefectList { get; set; }

        /// <summary>
        /// 构造函数 
        /// </summary>
        public DefectListFixture()
        {
            DefectList = CreateDefectList(2);
        }

        /// <summary>
        /// 创建缺陷代码集合
        /// </summary>
        /// <param name="count">创建数量</param>
        protected virtual EntityList<Defect> CreateDefectList(int count)
        {
            DefectTestController defectTestController = RT.Service.Resolve<DefectTestController>();
            EntityList<Defect> result = new EntityList<Defect>();
            for (int i = 0; i < count; i++)
            {
                var defectCategory = defectTestController.CreateDefectCategory();
                var defect = defectTestController.CreateDefectByCategory(defectCategory.Id);
                result.Add(defect);
            }
            return result;
        }
    }
}
