using SIE.Defects;
using SIE.Domain;
using SIE.xUnit.Core;

namespace SIE.xUnit.Defects.Defects.Fixtures
{
    /// <summary>
    /// 缺陷固件（缺陷分类、缺陷代码）
    /// </summary>
    public class DefectFixture : FixtureBase
    {
        /// <summary>
        /// 缺陷分类 
        /// </summary>
        public DefectCategory DefectCategory { get; set; }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public Defect Defect { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public DefectFixture()
        {
            DefectTestController defectTestController = RT.Service.Resolve<DefectTestController>();
            DefectCategory = defectTestController.CreateDefectCategory();
            Defect = defectTestController.CreateDefectByCategory(DefectCategory.Id);
        }
    }
}
