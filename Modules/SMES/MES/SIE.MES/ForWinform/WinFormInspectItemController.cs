using Newtonsoft.Json;
using SIE.Api;
using SIE.Domain;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.WIP;
using SIE.MES.WIP.Inspects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{

    /// <summary>
    /// 信创检验采集控制器
    /// </summary>
    public class WinFormInspectItemController : InspectByItemController
    {

        /// <summary>
        /// 根据产品或产品机型(优先产品)和工序获取检验项目
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        [ApiService("根据产品或产品机型(优先产品)和工序获取检验项目")]
        [return: ApiReturn("返回产品或产品机型(优先产品)和工序获取检验项目")]
        public virtual List<XPModelInspectionItem> GetInspectionItemsForWin([ApiParameter("物料ID")] double itemId, [ApiParameter("工序ID")] double processId)
        {
            List<XPModelInspectionItem> XPResult = new List<XPModelInspectionItem>();
            var result = this.GetInspectionItems(itemId, processId);
            if (result.Any())
            {

                var json = JsonConvert.SerializeObject(result.ToList());
                XPResult = JsonConvert.DeserializeObject<List<XPModelInspectionItem>>(json);
            }
            return XPResult;
        }

        /// <summary>
        /// 检验项目数据采集
        /// </summary>
        /// <param name="barcodes">条码数组</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param> 
        /// <exception cref="ArgumentNullException">条码数组为空、采集数据为空、工作单元为空</exception>

        [ApiService("检验项目数据采集")]
        [return: ApiReturn("无返回")]
        public virtual void InspectionItemCollect([ApiParameter("条码数组")] string[] barcodes, [ApiParameter("采集数据")] CollectData collectData, [ApiParameter("工作单元")] Workcell workcell)
        {
            this.Collect(barcodes, collectData, workcell);
        }
    }
}
