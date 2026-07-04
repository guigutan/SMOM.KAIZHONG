using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Models
{
    /// <summary>
    /// 工治具模型查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工治具模型查询实体")]
    public class FixtureModelCriteria : Criteria
    {
        #region 型号编码 Code
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> CodeProperty = P<FixtureModelCriteria>.Register(e => e.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 型号名称 Name
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> NameProperty = P<FixtureModelCriteria>.Register(e => e.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode?> ManageModeProperty = P<FixtureModelCriteria>.Register(e => e.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode? ManageMode
        {
            get { return GetProperty(ManageModeProperty); }
            set { SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureModelCriteria>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<SIE.Fixtures.FixtureTypes.FixtureType> FixtureTypeProperty =
            P<FixtureModelCriteria>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public SIE.Fixtures.FixtureTypes.FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureTypes
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypesProperty = P<FixtureModelCriteria>.Register(e => e.FixtureTypes);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypes
        {
            get { return this.GetProperty(FixtureTypesProperty); }
            set { this.SetProperty(FixtureTypesProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取工治具模型列表
        /// </summary>
        /// <returns>工治具模型列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureModelList(this);
        }
    }
}
