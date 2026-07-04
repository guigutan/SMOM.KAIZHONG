using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.Gantt.Areas.SchedulGantt.ViewModel
{

    /// <summary>
    /// 计划与生产资源关系
    /// </summary>
    public class AssignmentModel
    {
        /// <summary>
        /// 计划关系
        /// </summary>
        public AssignmentModel()
        {
            Rows = new List<WipAssignmentModel>();
        }

        /// <summary>
        /// 列表
        /// </summary>
        [JsonProperty("rows")]
        public List<WipAssignmentModel> Rows { get; }
    }
    /// <summary>
    /// 实体
    /// </summary>
    public class WipAssignmentModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 计划ID
        /// </summary>
        [JsonProperty("event")]
        public string EventId { get; set; }

        /// <summary>
        /// 生产资源ID
        /// </summary>
        [JsonProperty("resource")]
        public double ResourceId { get; set; }

    }
}
