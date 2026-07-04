using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts.Criterias
{
    /// <summary>
    /// 设备型号查询器
    /// </summary>
    [QueryEntity, Serializable]
    public class AccountByModleTypeCriteria : Criteria
    {
        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<AccountByModleTypeCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

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
            P<AccountByModleTypeCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipController>().AccountByModleTypeCriteria(this);
        }
    }
}
