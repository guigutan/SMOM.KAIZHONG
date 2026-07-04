using DocumentFormat.OpenXml.Office2010.Excel;
using SIE.Api;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.ESop.Configs;
using SIE.ESop.Displays;
using SIE.ESop.Displays.Configs;
using SIE.ESop.Documents;
using SIE.ESop.EngDocuments.Services;
using SIE.ManagedProperty;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.ForWinform
{
    /// <summary>
    /// 
    /// </summary>
    public class WinESopController : DisplayPointController
    {
        /// <summary>
        /// 获取文档接口
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="workOrderId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [ApiService("EOSP-获取文档接口")]
        [return: ApiReturn("EOSP-文档集合")]
        public virtual List<Document> GetDocuments([ApiParameter("工序Id")] double? processId, [ApiParameter("工单Id")] double? workOrderId, [ApiParameter("物料Id")] double? itemId)
        {
            var DataFromConfig = ConfigService.GetConfig(new DisplayPointDataConfig(), typeof(DisplayPoint));
            EntityList<Document> data = new EntityList<Document>();
            if (DataFromConfig.DataFrom == SIE.ESop.Displays.Enums.DisplayDataSource.Document)
            {
                data = RT.Service.Resolve<DocumentCollectionController>().GetList(new DocumentCriteria() { ProcessId = processId, WorkOrderId = workOrderId });
                if (data.Count == 0)
                    data = RT.Service.Resolve<DocumentCollectionController>().GetList(new DocumentCriteria() { ItemId = itemId, ProcessId = processId });
            }
            else
            {
                data = RT.Service.Resolve<EngDocumentDetailService>().GetDocuments(workOrderId, itemId);
            }
            return data.ToList();
        }

        /// <summary>
        /// 获取现实点配置项
        /// </summary>
        /// <returns></returns>
        [ApiService("EOSP-获取显示点配置")]
        [return: ApiReturn("EOSP-获取显示点配置")]
        public virtual DisplayPointConfigValue DisplayPointConfigValue()
        {
            var config = ConfigService.GetConfig(new DisplayConfig(), typeof(DisplayPoint));
            if (config != null)
            {
                return config;
            }
            return null;
        }

        [ApiService("EOSP-检查用户工作站")]
        [return: ApiReturn("真/假")]
        public virtual bool CheckUserWorkStation([ApiParameter("用户Id")] double userId, [ApiParameter("资源Id")] double resourceId, [ApiParameter("显示点Id")] double displayPointId)
        {
            var ctlResource = RT.Service.Resolve<EmployeeController>();
            if (!ctlResource.UserHasResource(userId, resourceId))
                return false;

            var hasPoint = RT.Service.Resolve<DisplayPointController>().HasResourceDisplayPoint(resourceId, displayPointId);
            if (!hasPoint)
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        [ApiService("EOSP-根据用户ID获取生产资源")]
        [return: ApiReturn("生产资源集合")]

        public virtual List<WipResource> GetResourcesByUserId([ApiParameter("用户Id")] double userId, [ApiParameter("搜索关键字")] string keyword, [ApiParameter("显示点Id")] PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<WipResourceController>().GetResourcesByUserId(RT.IdentityId, keyword, pagingInfo).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>

        [ApiService("EOSP-根据生产资源获取显示点集合")]
        [return: ApiReturn("显示点集合")]
        public virtual List<DisplayPoint> GetDisplayPointList([ApiParameter("资源Id")] double resourceId, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<DisplayPointController>().GetDisplayPointList(resourceId, keyword, pagingInfo).ToList();
        }

        /// <summary>
        /// EOSP-获取显示点明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiService("EOSP-获取显示点明细信息")]
        [return: ApiReturn("返回显示点明细 包含工序列表")]
        public virtual DisplayPoint GetDisplayPointDetail([ApiParameter("资源Id")] double id)
        {
            return Query<DisplayPoint>().Where(m => m.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWith(DisplayPoint.DisplayPointProcessListProperty).LoadWithViewProperty());
        }
        /// <summary>
        /// 获取文档集
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiService("EOSP-获取显示点明细信息")]
        [return: ApiReturn("返回显示点明细 包含工序列表")]
        public virtual DocumentCollection GetDocumentCollection([ApiParameter("文档集合Id")] double id)
        {

            return Query<DocumentCollection>().Where(m => m.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
