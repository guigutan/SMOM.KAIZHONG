using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Resources.WipResources
{
    /// <summary>
    /// 生产资源控制器
    /// </summary>
    public class WipResourceTestController : DomainController
    {
        public virtual EntityList<WipResource> GetWipResource(List<string> codes)
        {
            return Query<WipResource>().Where(p => codes.Contains(p.Code)).ToList();
        }

        /// <summary>
        /// 获取一个有所属车间的资源(没有时新建)
        /// </summary>
        /// <returns>资源</returns>
        public virtual WipResource GetFirstWipResource()
        {
            var wipResource = Query<WipResource>().Where(p => p.WorkShopId != null).OrderByDescending(p => p.Id).FirstOrDefault();
            if (wipResource == null)
            {
                var enterprises = RT.Service.Resolve<ResTestController>().CreateEnterprises(new List<EnterpriseType> { EnterpriseType.Shop, EnterpriseType.Line });
                RT.Service.Resolve<WipResourceController>().RunSync();
                var resource = enterprises.FirstOrDefault(p => p.Level.Type == EnterpriseType.Line);
                var wipResources = RT.Service.Resolve<WipResourceTestController>().GetWipResource(new List<string> { resource.Code });
                wipResource = wipResources.FirstOrDefault();
            }
            return wipResource;
        }
    }
}