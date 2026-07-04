using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// 编码类工治具台帐查询体
	/// </summary>
	[QueryEntity, Serializable]

    public partial class FixtureCodeAccountCriteria : Criteria
    {
        #region 工治具ID
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> CodeProperty = P<FixtureCodeAccountCriteria>.Register(e => e.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<FixtureCodeAccountCriteria>.Register(e => e.EncodeCode);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return GetProperty(EncodeCodeProperty); }
            set { SetProperty(EncodeCodeProperty, value); }
        }
        #endregion

        #region ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureCodeAccountCriteria>.Register(e => e.ModelCode);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return GetProperty(ModelCodeProperty); }
            set { SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureCodeAccountCriteria>.Register(e => e.ModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion


        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureCodeAccountCriteria>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

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
            P<FixtureCodeAccountCriteria>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取编码类工治具台帐列表
        /// </summary>
        /// <returns>编码类工治具台帐列表</returns>
		protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureCodeAccountList(this);
        }
    }
}
