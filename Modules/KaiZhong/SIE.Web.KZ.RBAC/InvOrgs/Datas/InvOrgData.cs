using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.RBAC.InvOrgs.Datas
{
    /// <summary>
    /// 库存组织
    /// </summary>
    [Serializable]
    public class InvOrgData
    {

        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ExternalId
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

    }
}
