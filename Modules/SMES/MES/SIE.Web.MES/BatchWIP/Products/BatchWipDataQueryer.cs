using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.Web.Data;
using System.Collections.Generic;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品数据查询
    /// </summary>
    public class BatchWipDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取批次工序出入站明细
        /// </summary>
        /// <param name="processId">批次工序ID</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>出入站明细列表</returns>
        public EntityList<BatchWipProductProcessDetail> GetBatchWipProductProcessInDetail(double processId, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductProcessInDetail(processId, pagingInfo);
        }

        /// <summary>
        /// 删除批次（未过站的）
        /// </summary>
        /// <param name="batchIds"></param>
        public void DeleteWipBatch(List<double> batchIds)
        {
            RT.Service.Resolve<BatchManageController>().DeleteWipBatchList(batchIds);
        }

        /// <summary>
        /// 获取拆分数据去向
        /// </summary>
        /// <param name="sourceBatchNo">源批次号</param>
        /// <param name="resourceId">资源id</param>
        /// <param name="processId">工序id</param>
        /// <param name="stationId">工位id</param>
        /// <returns></returns>
        public EntityList<BatchWipSplitViewModel> GetSplitSourceDatas(string sourceBatchNo, double resourceId, double processId, double stationId)
        {
            return RT.Service.Resolve<BatchManageController>().GetSplitSourceDatas(sourceBatchNo, resourceId, processId, stationId);
        }
    }
}