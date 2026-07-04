using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 保养确认提交实体数据
    /// </summary>
    [Serializable]
    public class MaintainConfirmationSubmitInfo
    {
        /// <summary>
        /// 保养计划ID
        /// </summary>
        public double? MaintainPlanId { get; set; }

        /// <summary>
        /// 评分项ID
        /// </summary>
        public double? TpmScoreProjectId { get; set; }

        /// <summary>
        /// 评分(1到5分)
        /// </summary>
        public double? Score { get; set; }

        /// <summary>
        /// 确认部门ID
        /// </summary>
        public double? ConfirmDeptId { get; set; }

        /// <summary>
        /// 确认结果(1:OK合格,2:NG不合格)
        /// </summary>
        public double? ConfirmResult { get; set; }

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 图片内容(图片转成base64的字符串)
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 图片扩展名
        /// </summary>
        public string FileExtesion { get; set; }

        /// <summary>
        /// 图片大小
        /// </summary>
        public string FileSize { get; set; }

    }
}
