using SIE.Domain;
using SIE.MES.LoadItems;
using System;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 倒扣料帮助类
    /// </summary>
    public static class BackflushMaterialHelper
    {
        /// <summary>
        /// 批量获取扣料需求单号
        /// </summary>
        /// <param name="deductItems"></param>
        public static void BatchSetCostNos(EntityList<WoCostItem> deductItems)
        {
            var costNoIsEmptys = deductItems.Where(x => x.CostNo.IsNullOrEmpty()).ToList();

            if (!costNoIsEmptys.Any())
            {
                return;
            }

            var costNos = RT.Service.Resolve<WoCostItemController>().GetCostNoRule(costNoIsEmptys.Count).ToArray();
            for (int i = 0; i < costNoIsEmptys.Count; i++)
            {
                costNoIsEmptys[i].CostNo = costNos[i];
            }
        }

    }
}
