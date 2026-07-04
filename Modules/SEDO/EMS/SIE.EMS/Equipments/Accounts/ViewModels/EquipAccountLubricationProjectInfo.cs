using SIE.Domain;
using System;

namespace SIE.EMS.Equipments.Accounts.ViewModels
{
    /// <summary>
    /// 润滑备件信息
    /// </summary>
    [Serializable]
    public class EquipAccountLubricationProjectInfo
    {
        #region 子列表
        /// <summary>
        /// 备件清单列表
        /// </summary>
        public EntityList<EquipAccountLubricaSparePart> EquipAccountLubricaSparePartList { get; set; } = new EntityList<EquipAccountLubricaSparePart>();

        #endregion
    }
}
