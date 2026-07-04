using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.LoadItems;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.BatchWIP.Assemlys
{
    /// <summary>
    /// 批次上料采集控制器
    /// </summary>
    public class BatchAssemblyController : AssemblyController
    {
        /// <summary>
        /// 过站数据收集
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">产品</param>
        /// <param name="barcodes">收集的条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="relationBatches">关联批次</param>
        protected override void OnBatchWipProductProcessFinished(BatchWipRecord wipProductProcess, product product,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell,EntityList<RelationBatch> relationBatches)
        {
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            if (collectData == null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (collectData.CollectBarcode?.AssemblyItems != null) //上料采集优先上料后过站
            {
                foreach (var assemblyItem in collectData.CollectBarcode?.AssemblyItems)
                {
                    RT.Service.Resolve<LoadItemController>().NewLoadItem(assemblyItem.Value, workcell);
                }
            }

            var outBatch = collectData.OutputBatch;

            BatchUseLoadItemExecutor batchUseLoadItemExecutor = new BatchUseLoadItemExecutor(product, workcell);
            batchUseLoadItemExecutor.UseLoadItem(outBatch, wipProductProcess);
            var keyItems = batchUseLoadItemExecutor.GetKeyItems();
            if (keyItems.Any() && wipProductProcess != null)
            {
                wipProductProcess.KeyItemList.AddRange(keyItems);
            }
        }
    }
}