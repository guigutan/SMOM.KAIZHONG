using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.ApiModels
{
    /// <summary>
    /// 设备归还信息
    /// </summary>
    [Serializable]
    public class EquipReturnInfo
    {
        /// <summary>
        /// 归还单Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 归还单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        /// 借机部门
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 借机人
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 借机时间
        /// </summary>
        public string LendTime { get; set; }

        /// <summary>
        /// 借机对象
        /// </summary>
        public int LendObject { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        
        /// <summary>
        /// 借出原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 归还说明
        /// </summary>
        public string ReturnRemark { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelCode { get; set; }
    }

    /// <summary>
    /// 查询信息
    /// </summary>
    [Serializable]
    public class EquipReturnQueryInfo
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 数据实体
        /// </summary>
        public List<EquipReturnInfo> EquipReturnInfos { get; set; }
    }
}
