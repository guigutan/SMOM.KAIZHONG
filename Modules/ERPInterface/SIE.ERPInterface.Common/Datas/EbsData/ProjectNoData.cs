using SIE.Core.ProjectMaintains;
using System;

namespace SIE.ERPInterface.Common.Datas.EbsData
{
    /// <summary>
    /// 项目号
    /// </summary>
    [Serializable]
    public class ProjectNoData : EbsDataBase
    {                
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project_Name { get; set; }

        /// <summary>
        /// 项目代码
        /// </summary>
        public string Project_Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
              
        /// <summary>
        /// 启用标志
        /// </summary>
        public string Enable_Flag { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public ProjectMaintain? ProjectMaintain { get; set; }
    }
}
