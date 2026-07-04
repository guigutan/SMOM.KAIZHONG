using Newtonsoft.Json;
using System.Collections.Generic;

namespace SIE.EventMessages.EAP.Infs.Datas
{
    /// <summary>
    /// 取消任务参数
    /// </summary>
    public class CancelTaskParam
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        [JsonProperty("jobGroupId")]
        public List<string> JobGroupId { get; set; }
    }
}
