using MimeKit.Cryptography;
using SIE.Andon.Andons.IOT;
using SIE.Api;
using SIE.Barcodes.WipBatchs;
using SIE.Core.ApiLogs;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.MES.ProcessProperty;
using SIE.MES.Engrave;
using SIE.MES.PackingQC;
using SIE.MES.PackRule;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PackingQcs.Data;
using SIE.MES.WIP.Pressure;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs
{
    /// <summary>
    /// 连接器单体包装采集PDA
    /// </summary>
    public partial class ConnectorSnPackingController : DomainController
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
        /// 刻码替换
        /// </summary>
        /// <param name="oldSn"></param>
        /// <param name="newSn"></param>
        public virtual void EngraveReplace(string oldSn, string newSn)
        {
            //刻码明细
            EngraveSn engraveSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(oldSn);

            //包装QC确认
            PackingDetail packingDetail = RT.Service.Resolve<PackingQcController>().GetPackingDetailByProductLabel(oldSn);

            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                //创建替换记录
                EngraveLabelSnReplaceLog log = new EngraveLabelSnReplaceLog();
                log.PackingDetailId = packingDetail.Id;
                log.EngraveSnId = engraveSn.Id;
                log.OldSn = oldSn;
                log.NewSn = newSn;
                log.PersistenceStatus = PersistenceStatus.New;
                RF.Save(log);
                //替换原刻码查询明细旧刻码
                engraveSn.Sn = newSn;
                engraveSn.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(engraveSn);
                //替换原来包装QC明细旧刻码
                packingDetail.ProductLabel = newSn;
                packingDetail.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(packingDetail);

                tran.Complete();
            }
        }

        /// <summary>
        /// 校验规则
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="sn"></param>
        public virtual void ValidItemQRCodeRule(double itemId, string sn)
        {
            var itemQrRule = RT.Service.Resolve<PackRuleController>().GetItemQRCodeRule(itemId);
            if (itemQrRule == null)
            {
                throw new ValidationException("当前产品,没有维护包装物料二维码规则关系!");
            }

            var qRCodeRule = RT.Service.Resolve<PackRuleController>().GetQRCodeRule(itemQrRule.QRCodeRuleId);
            if (qRCodeRule == null)
            {
                throw new ValidationException("包装二维码规则维护没有!");
            }

            if (itemQrRule.IsQualityStatus == true)
            {
                //验证质量状态，如果产品勾选了这个，则一定要在“工序参数数采”里面存在记录，并且最新的那条是OK的
                var collect = RT.Service.Resolve<ProcessParamCollectController>().GetProcessParamCollect(sn);
                if (collect == null)
                {
                    throw new ValidationException("SN不存在工序参数数采记录，请检查!".L10N());
                }
                if (collect.QualityStatus == Core.Common.InspResult.NG)
                {
                    throw new ValidationException("产品质量不合格，请检查!".L10N());
                }
            }
            //总位数验证
            if (qRCodeRule.TotalDigit != null)
            {
                if (qRCodeRule.TotalDigit != "")
                {
                    int total = int.Parse(qRCodeRule.TotalDigit);
                    if (sn.Length != total)
                    {
                        throw new ValidationException("当前二维码的长度跟规则不匹配,当前总位数是：【" + total + "】,规则总位数【" + sn.Length + "】!");
                    }
                }
            }

            //客户零件号验证 开始，结束
            int khljStart = -1;
            int khljEnd = -1;
            if (qRCodeRule.CustomPnStartDigit != null)
            {
                if (qRCodeRule.CustomPnStartDigit != "")
                {
                    khljStart = int.Parse(qRCodeRule.CustomPnStartDigit);
                }
            }
            if (qRCodeRule.CustomPnEndDigit != null)
            {
                if (qRCodeRule.CustomPnEndDigit != "")
                {
                    khljEnd = int.Parse(qRCodeRule.CustomPnEndDigit);
                }
            }
            if (khljStart != -1 && khljEnd != -1)
            {
                string khlj = sn.Substring(khljStart - 1, khljEnd - khljStart + 1);
                if (khlj != itemQrRule.CustomPn)
                {
                    throw new ValidationException("二维码中的客户零件号不正确,当前客户零件号【" + khlj + "】,规则客户零件号【" + itemQrRule.CustomPn + "】!");
                }
            }
            //客户版本号验证 开始，结束
            int khbbStart = -1;
            int khbbEnd = -1;
            if (qRCodeRule.VersionNumberStartDigit != null)
            {
                if (qRCodeRule.VersionNumberStartDigit != "")
                {
                    khbbStart = int.Parse(qRCodeRule.VersionNumberStartDigit);
                }
            }
            if (qRCodeRule.VersionNumberEndDigit != null)
            {
                if (qRCodeRule.VersionNumberEndDigit != "")
                {
                    khbbEnd = int.Parse(qRCodeRule.VersionNumberEndDigit);
                }
            }
            if (khbbStart != -1 && khbbEnd != -1)
            {
                string khbb = sn.Substring(khbbStart - 1, khbbEnd - khbbStart + 1);
                if (khbb != itemQrRule.VersionNumber)
                {
                    throw new ValidationException("二维码中的客户版本号不正确,当前客户版本号【" + khbb + "】,规则客户零件号【" + itemQrRule.VersionNumber + "】!");
                }
            }


            //序号验证 开始，结束
            int xhStart = -1;
            int xhEnd = -1;
            if (qRCodeRule.SerialNumberStartDigit != null)
            {
                if (qRCodeRule.SerialNumberEndDigit != "")
                {
                    xhStart = int.Parse(qRCodeRule.SerialNumberStartDigit);
                }
            }
            if (qRCodeRule.SerialNumberEndDigit != null)
            {
                if (qRCodeRule.SerialNumberEndDigit != "")
                {
                    xhEnd = int.Parse(qRCodeRule.SerialNumberEndDigit);
                }
            }
            if (xhStart != -1 && xhEnd != -1)
            {
                string xlh = sn.Substring(xhStart - 1, xhEnd - xhStart + 1);
                var packingDetails = RT.Service.Resolve<PackingQcController>().GetPackingDetails(xlh);
                if (packingDetails.Count > 0)
                {
                    foreach (var item in packingDetails)
                    {
                        string label = item.ProductLabel.Substring(xhStart - 1, xhEnd - xhStart + 1);
                        if (xlh == label)
                        {
                            throw new ValidationException("有一样的序列号，当前序列号二维码：【" + sn + "】,以扫过的序列号二维码：【" + item.ProductLabel + "】!");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 替换刻码-提交
        /// </summary>
        /// <param name="oldSn"></param>
        /// <param name="newSn"></param>
        [ApiService("替换刻码-提交")]
        public virtual void SubmitConnectorSnReplaceData(string oldSn, string newSn)
        {
            if (oldSn.IsNullOrEmpty())
                throw new ValidationException("要扫描原刻码!".L10N());
            if (newSn.IsNullOrEmpty())
                throw new ValidationException("要扫描替换刻码!".L10N());

            //原刻码不为空，就代码已经扫码原刻码了，现在要扫描替换刻码
            EngraveSn engraveSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(newSn);
            if (engraveSn != null)
            {
                throw new ValidationException("替换刻码已存在!".L10N());
            }

            try
            {
                EngraveSn oldEngraveSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(oldSn);

                //校验二维码规则
                RT.Service.Resolve<ConnectorSnPackingController>().ValidItemQRCodeRule(oldEngraveSn.EngraveLabel.ProductId.Value, newSn);
                //替换、创建记录
                EngraveReplace(oldSn, newSn);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// 替换刻码-扫描刻码
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [ApiService("替换刻码-扫描刻码")]
        public virtual ConnectorSnReplaceData GetConnectorSnReplaceData(string sn)
        {
            EngraveSn engraveSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(sn);
            if (engraveSn == null)
            {
                throw new ValidationException("原刻码不存在!".L10N());
            }
            PackingDetail packingDetail = RT.Service.Resolve<PackingQcController>().GetPackingDetailByProductLabel(sn);
            if (packingDetail == null)
            {
                throw new ValidationException("包装QC明细不存在!".L10N());
            }

            ConnectorSnReplaceData data = new ConnectorSnReplaceData();

            data.ProductCode = engraveSn.EngraveLabel.Product.Code;
            data.ProductName = engraveSn.EngraveLabel.Product.Name;
            data.ShortDescription = engraveSn.EngraveLabel.Product.ShortDescription;
            data.BatchNo = engraveSn.EngraveLabel.BatchNo;

            return data;
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
        public virtual ConnectorSnData ScanBluelLabel([ApiParameter("扫描蓝标")] string barcode, double resourceId, string resourceName, int boxExChange, string firstBarcode, bool isFirst)
        {
            ConnectorSnData info = new ConnectorSnData();
            var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(barcode);
            if (boxExChange == 0)
            {
                info.PackingDetail = null;
            }

            info.Scan = 0;
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
            //验证蓝标应该单体包装还是批次包装
            var msg = ValidBlueLable(blueBable);
            if (!msg.IsNullOrEmpty())
                throw new ValidationException(msg);
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

                            info.Scan = 0;
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
                List<ConnectorPacking> list = new List<ConnectorPacking>();
                foreach (var item in packDetailList)
                {
                    ConnectorPacking pqc = new ConnectorPacking();
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
            info.Scan = 1;
            info.Tips = "请输入批次标签";
            return info;
        }

        /// <summary>
        /// 扫码批次
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        [ApiService("扫描批次")]
        public virtual ConnectorSnData ScanBatchLabel([ApiParameter("扫描批次标签")] string barcode, double resourceId, ConnectorSnData woInfo)
        {
            ConnectorSnData info = new ConnectorSnData();
            info = woInfo;
            //批次标签
            WipBatch wipBatch = new WipBatch();
            //刻码标签
            EngraveLabel engrave = new EngraveLabel();
            EntityList<EngraveSn> labelSn = new EntityList<EngraveSn>();
            //第一步 批次标签是否存在
            wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatchPc(barcode);
            if (wipBatch == null)
            {
                throw new ValidationException("批次标签【{0}】不存在!".L10nFormat(barcode));
            }

            //获取蓝标的总数
            if (woInfo.WoId != wipBatch.WorkOrderId)
            {
                throw new ValidationException("批次标签的工单,和蓝标的工单不一致!");
            }
            info.BatchLable = barcode;
            info.BatchZInt = (int)wipBatch.Qty;

            engrave = RT.Service.Resolve<EngraveLabelController>().BoolEngraveLabel(barcode);

            if (engrave == null)
            {
                engrave = new EngraveLabel();
                engrave.BatchNo = barcode;
                engrave.WorkOrderId = wipBatch.WorkOrderId;
                engrave.ResourceId = resourceId;
                engrave.ProductId = wipBatch.WorkOrder.ProductId;
                engrave.Qty = wipBatch.Qty;
                bool saveEngrave = RT.Service.Resolve<EngraveLabelController>().SaveEngraveLabel(engrave);
                if (saveEngrave == false)
                {
                    throw new ValidationException("刻码标签保存失败!");
                }
            }
            else
            {
                info.BatchZInt = (int)engrave.Qty;
                labelSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSns(engrave.Id);
                if (labelSn.Count > 0)
                    info.BatchInt = labelSn.Count;
                else
                    info.BatchInt = 0;
            }

            RT.Service.Resolve<ITaskReportKZ>().ValidatePrepareProcessHasReport(barcode, "包装");
            info.Scan = 2;
            info.Tips = "请输入SN标签!";
            return info;
        }

        /// <summary>
        /// 验证蓝标应该单体包装还是批次包装
        /// </summary>
        /// <param name="blue">蓝标</param>
        /// <returns>错误信息</returns>
        public virtual string ValidBlueLable(BlueLable.BlueLable blue)
        {
            //若在功能“包装物料二维码规则关系”中，若未维护物料的关系，则该物料只能使用批次包装采集的功能，使用单体采集的功能时，则提示“该产品请使用连接器批次包装采集功能”
            var count = Query<ItemQRCodeRule>().Where(p => p.ItemId == blue.ItemId).Count();
            if (count == 0)
            {
                //MES系统产生单件二维码，扫描装箱只能用单体包装（汇流排包装+连接器单体包装）
                var snCount = Query<WipPressure>().Where(p => p.WorkOrder.No == blue.ProductionNo).Count();
                if (snCount > 0)
                {
                    return "该工单已产生单件二维码，扫描装箱只能用单体包装，请维护【包装物料二维码规则关系】";
                }
                return "该产品请使用【连接器批次包装采集】功能";
            }
            return string.Empty;
        }

        /// <summary>
        /// 扫码刻码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceName"></param>
        /// <param name="woInfo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("扫描刻码")]
        [ApiLog]
        public virtual ConnectorSnData ScanSnLabel([ApiParameter("扫描刻码标签")] string barcode, double resourceId, string resourceName, ConnectorSnData woInfo, int deleteIdent = 0)
        {
            ConnectorSnData info = new ConnectorSnData();
            info = woInfo;
            info.Error = "";
            info.Tips = "";
            info.Scan = 2;
            var list = info.PackingDetail;
            if (list == null)
            {
                list = new List<ConnectorPacking>();
            }
            //批次已装箱数
            int BatchInt = 0;
            //批次装箱总数
            int BatchZInt = 0;
            //装箱已装箱数
            int BlueInt = 0;
            //蓝标装箱总数
            int BlueZInt = 0;
            //包装采集主表
            PackingQc packingQc = new PackingQc();
            //界面显示明细
            PackingDetail packingDetail = new PackingDetail();
            EngraveSn engraveSn = new EngraveSn();
            //刻码标签
            //EngraveLabel engrave = new EngraveLabel();
            //engrave = RT.Service.Resolve<EngraveLabelController>().BoolEngraveLabel("");

            var blueBable = RT.Service.Resolve<PackingQcController>().AllBlueLable(info.XtBlue);
            //根据蓝标获取工单
            var WorkOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo(blueBable.ProductionNo);
            //获取产品
            var itemData = RT.Service.Resolve<WipBatchController>().GetItem(WorkOrder.ProductId);

            if (deleteIdent == 1)
            {
                return DeleteLabel(woInfo.XtBlue, barcode, itemData, info);

            }
            else
            {
                if (blueBable.CreateDeleteident == "删除")
                {
                    throw new ValidationException("删除的蓝标不允许其他操作，只能移除!");
                }
            }


            if (RT.Service.Resolve<EngraveLabelController>().BoolEngraveSn(barcode))
            {
                throw new ValidationException("刻码SN已经存在!");
            }

            //刻码标签
            EngraveLabel engrave = new EngraveLabel();

            engrave = RT.Service.Resolve<EngraveLabelController>().BoolEngraveLabel(info.BatchLable);
            if (engrave != null)
            {
                BatchZInt = (int)engrave.Qty;

                //之前已扫描过的+当前扫描的这条 > 批次标签的数量，就不能再扫描进去
                if (engrave.EngraveSnList.Count + 1 > engrave.Qty)
                    throw new ValidationException("当前批次标签数量已满，请重新扫描批次标签".L10N());
            }

            var dispatchTasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByWoPacking(WorkOrder.Id, resourceId);

            if (dispatchTasks.Count == 0)
            {
                throw new ValidationException("工单【{0}】,资源【{1}】,工序【包装】对应的派工任务单不存在!".L10nFormat(WorkOrder.No, resourceName));
            }
            var taskRemainQty = (int)dispatchTasks.Sum(p => p.RemainQty);
            if (taskRemainQty <= 0)
                throw new ValidationException("当前资源对应的任务单没有剩余可报工数");

            #region 包装规则验证
            ValidItemQRCodeRule(itemData.Id, barcode);

            var wipResource = RT.Service.Resolve<WipResourceController>().GetWipResource(resourceId);
            if (dispatchTasks.Count == 0)
            {
                throw new ValidationException("工单【{0}】,资源【{1}】,工序【包装】对应的派工任务单不存在!".L10nFormat(WorkOrder.No, wipResource.Name));
            }
            decimal trq = 0;
            //任务单与剩余可报工数字典，用于后面拆分标签的时候使用
            Dictionary<double, decimal> taskDic = new Dictionary<double, decimal>();
            decimal NeedQty = 0;
            foreach (var dispatchTask in dispatchTasks)
            {
                var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(dispatchTask);

                var packingTasks = RT.Service.Resolve<WipBatchController>().GetWipBatchesByPackingTaskIds(new List<double>() { dispatchTask.Id });
                //剩余最大可包装数
                var RemainQty = tuple.Item2;
                var maxReportQty = tuple.Item1;

                //需要装箱数
                NeedQty += maxReportQty;

                //如果剩余可包装数，已经超了，就直接跳过
                if (RemainQty <= 0)
                    continue;

                taskDic.Add(dispatchTask.Id, RemainQty);
            }
            if (taskDic.Count > 0)
            {
                taskDic = taskDic.OrderBy(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
                trq = taskDic.Sum(p => p.Value);
            }

            if (trq <= 0)
            {
                throw new ValidationException("当前资源对应的任务单没有剩余可报工数".L10N());
            }

            var details = RT.Service.Resolve<PackingQcController>().GetPackingDetails(resourceId, WorkOrder.No);
            if (details.Sum(p => p.PackingNum) + 1 > trq)
            {
                throw new ValidationException("数量超过了剩余可报工数".L10N());
            }

            #endregion



            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DB.Update<BlueLable.BlueLable>().Set(p => p.Id, blueBable.Id).Where(p => p.Id == blueBable.Id).Execute();   //并发锁

                packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(info.XtBlue);
                if (packingQc != null)
                {
                    //蓝标装箱数
                    var packdetails = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                    BlueInt = packdetails.Sum(p => p.PackingNum);
                    //批次装箱数
                    var packdetailBatch = RT.Service.Resolve<PackingQcController>().GetPackingDetailsBatchByids(packingQc.Id, info.BatchLable);
                    BatchInt = packdetailBatch.Sum(p => p.PackingNum);
                }

                BlueInt += 1;
                BatchInt += 1;
                info.BatchInt += 1;
                info.BlueInt += 1;

                if (BatchInt > BatchZInt)
                {
                    if (BlueInt != blueBable.PackageNum)
                    {
                        BlueInt -= 1;
                        BatchInt -= 1;
                        info.BlueInt = BlueInt;
                        info.BatchInt = 0;
                        info.BatchZInt = 0;
                        info.BatchLable = "";
                        info.Scan = 1;
                        info.Tips = "该批次已经装箱完成,请输入新的批次标签!";
                        return info;
                    }
                }

                #region 生成QC装箱数据
                ConnectorPacking pqc = new ConnectorPacking();
                pqc.BlueLabel = info.XtBlue;
                pqc.ItemId = itemData.Id;
                pqc.ItemName = itemData.Name;
                pqc.ItemCode = itemData.Code;
                pqc.PackIdent = "不满箱";
                pqc.Confirm = "是";
                pqc.PackingNum = 1;
                pqc.BatchLabel = info.BatchLable;
                pqc.ProductLabel = barcode;
                list.Add(pqc);

                if (packingQc == null)
                {
                    packingQc = new PackingQc();
                    packingQc.BlueLabel = info.XtBlue;
                    packingQc.Confirm = ConfirmEnum.YES;
                    packingQc.PackIdent = PackIdentEnum.NotFullTank;
                    packingQc.ProductLabel = barcode;
                    packingQc.ItemId = itemData.Id;
                    packingQc.ItemName = itemData.Name;
                    packingQc.BlueLableNum = blueBable.PackageNum;
                    packingQc.PackingNum = BlueInt;
                    packingQc.ResourceId = resourceId;
                    packingQc.BoxState = BoxStateEnum.YES;
                    packingQc.ReportsType = ReportsTypeEnum.NO;
                }
                blueBable.IsPack = true;
                packingQc.ResourceId = resourceId;
                packingQc.PackingNum = BlueInt;
                packingQc.PackIdent = BlueInt >= blueBable.PackageNum ? PackIdentEnum.FullTank : PackIdentEnum.NotFullTank;
                if (packingQc.PackIdent == PackIdentEnum.FullTank)
                    packingQc.BoxState = BoxStateEnum.NO;
                RF.Save(packingQc);
                RF.Save(blueBable);

                //PackingDetail packingDetail = new PackingDetail();
                packingDetail.PackingNum = 1;
                packingDetail.ProductLabel = barcode;
                packingDetail.PackingQcId = packingQc.Id;
                packingDetail.Confirm = ConfirmEnum.YES;
                packingDetail.BatchLabel = info.BatchLable;
                packingDetail.WorkOrderNo = WorkOrder.No;
                packingDetail.LabelType = LabelTypeEnum.KmLabel;
                packingDetail.ReportsType = ReportsTypeEnum.NO;
                RF.Save(packingDetail);

                engraveSn.EngraveLabelId = engrave.Id;
                engraveSn.Sn = barcode;
                RF.Save(engraveSn);

                if (packingQc.PackIdent == PackIdentEnum.FullTank)
                {
                    PackingReportRecord record = new PackingReportRecord();
                    record.BlueLabel = blueBable.BlueLableBox;
                    record.BeginDate = DateTime.Now;
                    record.Report = ReportType.PDAItemLabelSum;
                    info.Tips = "已经装箱完成,请输入蓝标标签!";
                    string reportMessage = RT.Service.Resolve<PackingQcController>().SubmitData(packingQc, false);
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

                #endregion

                tran.Complete();
            }
            return info;
        }

        /// <summary>
        /// 开箱
        /// </summary>
        /// <param name="barcode">扫描信息</param>
        /// <param name="woInfo"></param>
        [ApiService("开箱")]
        public virtual ConnectorSnData NewUnboxing([ApiParameter("开箱")] string barcode, ConnectorSnData woInfo)
        {
            if (barcode == "")
            {
                throw new PlatformException("蓝标不能为空!");
            }
            ConnectorSnData info = new ConnectorSnData();
            info = woInfo;
            List<ConnectorPacking> list = new List<ConnectorPacking>();
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
                ConnectorPacking pqc = new ConnectorPacking();
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
            info.Tips = "已开箱请输入批次标签!";
            info.Scan = 1;
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
                var detailSum = packdetails.Count();

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
                            message = RT.Service.Resolve<PackingQcController>().SubmitData(packingQc, false);
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
        /// 移除批次标签
        /// </summary>
        /// <param name="xtBlue">蓝标</param>
        /// <param name="barcode">批次标签</param>
        /// <param name="itemData">物料</param>
        public virtual ConnectorSnData DeleteLabel(string xtBlue, string barcode, Item itemData, ConnectorSnData info)
        {
            List<ConnectorPacking> list = new List<ConnectorPacking>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //刻码批次
                var engrave = RT.Service.Resolve<EngraveLabelController>().BoolEngraveLabel(info.BatchLable);
                if (engrave == null)
                {
                    throw new ValidationException("批次标签不存在!");
                }
                var enSn = RT.Service.Resolve<EngraveLabelController>().GetEngraveSn(barcode);
                if (enSn == null)
                {
                    throw new ValidationException("刻码标签不存在!");
                }
                //主表
                var packingQc = RT.Service.Resolve<PackingQcController>().GetPackingQc(xtBlue);
                if (packingQc == null)
                {
                    throw new ValidationException("此蓝标没有包装!");
                }
                //从表
                var packingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                var packingBatch = packingDetailList.Where(p => p.BatchLabel == info.BatchLable).ToList();
                //var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(barcode);
                if (packingDetailList.Count > 0)
                {
                    if (packingBatch.Count > 0)
                    {
                        var packDetail = packingDetailList.Where(p => p.ProductLabel == barcode && p.BatchLabel == info.BatchLable).FirstOrDefault();
                        if (packDetail != null)
                        {
                            if (packDetail.ReportsType == ReportsTypeEnum.NO)
                            {
                                info.BlueInt -= 1;
                                info.BatchInt -= 1;
                                packingQc.PackingNum -= 1;
                                packDetail.PersistenceStatus = PersistenceStatus.Deleted;
                                RF.Save(packDetail);
                                enSn.PersistenceStatus = PersistenceStatus.Deleted;
                                RF.Save(enSn);
                                //wipBatch.Isuse = false;
                                //RF.Save(wipBatch);
                                if (packingDetailList.Count == 1)
                                {
                                    packingQc.PersistenceStatus = PersistenceStatus.Deleted;
                                    info.PackingDetail = null;
                                }
                                else
                                {
                                    if (packingBatch.Count == 1)
                                    {
                                        engrave.PersistenceStatus = PersistenceStatus.Deleted;
                                        RF.Save(engrave);
                                    }

                                    var newPackingDetailList = RT.Service.Resolve<PackingQcController>().GetPackingDetailsByids(packingQc.Id);
                                    foreach (var item in newPackingDetailList)
                                    {
                                        ConnectorPacking pqc = new ConnectorPacking();
                                        pqc.BlueLabel = xtBlue;
                                        pqc.Confirm = "是";
                                        pqc.PackIdent = "不满箱";
                                        pqc.ProductLabel = item.ProductLabel;
                                        pqc.BatchLabel = item.BatchLabel;
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
                                throw new ValidationException("该标签【" + barcode + "】已经报工,没法移除!");
                            }
                        }
                        else
                        {
                            throw new ValidationException("该刻码标签不存在,刻码标签【" + barcode + "】!");
                        }
                    }
                    else
                    {
                        throw new ValidationException("该批次标签下,已经没有刻码标签!");
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

    }
}
