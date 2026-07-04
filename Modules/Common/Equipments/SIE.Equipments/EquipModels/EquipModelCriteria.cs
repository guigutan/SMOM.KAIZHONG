using SIE.Domain;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipModels
{
    /// <summary>
    /// 设备型号查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备型号查询实体")]
    public partial class EquipModelCriteria : Criteria
    {
        /// <summary>
        /// 构造函数-初始化查询对象
        /// </summary>
        public EquipModelCriteria()
        {
            CreateDate = new DateRange();
            CreateDate.DateTimePart = DateTimePart.Date;  //选择日期格式为天
            CreateDate.DateRangeType = DateRangeType.All;  //默认日期为全部
        }

        #region 型号编码 Code
        /// <summary>
        /// 型号编码
        /// </summary>
        [Required]
        [Label("型号编码")]
        public static readonly Property<string> CodeProperty = P<EquipModelCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<EquipModelCriteria>.Register(e => e.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 设备类别 TypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> TypeCategoryProperty = P<EquipModelCriteria>.Register(e => e.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string TypeCategory
        {
            get { return GetProperty(TypeCategoryProperty); }
            set { SetProperty(TypeCategoryProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<EquipModelCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public static readonly IRefIdProperty EquipTypeIdProperty = P<EquipModelCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double EquipTypeId
        {
            get { return (double)GetRefId(EquipTypeIdProperty); }
            set { SetRefId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<EquipModelCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipModelController>().GetEquipModelsByCriteria(this);
        }
    }
}
