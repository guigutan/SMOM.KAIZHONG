using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.WIP;
using SIE.MES.WIP.Inspects;
using SIE.MES.WIP.Runtime;
using System.Collections.Generic;

namespace SIE.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验采集控制器
    /// </summary>
    public class BatchInspectController : InspectController
    {
        /// <summary>
        /// 批次检验时保存不良信息
        /// </summary>
        /// <param name="wipProductProcess">批次采集记录</param>
        /// <param name="product">采集运行时产品模型</param>
        /// <param name="barcodes">采集条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="relationBatches">关联批次</param>
        protected override void OnBatchWipProductProcessFinished(BatchWipRecord wipProductProcess, product product,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell, EntityList<RelationBatch> relationBatches)
        {
            if (relationBatches == null || relationBatches.Count <= 0)
            {
                return;
            }
            if (wipProductProcess == null)
            {
                return;
            }
            foreach (var relationBatch in relationBatches)
            {
                if (relationBatch.BatchWipProductDefects.Count > 0)
                {
                    foreach (var batchWipPrdDft in relationBatch.BatchWipProductDefects)
                    {
                        batchWipPrdDft.Remark = string.Empty;
                        batchWipPrdDft.Version = wipProductProcess.BatchVersion;
                    }
                    wipProductProcess.BatchVersion.DefectList.AddRange(relationBatch.BatchWipProductDefects);
                }
            }
        }
    }
}