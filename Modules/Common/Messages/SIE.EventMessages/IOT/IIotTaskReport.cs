using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.IOT
{
    /// <summary>
    /// IOT报工接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIIotTaskReport))]
    public interface IIotTaskReport
    {
        /// <summary>
        /// 设置IOT设备初始任务信息
        /// </summary>
        /// <param name="iotTaskInfo">任务信息</param>
        void SetIotTaskInfo(IotTaskInfo iotTaskInfo);
        /// <summary>
        /// 设置IOT设备初始任务信息
        /// </summary>
        /// <param name="iotTaskInfos">任务信息</param>
        /// <param name="resourceCode">资源编码</param>
        void SetIotTaskInfos(List<IotTaskInfo> iotTaskInfos, string resourceCode);

        /// <summary>
        /// 获取IOT设备任务信息
        /// </summary>
        /// <param name="iotTaskInfo">任务信息</param>
        IotTaskInfo GetIotTaskInfo(IotTaskInfo iotTaskInfo);

        /// <summary>
        /// 获取IOT设备任务信息
        /// </summary>
        /// <param name="resourceCode">资源编码</param>
        /// <returns></returns>
        List<IotTaskInfo> GetIotTaskInfos(string resourceCode);
    }

    class DefaultIIotTaskReport : IIotTaskReport
    {

        /// <summary>
        /// 设置IOT设备初始任务信息
        /// </summary>
        /// <param name="iotTaskInfo">任务信息</param>
        public void SetIotTaskInfo(IotTaskInfo iotTaskInfo)
        {

        }

        /// <summary>
        /// 设置IOT设备初始任务信息
        /// </summary>
        /// <param name="iotTaskInfos">任务信息</param>
        /// <param name="resourceCode">资源编码</param>
        public void SetIotTaskInfos(List<IotTaskInfo> iotTaskInfos, string resourceCode)
        {

        }
        /// <summary>
        /// 获取IOT设备任务信息
        /// </summary>
        /// <param name="iotTaskInfo">任务信息</param>
        public IotTaskInfo GetIotTaskInfo(IotTaskInfo iotTaskInfo)
        {
            return null;
        }

        /// <summary>
        /// 获取IOT设备任务信息
        /// </summary>
        /// <param name="resourceCode">资源编码</param>
        public List<IotTaskInfo> GetIotTaskInfos(string resourceCode)
        {
            return null;
        }
    }

    /// <summary>
    /// IOT任务信息
    /// </summary>
    [Serializable]
    public class IotTaskInfo
    {
        /// <summary>
        /// 任务单号
        /// </summary>
        public string TaskNo { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 初始产量
        /// </summary>
        public decimal InitQty { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal OutPutQty { get; set; }

    }

}
