using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.AssetTransfers
{

    /// <summary>
    /// 设备台账附加信息
    /// </summary>

    [Serializable]
    public class EquipAccountExtInfo
    {
        /// <summary>
        /// 
        /// </summary>

        public double? OriginalResponsibleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalResponsibleId_Display { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? OriginalWorkshopId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalWorkshopId_Display { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? OriginalResourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalResourceId_Display { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalLocation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? OriginalWarehouseId { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string OriginalWarehouseId_Display { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? OrinialStorageLocationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrinialStorageLocationId_Display { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public double? OrignalKeeperId { get; set; }
        /// <summary>
        /// /
        /// </summary>
        public string OrignalKeeperId_Display { get; set; }

    }
}
