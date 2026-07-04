using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 异常信息查询结果实体
    /// </summary>
    [Serializable]
    public class AbnormalsPagingResultInfo : PagingResultInfo
    {
        /// <summary>
        /// 异常信息查询结果明细实体
        /// </summary>
        public List<AbnormalsResultInfo> AbnormalsResultInfos { get; set; } = new List<AbnormalsResultInfo>();
    }

    /// <summary>
    /// 异常信息查询结果明细实体
    /// </summary>
    [Serializable]
    public class AbnormalsResultInfo
    {
        /// <summary>
        /// 异常信息ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 异常信息编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double? EquipTypeId { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public int AbnormalType { get; set; }

        /// <summary>
        /// 异常类型名称
        /// </summary>
        public string AbnormalTypeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
