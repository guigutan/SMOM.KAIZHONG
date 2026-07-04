using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Web.Data;

namespace SIE.Web.Resources.Enterprises.DataQuery
{
    /// <summary>
    /// 查询
    /// </summary>
    public class EnterpriseNodeQuery : DataQueryer
    {
        /// <summary>
        /// 返回节点数据
        /// </summary>
        /// <param name="itemId">父节点ID</param>
        /// <returns></returns>
        public EntityList<Enterprise> GetNodes(double itemId)
        {
            return RT.Service.Resolve<EnterpriseController>().GetSubEnterprise(itemId);
        }
    }
}
