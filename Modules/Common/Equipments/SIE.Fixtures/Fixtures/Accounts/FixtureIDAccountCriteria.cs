using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台账（ID管理）查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工治具台账（ID管理）查询实体")]
    public class FixtureIDAccountCriteria : Criteria
    {
        #region 工治具ID Code
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> CodeProperty = P<FixtureIDAccountCriteria>.Register(e => e.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<FixtureIDAccountCriteria>.Register(e => e.FixtureEncodeCode);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
            set { this.SetProperty(FixtureEncodeCodeProperty, value); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureIDAccountCriteria>.Register(e => e.ModelCode);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureIDAccountCriteria>.Register(e => e.ModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureIDAccountCriteria>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

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
            P<FixtureIDAccountCriteria>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 状态 AccountState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<FixtureAccountState?> AccountStateProperty = P<FixtureIDAccountCriteria>.Register(e => e.AccountState);

        /// <summary>
        /// 状态
        /// </summary>
        public FixtureAccountState? AccountState
        {
            get { return GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 质量状态 QualityState
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<FixtureQualityState?> QualityStateProperty = P<FixtureIDAccountCriteria>.Register(e => e.QualityState);

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState? QualityState
        {
            get { return GetProperty(QualityStateProperty); }
            set { SetProperty(QualityStateProperty, value); }
        }
        #endregion

        #region 超过使用上限 IsExceed
        /// <summary>
        /// 超过使用上限
        /// </summary>
        [Label("超过使用上限")]
        public static readonly Property<YesNo?> IsExceedProperty = P<FixtureIDAccountCriteria>.Register(e => e.IsExceed);

        /// <summary>
        /// 超过使用上限
        /// </summary>
        public YesNo? IsExceed
        {
            get { return this.GetProperty(IsExceedProperty); }
            set { this.SetProperty(IsExceedProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取工治具台账（ID管理）
        /// </summary>
        /// <returns>工治具台账（ID管理）列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureIDAccounts(this);
        }
    }
}
