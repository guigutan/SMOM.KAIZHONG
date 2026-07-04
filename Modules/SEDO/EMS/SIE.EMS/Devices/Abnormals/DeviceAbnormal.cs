using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Devices.Abnormals
{
    /// <summary>
    /// 设备异常维护
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DeviceAbnormalCriteria))]
    [DisplayMember(nameof(DeviceAbnormal.Code))]
    [Label("设备异常维护")]
    public partial class DeviceAbnormal : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Label("故障名称")]
        public static readonly Property<string> CodeProperty = P<DeviceAbnormal>.Register(e => e.Code);

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
        public static readonly Property<string> DescriptionProperty = P<DeviceAbnormal>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty =
            P<DeviceAbnormal>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
            P<DeviceAbnormal>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return this.GetRefEntity(EquipTypeProperty); }
            set { this.SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion


        #region 类型 AbnormalType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<AbnormalType?> AbnormalTypeProperty = P<DeviceAbnormal>.Register(e => e.AbnormalType);

        /// <summary>
        /// 类型
        /// </summary>
        public AbnormalType? AbnormalType
        {
            get { return GetProperty(AbnormalTypeProperty); }
            set { SetProperty(AbnormalTypeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备类型名称 EquipTypeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型名称")]
        public static readonly Property<string> EquipTypeNameProperty = P<DeviceAbnormal>.RegisterView(e => e.EquipTypeName, p => p.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备异常维护 实体配置
    /// </summary>
    internal class DeviceAbnormalConfig : EntityConfig<DeviceAbnormal>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                   DeviceAbnormal.CodeProperty,
                   DeviceAbnormal.AbnormalTypeProperty
                },
                MessageBuilder = o =>
                {
                    var deviceAbnormal = o as DeviceAbnormal;
                    return "已存在故障名称为[{0}]和类型为[{1}]的设备异常维护".L10nFormat(deviceAbnormal.Code,deviceAbnormal.AbnormalType.ToLabel());
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }


        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_DEVICE_ABNO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}