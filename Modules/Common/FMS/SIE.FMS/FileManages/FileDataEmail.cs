using System;
using System.Collections.Generic;

namespace SIE.FMS
{
    /// <summary>
    /// 邮件文件数据
    /// </summary>
    [Serializable]
    public class FileDataEmail
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        public List<FileData> FileDatas
        {
            get;set;
        }

        /// <summary>
        /// 员工Id
        /// </summary>
        public List<double> EmployeeIds
        {
            get;set;
        }

        /// <summary>
        /// 邮件类型 0-发布 1-驳回
        /// </summary>
        public int EmailType
        {
            get;set;
        }

        /// <summary>
        /// 驳回原因
        /// </summary>
        public string ReturnReason
        {
            get;set;
        }

        /// <summary>
        /// 审核链接
        /// </summary>
        public string AuditUrl
        {
            get;set;
        }
    }
}
