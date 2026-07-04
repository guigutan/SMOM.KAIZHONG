using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.Gantt.Areas.SchedulGantt.ViewModel
{
    /// <summary>
    /// 配置实体类
    /// </summary>
    public class ConfigViewModel
    {
        /// <summary>
        /// 甘特图显示范围起始时间
        /// </summary>
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 甘特图显示范围结束时间
        /// </summary>
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
    }
}
