using SIE.Domain;
using SIE.Equipments.EquipStatus.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipStatus
{
    /// <summary>
    /// 设备状态查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备状态查询实体")]
    public class EquipStatusCriteria : Criteria
    {
        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipStatusCriteria>.Register(e => e.EquipAccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { this.SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipStatusCriteria>.Register(e => e.EquipAccountName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 状态 Status
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<EquipStatusDetailStatus?> StatusProperty = P<EquipStatusCriteria>.Register(e => e.Status);

        /// <summary>
        /// 状态
        /// </summary>
        public EquipStatusDetailStatus? Status
        {
            get { return this.GetProperty(StatusProperty); }
            set { this.SetProperty(StatusProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<EquipStatusCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipStatusController>().CriteriaEquipStatus(this);
        }
    }
}
