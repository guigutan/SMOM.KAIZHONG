using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceIOTParas.Criterias
{
    /// <summary>
    /// 物联参数查询器
    /// </summary>
    [QueryEntity, Serializable]
    public class PhysicalUnionSelCriteria : Criteria
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<PhysicalUnionSelCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<PhysicalUnionSelCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备台账编码 Code
        /// <summary>
        /// 设备台账编码
        /// </summary>
        [Label("设备台账编码")]
        public static readonly Property<string> CodeProperty = P<PhysicalUnionSelCriteria>.RegisterView(e => e.Code, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备台账编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
        }
        #endregion

        #region 设备台账名称 Name
        /// <summary>
        /// 设备台账名称
        /// </summary>
        [Label("设备台账名称")]
        public static readonly Property<string> NameProperty = P<PhysicalUnionSelCriteria>.RegisterView(e => e.Name, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备台账名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
        }
        #endregion

        #region 是否只读 IsReadOnly
        /// <summary>
        /// 是否只读
        /// </summary>
        [Label("是否只读")]
        public static readonly Property<bool?> IsReadOnlyProperty = P<PhysicalUnionSelCriteria>.Register(e => e.IsReadOnly);

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool? IsReadOnly
        {
            get { return this.GetProperty(IsReadOnlyProperty); }
            set { this.SetProperty(IsReadOnlyProperty, value); }
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
