using System;
using System.Collections.Generic;

namespace SIE.EMS.WorkFlow
{
    /// <summary>
    /// 表单属性
    /// </summary>
    [Serializable]
    public class WorkFlowFormField
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 工作流通用表单自定义列表字段
    /// </summary>
    [Serializable]
    public class WorkFlowCutomListField
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string FieldLabel { get; set; }

        /// <summary>
        /// 栏位宽度
        /// </summary>
        public uint FieldWidth { get; set; }
    }

    /// <summary>
    /// 工作流通用表单自定义列表值
    /// </summary>
    [Serializable]
    public class WorkFlowCutomListValue
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string FieldValue { get; set; }
    }

    /// <summary>
    /// 工作流通用表单的子表
    /// </summary>
    [Serializable]
    public class WorkFlowCustomList
    {
        /// <summary>
        /// 列表名称
        /// </summary>
        public string ListName { get; set; }

        /// <summary>
        /// 自定义列表字段名列表
        /// </summary>
        public List<WorkFlowCutomListField> CutomListFields { get; set; }

        /// <summary>
        /// 自定义列表值列表
        /// </summary>
        public List<List<WorkFlowCutomListValue>> CutomListValueList { get; set; }
    }
}
