using SIE.Domain;
using SIE.EMS.Equipments.Models;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.Applys.Criterias
{
    /// <summary>
    /// 备件申请单查询器
    /// </summary>
    [QueryEntity, Serializable]
    public class SparePartAppCriteria : Criteria
    {
        #region 备件申请单号 No
        /// <summary>
        /// 备件申请单号
        /// </summary>
        [Label("备件申请单号")]
        public static readonly Property<string> NoProperty = P<SparePartAppCriteria>.Register(e => e.No);

        /// <summary>
        /// 备件申请单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 来源单号 FromNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> FromNoProperty = P<SparePartAppCriteria>.Register(e => e.FromNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string FromNo
        {
            get { return this.GetProperty(FromNoProperty); }
            set { this.SetProperty(FromNoProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<SparePartAppCriteria>.Register(e => e.EquipAccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { this.SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 来源类型 FromType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<FromType?> FromTypeProperty = P<SparePartAppCriteria>.Register(e => e.FromType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public FromType? FromType
        {
            get { return this.GetProperty(FromTypeProperty); }
            set { this.SetProperty(FromTypeProperty, value); }
        }
        #endregion

        #region 状态 AuditState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<AuditState?> AuditStateProperty = P<SparePartAppCriteria>.Register(e => e.AuditState);

        /// <summary>
        /// 状态
        /// </summary>
        public AuditState? AuditState
        {
            get { return this.GetProperty(AuditStateProperty); }
            set { this.SetProperty(AuditStateProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<SparePartAppCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<SparePartAppCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 备件编码 SparePart
        /// <summary>
        /// 备件编码Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<SparePartAppCriteria>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件编码Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件编码
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<SparePartAppCriteria>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件编码
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartAppCriteria>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartAppCriteria>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<SparePartAppCriteria>.Register(e => e.CreateDate);

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
            return RT.Service.Resolve<SparePartAppController>().SelectByCriteria(this);
        }
    }
}
