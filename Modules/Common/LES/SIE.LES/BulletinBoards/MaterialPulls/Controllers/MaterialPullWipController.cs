using SIE.Api;
using SIE.Common.Sender;
using SIE.LES.BulletinBoards.MaterialPulls.APIModels;
using SIE.LES.BulletinBoards.MaterialPulls.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.BulletinBoards.MaterialPulls.Controllers
{
    /// <summary>
    /// 物料拉动看板（生产）控制器
    /// </summary>
    public class MaterialPullWipController : DomainController
    {
        /// <summary>
        /// 获取物料拉通(生产)看板基本数据
        /// </summary>
        /// <param name="workShopIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        [ApiService("获取物料拉通(生产)看板基本数据")]
        [return: ApiReturn("返回物料拉通(生产)看板基本数据:List<MaterialPullWipInfo>")]
        public virtual List<MaterialPullWipInfo> GetMaterialPullWipInfos([ApiParameter] List<double?> workShopIds, [ApiParameter] List<double?> resourceIds)
        {
            return RT.Service.Resolve<MaterialPullWipService>().GetMaterialPullWipInfos(workShopIds, resourceIds);
        }
    }
}
