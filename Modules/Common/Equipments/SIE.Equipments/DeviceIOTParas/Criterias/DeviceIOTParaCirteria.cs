using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceIOTParas.Criterias
{
    /// <summary>
    /// 设备物联参数查询器
    /// </summary>
    [QueryEntity, Serializable]
    public class DeviceIOTParaCirteria: Criteria
    {
        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> TypeCategoryProperty = P<DeviceIOTParaCirteria>.Register(e => e.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return this.GetProperty(TypeCategoryProperty); }
            set { this.SetProperty(TypeCategoryProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<DeviceIOTParaCirteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefNullableId(EquipModelIdProperty); }
            set { this.SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<DeviceIOTParaCirteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 模型编码 Code
        /// <summary>
        /// 模型编码
        /// </summary>
        [Label("模型编码")]
        public static readonly Property<string> CodeProperty = P<DeviceIOTParaCirteria>.Register(e => e.Code);

        /// <summary>
        /// 模型编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<DeviceIOTParaCirteria>.Register(e => e.CreateDate);                                                                      
       
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DeviceIOTParaController>().SelectByCriteria(this);
        }
    }
}
