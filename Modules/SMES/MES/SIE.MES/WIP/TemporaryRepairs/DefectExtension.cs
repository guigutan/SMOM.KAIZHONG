using SIE.Defects;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.MES.TemporaryRepairs
{
    /// <summary>
    /// 缺陷代码扩展属性
    /// </summary>
    [Label("缺陷代码扩展属性")]
    [CompiledPropertyDeclarer]
    public partial class DefectExtension
    {
        #region 缺陷维修方案 SolutionList
        /// <summary>
        /// 缺陷维修方案
        /// </summary>
        [Label("缺陷维修方案")]
        public static readonly ListProperty<EntityList<DefectRepairSolution>> SolutionListProperty = P<Defect>.RegisterExtensionList<EntityList<DefectRepairSolution>>("SolutionList", typeof(DefectExtension));

        /// <summary>
        /// 缺陷维修方案
        /// </summary>
        /// <param name="me">缺陷代码</param>
        /// <returns>缺陷维修方案</returns>
        public static EntityList<DefectRepairSolution> GetSolutionList(Defect me)
        {
            return me.GetProperty(SolutionListProperty);
        }

        /// <summary>
        /// 缺陷维修方案
        /// </summary>
        /// <param name="me">缺陷代码</param>
        /// <param name="value">值</param>
        public static void SetSolutionList(Defect me, EntityList<DefectRepairSolution> value)
        {
            me.SetProperty(SolutionListProperty, value);
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class DefectExtensionConfig : EntityConfig<Defect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(DefectExtension.SolutionListProperty).DontMapColumn();
        }
    }
}
