using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 员工信息
    /// </summary>
    [Serializable]
    public class EmployeeData
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string PERNR { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ENAME { get; set; }

        /// <summary>
        /// 性别(1:男;2:女)
        /// </summary>
        public string GESCH { get; set; }

        /// <summary>
        /// 员工状态(在职:3;离职:0、1、2)
        /// </summary>
        public string STAT2 { get; set; }

        /// <summary>
        /// 一级组织
        /// </summary>
        public string ZYJZXID { get; set; }

        /// <summary>
        /// 二级组织
        /// </summary>
        public string ZEJBMID { get; set; }

        /// <summary>
        /// 三级组织
        /// </summary>
        public string ZSJBZID { get; set; }

        /// <summary>
        /// 四级组织
        /// </summary>
        public string ZSJXBID { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string TELNR { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string EMAIL { get; set; }

    }
}
