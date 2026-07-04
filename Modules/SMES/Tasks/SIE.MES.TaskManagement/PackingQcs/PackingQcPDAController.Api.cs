using Microsoft.Scripting.Utils;
using SIE.Api;
using SIE.Barcodes.WipBatchs;
using SIE.Core.ApiLogs;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PackingQcs.Data;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.TaskManagement.SuspectProductLabels.ApiModels;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace SIE.MES.TaskManagement.PackingQcs
{
    /// <summary>
    /// 换向器包装采集
    /// </summary>
    public partial class PackingQcPDAController : DomainController
    {
        /// <summary>
        /// 根据员工编码 获取资源||资源名称，资源编码查询
        /// </summary>
        /// <param name="empId">员工编码</param>
        /// <param name="keyWord">资源名称，资源编码</param>
        /// <returns></returns>
        [ApiService("根据员工编码 获取资源||资源名称，资源编码查询")]
        public virtual List<PackingResource> ShowResource([ApiParameter("过滤条件")] double empId, string keyWord)
        {
            try
            {
                List<PackingResource> resources1 = new List<PackingResource>();
                //根据员工获取资源权限
                var listResourceIds = Query<EmployeeResource>().Where(p => p.EmployeeId == empId).ToList().Select(p => p.ResourceId).ToList();
                //获取资源
                var resources = listResourceIds.Distinct().SplitContains(tempId =>
                {
                    return Query<WipResource>().Where(x => tempId.Contains(x.Id)).ToList();
                });


                foreach (var item in resources)
                {
                    PackingResource resource = new PackingResource();
                    resource.ResourceId = item.Id;
                    resource.ResourceName = item.Name;
                    resource.ResourceCode = item.Code;
                    resources1.Add(resource);
                }

                if (!keyWord.IsNullOrEmpty())
                {
                    resources1 = resources1.Where(p => p.ResourceCode.Contains(keyWord) || p.ResourceName.Contains(keyWord)).ToList();
                }

                return resources1;
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 扫描蓝标
        /// </summary>
        /// <param name="barcode">扫描信息</param>
        /// <param name="resourceId">资源id</param>
        /// <param name="resourceName">资源名称</param>
        /// <param name="boxExChange">是否开想 1 换箱  0 不换箱</param>
        /// <param name="firstBarcode">原蓝标</param>
        /// <param name="isFirst">是否原蓝标</param>
        /// <returns></returns>
        [ApiService("扫描蓝标")]
        public virtual PakcingWoInfo ScanBluelLabel([ApiParameter("扫描蓝标")] string barcode, double resourceId, string resourceName, int boxExChange, string firstBarcode, bool isFirst)
        {
            PakcingWoInfo info = new PakcingWoInfo();
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(barcode);
            if (boxExChange == 0)
            {
                info.PackingDetail = null;
            }

            info.Scan = false;
            info.IsUse = true;
            info.ResourceName = resourceName;

            SIE.MES.BlueLable.BlueLable blueBable = new BlueLable.BlueLable();
            //if (boxExChange == 0)
            //{
            //    blueBable = RT.Service.Resolve<PackingQcController>().GetBlueLable(barcode);
            //}
            //else
            //{
            //    blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(barcode);
            //}
            blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(barcode);

            if (blueBable == null)
            {
                info.Error = "系统中没有此蓝标!";
                return info;
            }
            info.XtBlue = barcode;
            //根据蓝标获取工单
            var WorkOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo(blueBable.ProductionNo);
            if (WorkOrder == null)
            {
                throw new ValidationException("蓝标中的工单【{0}】不存在!".L10nFormat(blueBable.ProductionNo));
            }
            //获取蓝标的总数
            info.BlueZInt = blueBable.PackageNum;
            info.WoId = WorkOrder.Id;
            info.WoNo = WorkOrder.No;
            info.ProductName = WorkOrder.Product.Name;
            info.ProductCode = WorkOrder.Product.Code;
            info.ShortDescription = WorkOrder.Product.ShortDescription;


            if (packingQc == null)
            {
                if (boxExChange == 1 && isFirst == true)
                {
                    info.Error = "新蓝标无需换箱";
                    return info;
                }

                //蓝标下明细为空，且有删除标识
                var Blue = RT.Service.Resolve<PackingQcController>().AllBlueLable(barcode);
                if (Blue.CreateDeleteident == "删除")
                {
                    info.Error = "该蓝标存在装箱明细，不允许换箱";
                    return info;
                }

                if (boxExChange == 1)
                {
                    info.XtBlue = barcode;

                    info.Tips = "请点提交按钮,确认换箱!!!";
                    return info;
                }
                //获取蓝标QC确认
                var packingQcData = RT.Service.Resolve<PackingQcController>().GetPackingQcByState(resourceId);
                foreach (var item in packingQcData)
                {
                    if (item.BlueLabel != barcode)
                    {
                        if (item.Confirm == ConfirmEnum.NO)
                        {
                            info.Error = "蓝标标签[" + item.BlueLabel + "]QC未确认,不允许使用其它蓝标标签!";

                            info.Scan = true;
                            return info;
                        }
                    }
                }
            }
            else
            {
                //换箱，就说明当前扫的是第二个蓝标
                if (boxExChange == 1 && isFirst == false)
                {
                    //第二个蓝标需要校验
                    if (packingQc.PackingDetailList.Count > 0)
                    {
                        throw new ValidationException("该蓝标存在装箱明细，不允许换箱".L10N());
                        //info.Error = "该蓝标存在装箱明细，不允许换箱";
                        //return info;
                    }
                }

                //开箱
                var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
                //记录已经装了多少箱数量
                List<NewPackingQcCModel> list = new List<NewPackingQcCModel>();
                foreach (var item in packDetailList)
                {
                    NewPackingQcCModel pqc = new NewPackingQcCModel();
                    pqc.BlueLabel = barcode;
                    if (item.Confirm == ConfirmEnum.YES)
                        pqc.Confirm = "是";
                    else
                        pqc.Confirm = "否";
                    if (packingQc.PackIdent == PackIdentEnum.FullTank)
                        pqc.PackIdent = "满箱";
                    else
                        pqc.PackIdent = "不满箱";
                    pqc.ProductLabel = item.ProductLabel;
                    pqc.PackingNum = item.PackingNum;
                    pqc.ItemId = packingQc.ItemId;
                    pqc.ItemName = packingQc.Item.Name;
                    info.BlueInt += item.PackingNum;
                    list.Add(pqc);
                }
                info.PackingDetail = list;

                if (boxExChange == 1)
                {
                    info.YXtBlue = barcode;
                    info.Tips = "请扫码换箱的蓝标!!!";
                    return info;
                }

                if (packingQc.BoxState != BoxStateEnum.NO)
                {

                }
                else
                {
                    if (packingQc.BlueLableNum == packingQc.PackingNum)
                    {
                        info.Error = "该蓝标已经满箱!";
                        return info;
                    }
                    info.Error = "该蓝标已经封箱,请先点击开箱按钮!";
                    return info;
                }
            }

            if (boxExChange == 0)
            {
                //是否派工单有包装的任务单
                var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByResourceFinishedId(resourceId, null, null);
                if (tasks.Count <= 0)
                {
                    info.Error = "派工单中没有当前资源的任务单!";
                    return info;
                }
                var task = tasks.Where(p => p.WorkOrderId == WorkOrder.Id && p.ProcessName.Contains("包装")).ToList();
                if (task.Count <= 0)
                {
                    info.Error = "派工单中没有当前资源下包装的任务单!";
                    return info;
                }
                else
                {
                    bool error = true;
                    foreach (var item in task)
                    {
                        if (item.TaskStatus == DispatchTaskStatus.Dispatched || item.TaskStatus == DispatchTaskStatus.Executing)
                            error = false;
                    }
                    if (error)
                    {
                        info.Error = "该资源下的任务单状态为已完成!";
                        return info;
                    }
                }
            }

            info.XtBlue = barcode;
            info.Scan = true;
            info.Tips = "请输入工序标签";
            return info;
        }


        /// <summary>
        /// 扫描批次标签
        /// </summary>
        /// <param name="barcode">扫描信息</param>
        /// <param name="resourceId">资源id</param>
        /// <param name="resourceName">资源名称</param>
        /// <param name="woInfo"></param>
        /// <param name="exceed">0是不超出，1是超出</param>
        /// /// <param name="packType">0：取数量，1：累加1</param>
        /// <returns></returns>
        [ApiService("扫描批次标签")]
        [ApiLog]
        public virtual PakcingWoInfo ScanBatchLabel([ApiParameter("扫描批次标签")] string barcode, double resourceId, string resourceName, PakcingWoInfo woInfo, int exceed, int deleteIdent = 0)
        {
            PakcingWoInfo info = new PakcingWoInfo();
            info = woInfo;
            info.Error = "";
            info.Tips = "";
            info.IsUse = true;
            //装箱总数
            int totalPackingNum = 0;

            var list = info.PackingDetail;
            if (list == null)
            {
                list = new List<NewPackingQcCModel>();
            }
            //批次标签
            WipBatch wipBatch = new WipBatch();
            //包装采集主表
            PackingQc packingQc = new PackingQc();
            //物料标签
            ItemLabel itemLabel = new ItemLabel();
            //批次标签等于null， 在去物料标签查询。
            itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);

            var blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(info.XtBlue);
            //根据蓝标获取工单
            var WorkOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo(blueBable.ProductionNo);
            //获取产品
            var itemData = RT.Service.Resolve<WipBatchController>().GetItem(WorkOrder.ProductId);
            //判断在批次标签中是否存在
            string batchNo = $"{info.XtBlue}*{barcode}";
            wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(batchNo);
            if (deleteIdent == 1)
            {
                return DeleteLabel(info.XtBlue, batchNo, itemData, info, wipBatch, itemLabel);
            }
            else
            {
                if (blueBable.CreateDeleteident == "删除")
                {
                    throw new ValidationException("删除的蓝标不允许其他操作，只能移除!");
                }
            }


            if (itemLabel == null)
            {
                throw new ValidationException("当前物料标签【{0}】不存在!".L10nFormat(barcode));

            }
            if (itemLabel.Isuse)
            {
                throw new ValidationException("物料标签【{0}】已经使用!".L10nFormat(barcode));
            }

            if (wipBatch != null)
            {
                throw new ValidationException("批次标签【{0}】已经使用!".L10nFormat(batchNo));
            }

            //看当前物料是否在 这个工单的 BOM里面
            if (RT.Service.Resolve<WorkOrderBomController>().GetWorkOrderBom(WorkOrder.Id, itemLabel.ItemId))
            {
                throw new ValidationException("当前物料标签【{0}】物料号【{1}】,没有在工单【{2}】的BOM里面!".L10nFormat(barcode, itemLabel?.Item?.Code, WorkOrder?.No));
            }

            var dispatchTasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByWoPacking(WorkOrder.Id, resourceId);

            if (dispatchTasks.Count == 0)
            {
                throw new ValidationException("工单【{0}】,资源【{1}】,工序【包装】对应的派工任务单不存在!".L10nFormat(WorkOrder.No, resourceName));
            }
            var taskRemainQty = (int)dispatchTasks.Sum(p => p.RemainQty);
            if (taskRemainQty <= 0)
                throw new ValidationException("当前资源对应的任务单没有剩余可报工数");

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DB.Update<BlueLable.BlueLable>().Set(p => p.Id, blueBable.Id).Where(p => p.Id == blueBable.Id).Execute();   //并发锁

                packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(info.XtBlue);
                if (packingQc != null)
                {
                    var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                    totalPackingNum = packdetails.Sum(p => p.PackingNum);
                    if (totalPackingNum > blueBable.PackageNum)
                    {
                        throw new ValidationException("蓝标装箱数已满,不允许继续装箱!");
                    }

                    packingQc.ResourceId = resourceId;  //换产线时,需要切换为新产线
                    RF.Save(packingQc);
                }
                decimal packingQty = blueBable.PackageNum - totalPackingNum;    //剩余可装箱数
                if (packingQty <= 0)
                    throw new ValidationException("当前蓝标没有剩余可装箱数,请确认!");

                //校验不允许超任务数
                var qcToReportDetails = Query<PackingDetail>().Where(p => p.PackingQc.ResourceId == resourceId && p.WorkOrderNo == WorkOrder.No && p.ReportsType == ReportsTypeEnum.NO).ToList();
                var qcToResportQty = qcToReportDetails.Sum(p => p.PackingNum);    //已装箱待报工数
                taskRemainQty = taskRemainQty - qcToResportQty; //任务单可报工数需要去除待报工数
                if (taskRemainQty <= 0)
                    throw new ValidationException("当前资源对应的任务单,剩余报工数已全部装箱,请确认!");

                if (packingQty > taskRemainQty)
                {
                    packingQty = taskRemainQty;
                }

                //校验不允许超工单计划数
                //当前工单批次总数
                var batchQty = RT.Service.Resolve<WipBatchController>().GetWipBatchSumBatchQty(WorkOrder.Id);
                //工单计划数
                var planQty = RT.Service.Resolve<WipBatchController>().GetWoQty(WorkOrder.Id);
                //工单批次剩余数
                var batchRemainQty = planQty - batchQty;
                if (batchRemainQty <= 0)
                    throw new ValidationException("工单批次装箱总数已达到工单计划数,请确认!");

                if (packingQty > batchRemainQty)
                {
                    packingQty = batchRemainQty;
                }

                //0:不超出 1：超出
                if (exceed == 0)
                {
                    if (itemLabel.Qty > packingQty)
                    {
                        info.Error = "当前物料标签的批次数量已超剩余可装箱数量，是否继续扫描?".L10nFormat();
                        //info.BlueInt -= (int)wipBatch.Qty;
                        info.Exceed = 1;
                        info.SuccessCount = (int)packingQty;
                        info.SurplusCount = (int)itemLabel.Qty - info.SuccessCount;
                        return info;
                    }
                    else
                        packingQty = itemLabel.Qty; //当前装箱数
                }


                if (packingQty > taskRemainQty)
                {
                    throw new ValidationException("装箱数[{0}]，已超过任务单剩余可报工数[{1}]!".L10nFormat(itemLabel.Qty, taskRemainQty));
                }

                if ((batchQty + packingQty) > planQty)
                {
                    throw new ValidationException("批次总数已经大于工单计划数!");
                }

                wipBatch = SaveWipBatch(itemLabel, info.WoId, packingQty, batchNo, "包装");

                //修改物料标签是否已经使用
                itemLabel.Isuse = true;
                RF.Save(itemLabel);


                #region 生成QC装箱数据

                //界面显示明细
                NewPackingQcCModel pqc = new NewPackingQcCModel();
                pqc.BlueLabel = blueBable.BlueLableBox;
                pqc.Confirm = "是";
                pqc.PackIdent = "不满箱";
                pqc.ProductLabel = wipBatch.BatchNo;
                pqc.ItemId = itemData.Id;
                pqc.ItemName = itemData.Name;
                pqc.PackingNum = (int)wipBatch.Qty;
                list.Add(pqc);

                //需要修改
                info.BlueInt = totalPackingNum + (int)wipBatch.Qty;
                totalPackingNum += (int)wipBatch.Qty;


                //主表
                if (packingQc == null)
                {
                    packingQc = new PackingQc();
                    packingQc.BlueLabel = blueBable.BlueLableBox;
                    packingQc.Confirm = ConfirmEnum.YES;
                    packingQc.PackIdent = PackIdentEnum.NotFullTank;
                    packingQc.ProductLabel = barcode;
                    packingQc.ItemId = itemData.Id;
                    packingQc.ItemName = itemData.Name;
                    packingQc.BlueLableNum = blueBable.PackageNum;
                    packingQc.PackingNum = totalPackingNum;
                    packingQc.BoxState = BoxStateEnum.YES;
                    packingQc.ReportsType = ReportsTypeEnum.NO;
                }
                blueBable.IsPack = true;
                packingQc.ResourceId = resourceId;
                packingQc.PackingNum = totalPackingNum;
                packingQc.PackIdent = totalPackingNum >= blueBable.PackageNum ? PackIdentEnum.FullTank : PackIdentEnum.NotFullTank;
                if (packingQc.PackIdent == PackIdentEnum.FullTank)
                    packingQc.BoxState = BoxStateEnum.NO;
                RF.Save(packingQc);
                RF.Save(blueBable);
                PackingDetail packingDetail = new PackingDetail();
                packingDetail.PackingNum = (int)wipBatch.Qty;
                packingDetail.ProductLabel = wipBatch.BatchNo;
                packingDetail.PackingQcId = packingQc.Id;
                packingDetail.Confirm = ConfirmEnum.YES;
                packingDetail.BatchLabel = "";
                packingDetail.WorkOrderNo = WorkOrder.No;
                packingDetail.LabelType = LabelTypeEnum.BatchLabel;
                packingDetail.ReportsType = ReportsTypeEnum.NO;
                RF.Save(packingDetail);
                #endregion


                if (packingQc.PackIdent == PackIdentEnum.FullTank)
                {
                    PackingReportRecord record = new PackingReportRecord();
                    record.BlueLabel = blueBable.BlueLableBox;
                    record.BeginDate = DateTime.Now;
                    record.Report = ReportType.PDAItemLabelSum;
                    info.Tips = "已经装箱完成,请输入蓝标标签!";
                    string reportMessage = RT.Service.Resolve<PackingQcController>().SubmitData(packingQc, true);
                    if (reportMessage != "")
                    {
                        info.Error = "报工错误：" + reportMessage;
                        record.ReturnMessage = reportMessage;
                        return info;
                    }
                    else
                    {
                        record.ReturnMessage = "";
                    }
                    record.EndDate = DateTime.Now;
                    RF.Save(record);
                }
                info.PackingDetail = list;

                tran.Complete();
                return info;
            }
        }


        /// <summary>
        /// 开箱
        /// </summary>
        /// <param name="barcode">扫描信息</param>
        /// <param name="woInfo"></param>
        [ApiService("开箱")]
        public virtual PakcingWoInfo NewUnboxing([ApiParameter("开箱")] string barcode, PakcingWoInfo woInfo)
        {
            if (barcode == "")
            {
                throw new ValidationException("蓝标不能为空!");
            }
            PakcingWoInfo info = new PakcingWoInfo();
            info = woInfo;
            List<NewPackingQcCModel> list = new List<NewPackingQcCModel>();
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(barcode);
            if (packingQc.BoxState == BoxStateEnum.YES)
            {
                info.Error = "该蓝标是开箱状态,无需再次开箱!";
                return info;
            }

            var packDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetail(packingQc.Id);
            //list = null;
            foreach (var item in packDetailList)
            {
                NewPackingQcCModel pqc = new NewPackingQcCModel();
                pqc.BlueLabel = info.XtBlue;
                pqc.Confirm = "是";
                pqc.PackIdent = "不满箱";
                pqc.ProductLabel = item.ProductLabel;
                pqc.ItemId = packingQc.ItemId;
                pqc.ItemName = packingQc.Item.Name;
                pqc.PackingNum = item.PackingNum;
                list.Add(pqc);
            }
            info.PackingDetail = list;
            packingQc.BoxState = BoxStateEnum.YES;
            packingQc.ReportsType = ReportsTypeEnum.NO;
            RF.Save(packingQc);
            info.Tips = "已开箱请输入工序标签!";
            info.Error = "";
            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xtBlue">蓝标</param>
        /// <param name="yxtBlue">原蓝标</param>
        /// <param name="boxExChange">1换箱 其他提交</param>
        /// <returns></returns>
        [ApiService("提交")]
        public virtual string NewNormal([ApiParameter("提交")] string xtBlue, string yxtBlue, int boxExChange)
        {
            if (boxExChange == 1)
            {
                if (xtBlue == null)
                {
                    throw new ValidationException("蓝标不能为空!");
                }
                if (yxtBlue == null)
                {
                    throw new ValidationException("原蓝标不能为空!");
                }

                var XtblueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(xtBlue);
                if (XtblueBable == null)
                {
                    throw new ValidationException("蓝标不存在!");
                }
                var YXtblueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(yxtBlue);
                if (YXtblueBable == null)
                {
                    throw new ValidationException("原蓝标不存在!");
                }

                //原蓝标查询装箱数量
                var yXpackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(yxtBlue);

                //现有蓝标
                var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(yXpackingQc.Id);
                var detailSum = packdetails.Sum(p => p.PackingNum);

                if (XtblueBable.ProductionNo == YXtblueBable.ProductionNo)
                {
                    //查找包装QC主表BlueLableController
                    var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(yxtBlue);
                    if (packingQc != null)
                    {
                        if (detailSum != XtblueBable.PackageNum)
                        {
                            packingQc.PackIdent = PackIdentEnum.NotFullTank;
                        }
                        //if (XtblueBable.PackageNum >= detailSum)
                        if (XtblueBable.PackageNum >= packdetails.Sum(p => p.PackingNum))
                        {
                            packingQc.BlueLableNum = XtblueBable.PackageNum;

                            packingQc.BlueLabel = xtBlue;
                            packingQc.OldBlueLabel = yxtBlue;
                            packingQc.IsUploadSap = false;
                            packingQc.UploadResult = "";
                            if (packingQc.PackingDetailList.Sum(p => p.PackingNum) == packingQc.BlueLableNum)
                            {
                                packingQc.PackIdent = PackIdentEnum.FullTank;
                            }
                            RF.Save(packingQc);
                            YXtblueBable.IsPack = false;
                            XtblueBable.IsPack = true;
                            RF.Save(YXtblueBable);
                            RF.Save(XtblueBable);
                        }
                        else
                        {
                            throw new ValidationException("装箱数大于蓝标数!");
                        }
                    }
                    var XzpackingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(xtBlue);



                    return "换箱成功!!!";
                }
                else
                {
                    throw new ValidationException("现在的蓝标和原蓝标的工单号不一致!");
                }
            }
            else
            {
                string message = "";
                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    //查找包装QC主表
                    var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(xtBlue);

                    if (packingQc != null)
                    {
                        var boolWork = RT.Service.Resolve<PackingQcController>().BoolUnreportedWork(packingQc.Id);
                        if (boolWork == true)
                        {
                            packingQc.Confirm = ConfirmEnum.YES;
                            packingQc.BoxState = BoxStateEnum.NO;
                            RF.Save(packingQc);
                            PackingReportRecord record = new PackingReportRecord();
                            record.BeginDate = DateTime.Now;
                            record.BlueLabel = xtBlue;
                            record.Report = ReportType.PDASubmit;
                            message = RT.Service.Resolve<PackingQcController>().SubmitData(packingQc, true);
                            record.EndDate = DateTime.Now;
                            record.ReturnMessage = message;
                            RF.Save(record);
                        }
                        else
                        {
                            return "没有需要报工的数据,无需提交！！！";
                        }
                    }
                    tran.Complete();
                    return message;

                }
            }
        }

        /// <summary>
        /// 保存批次标签
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <param name="workOrderId"></param>
        /// <param name="qty"></param>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public virtual WipBatch SaveWipBatch(ItemLabel itemLabel, double workOrderId, decimal qty, string batchNo, string processCode = null)
        {
            WipBatch wipBatch = new WipBatch();
            wipBatch.BatchNo = batchNo;
            wipBatch.IsChild = false;
            wipBatch.IsGenerate = true;
            wipBatch.IsGenerateChild = false;
            wipBatch.IsMantissa = false;
            wipBatch.IsScraped = false;
            wipBatch.PrintTimes = 0;
            wipBatch.PrintedState = SIE.Barcodes.BarcodeState.Notprint;
            wipBatch.EditQtyProcessCode = processCode;
            wipBatch.Qty = (decimal)qty;
            wipBatch.WorkOrderId = workOrderId;
            wipBatch.Isuse = true;
            wipBatch.IsRework = false;
            RF.Save(wipBatch);
            return wipBatch;
        }

        /// <summary>
        /// 移除批次标签
        /// </summary>
        /// <param name="xtBlue">蓝标</param>
        /// <param name="barcode">批次标签</param>
        /// <param name="itemData">物料</param>
        public virtual PakcingWoInfo DeleteLabel(string xtBlue, string batchNo, Item itemData, PakcingWoInfo info, WipBatch wipBatch, ItemLabel itemLabel)
        {
            List<NewPackingQcCModel> list = new List<NewPackingQcCModel>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //主表
                var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(xtBlue);
                if (packingQc == null)
                {
                    throw new ValidationException("此蓝标没有包装!");
                }
                //从表
                var packingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                if (packingDetailList.Count > 0)
                {
                    var packDetail = packingDetailList.Where(p => p.ProductLabel == batchNo).FirstOrDefault();
                    if (packDetail != null)
                    {
                        if (packDetail.ReportsType == ReportsTypeEnum.NO)
                        {
                            info.BlueInt -= packDetail.PackingNum;
                            packingQc.PackingNum -= packDetail.PackingNum;
                            packDetail.PersistenceStatus = PersistenceStatus.Deleted;
                            RF.Save(packDetail);
                            wipBatch.PersistenceStatus = PersistenceStatus.Deleted;
                            RF.Save(wipBatch);
                            //itemLabel.Qty+= packDetail.PackingNum;
                            itemLabel.Isuse = false;
                            RF.Save(itemLabel);
                            if (packingDetailList.Count == 1)
                            {
                                packingQc.PersistenceStatus = PersistenceStatus.Deleted;
                                info.PackingDetail = null;
                            }
                            else
                            {
                                var newPackingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                                foreach (var item in newPackingDetailList)
                                {
                                    NewPackingQcCModel pqc = new NewPackingQcCModel();
                                    pqc.BlueLabel = xtBlue;
                                    pqc.Confirm = "是";
                                    pqc.PackIdent = "不满箱";
                                    pqc.ProductLabel = item.ProductLabel;
                                    pqc.ItemId = itemData.Id;
                                    pqc.ItemName = itemData.Name;
                                    pqc.PackingNum = item.PackingNum;
                                    list.Add(pqc);
                                }
                                info.PackingDetail = list;
                            }

                            RF.Save(packingQc);
                        }
                        else
                        {
                            throw new ValidationException("该标签【" + batchNo + "】已经报工,没法移除!");
                        }
                    }
                    else
                    {
                        throw new ValidationException("该标签不存在,标签【" + batchNo + "】!");
                    }
                }
                else
                {
                    throw new ValidationException("蓝标【" + xtBlue + "】没有装箱!");
                }
                tran.Complete();
            }
            info.IsUse = true;
            info.Tips = "移除成功!";
            return info;
        }

        /// <summary>
        /// 输入或扫描蓝标
        /// </summary>
        /// <param name="inputParam">扫描信息</param>
        /// <returns></returns>
        [ApiService("输入或扫描蓝标")]
        [ApiLog]
        public virtual List<DispatchTask> GetTaskProdLineData([ApiParameter("输入或扫描蓝标")] string inputParam)
        {
            RT.InvOrg = 2710;
            SIE.MES.BlueLable.BlueLable blueLable = new BlueLable.BlueLable();
            blueLable = RT.Service.Resolve<PackingQcController>().AllBlueLable(inputParam);
            //根据蓝标获取工单
            if (blueLable == null) {
                throw new ValidationException("蓝标"+ inputParam + "不存在");
            }
            var WorkOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo(blueLable.ProductionNo);
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { WorkOrder.Id });
            var taskList = new List<DispatchTask>();
            taskList = tasks.Where(p => p.WorkOrderId == WorkOrder.Id && (p.ProcessCode.Contains("包装") || p.ProcessName.Contains("包装"))).OrderBy(p => p.PlanBeginTime).ToList();
            if (taskList.Count == 0) throw new ValidationException("蓝标"+ inputParam + "未匹配到对应的任务单");
            return taskList;
        }
    }
}
