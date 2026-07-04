using SIE.Core.Equipments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Models
{
    /// <summary>
	/// 设备清单
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("设备清单")]
    public partial class FixtureModelEquipDetail : DataEntity
    {
        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public static readonly IRefIdProperty EquipModelIdProperty = P<FixtureModelEquipDetail>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<FixtureModelEquipDetail>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备清单列表 FixtureModel
        /// <summary>
        /// 设备清单列表Id
        /// </summary>
        public static readonly IRefIdProperty FixtureModelIdProperty = P<FixtureModelEquipDetail>.RegisterRefId(e => e.FixtureModelId, ReferenceType.Parent);

        /// <summary>
        /// 设备清单列表Id
        /// </summary>
        public double FixtureModelId
        {
            get { return (double)GetRefId(FixtureModelIdProperty); }
            set { SetRefId(FixtureModelIdProperty, value); }
        }

        /// <summary>
        /// 设备清单列表
        /// </summary>
        public static readonly RefEntityProperty<FixtureModel> FixtureModelProperty = P<FixtureModelEquipDetail>.RegisterRef(e => e.FixtureModel, FixtureModelIdProperty);

        /// <summary>
        /// 设备清单列表
        /// </summary>
        public FixtureModel FixtureModel
        {
            get { return GetRefEntity(FixtureModelProperty); }
            set { SetRefEntity(FixtureModelProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<FixtureModelEquipDetail>.RegisterView(e => e.EquipModelCode, p => p.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
            set { this.SetProperty(EquipModelCodeProperty, value); }
        }
        #endregion 

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<FixtureModelEquipDetail>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// 设备清单 实体配置
    /// </summary>
    internal class FixtureModelEquipDetailConfig : EntityConfig<FixtureModelEquipDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MODEL_EQUIP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
