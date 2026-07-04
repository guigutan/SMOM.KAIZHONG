using SIE.Domain;
using SIE.Equipments.EquipStatus.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipStatus
{
    /// <summary>
    /// 设备状态明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备状态明细")]
    public class EquipStatusDetail : DataEntity
    {
        #region 设备状态 EquipStatus
        /// <summary>
        /// 设备状态Id
        /// </summary>
        [Label("属性名")]
        public static readonly IRefIdProperty EquipStatusIdProperty =
            P<EquipStatusDetail>.RegisterRefId(e => e.EquipStatusId, ReferenceType.Parent);

        /// <summary>
        /// 设备状态Id
        /// </summary>
        public double EquipStatusId
        {
            get { return (double)this.GetRefId(EquipStatusIdProperty); }
            set { this.SetRefId(EquipStatusIdProperty, value); }
        }

        /// <summary>
        /// 设备状态
        /// </summary>
        public static readonly RefEntityProperty<EquipStatus> EquipStatusProperty =
            P<EquipStatusDetail>.RegisterRef(e => e.EquipStatus, EquipStatusIdProperty);

        /// <summary>
        /// 设备状态
        /// </summary>
        public EquipStatus EquipStatus
        {
            get { return this.GetRefEntity(EquipStatusProperty); }
            set { this.SetRefEntity(EquipStatusProperty, value); }
        }
        #endregion

        #region 状态 Status
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<EquipStatusDetailStatus> StatusProperty = P<EquipStatusDetail>.Register(e => e.Status);

        /// <summary>
        /// 状态
        /// </summary>
        public EquipStatusDetailStatus Status
        {
            get { return this.GetProperty(StatusProperty); }
            set { this.SetProperty(StatusProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime?> BeginTimeProperty = P<EquipStatusDetail>.Register(e => e.BeginTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get { return this.GetProperty(BeginTimeProperty); }
            set { this.SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 截止时间 EndTime
        /// <summary>
        /// 截止时间
        /// </summary>
        [Label("截止时间")]
        public static readonly Property<DateTime> EndTimeProperty = P<EquipStatusDetail>.Register(e => e.EndTime);

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 时长(分钟) Minute
        /// <summary>
        /// 时长(分钟)
        /// </summary>
        [Label("时长(分钟)")]
        public static readonly Property<decimal?> MinuteProperty = P<EquipStatusDetail>.Register(e => e.Minute);

        /// <summary>
        /// 时长(分钟)
        /// </summary>
        public decimal? Minute
        {
            get { return this.GetProperty(MinuteProperty); }
            set { this.SetProperty(MinuteProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备Id EquipAccountId
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备Id")]
        public static readonly Property<double> EquipAccountIdProperty = P<EquipStatusDetail>.RegisterView(e => e.EquipAccountId, p => p.EquipStatus.EquipAccountId);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return this.GetProperty(EquipAccountIdProperty); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipStatusDetail>.RegisterView(e => e.EquipAccountCode, p => p.EquipStatus.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #endregion
    }

    internal class EquipStatusDetailConfig : EntityConfig<EquipStatusDetail>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("EQUIP_STATUS_DTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
