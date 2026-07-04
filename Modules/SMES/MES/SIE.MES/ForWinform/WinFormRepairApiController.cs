using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using SIE.Api;
using SIE.Common;
using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.Wip.Repairs;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Repairs;
using SIE.MES.WIP.TaskExtensions;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{
    /// <summary>
    ///XP维修API控制器
    /// </summary>
    public class WinFormRepairApiController : RepairController
    {
        //后端逻辑待迁入
        /// <summary>
        /// 获取或创建维修记录
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="submitBarcodeCode">扫描条码</param>
        [ApiService("获取或创建维修记录")]
        [return: ApiReturn("获取或创建维修记录")]
        public virtual void GetRepairRecord([ApiParameter("工单Id")] double workOrderId, [ApiParameter("条码")] string submitBarcodeCode)
        {
            RT.Service.Resolve<RepairController>().GetRepairRecord(workOrderId, submitBarcodeCode, true);
        }

        /// <summary>
        /// 加载关键项
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="isItem"></param>
        /// <returns></returns>
        [ApiService("加载关键项")]
        [return: ApiReturn("加载关键项")]

        public virtual List<WipProductProcessKeyItem> GetLoadkeyItems([ApiParameter("条码")] string barcode, [ApiParameter("是否半成品")] bool isItem)
        {
            var keyItems = RT.Service.Resolve<RepairController>().LoadkeyItems(barcode, isItem);
            if (keyItems.Any())
            {
                return keyItems.ToList();
            }
            else
            {
                return new List<WipProductProcessKeyItem>();
            }
        }

        /// <summary>
        /// 验证工艺路线并加载产品未维修的缺陷
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>

        [ApiService("验证工艺路线并加载产品未维修的缺陷")]
        [return: ApiReturn("验证工艺路线并加载产品未维修的缺陷")]
        public virtual List<XPRepairDefectViewModel> GetLoadDefects([ApiParameter("条码")] string barcode, [ApiParameter("工作单元")] WIP.Workcell workcell)
        {
            List<XPRepairDefectViewModel> repairDefectViewModelList = new List<XPRepairDefectViewModel>();
            var defects = RT.Service.Resolve<RepairController>().LoadDefectsWithViewProperty(barcode, workcell);
            if (defects.Any())
            {
                var processIds = defects.Select(m => m.ProcessId).ToList();
                var processList = RT.Service.Resolve<ProcessController>().GetProcessByIds(processIds);
                defects.ForEach(f =>
                {
                    var responsibilityList = f.ResponsibilityList.ToList();

                    var copyf = JsonConvert.DeserializeObject<XPWipProductDefect>(JsonConvert.SerializeObject(f));
                    copyf.ResponsibilityList = responsibilityList;
                    copyf.MeasureList = f.MeasureList.ToList();
                    var repairDefect = new XPRepairDefectViewModel()
                    {
                        WipProductDefectId = copyf.Id,
                        WipProductDefect = copyf,
                        ProcessId = f.ProcessId,
                        ActualDefectId = f.DefectId,
                        Remark = f.Remark,
                        DefectCode = f.DefectCode,
                        DefectDesc = f.DefectDesc,
                        DefectId = f.DefectId.Value,
                        IsFixed = f.IsFixed,
                        ProcessName = processList.FirstOrDefault(m => m.Id == f.ProcessId)?.Name,
                    };
                    repairDefect.MeasureList.AddRange(copyf.MeasureList.Select(p => p.RepairMeasure));
                    repairDefect.ResponsibilityList.AddRange(copyf.ResponsibilityList.Select(p => p.DefectResponsibility));
                    repairDefectViewModelList.Add(repairDefect);
                });
            }
            return repairDefectViewModelList;
        }

        /// <summary>
        /// 获取所有维修措施
        /// </summary>
        /// <returns></returns>
        [ApiService("获取所有维修措施")]
        [return: ApiReturn("获取所有维修措施")]
        public virtual List<RepairMeasure> GetAllRepairMeasure()
        {
            return RF.GetAll<RepairMeasure>().ToList();
        }

        /// <summary>
        /// 获取所有维修措施
        /// </summary>
        /// <returns></returns>
        [ApiService("获取所有缺陷责任")]
        [return: ApiReturn("获取缺陷责任")]
        public virtual List<Defects.DefectResponsibility> GetAllDefectResponsibility()
        {
            return RF.GetAll<Defects.DefectResponsibility>().ToList();
        }

        /// <summary>
        /// 获取所有缺陷责任分类
        /// </summary>
        /// <returns></returns>
        [ApiService("获取所有缺陷责任分类")]
        [return: ApiReturn("获取所有缺陷责任分类")]
        public virtual List<XPDefectResponsibilityCategory> GetAllDefectResponsibilityCategory()
        {
            List<XPDefectResponsibilityCategory> XPDefectResponsibilityCategorys = new List<XPDefectResponsibilityCategory>();
            var results = RT.Service.Resolve<DefectController>().GetAllDefectResponsibilityCategory().ToList();
            foreach (var defectResponsibilityCategory in results)
            {
                var treeId = defectResponsibilityCategory.GetTreePId();
                XPDefectResponsibilityCategory xpDefectResponsibilityCategory = new XPDefectResponsibilityCategory()
                {
                    Code = defectResponsibilityCategory.Code,
                    Description = defectResponsibilityCategory.Description,
                    Id = defectResponsibilityCategory.Id,
                };
                if (treeId == null)
                {
                    xpDefectResponsibilityCategory.TreePId = null;
                }
                else
                {
                    xpDefectResponsibilityCategory.TreePId = double.Parse(treeId.ToString());
                }
                XPDefectResponsibilityCategorys.Add(xpDefectResponsibilityCategory);
            }
            return XPDefectResponsibilityCategorys;
        }



        /// <summary>
        /// 维修采集过站
        /// </summary>
        /// <param name="barcodes"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        [ApiService("维修采集过站")]
        [return: ApiReturn("维修采集过站,无返回")]
        public virtual void CollectApi([ApiParameter("条码集合")] string[] barcodes, [ApiParameter("采集数据")] CollectData collectData, [ApiParameter("工作单元")] WIP.Workcell workcell)
        {
            RT.Service.Resolve<RepairController>().Collect(barcodes, collectData, workcell);
        }


        /// <summary>
        /// 维修采集
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        /// <param name="uplineProcess"></param>
        [ApiService("维修采集")]
        [return: ApiReturn("维修采集")]
        public virtual void RepairCollect([ApiParameter("条码集合")] string barcode, [ApiParameter("采集数据")] CollectData collectData, [ApiParameter("工作单元")] WIP.Workcell workcell, [ApiParameter("上线工序Id")] double? uplineProcess = null)
        {
            RT.Service.Resolve<RepairController>().Collect(barcode, collectData, workcell, uplineProcess);
        }

        /// <summary>
        /// 获取工序清单
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        [ApiService("获取可选工序清单")]
        [return: ApiReturn("获取可选工序清单")]
        public virtual List<GotoProcessViewModel> GetRepariRoutingProcessList([ApiParameter("条码")] string barcode, [ApiParameter("工作单元")] WIP.Workcell workcell)
        {
            return RT.Service.Resolve<RepairController>().GetRoutingProcessList(barcode, workcell);
        }


        /// <summary>
        /// 获取可选工艺路线
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        [ApiService("获取可选工艺路线")]
        [return: ApiReturn("获取可选工艺路线")]
        public virtual List<GotoChildProcessViewModel> GetRepariOptionalPaths([ApiParameter("条码")] string barcode, [ApiParameter("工作单元")] WIP.Workcell workcell)
        {
            return RT.Service.Resolve<RepairController>().GetOptionalPaths(barcode, workcell);
        }
        

        /// <summary>
        /// 保存维修记录
        /// </summary>
        /// <param name="defects"></param>
        [ApiService("保存维修记录")]
        [return: ApiReturn("")]
        public virtual void SaveRepairRecord([ApiParameter("缺陷维修记录")] List<WipProductDefect> defects)
        {
            RT.Service.Resolve<RepairController>().SaveRepairRecord(defects.AsEntityList());
        }

        /// <summary>
        /// 提交维修
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="routingProcessId"></param>
        [ApiService("提交维修")]
        [return: ApiReturn("")]
        public virtual void RepairOptionalSubmit([ApiParameter("条码")] string barcode, [ApiParameter("工作单元")] WIP.Workcell workcell, [ApiParameter("工序Id")] double routingProcessId)
        {
            RT.Service.Resolve<RepairController>().RepairSubmit(barcode, workcell, routingProcessId);
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
        ///验证工单自动报工
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="workcell"></param>
        /// <param name="reportModel"></param>
        /// <exception cref="ValidationException"></exception>
        protected virtual void ValidateTaskReport(double workOrderId, Workcell workcell, int? reportModel)
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
        /// <summary>
        /// 更新报工模式
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        protected virtual int? UpdateWorkOrdeReportModel(double workOrderId)
        {
            return RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(workOrderId);
        }
    }
}
