using SIE.Domain.Validation;
using System.ComponentModel;

namespace SIE.Defects
{
    #region 缺陷分类验证
    /// <summary>
    /// 关联缺陷代码不能删除
    /// </summary>
    [DisplayName("缺陷分类删除规则")]
    [Description("关联缺陷代码不能删除")]
    public class DefectNoReferenceRoutingRule : NoReferencedRule<DefectCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefectNoReferenceRoutingRule()
        {
            Properties.Add(Defect.DefectCategoryIdProperty);
        }
    }
    #endregion

    #region 缺陷责任分类验证
    /// <summary>
    /// 关联缺陷责任不能删除
    /// </summary>
    [DisplayName("缺陷分类删除规则")]
    [Description("关联缺陷责任不能删除")]
    public class DefectResponsibilityNoReferenceRoutingRule : NoReferencedRule<DefectResponsibilityCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefectResponsibilityNoReferenceRoutingRule()
        {
            Properties.Add(DefectResponsibility.CategoryIdProperty);
        }
    }
    #endregion

    #region 缺陷等级验证
    /// <summary>
    /// 关联缺陷代码不能删除
    /// </summary>
    [DisplayName("缺陷等级删除规则")]
    [Description("关联缺陷代码不能删除")]
    public class DefectGradeNoReferenceDefectRule : NoReferencedRule<DefectGrade>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefectGradeNoReferenceDefectRule()
        {
            Properties.Add(Defect.DefectGradeIdProperty);
        }
    }
    #endregion
}