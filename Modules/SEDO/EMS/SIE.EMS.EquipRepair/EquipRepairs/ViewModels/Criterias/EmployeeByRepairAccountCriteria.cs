
using SIE.Domain;
using SIE.EMS.EquipRepair.Controller;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels.Criterias
{
    /// <summary>
    /// 员工可以维护该台账查询器
    /// </summary>
    [QueryEntity, Serializable]
    public class EmployeeByRepairAccountCriteria:Criteria
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EmployeeByRepairAccountCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<EmployeeByRepairAccountCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<RepairController>().GetByEmployeeByRepairAccountCriteria(this);
        }
    }
}
