using SIE.Domain;
using SIE.Items;
using SIE.MES.Routings.RoutingBoms;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.ViewModels;
using SIE.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.Routings.RoutingBoms.DataQueryers
{

    /// <summary>
    /// 工序bom执行查询器
    /// </summary>
    public class RoutingBomDetailDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取产品指定物料Bom的物料
        /// </summary>
        /// <param name="code">物料编码</param>
        /// <returns></returns>
        public Item GetItemByCode(string code)
        {
            return RT.Service.Resolve<ItemController>().GetItem(code);
        }

        /// <summary>
        /// 获取产品指定物料Bom的主物料
        /// </summary>
        /// <param name="productId">产品</param>
        /// <param name="itemId">物料</param>
        /// <param name="segmentId">工段</param>
        /// <returns></returns>
        public BomDetailViewModel GetProductBomDetailViewModel(double productId, double itemId, double? segmentId)
        {
            return RT.Service.Resolve<RoutingBomController>().GetRoutingBomDetailViewModel(productId, itemId, segmentId);
        }

        /// <summary>
        /// 获取工艺路线默认版本
        /// </summary>
        /// <param name="routingId">工艺路线</param>
        /// <returns></returns>
        public RoutingVersion GetDefaultRoutingVersion(double routingId)
        {
            return RT.Service.Resolve<RoutingController>().GetDefaultRoutingVersion(routingId);
        }

        /// <summary>
        /// 获取工艺路线默认版本工段等信息
        /// </summary>
        /// <param name="routingId">工艺路线</param>
        /// <param name="productId">产品ID</param>
        /// <returns></returns>
        public RoutingVersionInfo GetDefaultRoutingVersionInfo(double routingId, double productId)
        {
            var rv = RT.Service.Resolve<RoutingController>().GetDefaultRoutingVersion(routingId);
            var segments = RT.Service.Resolve<RoutingBomController>().GetProcessSegmentByProductRouting(routingId, productId, null, null);
            RoutingVersionInfo result = new RoutingVersionInfo();
            if (rv != null)
            {
                result.RoutingVersionId = rv.Id;
                result.RoutingVersionName = rv.Name;
            }
            if (segments.Count > 0)
            {
                result.ProccessSegmentId = segments[0].Id;
                result.ProccessSegmentCode = segments[0].Code;
            }
            return result;
        }

        /// <summary>
        /// 获取附件导入日志
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <returns></returns>
        public EntityList<RoutingBomDetail> GetBomImportRecordByAttachment(double attachmentId)
        {
            return RT.Service.Resolve<RoutingBomController>().GetBomImportRecordByAttachment(attachmentId, null, null);

        }

        /// <summary>
        /// 根据工序bom主表对应的工艺路线ID获取工序
        /// </summary>
        /// <param name="routingBomId">工序ID</param>
        /// <param name="routingVersionId">工艺路线版本ID</param>
        /// <returns>工序列表</returns>
        public virtual List<RoutingProcess> GetRoutingBomProcesses(double routingBomId, double routingVersionId)
        {
            var lstProc = RT.Service.Resolve<RoutingBomController>().GetRoutingProcessesExceptGroup(routingVersionId);
            List<RoutingProcess> lstResult = new List<RoutingProcess>();
            lstProc.ForEach(item =>
            {
                if (item.Type == ProcessType.Assembly || item.Type == ProcessType.Packing
                || item.Type == ProcessType.BatchAssembly||item.Type== ProcessType.BatchPacking)
                {
                    lstResult.Add(item);
                }
            });
            return lstResult;
        }
    }
}
