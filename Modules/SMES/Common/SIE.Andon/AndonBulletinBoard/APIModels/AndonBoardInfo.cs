using SIE.Andon.Andons.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonBulletinBoard.APIModels
{
    /// <summary>
    /// 安灯状态统计类
    /// </summary>
    [Serializable]
    public class AndonBoardStateInfo
    {
        /// <summary>
        /// 安灯状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 统计数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 安灯大类统计类
    /// </summary>
    [Serializable]
    public class AndonBoardClassInfo
    {
        /// <summary>
        /// 安灯大类
        /// </summary>
        public string AndonClass { get; set; }

        /// <summary>
        /// 统计数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 安灯停线信息
    /// </summary>
    [Serializable]
    public class AndonLineStop
    {
        /// <summary>
        /// 安灯总数
        /// </summary>
        public decimal AndonCount { get; set; }

        /// <summary>
        /// 待响应数
        /// </summary>
        public decimal StandbyCount { get; set; }
        
        /// <summary>
        /// 处理中数
        /// </summary>
        public decimal ProcessingCount { get; set; }

        /// <summary>
        /// 待接收数
        /// </summary>
        public decimal ToAcceptedCount { get; set; }

        /// <summary>
        /// 已关闭数量
        /// </summary>
        public decimal ClosedCount { get; set; }
        
        /// <summary>
        /// 停线总次数
        /// </summary>
        public decimal StopTimes { get; set; }

        /// <summary>
        /// 停线总时长
        /// </summary>
        public decimal TotalStop { get; set; }

        /// <summary>
        /// 最长停线产线
        /// </summary>
        public string MaxStopLine { get; set; }

        /// <summary>
        /// 最长停线时长
        /// </summary>
        public decimal MaxStopHour { get; set; }
    }

    /// <summary>
    /// 安灯类型信息
    /// </summary>
    [Serializable]
    public class AndonTypeInfo
    {
        /// <summary>
        /// 安灯类型
        /// </summary>
        public string AndonType { get; set; }

        /// <summary>
        /// 统计数
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 安灯类型柏拉图
    /// </summary>
    [Serializable]
    public class AndonTypePlato
    {
        /// <summary>
        /// 安灯类型统计信息
        /// </summary>
        public List<AndonTypeInfo> AndonTypeInfos { get; set; }

        /// <summary>
        /// 柏拉图折线
        /// </summary>
        public List<decimal> Plato { get; set; }
    }

    /// <summary>
    /// 安灯管理
    /// </summary>
    [Serializable]
    public class AndonManageInfo
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 安灯事件编码
        /// </summary>
        public string AndonManageCode { get; set; }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public string AndonType { get; set; }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string Andon { get; set; }

        /// <summary>
        /// 产线 或 工位
        /// </summary>
        public string WipResource { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccount { get; set; }

        /// <summary>
        /// 触发时间
        /// </summary>
        public string TriggerTime { get; set; }

        /// <summary>
        /// 负责部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string Handler { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public string LastTime { get; set; }
    }

    /// <summary>
    /// 安灯工位
    /// </summary>
    [Serializable]
    public class AndonStationInfo
    {
        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 待响应数量
        /// </summary>
        public decimal Standby { get; set; }

        /// <summary>
        /// 处理中数量
        /// </summary>
        public decimal Processing { get; set; }

        /// <summary>
        /// 待验收数
        /// </summary>
        public decimal ToAccepted { get; set; }
    }
}
