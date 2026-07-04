using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Equipments.Models.ViewModels
{
    /// <summary>
    /// 润滑备件信息
    /// </summary>
    [Serializable]
    public class EquipModelLubricationProjectInfo
    {
        #region 子列表
        /// <summary>
        /// 备件清单列表
        /// </summary>
        public EntityList<EquipModelLubricaSparePart> EquipModelLubricaSparePartList { get; set; } = new EntityList<EquipModelLubricaSparePart>();

        #endregion
    }
}
