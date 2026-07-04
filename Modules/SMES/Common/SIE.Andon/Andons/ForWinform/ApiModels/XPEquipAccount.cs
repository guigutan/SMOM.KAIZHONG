using SIE.Core.Enums;
using SIE.Core.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XPEquipAccount
    {
        public double Id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public AccountState State { get; set; }

        /// <summary>
        /// 管理状态（原使用状态）
        /// </summary>
        public AccountUseState UseState { get; set; }

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipTypeCode { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipTypeCategory { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string InstallationLocation { get; set; }

        public static XPEquipAccount Gen(EquipAccount ea)
        {
            return new XPEquipAccount() {
                Id = ea.Id,
                Code = ea.Code,
                Name = ea.Name,
                State = ea.State,
                UseState = ea.UseState,
                ModelCode = ea.ModelCode,
                ModelName = ea.ModelName,
                //SupplierId = ea.SupplierId,
                //SupplierCode = ea.SupplierCode,
                //SupplierName = ea.SupplierName,
                EquipTypeCode = ea.EquipTypeCode,
                EquipTypeName = ea.EquipTypeName,
                EquipTypeCategory = ea.EquipTypeCategory,
               //WorkShopId = ea.WorkShopId,
                //WorkShopCode = ea.WorkShopCode,
                //WorkShopName = ea.WorkShopName,
                //ProcessId = ea.ProcessId,
                //InstallationLocation = ea.InstallationLocation,
            };
        }
    }
}
