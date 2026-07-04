using Castle.Core;
using DocumentFormat.OpenXml;
using SIE.Domain;
using SIE.Items;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MES.Projects;
using SIE.MES.Projects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ViewModels
{
    /// <summary>
    /// 工艺卡控制器
    /// </summary>
    public class ProjectCardController : DomainController
    {
        /// <summary>
        /// 工艺卡-查询基础资料
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="orderInfos">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<ProjectDesignCardProperty> GetCardPropertys(double? projectId, double productId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            if (projectId == null)
            {
                return new EntityList<ProjectDesignCardProperty>();
            }

            EntityList<ProjectDesignCardProperty> projectDesignCardProperties = new EntityList<ProjectDesignCardProperty>();
            var q = Query<DesignBasicProperty>().Join<ProjectDesignDetail>((dbp, pd) => dbp.ProjectDesignId == pd.Id && pd.ProjectMaintainId == projectId && pd.ProductId == productId)
                .Select(dbp => new
                {
                    PropertyName = dbp.BasicProperty,
                    PropertyValue = dbp.BasicProValue,
                    PropertyUnit = dbp.BasicProUnit,
                });
            var count = q.Count();
            var list = q.OrderBy(orderInfos).ToList<ProjectDesignCardProperty>(pagingInfo);
            projectDesignCardProperties.AddRange(list);
            projectDesignCardProperties.SetTotalCount(count);
            return projectDesignCardProperties;
        }

        /// <summary>
        /// 获取项目号参数信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="orderInfos">排序信息</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public virtual EntityList<ProjectDesignCardParamter> GetCardParamters(double? projectId, double productId, double processId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            if (projectId == null)
            {
                return new EntityList<ProjectDesignCardParamter>();
            }

            EntityList<ProjectDesignCardParamter> projectDesignCardParamters = new EntityList<ProjectDesignCardParamter>();
            var q = Query<DesignTreeRoutingParamer>().Join<DesignTreeRouting>((dtrp, dtr) => dtrp.DesignTreeRoutingId == dtr.Id && dtr.TreeLevel == 1)
                .LeftJoin<DesignTreeRouting, ProjectDesignDetail>((dtr, pd) => dtr.ProjectDesignId == pd.Id)
                .LeftJoin<ProjectParam>((dtrp, pp) => dtrp.ProjectParamId == pp.Id)
                .Where<ProjectDesignDetail>((dtrp, pd) => pd.ProjectMaintainId == projectId && pd.ProductId == productId)
                .Where(dtrp => dtrp.ProcessId == processId)
                .Select<ProjectParam>((dtrp, pp) => new
                {
                    ParameterCode = pp.Code,
                    ParameterName = pp.Name,
                    ParameterType = pp.Type,
                    ProcessStDtlValueType = dtrp.ProcessStDtlValueType,
                    Unit = dtrp.Unit,
                    SingleValue = dtrp.SingleValue,
                    RangeMaxValue = dtrp.RangeMaxValue,
                    RangeMinValue = dtrp.RangeMinValue,
                });
            var count = q.Count();
            var list = q.OrderBy(orderInfos).ToList<ProjectDesignCardParamter>(pagingInfo);
            projectDesignCardParamters.AddRange(list);
            projectDesignCardParamters.SetTotalCount(count);
            return projectDesignCardParamters;
        }

        /// <summary>
        /// 获取项目号需求设计附件
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="productId"></param>
        /// <param name="orderInfos"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<DesignTreeDocument> GetDesignTreeDocuments(double? projectId, double productId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            if (projectId == null)
            {
                return new EntityList<DesignTreeDocument>();
            }

            EntityList<DesignTreeDocument> designTreeDocuments = new EntityList<DesignTreeDocument>();
            var q = Query<DesignTreeDocument>()
                .Join<ProjectDesignDetail>((dtd, pd) => dtd.ProjectDesignId == pd.Id && pd.ProjectMaintainId == projectId && pd.ProductId == productId)
                .OrderBy(orderInfos).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return q;
        }
    }
}
