using Microsoft.Scripting.Utils;
using SIE.Api;
using SIE.Barcodes;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.LinesideWarehouses;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Repairs;
using SIE.MES.WIP.Reworks;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{

    /// <summary>
    /// 返工采集API
    /// </summary>
    public class WinFormReworkApiController : ReworkController
    {
        /// <summary>
        /// 根据Workcell获取当前工单的相关信息
        /// </summary>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("根据Workcell获取当前工单的相关信息")]
        [return: ApiReturn("根据Workcell获取当前工单的相关信息 GetCurrentInfo")]
        public virtual XPApiResultRework GetCurrentInfo([ApiParameter("操作人ID")] double employeeId,
            [ApiParameter("工序ID")] double processId, [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId)
        {

            Process process = Query<Process>().Where(P => P.Id == processId).FirstOrDefault(new EagerLoadOptions());
            if (process == null)
                throw new ValidationException("获取工序采集步骤失败(工序ID{0}不存在)".L10nFormat(processId));



            XPApiResultRework result = new XPApiResultRework();
            result.IsChangeOrder = false;

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };

            //工单信息
            var wipLineWorkOrder = Query<WipResourceWorkOrder>()
                .Where(f => f.ResourceId == workcell.ResourceId && f.ProcessId == workcell.ProcessId && f.StationId == workcell.StationId)
                .FirstOrDefault();

            if (wipLineWorkOrder != null)
            {
                var workOrder = RF.GetById<WorkOrders.WorkOrder>(wipLineWorkOrder.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (workOrder != null)
                {
                    result.WorkOrder = new WorkOrderInfo(workOrder);
                }
            }


            result.Step = new XPReworkStep();
            result.Step.SetProcess(process);

            return result;
        }

        /// <summary>
        /// 获取产线线边仓库
        /// </summary>
        /// <param name="wipKeyItemProcessResourceId">工序过站记录资源ID</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("获取产线线边仓库")]
        [return: ApiReturn("获取产线线边仓库 GetWarehouse")]
        public virtual List<XPLinesideWarehouse> GetWarehouse([ApiParameter("工序过站记录资源ID")] double wipKeyItemProcessResourceId)
        {
            List<XPLinesideWarehouse> result = new List<XPLinesideWarehouse>();
            var whs = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouses();
            if (whs.Any())
            {
                foreach (var lineWarehouse in whs)
                {
                    result.Add(new XPLinesideWarehouse(lineWarehouse));
                }
            }

            return result;
        }

        /// <summary>
        /// 条码置换
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <param name="listCheckedKeyItemIds">选中的关键件ID列表</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("条码置换")]
        [return: ApiReturn("条码置换 PermuteAssemblyCollect")]
        public virtual XPApiResultRework PermuteAssemblyCollect(
            [ApiParameter("条码")] string barcode,
            [ApiParameter("工单ID")] double workOrderId,
            [ApiParameter("操作人ID")] double employeeId,
            [ApiParameter("工序ID")] double processId,
            [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId,
            [ApiParameter("采集步骤")] XPReworkStep step,
            [ApiParameter("当前的关键件ID列表")] List<double> listKeyItemIds,
            [ApiParameter("选中的关键件ID列表")] List<double> listCheckedKeyItemIds
            )
        {
            Process process = Query<Process>().Where(P => P.Id == processId).FirstOrDefault(new EagerLoadOptions().LoadWith(Process.CollectStepListProperty).LoadWith(Process.ParameterListProperty));
            if (process == null)
                throw new ValidationException("获取工序信息失败".L10N());

            XPApiResultRework result = new XPApiResultRework();
            result.ReworkOperate = ReworkOperate.Permute;
            result.CollectBarcode = CreateCollectBarcode(barcode, step.CurrentStep.BarcodeType);
            result.WipKeyItems = new List<XPWipProductProcessKeyItem>();
            result.Step = step;
            result.Step.SetProcess(process);

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };

            EntityList<WipProductProcessKeyItem> wipKeyItems = Query<WipProductProcessKeyItem>().Where(p => listKeyItemIds.Contains(p.Id)).ToList();
            foreach (WipProductProcessKeyItem itm in wipKeyItems)
            {
                if (listCheckedKeyItemIds.Contains(itm.Id))
                    itm.IsUnbound = true;
            }

            if (result.Step.StepIndex == 0)
            {
                var barcodeEntity = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode) ?? throw new ValidationException("条码不存在！".L10N());
                var woId = barcodeEntity.WorkOrderId ?? throw new ValidationException("条码没有归属工单！".L10N());

                if (woId != workOrderId)
                {
                    //新的工单
                    result.IsChangeOrder = true;
                    var workOrder = RF.GetById<WorkOrders.WorkOrder>(woId, new EagerLoadOptions().LoadWithViewProperty());
                    result.WorkOrder = new ApiModels.WorkOrderInfo(workOrder);
                    workOrderId = workOrder.Id;
                }

                PermuteCheckReworkBarcode(barcode); //验证返工工单条码
                ValidatePermute(result.CollectBarcode, workcell, workOrderId, result.Step);
                result.Step.AddReworkBarcodes(result.CollectBarcode.Code);
            }

            if (result.Step.StepIndex != 0)
            {
                PermuteCheckOriginalBarcode(barcode, workOrderId); //验证原工单条码
                SetModelKeyItems(barcode, wipKeyItems); //设置Model的关键件集合
                ValidateBarcode(result.CollectBarcode, workcell);
            }

            result.Step.AddBarcodes(result.CollectBarcode.Code);
            result.Step.ReworkCollectBarcodes.Add(result.CollectBarcode);

            if (!result.Step.NextStep())
            {
                var barcodes = result.Step.ReworkBarcodes.ToArray();
                try
                {
                    var collectData = SetCollectDataReworkData(result.ReworkOperate, wipKeyItems, result.Step);
                    Collect(barcodes, collectData, workcell);
                    result.ResultType = ResultType.Pass;
                    var curStepIndex = result.Step.StepIndex - 1 < 0 ? 0 : result.Step.StepIndex - 1;
                    result.Tips = "{0}[{1}]采集成功".L10nFormat(result.Step.StepBarcodeTypes[curStepIndex], collectData.CollectBarcode.Code);
                    result.Step.Reset();
                }
                catch (Exception)
                {
                    result.Step.Roolback();
                    result.ResultType = ResultType.Fail;
                    throw;
                }
            }
            else
            {
                var curStepIndex = result.Step.StepIndex - 1 < 0 ? 0 : result.Step.StepIndex - 1;
                result.Tips = "{0}[{1}]扫描采集成功，请扫描{2}".L10nFormat(result.Step.StepBarcodeTypes[curStepIndex], result.CollectBarcode.Code, result.Step.StepBarcodeTypes[result.Step.StepIndex]);
            }

            foreach (var keyItem in wipKeyItems)
                result.WipKeyItems.Add(new XPWipProductProcessKeyItem(keyItem));

            return result;
        }



        /// <summary>
        /// 条码置换解绑关健件采集
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="selectedBlankingWay">是否选择了置换后不良下料</param>
        /// <param name="selectedWarehouseId">下料目标线边仓ID</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <param name="listCheckedKeyItemIds">选中的关键件ID列表</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("条码置换解绑关健件采集")]
        [return: ApiReturn("条码置换解绑关健件采集 PermuteAssemblyCollect")]
        public virtual XPApiResultRework PermuteUnboundAssemblyCollect(
            [ApiParameter("条码")] string barcode,
            [ApiParameter("工单ID")] double workOrderId,
            [ApiParameter("操作人ID")] double employeeId,
            [ApiParameter("工序ID")] double processId,
            [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId,
            [ApiParameter("采集步骤")] XPReworkStep step,
            [ApiParameter("是否选择了置换后不良下料")] bool selectedBlankingWay,
            [ApiParameter("下料目标线边仓ID")] double selectedWarehouseId,
            [ApiParameter("当前的关键件ID列表")] List<double> listKeyItemIds,
            [ApiParameter("选中的关键件ID列表")] List<double> listCheckedKeyItemIds,
             [ApiParameter("换料后处理方式")] ChangeItemHandleMethod? changeItemHandleMethod
            )
        {

            Process process = Query<Process>().Where(P => P.Id == processId).FirstOrDefault(new EagerLoadOptions().LoadWith(Process.CollectStepListProperty).LoadWith(Process.ParameterListProperty));
            if (process == null)
                throw new ValidationException("获取工序信息失败".L10N());

            XPApiResultRework result = new XPApiResultRework();
            result.ReworkOperate = ReworkOperate.PermuteUnbound;
            result.CollectBarcode = CreateCollectBarcode(barcode, step.CurrentStep.BarcodeType);
            result.Step = step;
            result.WipKeyItems = new List<XPWipProductProcessKeyItem>();
            result.Step.SetProcess(process);

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };

            EntityList<WipProductProcessKeyItem> wipKeyItems = Query<WipProductProcessKeyItem>().Where(p => listKeyItemIds.Contains(p.Id)).ToList();
            foreach (WipProductProcessKeyItem itm in wipKeyItems)
            {
                if (listCheckedKeyItemIds.Contains(itm.Id))
                    itm.IsUnbound = true;
            }

            if (!PermuteUnboundSubmitCheck(barcode, workcell, result, selectedBlankingWay, selectedWarehouseId, wipKeyItems, changeItemHandleMethod))
            {
                result.ResultType = ResultType.Fail;
                return result;
            }

            bool checkRefOriginal = false;
            if (result.Step.StepIndex == 0)
            {
                PermuteUnboundCheckReworkBarcode(barcode); //验证返工工单条码
                checkRefOriginal = ValidatePermuteUnbound(result.CollectBarcode, workcell, workOrderId, result.Step);
                result.Step.AddReworkBarcodes(result.CollectBarcode.Code);
            }

            if (result.Step.StepIndex != 0 || checkRefOriginal)
            {
                if (!checkRefOriginal)
                    PermuteUnboundCheckOriginalBarcode(barcode, workOrderId); //验证原工单条码
                PermuteUnboundSetModelkeyItems(barcode, wipKeyItems);
                ValidateBarcode(result.CollectBarcode, workcell);
            }
            foreach (var keyItem in wipKeyItems)
            {
                result.WipKeyItems.Add(new XPWipProductProcessKeyItem(keyItem));
            }
            result.Step.AddBarcodes(result.CollectBarcode.Code);
            result.Step.ReworkCollectBarcodes.Add(result.CollectBarcode);

            if (!result.Step.NextStep())
            {
                if (checkRefOriginal)
                {
                    var curStepIndex = result.Step.StepIndex - 1 < 0 ? 0 : result.Step.StepIndex - 1;
                    result.ResultType = ResultType.Pass;
                    result.Tips = "{0}[{1}]扫描成功,请提交".L10nFormat(result.Step.StepBarcodeTypes[curStepIndex], result.CollectBarcode.Code);
                }
                else
                {
                    result.ResultType = ResultType.Pass;
                    result.Tips = "{0}[{1}]扫描成功,请提交".L10nFormat(result.Step.StepBarcodeTypes[result.Step.StepIndex], result.CollectBarcode.Code);
                }
            }
            else
            {
                var curStepIndex = result.Step.StepIndex - 1 < 0 ? 0 : result.Step.StepIndex - 1;
                result.ResultType = ResultType.Pass;
                result.Tips = "{0}[{1}]扫描采集成功，请扫描{2}".L10nFormat(result.Step.StepBarcodeTypes[curStepIndex], result.CollectBarcode.Code, result.Step.StepBarcodeTypes[result.Step.StepIndex]);
            }

            return result;
        }


        /// <summary>
        /// 提交条码置换解绑关健件采集
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="selectedBlankingWay">是否选择了置换后不良下料</param>
        /// <param name="selectedWarehouseId">下料目标线边仓ID</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <param name="listCheckedKeyItemIds">选中的关键件ID列表</param>
        /// <param name="changeItemHandleMethod"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("提交条码置换解绑关健件采集")]
        [return: ApiReturn("提交条码置换解绑关健件采集 SubmitPermuteUnboundAssemblyCollect")]
        public virtual XPApiResultRework SubmitPermuteUnboundAssemblyCollect(
            [ApiParameter("工单ID")] double workOrderId,
            [ApiParameter("操作人ID")] double employeeId,
            [ApiParameter("工序ID")] double processId,
            [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId,
            [ApiParameter("采集步骤")] XPReworkStep step,
            [ApiParameter("是否选择了置换后不良下料")] bool selectedBlankingWay,
            [ApiParameter("下料目标线边仓ID")] double selectedWarehouseId,
            [ApiParameter("当前的关键件ID列表")] List<double> listKeyItemIds,
            [ApiParameter("选中的关键件ID列表")] List<double> listCheckedKeyItemIds,
            [ApiParameter("换料后处理方式")] ChangeItemHandleMethod changeItemHandleMethod
            )
        {
            Process process = Query<Process>().Where(P => P.Id == processId).FirstOrDefault(new EagerLoadOptions().LoadWith(Process.CollectStepListProperty).LoadWith(Process.ParameterListProperty));
            if (process == null)
                throw new ValidationException("获取工序信息失败".L10N());

            XPApiResultRework result = new XPApiResultRework();
            result.ReworkOperate = ReworkOperate.PermuteUnbound;
            result.Step = step;
            result.WipKeyItems = new List<XPWipProductProcessKeyItem>();
            result.Step.SetProcess(process);
            result.CollectBarcode = CreateCollectBarcode(result.Step.Barcodes.LastOrDefault(), result.Step.CurrentStep.BarcodeType);

            LinesideWarehouse selectedWarehouse = null;
            if (selectedBlankingWay)
            {
                if (selectedWarehouseId <= 0)
                    throw new ValidationException("请选择线边仓".L10N());

                selectedWarehouse = RF.GetById<LinesideWarehouse>(selectedWarehouseId, new EagerLoadOptions().LoadWithViewProperty());
                if (selectedWarehouse == null)
                    throw new ValidationException("请选择线边仓(ID{0}不存在)".L10nFormat(selectedWarehouseId));
            }

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };
            //更新物料标签 并调用接口
            string newItemLabelString = "";

            EntityList<WipProductProcessKeyItem> wipKeyItems = Query<WipProductProcessKeyItem>().Where(p => listKeyItemIds.Contains(p.Id)).ToList();
            foreach (WipProductProcessKeyItem curKeyItem in wipKeyItems)
            {
                if (listCheckedKeyItemIds!=null&&listCheckedKeyItemIds.Contains(curKeyItem.Id))
                {
                    curKeyItem.IsUnbound = true;
                    if (selectedBlankingWay)
                    {
                        var newItemLabel = base.RepairReWorkUnloadItem(curKeyItem.SourceId, curKeyItem.Qty, selectedWarehouse.WarehouseId, selectedWarehouse.StorageLocationId, changeItemHandleMethod== ChangeItemHandleMethod.NGRecycle, workOrderId);
                        if (!string.IsNullOrEmpty(newItemLabel))
                        {
                            newItemLabelString += newItemLabel;
                        }
                    }
                }
            }

            CreateCollectBarcode(result.Step.Barcodes.LastOrDefault(), result.Step.CurrentStep.BarcodeType);
            var barcodes = result.Step.ReworkBarcodes.ToArray();
            var collectData = SetCollectDataReworkData(result.ReworkOperate, wipKeyItems, result.Step);
            Collect(barcodes, collectData, workcell);

            var curStepIndex = result.Step.StepIndex - 1 < 0 ? 0 : result.Step.StepIndex - 1;
            result.Tips = newItemLabelString.IsNullOrEmpty() ? "{0}[{1}]采集成功".L10nFormat(result.Step.StepBarcodeTypes[curStepIndex], collectData.CollectBarcode.Code) : "{0}[{1}]采集成功,关键件置换下料生成新条码【{1}】"
                .L10nFormat(result.Step.StepBarcodeTypes[curStepIndex], collectData.CollectBarcode.Code, newItemLabelString.TrimEnd(','));
            result.ResultType = ResultType.Pass;
            result.Step.Reset();

            return result;
        }

        /// <summary>
        /// 关健件解绑-前置
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("关健件解绑")]
        [return: ApiReturn("关健件解绑 PermuteAssemblyCollect")]
        public virtual XPWipProductProcessKeyItem PreKeyItemUnbound([ApiParameter("条码")] string barcode)
        {
            var curKeyItem = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItem(barcode);
            if (curKeyItem == null)
                throw new ValidationException("关健件条码 [{0}] 不存在".L10nFormat(barcode));

            if (curKeyItem.IsUnbound)
                throw new ValidationException("关健件条码 [{0}] 已解绑".L10nFormat(barcode));

            XPWipProductProcessKeyItem result = new XPWipProductProcessKeyItem(curKeyItem);

            return result;
        }

        /// <summary>
        /// 关健件解绑
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="selectedBlankingWay">是否选择了置换后不良下料</param>
        /// <param name="selectedWarehouseId">下料目标线边仓ID</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <param name="listCheckedKeyItemIds">选中的关键件ID列表</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("关健件解绑")]
        [return: ApiReturn("关健件解绑 PermuteAssemblyCollect")]
        public virtual XPApiResultRework KeyItemUnbound(
            [ApiParameter("条码")] string barcode,
            [ApiParameter("工单ID")] double workOrderId,
            [ApiParameter("操作人ID")] double employeeId,
            [ApiParameter("工序ID")] double processId,
            [ApiParameter("工位ID")] double stationId,
            [ApiParameter("资源ID")] double resourceId,
            [ApiParameter("采集步骤")] XPReworkStep step,
            [ApiParameter("是否选择了置换后不良下料")] bool selectedBlankingWay,
            [ApiParameter("下料目标线边仓Id")] double selectedWarehouseId,
            [ApiParameter("当前的关键件ID列表")] List<double> listKeyItemIds,
            [ApiParameter("选中的关键件ID列表")] List<double> listCheckedKeyItemIds,
            [ApiParameter("换料后处理方式")] ChangeItemHandleMethod changeItemHandleMethod)
        {
            if (listKeyItemIds == null || listKeyItemIds.Count <= 0)
                throw new ValidationException("请选择关键件".L10nFormat(barcode));

            var curKeyItem = RF.GetById<WipProductProcessKeyItem>(listKeyItemIds[0]);
            //var curKeyItem = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItem(barcode);
            if (curKeyItem == null)
                throw new ValidationException("关健件条码 [{0}] 不存在".L10nFormat(barcode));

            if (curKeyItem.IsUnbound)
                throw new ValidationException("关健件条码 [{0}] 已解绑".L10nFormat(barcode));

            Process process = Query<Process>().Where(P => P.Id == processId).FirstOrDefault(new EagerLoadOptions().LoadWith(Process.CollectStepListProperty).LoadWith(Process.ParameterListProperty));
            if (process == null)
                throw new ValidationException("获取工序信息失败".L10N());

            LinesideWarehouse selectedWarehouse = null;
            if (selectedBlankingWay)
            {
                if (selectedWarehouseId <= 0)
                    throw new ValidationException("请选择线边仓".L10N());

                selectedWarehouse = RF.GetById<LinesideWarehouse>(selectedWarehouseId, new EagerLoadOptions().LoadWithViewProperty());
                if (selectedWarehouse == null)
                    throw new ValidationException("请选择线边仓(ID{0}不存在)".L10nFormat(selectedWarehouseId));
            }

            XPApiResultRework result = new XPApiResultRework();
            result.ReworkOperate = ReworkOperate.Unbound;
            result.Step = step;
            result.WipKeyItems = new List<XPWipProductProcessKeyItem>();
            result.Step.SetProcess(process);

            WIP.Workcell workcell = new WIP.Workcell
            {
                EmployeeId = employeeId,
                ProcessId = processId,
                StationId = stationId,
                ResourceId = resourceId
            };


            var wipProductProcess = curKeyItem.Process;
            var wipPrcVersion = wipProductProcess.Version;

            //更新物料标签 并调用接口
            string newItemLabel = "";
            if (selectedBlankingWay)
            {
                newItemLabel = base.RepairReWorkUnloadItem(curKeyItem.SourceId, curKeyItem.Qty, selectedWarehouse.WarehouseId, selectedWarehouse.StorageLocationId, changeItemHandleMethod== ChangeItemHandleMethod.NGRecycle, wipPrcVersion.WorkOrder.Id);
            }

            string[] barcodes = new string[] { barcode };
            result.ResultType = ResultType.Pass;

            RT.Service.Resolve<ReworkController>().UnboundKeyItem(curKeyItem.Id);
            //AddDetail(collectBarcode, barcodes, ResultType.Pass, ReworkOperate.Unbound);
            var curWipKeyItems = wipPrcVersion.ProcessList.SelectMany(x => x.KeyItemList).ToList();
            result.CollectBarcode = CreateCollectBarcode(wipPrcVersion.Sn, result.Step.CurrentStep.BarcodeType);
            foreach (var keyItem in curWipKeyItems)
            {
                result.WipKeyItems.Add(new XPWipProductProcessKeyItem(keyItem));
            }
            result.Tips = newItemLabel.IsNullOrEmpty() ? "关健件[{0}]解绑成功".L10nFormat(barcode) : "关健件[{0}]解绑成功,关键件置换下料生成新条码【{1}】".L10nFormat(barcode, newItemLabel);

            return result;
        }


        #region 条码置换相关

        /// <summary>
        /// 创建CollectBarcode
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>采集条码对象</returns>
        private CollectBarcode CreateCollectBarcode(string barcode, BarcodeType barcodeType)
        {
            var collectBarcode = new CollectBarcode(barcode, barcodeType);
            return collectBarcode;
        }

        /// <summary>
        /// 置换采集时验证返工工单条码
        /// </summary>
        /// <param name="barcode">返工工单条码</param>
        private void PermuteCheckReworkBarcode(string barcode)
        {
            var result = CheckReworkBarcodeInReworkOrder(barcode); //Check是否为返工工单生产条码
            if (!result)
                throw new ValidationException("条码[{0}]非返工工单条码".L10nFormat(barcode));
            var flag = CheckReworkBarcodeHavePermuted(barcode); //Check返工条码是否已经置换
            if (flag)
                throw new ValidationException("条码[{0}]已置换, 无法操作".L10nFormat(barcode));
        }

        /// <summary>
        /// Check是否为返工工单生产条码
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <returns>true:条码存在 ; false:条码不存在</returns>
        private bool CheckReworkBarcodeInReworkOrder(string barcode)
        {
            var result = RT.Service.Resolve<BarcodeController>().Exists(barcode, WorkOrderType.Rework);
            return result;
        }

        /// <summary>
        /// Check返工条码是否已经置换
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>true: 已置换；false:未置换</returns>
        private bool CheckReworkBarcodeHavePermuted(string barcode)
        {
            var result = RT.Service.Resolve<WipProductVersionController>().CheckReworkBarcodeHavePermuted(barcode);
            return result;
        }

        /// <summary>
        /// Check返工工单条码是否使用原工单条码
        /// Check是否需要置换
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>true:返工条码使用原工单条码; false:使用新条码</returns>
        private bool CheckReworkBarcodeRefOriginal(string barcode, double workOrderId)
        {
            var curCheckFlag = RT.Service.Resolve<ReworkController>().ExistUnionBarcode(workOrderId, WorkOrderType.Rework, barcode, barcode);
            return curCheckFlag;
        }

        /// <summary>
        /// 验证:1.工艺路线。2.在制工单
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="step">返工采集步骤</param>
        private void ValidatePermute(CollectBarcode collectBarcode, WIP.Workcell workcell, double workOrderId, XPReworkStep step)
        {
            base.Validate(collectBarcode, workcell);
            var checkRefOriginal = CheckReworkBarcodeRefOriginal(collectBarcode.Code, workOrderId);
            if (checkRefOriginal)
            {
                throw new ValidationException("条码[{0}]不需要置换".L10nFormat(collectBarcode.Code));
            }
            else
            {
                ReworkCheckProcessStep(2, step);
            }
        }

        /// <summary>
        /// 判断返工工序的工序步骤
        /// </summary>
        /// <param name="stepCount">工序采集步骤</param>
        /// <param name="step">返工采集步骤</param>
        private void ReworkCheckProcessStep(int stepCount, XPReworkStep step)
        {
            if (step.ProcessSteps.Count() != stepCount)
                throw new ValidationException("返工采集工序的采集步骤必须是 {0} 步!".L10nFormat(stepCount));
        }

        /// <summary>
        /// 设置Model的关键件集合
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="wipKeyItems">关键件</param>
        private void SetModelKeyItems(string barcode, EntityList<WipProductProcessKeyItem> wipKeyItems)
        {
            wipKeyItems.Clear();
            var curKeyItems = RT.Service.Resolve<WipProductVersionController>().GetWipKeyItems(barcode);
            if (curKeyItems != null && curKeyItems.Any())
            {
                var notUnboundKeyItems = curKeyItems.Where(x => !x.IsUnbound).ToList();
                if (notUnboundKeyItems != null && notUnboundKeyItems.Count > 0)
                {
                    wipKeyItems.AddRange(notUnboundKeyItems);
                }
            }
        }

        /// <summary>
        /// 验证原工单生产条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workOrderId">工单ID</param>
        private void PermuteCheckOriginalBarcode(string barcode, double workOrderId)
        {
            CheckOriginalBarcodeScraped(barcode);
            CheckOriginalBarcodeUnionBarcode(barcode, workOrderId);
            CheckOriginalBarcodeHavePermuted(barcode, workOrderId); //条码是否已被置换
        }

        /// <summary>
        /// Check原工单生产条码是否已报废
        /// </summary>
        /// <param name="barcode">条码</param>
        private void CheckOriginalBarcodeScraped(string barcode)
        {
            const bool isScraped = true;
            var result = RT.Service.Resolve<BarcodeController>().Exists(barcode, isScraped);
            if (result)
                throw new ValidationException("条码[{0}]已报废".L10nFormat(barcode));
        }

        /// <summary>
        /// Check原工单生产条码是否进行返工配置
        /// </summary>
        /// <param name="originalBarcode">原工单条码</param>
        /// <param name="workOrderId">工单ID</param>
        private void CheckOriginalBarcodeUnionBarcode(string originalBarcode, double workOrderId)
        {
            var barcodeWorkOrderId = RT.Service.Resolve<BarcodeController>().GetBarcodeWorkOrderId(originalBarcode);
            var curUnionBarcode2 = RT.Service.Resolve<ReworkController>().ExistUnionBarcode(workOrderId, WorkOrderType.Rework, originalBarcode, null);

            var curUnionBarcode = RT.Service.Resolve<ReworkController>().ExistUnionBarcode(workOrderId, WorkOrderType.Rework, originalBarcode, null);
            if (!curUnionBarcode)
                throw new ValidationException("原工单条码[{0}]未进行返工配置".L10nFormat(originalBarcode));
        }

        /// <summary>
        /// 判断原条码是否已经置换
        /// </summary>
        /// <param name="originalBarcode">原工单条码</param>
        /// <param name="workOrderId">工单ID</param>
        private void CheckOriginalBarcodeHavePermuted(string originalBarcode, double workOrderId)
        {
            var checkFlag = RT.Service.Resolve<ReworkController>().CheckOriginalBarcodeHavePermuted(workOrderId, WorkOrderType.Rework, originalBarcode);
            if (checkFlag)
                throw new ValidationException("原工单条码[{0}]已被置换".L10nFormat(originalBarcode));
        }

        /// <summary>
        /// 设置CollectData的属性值
        /// </summary>
        /// <param name="reworkOperate">返工采集类型</param>
        /// <returns>采集数据</returns>
        /// <param name="wipKeyItems">关键件</param>
        /// <param name="step">返工采集步骤</param>
        private CollectData SetCollectDataReworkData(ReworkOperate reworkOperate, EntityList<WipProductProcessKeyItem> wipKeyItems, XPReworkStep step)
        {
            CollectData collectData = new CollectData()
            {
                CollectBarcode = step.ReworkCollectBarcodes.FirstOrDefault()
            };
            if (reworkOperate == ReworkOperate.PermuteUnbound)
            {
                SetCollectDataReworkDataKeyItems(collectData, wipKeyItems);
            }
            SetCollectDataReworkDataBarcodes(collectData, step.Barcodes);
            return collectData;
        }

        /// <summary>
        /// 设置CollectData的返工数据的KeyItems
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="wipKeyItems">关键件</param>
        private void SetCollectDataReworkDataKeyItems(CollectData collectData, EntityList<WipProductProcessKeyItem> wipKeyItems)
        {
            var wipKeyItemIds = wipKeyItems.Where(x => x.IsUnbound).Select(x => x.Id).Distinct().ToList();
            if (wipKeyItemIds != null && wipKeyItemIds.Count > 0)
            {
                collectData.ReworkData.KeyItems.AddRange(wipKeyItemIds);
            }
        }

        /// <summary>
        /// 设置CollectData的返工数据的条码
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="barcodes">采集的条码</param>
        private void SetCollectDataReworkDataBarcodes(CollectData collectData, List<string> barcodes)
        {
            collectData.ReworkData.ReworkBarcode = barcodes.FirstOrDefault();
            collectData.ReworkData.OriginalBarcode = barcodes.LastOrDefault();
        }
        #endregion

        #region 条码置换解绑关健件
        /// <summary>
        /// 判断是否未完成提交，强制扫描下一个条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell"></param>
        /// <param name="result">XP端返工采集API方法返回值</param>
        /// <param name="selectedBlankingWay"></param>
        /// <param name="selectedWarehouseId">线边仓Id</param>
        /// <param name="wipKeyItems"></param>
        /// <param name="changeItemHandleMethod"></param>
        /// <returns>true: 验证通过；false: 未通过</returns>
        /// <exception cref="ValidationException"></exception>
        private bool PermuteUnboundSubmitCheck(string barcode, WIP.Workcell workcell, XPApiResultRework result, bool selectedBlankingWay, double selectedWarehouseId,
            EntityList<WipProductProcessKeyItem> wipKeyItems, ChangeItemHandleMethod? changeItemHandleMethod)
        {
            bool checkFlag = true;
            if (!result.Step.HasNextStep())
            {
                checkFlag = false;
                if (barcode == SIE.Barcodes.Barcode.SubmitCode)
                {
                    LinesideWarehouse selectedWarehouse = null;
                    if (selectedBlankingWay)
                    {
                        if (selectedWarehouseId <= 0)
                            throw new ValidationException("请选择线边仓".L10N());

                        selectedWarehouse = RF.GetById<LinesideWarehouse>(selectedWarehouseId, new EagerLoadOptions().LoadWithViewProperty());
                        if (selectedWarehouse == null)
                            throw new ValidationException("请选择线边仓(Id{0}不存在)".L10nFormat(selectedWarehouseId));
                    }

                    //更新物料标签 并调用接口
                    var strNewLabels = "";
                    if (selectedBlankingWay)
                    {
                        var wipKeyUnboundItems = wipKeyItems.Where(x => x.IsUnbound).Distinct().ToList();
                        if (wipKeyUnboundItems.Any())
                        {
                            string newLabel = "";
                            foreach (var wipKeyUnboundItem in wipKeyUnboundItems)
                            {
                                newLabel = base.RepairReWorkUnloadItem(wipKeyUnboundItem.SourceId, wipKeyUnboundItem.Qty, selectedWarehouse.WarehouseId, selectedWarehouse.StorageLocationId, changeItemHandleMethod== ChangeItemHandleMethod.NGRecycle, wipKeyUnboundItem.Process.Version.WorkOrder.Id);

                                if (!string.IsNullOrEmpty(newLabel))
                                {
                                    strNewLabels += newLabel + ",";
                                }
                            }
                        }
                    }

                    result.CollectBarcode = CreateCollectBarcode(result.Step.Barcodes.LastOrDefault(), result.Step.CurrentStep.BarcodeType);
                    var barcodes = result.Step.ReworkBarcodes.ToArray();
                    var collectData = SetCollectDataReworkData(result.ReworkOperate, wipKeyItems, result.Step);
                    Collect(barcodes, collectData, workcell);

                    var curStepIndex = result.Step.StepIndex - 1 < 0 ? 0 : result.Step.StepIndex - 1;
                    var noNewLabel = "{0}[{1}]采集成功".L10nFormat(result.Step.StepBarcodeTypes[curStepIndex], collectData.CollectBarcode.Code);
                    result.Tips = strNewLabels.IsNullOrEmpty() ? noNewLabel : noNewLabel + ",关键件不良下料生成新物料标签：".L10N() + strNewLabels.TrimEnd(',');
                    result.Step.Reset();
                }
                else
                {
                    result.Tips = "上一条码未提交,扫[{0}]提交或者重新开始".L10nFormat(SIE.Barcodes.Barcode.SubmitCode);
                }
            }

            return checkFlag;
        }

        /// <summary>
        /// 置换解绑Check原工单条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workOrderId">工单ID</param>
        private void PermuteUnboundCheckOriginalBarcode(string barcode, double workOrderId)
        {
            CheckOriginalBarcodeScraped(barcode);
            CheckOriginalBarcodeUnionBarcode(barcode, workOrderId);
            CheckOriginalBarcodeHavePermuted(barcode, workOrderId); //条码是否已被置换
        }

        /// <summary>
        /// 置换解绑Check返工工单条码
        /// </summary>
        /// <param name="barcode">条码</param>
        private void PermuteUnboundCheckReworkBarcode(string barcode)
        {
            var result = CheckReworkBarcodeInReworkOrder(barcode); //Check是否为返工工单生产条码
            if (!result)
            {
                throw new ValidationException("条码[{0}]非返工工单条码".L10nFormat(barcode));
            }
            var flag = CheckReworkBarcodeHavePermuted(barcode); //Check返工条码是否已经置换
            if (flag)
            {
                throw new ValidationException("条码[{0}]已置换, 无法操作".L10nFormat(barcode));
            }
        }

        /// <summary>
        /// 验证:1.工艺路线。2.在制工单
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="step">返工采集步骤</param>
        private bool ValidatePermuteUnbound(CollectBarcode collectBarcode, WIP.Workcell workcell, double workOrderId, XPReworkStep step)
        {
            bool checkRefOriginal = false;
            base.Validate(collectBarcode, workcell);
            checkRefOriginal = CheckReworkBarcodeRefOriginal(collectBarcode.Code, workOrderId); //是否使用原工单条码
            if (checkRefOriginal)
            {
                ReworkCheckProcessStep(1, step);
            }
            else
            {
                ReworkCheckProcessStep(2, step);
            }
            return checkRefOriginal;
        }

        /// <summary>
        /// 设置Model的关健件集合
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <param name="wipKeyItems">关键件</param>
        private void PermuteUnboundSetModelkeyItems(string barcode, EntityList<WipProductProcessKeyItem> wipKeyItems)
        {
            SetModelKeyItems(barcode, wipKeyItems); //设置Model的关键件集合
            var keyItemUnbdCfgs = RT.Service.Resolve<ReworkController>().GetKeyItemUnboundConfigs(barcode);
            if (keyItemUnbdCfgs != null && keyItemUnbdCfgs.Count > 0)
            {
                foreach (var keyItem in wipKeyItems)
                {
                    var curKeyItemCfg = keyItemUnbdCfgs.FirstOrDefault(x => x.ItemId == keyItem.ItemId);
                    if (curKeyItemCfg != null)
                        keyItem.IsUnbound = curKeyItemCfg.IsUnbound;
                }

                //SetIsSelectedAll();
            }
        }
        #endregion

    }
}
