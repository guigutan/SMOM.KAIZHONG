using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.EquipModels;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备件查询")]
    public partial class SparePartCriteria : Criteria
    {
        #region 编码 SparePartCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartCriteria>.Register(e => e.SparePartCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 名称 SparePartName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartCriteria>.Register(e => e.SparePartName);

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType?> SpartTypeProperty = P<SparePartCriteria>.Register(e => e.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType? SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
            set { this.SetProperty(SpartTypeProperty, value); }
        }
        #endregion

        #region 设备型号 SpartEquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty SpartEquipModelIdProperty =
            P<SparePartCriteria>.RegisterRefId(e => e.SpartEquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? SpartEquipModelId
        {
            get { return (double?)this.GetRefNullableId(SpartEquipModelIdProperty); }
            set { this.SetRefNullableId(SpartEquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> SpartEquipModelProperty =
            P<SparePartCriteria>.RegisterRef(e => e.SpartEquipModel, SpartEquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel SpartEquipModel
        {
            get { return this.GetRefEntity(SpartEquipModelProperty); }
            set { this.SetRefEntity(SpartEquipModelProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<SparePartCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 分类层级 ItemCategory
        /// <summary>
        /// 分类层级Id
        /// </summary>
        [Label("分类层级")]
        public static readonly IRefIdProperty ItemCategoryIdProperty =
            P<SparePartCriteria>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 分类层级Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)this.GetRefNullableId(ItemCategoryIdProperty); }
            set { this.SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类层级
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty =
            P<SparePartCriteria>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类层级
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return this.GetRefEntity(ItemCategoryProperty); }
            set { this.SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<SparePartCriteria>.Register(e => e.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod? ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 库存情况 StorageState
        /// <summary>
        /// 库存情况
        /// </summary>
        [Label("库存情况")]
        public static readonly Property<StorageState?> StorageStateProperty = P<SparePartCriteria>.Register(e => e.StorageState);

        /// <summary>
        /// 库存情况
        /// </summary>
        public StorageState? StorageState
        {
            get { return this.GetProperty(StorageStateProperty); }
            set { this.SetProperty(StorageStateProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<SparePartCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion



        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SparePartController>().GetSparePartList(this);
        }
    }
}
