using SIE.Common.Attachments;
using System;
using System.Collections.Generic;

namespace SIE.EMS.WorkFlow
{
    /// <summary>
    /// 工作流通用表单
    /// </summary>
    [Serializable]
    public class WorkFlowForm
    {
        /// <summary>
        /// 表格键值对集合
        /// </summary>
        public List<WorkFlowFormField> FieldList { get; set; }

        /// <summary>
        /// 发起人,默认为当前操作人
        /// </summary>
        public double? StarterId { get; set; }

        /// <summary>
        /// 工作流定义ID
        /// </summary>
        public double FlowDefinitionId { get; set; }

        /// <summary>
        /// 自定义列表
        /// </summary>
        public List<WorkFlowCustomList> WorkFlowCustomLists { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public List<WorkFlowAttachmentInfoModel> Attachments { get; set; }
    }
}
