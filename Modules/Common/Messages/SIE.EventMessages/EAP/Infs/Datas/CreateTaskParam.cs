using Newtonsoft.Json;
using System.Collections.Generic;

namespace SIE.EventMessages.EAP.Infs.Datas
{
    /// <summary>
    /// 创建任务参数
    /// </summary>
    public class CreateTaskParam
    {
        /// <summary>
        /// 载具ID列表
        /// </summary>
        [JsonProperty("carrierId")]
        public List<string> CarrierId { get; set; }

        /// <summary>
        /// 载具类型
        /// </summary>
        [JsonProperty("carrierType")]
        public string CarrierType { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        [JsonProperty("jobGroupId")]
        public string JobGroupId { get; set; }

        /// <summary>
        /// 任务优先级 从1到5，默认传1，5优先级最高
        /// </summary>
        [JsonProperty("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// 初始位置编号
        /// </summary>
        [JsonProperty("src")]
        public string Src { get; set; }

        /// <summary>
        /// 初始位置类型 点位置类型为location,区位置类型为group
        /// </summary>
        [JsonProperty("srcType")]
        public string SrcType { get; set; }

        /// <summary>
        /// 目标位置编号
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }

        /// <summary>
        /// 目标位置类型 点位置类型为location,区位置类型为group
        /// </summary>
        [JsonProperty("targetType")]
        public string TargetType { get; set; }

        /// <summary>
        /// 任务类型 1普通类型，2特殊类型
        /// </summary>
        [JsonProperty("taskType")]
        public int TaskType { get; set; }

        /// <summary>
        /// 任务创建者 由WMS系统或MES系统提供
        /// </summary>
        [JsonProperty("taskUser")]
        public string TaskUser { get; set; }

    }
}
