using SIE.Api;
using SIE.LES.BulletinBoards.MaterialPulls.APIModels;
using SIE.LES.BulletinBoards.MaterialPulls.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.BulletinBoards.MaterialPulls.Controllers
{
    /// <summary>
    /// 线边库存水位看板控制器
    /// </summary>
    public class InventoryStageController : DomainController
    {
        /// <summary>
        /// 获取推式拉式水位信息
        /// </summary>
        /// <param name="resourceIds">资源ids</param>
        /// <param name="rate">安全水位预警比例</param>
        /// <returns></returns>
        [ApiService("获取推式拉式水位信息")]
        [return: ApiReturn("返回水位信息列表: List<InventoryStageInfo> ")]
        public virtual List<InventoryStageInfo> GetInventoryStageList([ApiParameter("资源ids")] List<double?> resourceIds, [ApiParameter("安全水位预警比例")] decimal? rate)
        {
            return RT.Service.Resolve<InventoryStageService>().GetInventoryStageList(resourceIds, rate);
        }
    }
}
