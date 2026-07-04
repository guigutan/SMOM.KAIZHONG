using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Andon.Andons.APIModel
{
    /// <summary>
    /// 安灯管理信息
    /// </summary>
    [Serializable]
    public class AndonManagerResultInfos: PagingBaseDataInfo
    {
        /// <summary>
        /// 安灯管理单据结果
        /// </summary>
        public List<AndonManagerResultInfo> AndonManagerResults { get; set; } = new List<AndonManagerResultInfo>();

    }

    /// <summary>
    /// 安灯管理结果信息
    /// </summary>
    [Serializable]
    public class AndonManagerResultInfo
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 事件编码
        /// </summary>

        public string Code { get; set; }

        /// <summary>
        /// 安灯类型
        /// </summary>

        public string AndonType { get; set; }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName { get; set; }

        /// <summary>
        /// 安灯Id
        /// </summary>
        public double AndonId { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        public string ExecptionDesc { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

    }



}
