using System;
using System.Collections.Generic;

namespace SIE.Equipments.DeviceControls.ApiModels
{
    /// <summary>
    /// 设备开停机信息
    /// </summary>
    [Serializable]
    public class DeviceStateInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 状态来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public bool IsStop { get; set; }
    }

    /// <summary>
    /// 停开机控制信息
    /// </summary>
    [Serializable]
    public class DeviceStopInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 状态来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public bool IsStop { get; set; }
    }

    /// <summary>
    /// MDC停开机接口参数
    /// </summary>
    [Serializable]
    public class RpcParaInfo
    {
        /// <summary>
        /// 停开机参数
        /// </summary>
        public RpcPara RpcPara { get; set; }
    }
    /// <summary>
    /// MDC停开机接口参数
    /// </summary>
    [Serializable]
    public class RpcPara
    {
        /// <summary>
        /// 接口方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 设备列表
        /// </summary>
        public List<string> Paras { get; set; }
    }

    /// <summary>
    /// MDC停开机接口返回对象
    /// </summary>
    public class MdcReturn
    {
        /// <summary>
        /// 设备停开机结果
        /// </summary>
        public MdcReturnData Data { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 请求ID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 接口总结果
        /// </summary>
        public bool? IsSuccess { get; set; }
    }

    /// <summary>
    /// MDC停开机接口返回对象
    /// </summary>
    public class MdcReturnData
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }
    }
}

