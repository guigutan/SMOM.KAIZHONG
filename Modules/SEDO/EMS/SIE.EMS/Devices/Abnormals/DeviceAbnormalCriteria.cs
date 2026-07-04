using SIE.Domain;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Devices.Abnormals
{
    /// <summary>
	/// 设备异常维护查询体
	/// </summary>
	[QueryEntity, Serializable]
    public partial class DeviceAbnormalCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("异常编码")]
        public static readonly Property<string> CodeProperty = P<DeviceAbnormalCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("故障描述")]
        public static readonly Property<string> DescriptionProperty = P<DeviceAbnormalCriteria>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 设备类型  EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        public static readonly IRefIdProperty EquipTypeIdProperty = P<DeviceAbnormalCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<DeviceAbnormalCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 类型 AbnormalType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<AbnormalType?> AbnormalTypeProperty = P<DeviceAbnormalCriteria>.Register(e => e.AbnormalType);

        /// <summary>
        /// 类型
        /// </summary>
        public AbnormalType? AbnormalType
        {
            get { return GetProperty(AbnormalTypeProperty); }
            set { SetProperty(AbnormalTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取设备异常维护列表
        /// </summary>
        /// <returns>设备异常维护列表</returns>
		protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DeviceAbnormalController>().GetDeviceAbnormalsByCriteria(this);
        }
    }
}
