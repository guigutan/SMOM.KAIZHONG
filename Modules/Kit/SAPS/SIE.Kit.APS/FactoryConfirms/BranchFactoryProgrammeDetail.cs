using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 分厂方案设置明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("分厂方案设置明细")]
    public partial class BranchFactoryProgrammeDetail : DataEntity
    {
        #region 分配规则 ProgrammeRule
        /// <summary>
        /// 分配规则
        /// </summary>
        [Label("分配规则")]
        public static readonly Property<ProgrammeRule> ProgrammeRuleProperty = P<BranchFactoryProgrammeDetail>.Register(e => e.ProgrammeRule);

        /// <summary>
        /// 分配规则
        /// </summary>
        public ProgrammeRule ProgrammeRule
        {
            get { return GetProperty(ProgrammeRuleProperty); }
            set { SetProperty(ProgrammeRuleProperty, value); }
        }
        #endregion

        #region 分厂方案设置 BranchFactoryProgramme
        /// <summary>
        /// 分厂方案设置Id
        /// </summary>
        public static readonly IRefIdProperty BranchFactoryProgrammeIdProperty = P<BranchFactoryProgrammeDetail>.RegisterRefId(e => e.BranchFactoryProgrammeId, ReferenceType.Parent);

        /// <summary>
        /// 分厂方案设置Id
        /// </summary>
        public double BranchFactoryProgrammeId
        {
            get { return (double)GetRefId(BranchFactoryProgrammeIdProperty); }
            set { SetRefId(BranchFactoryProgrammeIdProperty, value); }
        }

        /// <summary>
        /// 分厂方案设置
        /// </summary>
        public static readonly RefEntityProperty<BranchFactoryProgramme> BranchFactoryProgrammeProperty = P<BranchFactoryProgrammeDetail>.RegisterRef(e => e.BranchFactoryProgramme, BranchFactoryProgrammeIdProperty);

        /// <summary>
        /// 分厂方案设置
        /// </summary>
        public BranchFactoryProgramme BranchFactoryProgramme
        {
            get { return GetRefEntity(BranchFactoryProgrammeProperty); }
            set { SetRefEntity(BranchFactoryProgrammeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 分厂方案设置明细 实体配置
    /// </summary>
    internal class BranchFactiryProgrammeDetailConfig : EntityConfig<BranchFactoryProgrammeDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_BRANCH_FACTORY_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableSort();
        }
    }
}
