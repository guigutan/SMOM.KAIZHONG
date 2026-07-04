using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OrgLevels
{

    /// <summary>
    /// 人员组织架构信息属性
    /// </summary>
    [Serializable]
    public class OrgLevelInfo
    {
        /// <summary>
        /// 组织架构代码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织架构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 上级组织
        /// </summary>
        public string ParentLevel { get; set; }
        /// <summary>
        /// 组织层级
        /// </summary>
        public string TheLevel { get; set; }
    }




    /// <summary>
    /// 接口调用后结果返回信息
    /// </summary>
    [Serializable]
    public class SetOrgLevelInfoRes
    {       
        /// <summary>
        /// 组织架构ID
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 组织架构名称
        /// </summary>
        public string OrgName { get; set; }


        /// <summary>
        /// 上级组织
        /// </summary>
        public string ParentLevel { get; set; }

        /// <summary>
        /// 组织层级
        /// </summary>
        public string TheLevel { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMsg { get; set; }

    }

}
