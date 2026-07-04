using SIE.Api;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain.Validation;
using SIE.MES.WIP.ApiModels;
using SIE.MES.WIP.Moves;
using SIE.MES.WIP.TaskExtensions;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using SIE.Utils;
using System;
using System.Linq;

namespace SIE.MES.WIP.Inspects
{
    /// <summary>
    /// 检验采集控制器API
    /// </summary>
    public partial class InspectController : WipController
    { 
        /// <summary>
        /// 获取验证检验采集返回信息
        /// </summary>
        /// <param name="queryInfo">检验采集查询信息</param>
        /// <returns>验证检验采集返回信息</returns>
        [ApiService("验证检验采集")]
        [return: ApiReturn("验证检验采集 ValidateInspect")]
        public virtual RstInspValidateInfo ValidateInspect([ApiParameter("检验采集查询信息")] InspectQueryInfo queryInfo)
        {
            RT.Service.Resolve<MoveController>().ValidateWipQueryInfo(queryInfo);
            var rstInspInfo = new RstInspValidateInfo();
            if (queryInfo.IsSn)
            {
                var collectBarcode = new CollectBarcode { Code = queryInfo.Sn, Type = BarcodeType.SN };
                var workcell = new Workcell() { EmployeeId = queryInfo.EmployeeId, ResourceId = queryInfo.ResourceId, ProcessId = queryInfo.ProcessId, StationId = queryInfo.StationId };
                var product = Validate(collectBarcode, workcell);
                var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(product.WorkOrderId);
                var wipLineWorkOrderEntity = GetWipResourceWorkOrder(workcell);
                if (wipLineWorkOrderEntity == null || wipLineWorkOrderEntity.WorkOrderId != product.WorkOrderId)
                {
                    ChangeWipResourceWorkOrder(wo.Id, workcell);
                    //RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                    //RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(wo.Id);
                }

                rstInspInfo.RstWipInfo = new RstWipInfo()
                {
                    Sn = queryInfo.Sn,
                    WorkOrderNo = wo.No,
                    ProductCode = wo.ProductCode,
                    ProductName = wo.ProductName,
                    ProductModel = wo.ProductModelName
                };
            }
            else
            {
                if (!queryInfo.Sn.IsNotEmpty())
                    throw new ValidationException("缺陷代码不能为空！".L10N());
                var defect = RT.Service.Resolve<DefectController>().GetDefect(queryInfo.Sn);
                if (defect == null)
                    throw new ValidationException("缺陷不存在！".L10N());
                rstInspInfo.DefectData = new DefectData()
                {
                    DefectId = defect.Id,
                    DefectName = defect.Description,
                    CategoryId = defect.DefectCategoryId,
                    CategoryName = defect.DefectCategory?.Description
                };
            }

            return rstInspInfo;
        }

        /// <summary>
        /// 获取提交检验采集返回信息
        /// </summary>
        /// <param name="sumbitInfo">检验采集提交信息</param>
        /// <returns>提交检验采集返回信息</returns>
        [ApiService("提交检验采集")]
        [return: ApiReturn("提交检验采集 InspectSubmit")]
        public virtual RstInspectInfo InspectSubmit([ApiParameter("检验采集提交信息")] InspectSumbitInfo sumbitInfo)
        {
            RT.Service.Resolve<MoveController>().ValidateWipQueryInfo(sumbitInfo);
            var collectData = new CollectData();
            collectData.CollectBarcode = new CollectBarcode { Code = sumbitInfo.Sn, Type = BarcodeType.SN };
            var workcell = new Workcell() { EmployeeId = sumbitInfo.EmployeeId, ResourceId = sumbitInfo.ResourceId, ProcessId = sumbitInfo.ProcessId, StationId = sumbitInfo.StationId };
            ValidateDefects(sumbitInfo);
            collectData.Result = sumbitInfo.DefectDatas.Count > 0 ? ResultType.Fail : ResultType.Pass;
            collectData.Defects.AddRange(sumbitInfo.DefectDatas);
            RT.Service.Resolve<InspectController>().Collect(new string[] { sumbitInfo.Sn }, collectData, workcell);
            return new RstInspectInfo() { ResultType = EnumViewModel.EnumToLabel(collectData.Result).L10N() };
        }

        /// <summary>
        /// 验证缺陷代码是否合法
        /// </summary>
        /// <param name="sumbitInfo">检验采集提交信息</param>
        private void ValidateDefects(InspectSumbitInfo sumbitInfo)
        {
            var defectIds = sumbitInfo.DefectDatas.Select(p => p.DefectId).Distinct().ToList();
            var defects = RT.Service.Resolve<DefectController>().GetDefectList(defectIds);
            var dicDefects = defects.ToDictionary(p => p.Id);
            foreach (var defectData in sumbitInfo.DefectDatas)
            {
                if (!dicDefects.TryGetValue(defectData.DefectId, out Defect defect))
                    throw new ValidationException("缺陷Id[{0}]不存在！".L10nFormat(defectData.DefectId));
                if (defectData.DefectName != defect.Description)
                    throw new ValidationException("输入缺陷代码[{0}]与数据库缺陷代码不一致！".L10nFormat(defectData.DefectName, defect.Description));
                if (defectData.CategoryId != defect.DefectCategoryId)
                    throw new ValidationException("缺陷代码[{0}]的输入缺陷分类Id[{1}]与数据库缺陷分类Id[{2}]不一致！".L10nFormat(defect.Description, defectData.CategoryId, defect.DefectCategoryId));
                if (defectData.CategoryName != defect.DefectCategory?.Description)
                    throw new ValidationException("缺陷代码[{0}]的输入缺陷分类[{1}]与数据库缺陷分类[{2}]不一致！".L10nFormat(defect.Description, defectData.CategoryName, defect.DefectCategory?.Description));
                if (defectData.Qty != 1)
                    throw new ValidationException("缺陷代码[{0}]的输入不良数量必须等于1！".L10nFormat(defectData.DefectId));
            }
        }
    }
}
