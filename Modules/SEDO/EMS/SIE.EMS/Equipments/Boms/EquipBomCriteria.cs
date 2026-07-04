using SIE.Core.Equipments;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Equipments.Boms
{
    /// <summary>
    /// 设备BOM查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备BOM查询实体")]
    public partial class EquipBomCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EquipBomCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EquipBomCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty =
            P<EquipBomCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)this.GetRefNullableId(EquipTypeIdProperty); }
            set { this.SetRefNullableId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty =
            P<EquipBomCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return this.GetRefEntity(EquipTypeProperty); }
            set { this.SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDateTime
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateTimeProperty = P<EquipBomCriteria>.Register(e => e.CreateDateTime);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDateTime
        {
            get { return this.GetProperty(CreateDateTimeProperty); }
            set { this.SetProperty(CreateDateTimeProperty, value); }
        }
        #endregion


        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipBomController>().CriteriaEquipBoms(this);
        }
    }
}
