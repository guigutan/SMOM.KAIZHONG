using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts.ApiModels
{

    /// <summary>
    /// 项目信息
    /// </summary>
    [Serializable]
    public class ProjectInfo
    {
        /// <summary>
        /// 记录明细Id
        /// </summary>
        public double RecordDetailId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>

        public double? ProjectId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName { get; set; }


        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 父级记录的Id
        /// </summary>
        public double PrepareRecordId { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }

       /// <summary>
       /// 项目类型
       /// </summary>
        public int? ProjectType { get; set; }
    }

    /// <summary>
    /// 产前准备信息
    /// </summary>
    public class ProjectCheckInfo
    {
        /// <summary>
        /// 产前准备信息
        /// </summary>
        public ProjectCheckInfo()
        {
            this.ProjectInfos = new List<ProjectInfo>();
        }
        /// <summary>
        /// 单据头
        /// </summary>
        public PrepareProductsBill PrepareProductsBill { get; set; }

        /// <summary>
        /// 项目信息
        /// </summary>
        public List<ProjectInfo> ProjectInfos { get; set; }
    }
}
