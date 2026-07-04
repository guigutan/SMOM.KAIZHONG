using SIE.Api;
using SIE.LES.BulletinBoards.MaterialPulls.APIModels;
using SIE.LES.BulletinBoards.MaterialPulls.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.BulletinBoards.MaterialPulls.Controllers
{
    /// <summary>
    /// 物料拉动(仓库)控制器
    /// </summary>
    public class MaterialPullWareController : DomainController
    {
        /// <summary>
        /// 获取物料拉通(仓库)看板基本数据
        /// </summary>
        /// <param name="workShopIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        [ApiService("获取物料拉通(仓库)看板基本数据")]
        [return: ApiReturn("返回物料拉通(仓库)看板基本数据:List<MaterialPullWareInfo>")]
        public virtual List<MaterialPullWareInfo> GetMaterialPullWareInfos([ApiParameter]List<double?> workShopIds, [ApiParameter]List<double?> resourceIds)
        {
            return RT.Service.Resolve<MaterialPullWareService>().GetMaterialPullWareInfos(workShopIds, resourceIds);
        }
    }
}
