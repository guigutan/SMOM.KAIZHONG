using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Datas
{
    /// <summary>
    /// QT标准规则下的推送对象
    /// </summary>
    [Serializable]
    public class PushObjectStandard
    {
        /// <summary>
        /// 构造
        /// </summary>
        public PushObjectStandard()
        {
            PushObjectIds = new List<double>();
            UserGroupIds = new List<double>();
            WorkGroupIds = new List<double>();
            RoleIds = new List<double>();
            DepartmentIds = new List<double>();
        }

        #region 属性
        /// <summary>
        /// QT标准Id
        /// </summary>
        public double StandardId { get; set; }

        /// <summary>
        /// 推送Id
        /// </summary>
        public List<double> PushObjectIds { get; set; }

        /// <summary>
        /// 用户组Ids
        /// </summary>
        public List<double> UserGroupIds { get; set; }

        /// <summary>
        /// 班组Ids
        /// </summary>
        public List<double> WorkGroupIds { get; set; }

        /// <summary>
        /// 角色Ids
        /// </summary>
        public List<double> RoleIds { get; set; }

        /// <summary>
        /// 部门Ids
        /// </summary>
        public List<double> DepartmentIds { get; set; }
        #endregion

    }
}
