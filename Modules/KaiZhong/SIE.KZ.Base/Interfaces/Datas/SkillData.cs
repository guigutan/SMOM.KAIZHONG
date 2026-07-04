using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 人员技能信息
    /// </summary>
    [Serializable]
    public class SkillData
    {
        /// <summary>
        /// 员工号
        /// </summary>
        public string PERNR { get; set; }

        /// <summary>
        /// 技能名称(岗位名称)
        /// </summary>
        public string ZJNMC { get; set; }
    }
}
