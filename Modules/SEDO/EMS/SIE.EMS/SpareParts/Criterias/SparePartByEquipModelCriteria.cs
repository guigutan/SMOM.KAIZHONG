using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;

namespace SIE.EMS.SpareParts.Criterias
{
    /// <summary>
    /// 根据设备类型查询备件
    /// </summary>
    public class SparePartByEquipModelCriteria : Criteria
    {
        #region 设备模型 EquipModel
        /// <summary>
        /// 设备模型Id
        /// </summary>
        [Label("EquipModel")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<SparePartByEquipModelCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备模型Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefNullableId(EquipModelIdProperty); }
            set { this.SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备模型
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<SparePartByEquipModelCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备模型
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备模型名称 EquipModelCode
        /// <summary>
        /// 设备模型名称
        /// </summary>
        [Label("设备模型名称")]
        public static readonly Property<string> EquipModelCodeProperty = P<SparePartByEquipModelCriteria>.Register(e => e.EquipModelCode);

        /// <summary>
        /// 设备模型名称
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
            set { this.SetProperty(EquipModelCodeProperty, value); }
        }
        #endregion

        
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SparePartController>().GetSparePartByAccount(this);
        }

    }
}
