using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Fixtures.Abnormals
{
    /// <summary>
	/// 工治具异常类型查询体
	/// </summary>
	[QueryEntity, Serializable]
    public partial class FixtureAbnormalCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("异常编码")]
        public static readonly Property<string> CodeProperty = P<FixtureAbnormalCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("异常描述")]
        public static readonly Property<string> DescriptionProperty = P<FixtureAbnormalCriteria>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureAbnormalCriteria>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double? FixtureTypeId
        {
            get { return (double?)this.GetRefNullableId(FixtureTypeIdProperty); }
            set { this.SetRefNullableId(FixtureTypeIdProperty, value); }
        }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public static readonly RefEntityProperty<FixtureType> FixtureTypeProperty =
            P<FixtureAbnormalCriteria>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 类型 AbnormalType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<AbnormalType?> AbnormalTypeProperty = P<FixtureAbnormalCriteria>.Register(e => e.AbnormalType);

        /// <summary>
        /// 类型
        /// </summary>
        public AbnormalType? AbnormalType
        {
            get { return GetProperty(AbnormalTypeProperty); }
            set { SetProperty(AbnormalTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取工治具异常类型列表
        /// </summary>
        /// <returns>工治具异常类型列表</returns>
		protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureAbnormalsByCriteria(this);
        }
    }
}
