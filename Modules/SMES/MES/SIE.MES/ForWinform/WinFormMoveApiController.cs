using SIE.Api;
using SIE.Barcodes;
using SIE.Common.Attachments;
using SIE.Core.Items;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.Interfaces.TaskManages;
using SIE.MES.WIP;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Moves;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.TaskExtensions;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{

    /// <summary>
    /// 过站采集API
    /// </summary>
    public class WinFormMoveApiController : MoveController
    {

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="fileUrl">文件路径</param>
        [ApiService("下载附件")]
        [return: ApiReturn("下载附件")]
        public virtual XPAttach FileDownload([ApiParameter("文件路径")] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                throw new ArgumentException("要下载的文件不能为空".L10N());

            XPAttach result = new XPAttach();
            result.FileUrl = fileUrl;
            result.FileName = fileUrl.Split('/').LastOrDefault();
            result.Contents = RT.Service.Resolve<AttachmentController>().FileDownload(fileUrl, result.FileName);
            return result;
        }

        /// <summary>
        /// 判断员工是否存在该工序的技能
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="empId"></param>
        [ApiService("判断员工是否存在该工序的技能")]
        [return: ApiReturn("判断员工是否存在该工序的技能 IsEmpHasProcessSkill")]
        public virtual bool IsEmpHasProcessSkill([ApiParameter("工序Id")] double processId, [ApiParameter("员工Id")] double empId)
        {
            return RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(processId, empId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [ApiService("校验条码信息")]
        [return: ApiReturn("校验新条码信息")]
        public virtual decimal ValidateScanBarcode([ApiParameter("扫描条码")] string barcode, [ApiParameter("工单Id")] double workOrderId)
        {
            return RT.Service.Resolve<WipController>().ValidateNewBarcode(barcode, workOrderId);
        }

        /// <summary>
        /// 校验条码信息,返回产品信息和工单信息
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="woId">当前工单Id</param>
        /// <returns></returns>
        [ApiService("校验条码信息")]
        [return: ApiReturn("校验条码信息,返回产品信息和工单信息")]
        public virtual ValidateResult ValidateApiBarcode([ApiParameter("扫描条码")] CollectBarcode collectBarcode, [ApiParameter("工作单元")] Workcell workcell, [ApiParameter("当前工单Id")] double woId)
        {
            ApiModels.WorkOrderInfo resultWo = null;
            int? reportModel = -1;
            var product = this.Validate(collectBarcode, workcell);
            if (product.WorkOrderId != 0)
            {
                var workOrder = RF.GetById<WorkOrder>(product.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder != null && product.WorkOrderId != woId)
                {
                    resultWo = new ApiModels.WorkOrderInfo(workOrder);
                    this.ChangeWipResourceWorkOrder(workOrder.Id, workcell);
                    RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                    reportModel = UpdateWorkOrdeReportModel(product.WorkOrderId);
                }
            }
            ValidateTaskReport(product.WorkOrderId, workcell, reportModel);
            var result = new ValidateResult()
            {
                ProductInfo = product,
                WorkOrderInfo = resultWo
            };
            return result;
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        [ApiService("校验条码信息")]
        [return: ApiReturn("校验条码信息,无返回信息")]
        public virtual void ValidateScanApiBarcode([ApiParameter("扫描条码")] CollectBarcode barcode, [ApiParameter("工作单元")] Workcell workcell)
        {
            base.ValidateBarcode(barcode, workcell);
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="employeeId">当前员工</param>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="processId">工序Id</param>
        [ApiService("获取工序任务列表")]
        [return: ApiReturn("获取工序任务列表,返回工序任务列表")]
        public virtual List<ReportTaskViewModel> GetReportTasks([ApiParameter("当前员工")] double employeeId, [ApiParameter("追溯方式")] RetrospectType retrospectType, [ApiParameter("工序Id")] double? processId)
        {
            return RT.Service.Resolve<IWipTaskReport>().GetReportTasks(employeeId, retrospectType, processId ?? 0).ToList();
        }

        /// <summary>
        /// 采集过站
        /// </summary>
        /// <param name="barcodes"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        [ApiService("采集过站")]
        [return: ApiReturn("采集过站,无返回")]
        public virtual void CollectApi([ApiParameter("条码集合")] string[] barcodes, [ApiParameter("采集数据")] CollectData collectData, [ApiParameter("工作单元")] Workcell workcell)
        {
            base.Collect(barcodes, collectData, workcell);
        }

        /// <summary>
        /// 获取工序信息
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [ApiService("获取工序信息")]
        [return: ApiReturn("获取工序信息")]
        public virtual Process GetProcessInfo([ApiParameter("工序Id")] double processId)
        {
            return Query<Process>().Where(P => P.Id == processId).FirstOrDefault(new EagerLoadOptions().LoadWith(Process.CollectStepListProperty).LoadWith(Process.ParameterListProperty));
        }

        /// <summary>
        /// 获取工序的缺陷信息
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [ApiService("获取工序的缺陷信息")]
        [return: ApiReturn("获取工序的缺陷信息 返回缺陷列表")]
        public virtual List<Defect> GetProcessDefects([ApiParameter("工序Id")] double processId)
        {
            return RT.Service.Resolve<ProcessController>().GetProcessDefects(processId).ToList();
        }

        /// <summary>
        /// 获取缺陷所有分类
        /// </summary>
        /// <returns></returns>
        [ApiService("获取缺陷所有分类")]
        [return: ApiReturn("获取缺陷所有分类 返回缺陷分类列表")]
        public virtual List<DefectCategory> GetDefectCategory()
        {
            return RF.GetAll<DefectCategory>().ToList();
        }

        /// <summary>
        /// 获取所有缺陷
        /// </summary>
        /// <returns></returns>
        [ApiService("获取所有缺陷")]
        [return: ApiReturn("获取所有缺陷 返回缺陷列表")]
        public virtual List<Defect> GetAllDefects()
        {
            return RF.GetAll<Defect>().ToList();
        }

        /// <summary>
        /// 获取条码实体
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [ApiService("获取条码实体")]
        [return: ApiReturn("获取条码实体")]
        public virtual Barcode GetBarcode([ApiParameter("SN")] string sn)
        {
            return RT.Service.Resolve<BarcodeController>().GetBarcode(sn);
        }


        /// <summary>
        /// 获取条码打印外包装条码模板
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [ApiService("获取条码打印外包装条码模板")]
        [return: ApiReturn("获取条码打印外包装条码模板")]
        public virtual double GetBarcodeTemplateId([ApiParameter("SN")] string sn)
        {
            var barcode = RT.Service.Resolve<BarcodeController>().GetBarcode(sn);
            if (barcode == null)
            {
                return 0;
            }

            var packingTemplate = barcode.WorkOrder.Template?.PackingTemplate;
            return packingTemplate != null ? packingTemplate.Id : 0;
        }

        /// <summary>
        /// 获取条码实体
        /// </summary>
        /// <param name="barcodeIds"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [ApiService("获取条码实体")]
        [return: ApiReturn("获取条码实体")]
        public virtual void Reprint([ApiParameter("打印条码")] List<double> barcodeIds, [ApiParameter("打印日志")] string log = "打印外标签")
        {
            var barcodes = Query<Barcode>().Where(p => barcodeIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (barcodes.Any())
            {
                RT.Service.Resolve<BarcodeController>().Reprint(barcodes, BarcodeLogType.OutBox, log, 1);
            }
        }

        /// <summary>
        /// 获取并删除拼板码已绑定待打印的条码列表
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="panelCode">拼版码</param>
        /// <returns></returns>
        [ApiService("获取并删除拼板码已绑定待打印的条码列表")]
        [return: ApiReturn("获取并删除拼板码已绑定待打印的条码列表")]
        public virtual List<Barcode> GetAndDeleteToBePrintedSnList([ApiParameter("工单Id")] double workOrderId, [ApiParameter("拼版码")] string panelCode)
        {
            var result = RT.Service.Resolve<WipProductVersionController>().GetAndDeleteToBePrintedSnList(workOrderId, panelCode);
            if (result.Any())
            {
                return result.ToList();
            }
            return new List<Barcode>();
        }


        /// <summary>
        /// 获取条码打印信息
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        [ApiService("获取条码打印信息")]
        [return: ApiReturn("获取条码打印信息")]
        public virtual BarcodePrintInfo GetWorkOrderBarcodePrintInfo([ApiParameter("工单Id")] double woId)
        {
            var wo = RF.GetById<WorkOrder>(woId, new EagerLoadOptions().LoadWithViewProperty());
            if (wo != null)
            {
                return new BarcodePrintInfo()
                {
                    WorkOrderId = wo.Id,
                    PrintTemplateId = wo.Template?.LabelTemplateId ?? 0,
                    Printer = "",
                    TemplateType = wo.Template?.LabelTemplate?.Type
                };
            }
            return null;
        }

        /// <summary>
        /// 是否生成任务列表
        /// </summary>
        /// <returns></returns>
        [ApiService("是否生成任务列表")]
        [return: ApiReturn("是否生成任务列表")]
        public virtual bool IsGenerateTask()
        {
            return RT.Service.Resolve<ITaskManage>().IsGenerateTask();
        }


        /// <summary>
        /// 获取工单报工模式
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual int? UpdateWorkOrdeReportModel(double workOrderId)
        {
            return RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(workOrderId);
        }

        /// <summary>
        ///验证工单自动报工
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="workcell"></param>
        /// <param name="reportModel"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidateTaskReport(double workOrderId, Workcell workcell, int? reportModel)
        {
            using (Diagnostics.DebugTrace.Start("自动报工验证耗时：".L10N()))
            {
                if (reportModel == -1)
                    UpdateWorkOrdeReportModel(workOrderId);
                if (reportModel == null)
                    return;
                if (reportModel == 1)
                    throw new ValidationException("不允许采集，当前条码所属工单任务单报工方式为手动报工".L10N());
                RT.Service.Resolve<IWipTaskReport>().ValidateAutoReport(workOrderId, workcell.EmployeeId, workcell.ProcessId);
            }
        }
    }
}
