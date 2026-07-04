using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.Gantt.Areas.SchedulGantt.ViewModel
{
    /// <summary>
    /// 返回实体类
    /// </summary>
    public class GanttReturnViewModel
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        [JsonProperty("success")]
        public string Success { get; set; }

        /// <summary>
        /// 请求是否成功
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }


        /// <summary>
        /// 基础数据配置
        /// </summary>
        [JsonProperty("project")]
        public ConfigViewModel Project { get; set; }
       
    }
}