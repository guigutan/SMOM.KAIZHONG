using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TemporaryRepairs
{
    /// <summary>
    /// 缺陷维修方案
    /// </summary>
    [RootEntity, Serializable]
    [Label("缺陷维修方案")]
    public partial class DefectRepairSolution : DataEntity
    {
        #region 维修方案 Solution
        /// <summary>
        /// 维修方案
        /// </summary>
        [Required]
        [Label("维修方案")]
        public static readonly Property<string> SolutionProperty = P<DefectRepairSolution>.Register(e => e.Solution);

        /// <summary>
        /// 维修方案
        /// </summary>
        public string Solution
        {
            get { return GetProperty(SolutionProperty); }
            set { SetProperty(SolutionProperty, value); }
        }
        #endregion

        #region 推荐次数 RecommendQty
        /// <summary>
        /// 推荐次数
        /// </summary>
        [Label("推荐次数")]
        public static readonly Property<int?> RecommendQtyProperty = P<DefectRepairSolution>.Register(e => e.RecommendQty);

        /// <summary>
        /// 推荐次数
        /// </summary>
        public int? RecommendQty
        {
            get { return GetProperty(RecommendQtyProperty); }
            set { SetProperty(RecommendQtyProperty, value); }
        }
        #endregion

        #region 缺陷代码 Defect
        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public static readonly IRefIdProperty DefectIdProperty = P<DefectRepairSolution>.RegisterRefId(e => e.DefectId, ReferenceType.Parent);

        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<DefectRepairSolution>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public Defect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 缺陷维修方案 实体配置
    /// </summary>
    internal class DefectRepairSolutionConfig : EntityConfig<DefectRepairSolution>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_DEF_REPAIR_SOLU").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}