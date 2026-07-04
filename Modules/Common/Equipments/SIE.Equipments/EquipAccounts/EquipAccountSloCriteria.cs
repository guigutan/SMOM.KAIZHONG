using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
    /// 缸槽查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("缸槽查询实体")]
    public class EquipAccountSloCriteria : Criteria
    {
        #region 使用状态列表 UseStateList
        /// <summary>
        /// 使用状态列表
        /// </summary>
        
        public static readonly Property<List<AccountUseState>> UseStateListProperty = P<EquipAccountSloCriteria>.Register(e => e.UseStateList);

        /// <summary>
        /// 使用状态列表
        /// </summary>
        public List<AccountUseState> UseStateList
        {
            get { return GetProperty(UseStateListProperty); }
            set { SetProperty(UseStateListProperty, value); }
        }
        #endregion

        #region 设备状态列表 StateList
        /// <summary>
        /// 设备状态列表
        /// </summary>
      
        public static readonly Property<List<AccountState>> StateListProperty = P<EquipAccountSloCriteria>.Register(e => e.StateList);

        /// <summary>
        /// 设备状态列表
        /// </summary>
        public List<AccountState> StateList
        {
            get { return GetProperty(StateListProperty); }
            set { SetProperty(StateListProperty, value); }
        }
        #endregion
    }
}
