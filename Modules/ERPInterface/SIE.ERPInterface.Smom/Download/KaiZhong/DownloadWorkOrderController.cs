using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using DocumentFormat.OpenXml.Spreadsheet;
using IronPython.Compiler.Ast;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Bcpg.OpenPgp;
using SIE.Api;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.WorkOrders;
using SIE.ERPInterface.Common.Datas;
using SIE.EventMessages.ProductPanelQtys;
using SIE.EventMessages.WorkOrders;
using SIE.Items;
using SIE.Items.Items;
using SIE.Items.KzItemCategorys;
using SIE.Items.ProductFamilys;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.Outsourcing;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.MES.WorkOrders.Interfaces;
using SIE.MES.WorkOrders.WorkOrderPackageGenerators;
using SIE.MES.WorkOrders.WorkOrderProcessBomGenerators;
using SIE.ProductIntfc.OutputProducts;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Security;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.ImportRoutings;
using SIE.Tech.Routings.Technologys;
using SIE.Tech.Routings.ViewModels;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IContainer = SIE.Tech.Routings.Technologys.IContainer;
using InvOrg = SIE.Rbac.InvOrgs.InvOrg;
using Item = SIE.Items.Item;
using ProcessInfo = SIE.Tech.Routings.ImportRoutings.ProcessInfo;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;
using WorkOrderBom = SIE.MES.WorkOrders.WorkOrderBom;
using WorkOrderController = SIE.MES.WorkOrders.WorkOrderController;
using WorkOrderData = SIE.KZ.Base.Interfaces.Datas.WorkOrderData;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{

    public class DownloadWorkOrderController : DomainController
    {
        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveWorkOrder(WorkOrderData data)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = data.workOrderInfs.Count };
            List<WorkOrderInf> list = new List<WorkOrderInf>();
            var dataJson = JsonConvert.SerializeObject(data);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;


            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.WorkOrder, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, data.workOrderInfs.Count);

            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

            try
            {
                if (data.workOrderInfs != null || data.workOrderInfs.Count > 0)
                {
                    var productCodes = data.workOrderInfs.Select(p => p.MATNR).Distinct().ToList();
                    var itemCodes = data.bomInfs.Select(p => p.MATNR).Distinct().ToList();
                    //获取产品
                    var products = RT.Service.Resolve<ItemController>().GetItems(productCodes, new EagerLoadOptions().LoadWithViewProperty());
                    //获取物料
                    var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty());
                    //获取工厂
                    var factoryCodes = data.workOrderInfs.Select(p => p.WERKS).Distinct().ToList();
                    var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);
                    //获取车间
                    var WorkShopCodes = data.workOrderInfs.Select(p => p.DISPO).Distinct().ToList();
                    var workShops = RT.Service.Resolve<EnterpriseController>().GetWorkShopByCodes(WorkShopCodes);

                    //获取已存在的工单号
                    var woNos = data.workOrderInfs.Select(p => p.AUFNR).Distinct().ToList();
                    var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(woNos);

                    foreach (var item in data.workOrderInfs)
                    {
                        //判断是否报错,false:不报错,true:就是报错了
                        var isErr = false;
                        try
                        {
                            var wo = wos.FirstOrDefault(p => p.No == item.AUFNR);
                            var bomInfs = data.bomInfs.Where(p => p.AUFNR == item.AUFNR).ToList();
                            var layoutInfs = data.layoutInfs.Where(p => p.AUFNR == item.AUFNR).ToList();

                            //如果工单是关闭，且状态为REL，就代表又要打开这张工单
                            if (wo!= null && wo.State == WorkOrderState.Close && item.STATU == "REL")
                            {
                                OpenWorkOrder(ref wo);
                                //如果不是发放状态，只打开不做更新
                                if (wo.State != WorkOrderState.Release)
                                    continue;
                            }

                            var product = products.FirstOrDefault(p => p.Code == item.MATNR);
                            if (product == null)
                                throw new ValidationException("产品[{0}]不存在!".L10nFormat(item.MATNR));
                            var its = items.Where(p => bomInfs.Any(a => a.MATNR == p.Code)).ToList();
                            var factory = factorys.FirstOrDefault(p => p.Code == item.WERKS);
                            if (factory == null)
                                throw new ValidationException("工厂[{0}]不存在!".L10nFormat(item.WERKS));
                            var workShop = workShops.FirstOrDefault(p => p.Code == item.DISPO);
                            if (workShop == null)
                                throw new ValidationException("车间[{0}]不存在!".L10nFormat(item.DISPO));

                            //如果关闭工单就不需要校验工单状态
                            if (wo != null && wo.State != WorkOrderState.Release && item.STATU != "TECO")
                                throw new ValidationException("只有发放状态的工单才能进行修改".L10N());
                            using (var tran = DB.TransactionScope("INTERFACE"))
                            {
                                wo = CreateWorkOrder(wo, item, bomInfs, layoutInfs, product, its, factory, workShop, invOrg);
                                tran.Complete();
                            }
                            if (wo != null && wos.All(p => p.Id != wo.Id))
                                wos.Add(wo);
                        }
                        catch (Exception ex)
                        {
                            isErr = true;
                            throw new ValidationException($"工单号{item.AUFNR}:" + ex.GetBaseException()?.Message);
                        }
                        finally {
                            if (isErr == false)
                            {
                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<WorkOrderData>(erpDataInfLog, new List<WorkOrderData>() { data }, apiResult);

                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }

                var parentItemInfs = data.parentItemInfs.ToList();
                if (parentItemInfs.Count > 0)
                {
                    var itemCodes = parentItemInfs.Select(p => p.MATNR).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItemDataList(itemCodes);
                    //获取父级物料
                    var Smatnrs = parentItemInfs.Select(p => p.SMATNR).Distinct().ToList();
                    var parentItems = RT.Service.Resolve<ItemController>().GetItemDataList(Smatnrs);

                    foreach (var parentItemInf in parentItemInfs)
                    {
                        try
                        {
                            var item = items.FirstOrDefault(p => p.Code == parentItemInf.MATNR);
                            if (item == null)
                                throw new ValidationException("物料{0}不存在".L10nFormat(parentItemInf.MATNR));

                            var parentItem = parentItems.FirstOrDefault(p => p.Code == parentItemInf.SMATNR);
                            if (parentItem == null)
                                throw new ValidationException("上层物料{0}不存在".L10nFormat(parentItemInf.SMATNR));

                            //同步父级物料
                            SyncParentItem(parentItemInf, item);
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"物料{parentItemInf.MATNR}父级物料信息同步失败:" + ex.GetBaseException().Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = data.workOrderInfs.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.Add(data);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<WorkOrderData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveWorkOrder(WorkOrderData data, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = data.workOrderInfs.Count };

            List<WorkOrderInf> list = new List<WorkOrderInf>();
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            var dataJson = JsonConvert.SerializeObject(data);
            int failCount = 0;

            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            try
            {
                if (data.workOrderInfs != null || data.workOrderInfs.Count > 0)
                {
                    var productCodes = data.workOrderInfs.Select(p => p.MATNR).Distinct().ToList();
                    var itemCodes = data.bomInfs.Select(p => p.MATNR).Distinct().ToList();
                    //获取产品
                    var products = RT.Service.Resolve<ItemController>().GetItems(productCodes, new EagerLoadOptions().LoadWithViewProperty());
                    //获取物料
                    var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes, new EagerLoadOptions().LoadWithViewProperty());
                    //获取工厂
                    var factoryCodes = data.workOrderInfs.Select(p => p.WERKS).Distinct().ToList();
                    var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);
                    //获取车间
                    var WorkShopCodes = data.workOrderInfs.Select(p => p.DISPO).Distinct().ToList();
                    var workShops = RT.Service.Resolve<EnterpriseController>().GetWorkShopByCodes(WorkShopCodes);

                    //获取已存在的工单号
                    var woNos = data.workOrderInfs.Select(p => p.AUFNR).Distinct().ToList();
                    var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(woNos);

                    foreach (var item in data.workOrderInfs)
                    {
                        //判断是否报错,false:不报错,true:就是报错了
                        var isErr = false;
                        try
                        {
                            var wo = wos.FirstOrDefault(p => p.No == item.AUFNR);

                            //如果工单是关闭，且状态为REL，就代表又要打开这张工单(只打开不做更新，因为他们可能打开后不再是发放状态了)
                            if (wo != null && wo.State == WorkOrderState.Close && item.STATU == "REL")
                            {
                                OpenWorkOrder(ref wo);
                                //如果不是发放状态，只打开不做更新
                                if (wo.State != WorkOrderState.Release)
                                    continue;
                            }

                            var bomInfs = data.bomInfs.Where(p => p.AUFNR == item.AUFNR).ToList();
                            var layoutInfs = data.layoutInfs.Where(p => p.AUFNR == item.AUFNR).ToList();
                            var product = products.FirstOrDefault(p => p.Code == item.MATNR);
                            if (product == null)
                                throw new ValidationException("产品[{0}]不存在!".L10nFormat(item.MATNR));
                            var its = items.Where(p => bomInfs.Any(a => a.MATNR == p.Code)).ToList();
                            var factory = factorys.FirstOrDefault(p => p.Code == item.WERKS);
                            if (factory == null)
                                throw new ValidationException("工厂[{0}]不存在!".L10nFormat(item.WERKS));
                            var workShop = workShops.FirstOrDefault(p => p.Code == item.DISPO);
                            if (workShop == null)
                                throw new ValidationException("车间[{0}]不存在!".L10nFormat(item.DISPO));
                            //如果关闭工单就不需要校验工单状态
                            if (wo != null && wo.State != WorkOrderState.Release && item.STATU != "TECO")
                                throw new ValidationException("只有发放状态的工单才能进行修改".L10N());

                            using (var tran = DB.TransactionScope("INTERFACE"))
                            {
                                wo = CreateWorkOrder(wo, item, bomInfs, layoutInfs, product, its, factory, workShop, invOrg);
                                tran.Complete();
                            }
                            if (wo != null && wos.All(p => p.Id != wo.Id))
                                wos.Add(wo);

                        }
                        catch (Exception ex)
                        {
                            isErr = true;
                            apiResult.ErrorList.Add($"工单号{item.AUFNR}:" + ex.GetBaseException()?.Message);
                            failCount++;
                            continue;
                        }
                        finally
                        {
                            if (isErr == false)
                            {
                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }


                var parentItemInfs = data.parentItemInfs.ToList();
                if (parentItemInfs.Count > 0)
                {
                    var itemCodes = parentItemInfs.Select(p => p.MATNR).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItemDataList(itemCodes);
                    //获取父级物料
                    var Smatnrs = parentItemInfs.Select(p => p.SMATNR).Distinct().ToList();
                    var parentItems = RT.Service.Resolve<ItemController>().GetItemDataList(Smatnrs);

                    foreach (var parentItemInf in parentItemInfs)
                    {
                        try
                        {
                            var item = items.FirstOrDefault(p => p.Code == parentItemInf.MATNR);
                            if (item == null)
                                throw new ValidationException("物料{0}不存在".L10nFormat(parentItemInf.MATNR));

                            var parentItem = parentItems.FirstOrDefault(p => p.Code == parentItemInf.SMATNR);
                            if (parentItem == null)
                                throw new ValidationException("上层物料{0}不存在".L10nFormat(parentItemInf.SMATNR));

                            //同步父级物料
                            SyncParentItem(parentItemInf, item);
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"物料{parentItemInf}父级物料信息同步失败:" + ex.GetBaseException().Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = data.workOrderInfs.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<WorkOrderData>(erpDataInfLog, data.workOrderInfs.Count, new List<WorkOrderData> { data }, apiResult, false);
            }
            return apiResult;
        }

        /// <summary>
        /// 打开工单
        /// </summary>
        /// <param name="workOrder"></param>
        public virtual void OpenWorkOrder(ref WorkOrder workOrder)
        {
            using (var tran = DB.TransactionScope("INTERFACE"))
            {
                //打开任务单
                RT.Service.Resolve<DispatchController>().OpenTasks(workOrder.Id);
                //打开工单
                RT.Service.Resolve<WorkOrderController>().OpenWorkOrder(workOrder.Id);

                workOrder = RF.GetById<WorkOrder>(workOrder.Id, new EagerLoadOptions().LoadWithViewProperty());
                tran.Complete();
            }
        }

        private WorkOrder CreateWorkOrder(WorkOrder workOrder, WorkOrderInf woInf, List<BomInf> bomInfs, List<LayoutInf> layoutInfs, Item product, List<Item> items, Enterprise factory, Enterprise workShop, InvOrg invOrg)
        {
            //当工序明细的工厂没有一条是和工单主表同一工厂的，那么就不给它保存到工单对应的库存组织中去
            if (woInf.WERKS == invOrg.ExternalId && layoutInfs.All(p => p.WERKS != woInf.WERKS))
                return workOrder;

            bool isDeleteTask = false;
            if (workOrder == null)
            {
                workOrder = new WorkOrder()
                {
                    No = woInf.AUFNR,
                    Source = SIE.Common.SourceType.External,
                    State = Core.WorkOrders.WorkOrderState.Release,
                    MakeDate = DateTime.Now,
                };

                isDeleteTask = true;

                //新建关闭状态工单
                if (woInf.STATU == "TECO")
                {
                    workOrder.State = WorkOrderState.Close;
                    //关闭就不需要再创建任务单
                    isDeleteTask = false;
                }
                workOrder.GenerateId();
                workOrder.PersistenceStatus = PersistenceStatus.New;

            }
            else
            {
                ////如果已经关闭了，然后再打开
                //if (workOrder.State == WorkOrderState.Close && woInf.STATU == "REL")
                //{
                //    workOrder.PersistenceStatus = PersistenceStatus.Modified;
                //    workOrder.State = workOrder.ClosingState.Value;
                //    //重新打开任务单
                //    var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(workOrder.Id);
                //    foreach (var task in tasks)
                //    {
                //        task.PersistenceStatus = PersistenceStatus.Modified;
                //        task.TaskStatus = task.OldTaskStatus == null ? DispatchTaskStatus.ToDispatch : task.OldTaskStatus.Value;
                //    }
                //    RF.Save(tasks);
                //}

                //如果存在工单且原状态为未关闭，就关闭工单
                if (woInf.STATU == "TECO")
                {
                    if (workOrder.State != WorkOrderState.Close)
                    {
                        //获取这张工单下的所有任务单，然后关闭(原工单关闭逻辑是，必须先关闭对应的任务单，才能关闭工单)
                        var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(workOrder.Id).Where(p => p.TaskStatus != DispatchTaskStatus.Closed).ToList();

                        //把已经完成的也关闭，他们提的要求
                        //foreach (var p in tasks.Where(p => p.TaskStatus == DispatchTaskStatus.Finished))
                        //{
                        //    p.OldTaskStatus = p.TaskStatus;
                        //    p.TaskStatus = DispatchTaskStatus.Closed;
                        //    p.PersistenceStatus = PersistenceStatus.Modified;
                        //    RF.Save(p);
                        //    // 更新操作日志
                        //    //var log = RT.Service.Resolve<ReportController>().UpdateTaskOptStopLog(new EntityList<DispatchTask>() { p });
                        //}

                        //先暂停
                        var selectedIds = tasks.Where(p => p.TaskStatus != DispatchTaskStatus.Pause).Select(p => p.Id).Distinct().ToList();
                        RT.Service.Resolve<DispatchController>().SetPauseTasks(selectedIds);
                        selectedIds = tasks.Select(p => p.Id).Distinct().ToList();
                        //关闭任务单
                        RT.Service.Resolve<DispatchController>().SetCloseTasks(selectedIds);
                        //先暂停工单
                        if (workOrder.IsPause == YesNo.No && workOrder.State != WorkOrderState.Finish)
                            RT.Service.Resolve<WorkOrderController>().Pause(workOrder.Id, "SAP关闭工单");
                        //关闭工单
                        RT.Service.Resolve<WorkOrderController>().Close(workOrder.Id, "SAP关闭工单");
                    }
                    return workOrder;
                }

                //当工单数量改变了，就要删除原有任务单
                if (workOrder.PlanQty != woInf.GAMNG)
                {
                    isDeleteTask = true;
                }
                //当同一个工序，工作中心不同或者工序不同就要删除任务单
                if (workOrder.LayoutInfoList.Count != layoutInfs.Count || workOrder.LayoutInfoList.Any(p => layoutInfs.All(a => a.AUFPL != p.Aufpl && a.APLZL != p.Aplzl)) || workOrder.LayoutInfoList.Any(p => layoutInfs.Any(a => a.AUFPL == p.Aufpl && a.APLZL == p.Aplzl && (a.ARBPL != p.WorkCenterCode || p.ProcessCode != a.KTSCH))))
                {
                    isDeleteTask = true;
                }
            }
            workOrder.PlanQty = woInf.GAMNG;
            workOrder.OrderQty = woInf.GAMNG;
            workOrder.Ztfl = woInf.ZTFL;
            workOrder.BatchNo = woInf.CHARG;
            workOrder.Uebto = woInf.UEBTO;
            workOrder.Fevor = woInf.FEVOR;
            workOrder.Lgort = woInf.LGORT;
            workOrder.Remark = woInf.TEXT;
            workOrder.OrderNo = woInf.AUFNR2;
            if (DateTime.TryParseExact(woInf.GSTRP, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime beginTime))
            {
                workOrder.PlanBeginDate = beginTime;
            }
            if (DateTime.TryParseExact(woInf.GLTRP, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endTime))
            {
                workOrder.PlanEndDate = endTime;
            }
            if (woInf.DAUAT == "KZ01")
                workOrder.Type = Core.WorkOrders.WorkOrderType.Mass;
            if (woInf.DAUAT == "KZ02")
                workOrder.Type = Core.WorkOrders.WorkOrderType.Pilot;
            if (woInf.DAUAT == "KZ03" || woInf.DAUAT == "KZ04" || woInf.DAUAT == "KZ05")
                workOrder.Type = Core.WorkOrders.WorkOrderType.Rework;
            workOrder.DAUAT = woInf.DAUAT;
            workOrder.FactoryId = factory.Id;
            workOrder.WorkShopId = workShop.Id;
            workOrder.ProductId = product.Id;


            //创建工单BOM
            //把旧的全给删了，创新创建覆盖
            workOrder.BomList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            var deleteBoms = new EntityList<WorkOrderBom>();
            deleteBoms.AddRange(workOrder.BomList);
            workOrder.BomList.Clear();
            //通过接口获取集团【是否启用制卡数量维护】数据
            EntityList<MtartZtflRelation> ztflRelations = RT.Service.Resolve<SmomBaseController>().GetMtartZtflRelations(new List<string>() { workOrder.Product.Mtart });

            foreach (var bomInf in bomInfs)
            {
                var item = items.FirstOrDefault(p => p.Code == bomInf.MATNR);
                //如果这个物料在这个工厂里面找不到，就认为是需要在这个生产的(暂定如此)(可能会出现物料只是接口还没传过来(晚过来了)，并不是不在这个工厂中)
                if (item == null)
                    continue;
                    //throw new ValidationException("工单BOM物料[{0}]不存在!".L10nFormat(bomInf.MATNR));
                var bom = new WorkOrderBom();
                bom.ItemId = item.Id;
                bom.Item = item;

                var SingleQty = workOrder.PlanQty == 0 ? 1 : Math.Truncate((bomInf.BDMNG / workOrder.PlanQty) * 100000000) / 100000000;
                //启用制卡数量的时候，用制卡数量去计算
                if (ztflRelations.Any(p => p.IsZtfl == true))
                {
                    SingleQty = (workOrder.Ztfl == 0 || workOrder.Ztfl == null) ? 1 : Math.Truncate((bomInf.BDMNG * 100000000) / workOrder.Ztfl.Value)  / 100000000;
                }
                bom.SingleQty = SingleQty;

                bom.Rsnum = bomInf.RSNUM;
                bom.Rspos = bomInf.RSPOS;
                bom.Posnr = bomInf.POSNR;
                bom.Bwart = bomInf.BWART;
                bom.Enmng = bomInf.ENMNG;
                bom.Lgort = bomInf.LGORT;
                bom.Werks = bomInf.WERKS;
                bom.Meins = bomInf.MEINS;
                bom.RequireQty = bomInf.BDMNG;
                bom.IsRecoilItem = false;
                bom.IsByBill = false;
                if (bomInf.DUMPS == "X")
                    bom.IsVritualItem = true;
                else
                    bom.IsVritualItem = false;
                bom.PersistenceStatus = PersistenceStatus.New;
                workOrder.BomList.Add(bom);
            }
            //创建联副产品
            CreateWorkOrderOutputProductList(workOrder);

            bool ProcessBomDirty = false;
            //把旧的工序BON拿出来，用于下面判断是否要更新任务单的工序BOM，否则下面方法会重新生成工单工序BOM，旧的会被删掉
            var oldsProcessBoms = workOrder.ProcessBomList;

            //创建工艺路线、工序BOM等
            CreateLayout(workOrder, layoutInfs);
            RF.Save(deleteBoms);
            RF.Save(workOrder);
            //增加两个判断，分别是旧工序BOM对新工序BOM、新工序BOM对旧工序BOM的物料、单机定额、工序进行判断，只有有一个不满足，就代表工序BOM改变了
            //分成两个判断因为，即使数量相同也有可能单机定额、物料、工序不同
            //旧工序BOM对新工序BOM进行判断
            if (oldsProcessBoms.All(a => workOrder.ProcessBomList.Any(f => f.ItemId == a.ItemId && f.SingleQty == a.SingleQty && f.ProcessId == a.ProcessId)))
            { }
            else
            {
                ProcessBomDirty = true;
            }
            //新工序BOM对旧工序BOM进行判断
            if (workOrder.ProcessBomList.All(f => oldsProcessBoms.Any(a => a.ItemId == f.ItemId && f.SingleQty == a.SingleQty && f.ProcessId == a.ProcessId)))
            { }
            else
            {
                ProcessBomDirty = true;
            }

            //不删除任务单，并且需要更新任务单工序BOM，就更新任务单BOM
            if (isDeleteTask == false && ProcessBomDirty == true)
            {
                //发布工单修改更新工单任务单事件，订阅端目前只有派工管理工程，会更新任务单
                RT.EventBus.Publish(new WorkOrderUpdateDispathTaskEvent()
                {
                    WorkOrderId = workOrder.Id,
                    ProcessBomDirty = true
                });
            }


            if (workOrder.State != SIE.Core.WorkOrders.WorkOrderState.Release || workOrder.IsPause == YesNo.Yes)
            {
                //发放状态工单才可生成任务单、以及其他操作
            }
            else
            {
                if (isDeleteTask == true)
                {
                    workOrder = RF.GetById<WorkOrder>(workOrder.Id, new EagerLoadOptions().LoadWithViewProperty());
                    try
                    {
                        //创建任务单
                        GenerateTasks(workOrder, isDeleteTask, invOrg);
                    }
                    catch (Exception ex)
                    {
                        throw new ValidationException("创建任务单失败:" + ex.GetBaseException().Message);
                    }
                }
            }

            try
            {
                //创建工序委外需求单
                CreateOutsourcingRequest(workOrder, invOrg);
            }
            catch (Exception ex)
            {
                throw new ValidationException("创建工序委外需求单失败:" + ex.GetBaseException()?.Message);
            }

            return workOrder;
        }

        /// <summary>
        /// 只是为了生成委外需求单，不可随便使用
        /// </summary>
        /// <param name="keys"></param>
        //[ApiService]
        //public virtual void CreateOutsourcingRequest(List<string> keys)
        //{
        //    var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
        //    foreach (var key in keys)
        //    {
        //        var workOrder = Query<WorkOrder>().Where(p => p.No == key).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        //        CreateOutsourcingRequest(workOrder, invOrg);
        //    }
        //}

        /// <summary>
        /// 创建工序委外需求单
        /// </summary>
        /// <param name="workOrder"></param>
        public virtual void CreateOutsourcingRequest(WorkOrder workOrder, InvOrg invOrg)
        {
            //关闭原来的委外单,下面重新创建
            DB.Update<OutsourcingRequest>().Set(p => p.OutsourcingState, OutsourcingState.Close).Where(p => p.WorkOrderId == workOrder.Id).Execute();
            //首工序的地方就创建主委外单(源头发起者，因为只有首工序会产生标签，发给其他工厂去使用)
            var layoutInfo = workOrder.LayoutInfoList.OrderBy(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault();
            if (layoutInfo != null && layoutInfo.Factory == invOrg.ExternalId)
            {
                var layoutInfos = workOrder.LayoutInfoList.Where(p => p.Factory != invOrg.ExternalId).ToList();
                var processCodes = layoutInfos.Select(p => p.ProcessCode).Distinct().ToList();
                var pros = RT.Service.Resolve<ProcessController>().GetProcessesList(processCodes);

                EntityList<OutsourcingRequest> newRecords = new EntityList<OutsourcingRequest>();
                //然后找出全部不是这个工厂的工序，去创建委外需求单
                foreach (var lf in layoutInfos)
                {
                    var pro = pros.FirstOrDefault(p => p.Code == lf.ProcessCode);
                    var no = RT.Service.Resolve<OutsourcingRequestController>().GetOutsourcingRequestCode(1).FirstOrDefault();
                    var newRecord = new OutsourcingRequest()
                    {
                        NO = no,
                        WorkOrderId = workOrder.Id,
                        WorkOrder = workOrder,
                        FactoryId = 0,
                        InitiatorFactory = invOrg.ExternalId,
                        OutFactory = lf.Factory,
                        RequestQty = workOrder.PlanQty,
                        OutboundQty = 0,
                        WarehousingQty = 0,
                        BeginProcessId = workOrder.RoutingProcessList.FirstOrDefault(p => p.ProcessId == pro.Id).Id,
                        EndProcessId = workOrder.RoutingProcessList.FirstOrDefault(p => p.ProcessId == pro.Id).Id,
                        OutsourcingState = OutsourcingState.NotStarted,
                        OutboundState = OutboundState.NotStarted,
                        ReportState = ReportState.NotStarted
                    };
                    newRecords.Add(newRecord);
                }
                if (newRecords.Count > 0)
                    RF.Save(newRecords);
            }

        }

        /// <summary>
        /// 创建联副产品
        /// </summary>
        /// <param name="workOrder"></param>
        public virtual void CreateWorkOrderOutputProductList(WorkOrder workOrder)
        {
            workOrder.WorkOrderOutputProductList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);

            int rowNumber = 1;
            //联副产品不允许出现相同物料，所以先分组s
            foreach (var group in workOrder.BomList.Where(p => p.Bwart == "531").GroupBy(p => p.ItemId))
            {
                var bom = group.FirstOrDefault();
                //if (bom.Bwart != "531")
                //    continue;
                var workOrderOutputProduct = workOrder.WorkOrderOutputProductList.FirstOrDefault(p => p.ItemId == bom.ItemId);
                if (workOrderOutputProduct == null)
                {
                    workOrderOutputProduct = new WorkOrderOutputProduct();
                    workOrderOutputProduct.RowNumber = (workOrder.WorkOrderOutputProductList.Count + 1).ToString();
                }
                else
                {
                    //已经存在该物料,不要做删除
                    workOrderOutputProduct.PersistenceStatus = PersistenceStatus.Modified;
                }

                workOrderOutputProduct.ItemId = bom.ItemId;
                workOrderOutputProduct.WorkOrder = workOrder;
                workOrderOutputProduct.Qty = group.Sum(p => p.RequireQty);//bom.RequireQty;  //对相同物料BOM进行求和
                workOrderOutputProduct.OutputListType = OutputListType.ByProducts;
                workOrderOutputProduct.ItemExtProp = bom.ItemExtProp;
                workOrderOutputProduct.ItemExtPropName = bom.ItemExtPropName;

                //rowNumber += 1;

                ////生成工单联副产品列表
                workOrder.WorkOrderOutputProductList.Add(workOrderOutputProduct);
            }
        }

        //[ApiService]
        //public virtual void SaveGenerateTasks(List<string> str)
        //{
        //    var wos = Query<WorkOrder>().Where(p => str.Contains(p.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        //    var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
        //    foreach (var workOrder in wos)
        //    {
        //        var taskConfig = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
        //        if (!taskConfig.IsGenerate)
        //        {
        //            //throw new ValidationException("工单没有配置生成任务单".L10N());
        //        }
        //        else
        //        {
        //            RstTaskBillInfo billInfo = RT.Service.Resolve<DispatchController>().IsCanSyntypeReport(workOrder);
        //            bool isAccordConfig = true;
        //            if (billInfo.OrgIsSyntype == true && billInfo.IsSyntype == false)
        //            {
        //                isAccordConfig = false;
        //            }

        //            var tasks = RT.Service.Resolve<DispatchController>().CreateDispatch(workOrder, isAccordConfig, taskConfig, true, true);
        //            //获取工序属性
        //            var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();
        //            EntityList<ProcessPty> processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds);

        //            //获取首工序
        //            var processId = workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault()?.ProcessId;

        //            foreach (var task in tasks)
        //            {

        //                if (task.ProcessId == null)
        //                    throw new ValidationException("任务单未获取到工序，无法生成任务单!".L10N());
        //                //派工管理的资源字段取值修改：由原来取工单的资源，改成取工单工艺路线中工序对应的工作中心字段
        //                if (task.ProcessId != null)
        //                {
        //                    //这个工序必须要是在该工厂(当下库存组织下的)
        //                    var info = workOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == task.Process.Code && p.Factory == invOrg.ExternalId);
        //                    if (info == null)
        //                        throw new ValidationException("未找到工序[{0}]工厂[{1}]的工艺路线信息,无法生成任务单".L10nFormat(task.Process.Code, invOrg.ExternalId));
        //                    if (info != null)
        //                    {
        //                        WipResource wipResource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(info.WorkCenterCode);
        //                        if (wipResource == null)
        //                            throw new ValidationException("生产资源未维护编码[{0}],无法生成任务单".L10nFormat(info.WorkCenterCode));
        //                        if (wipResource != null)
        //                        {
        //                            //将任务单的来源状态改为MES生成
        //                            task.SourceType = MES.TaskManagement.Dispatchs.SourceType.Mes;
        //                            task.ResourceId = wipResource.Id;
        //                            //task.PersistenceStatus = PersistenceStatus.Modified;
        //                            RF.Save(task);
        //                        }

        //                    }
        //                }

        //                if (task.ProcessId != null && processPtys.Any(p => p.ProcessId == task.ProcessId && p.IsAutoDispatch == true))
        //                {
        //                    //当工序属性中维护了自动派工且资源类型必须是产线，那么生成派工单后，自动进行派工
        //                    if (task.Resource.SourceType == SyncSourceType.LineAndon)
        //                    {
        //                        var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new List<double>() { task.Id });
        //                        if (errMsg.Length == 0)
        //                        { }
        //                        else
        //                            throw new ValidationException(errMsg);
        //                    }
        //                    //当资源类型不为产线时候，除了满足非排程点 + 首工序的任务之外，其他都可以自动派工
        //                    if (task.Resource.SourceType != SyncSourceType.LineAndon)
        //                    {
        //                        //非排程点 + 首工序 任务,不要自动派工(因为他们要选择资源产线后才可以派工)
        //                        if (processId == task.ProcessId && (processPtys.Any(p => p.ProcessId == task.ProcessId && p.Scheduling == false) || processPtys.All(p => p.ProcessId != task.ProcessId)))
        //                        { }
        //                        else
        //                        {
        //                            var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new List<double>() { task.Id });
        //                            if (errMsg.Length == 0)
        //                            { }
        //                            else
        //                                throw new ValidationException(errMsg);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 创建任务单
        /// </summary>
        /// <param name="workOrder"></param>
        public virtual void GenerateTasks(WorkOrder workOrder, bool isDeleteTask, InvOrg invOrg)
        {

            var taskConfig = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            if (!taskConfig.IsGenerate)
            {
                //throw new ValidationException("工单没有配置生成任务单".L10N());
            }
            else
            {
                RstTaskBillInfo billInfo = RT.Service.Resolve<DispatchController>().IsCanSyntypeReport(workOrder);
                bool isAccordConfig = true;
                if (billInfo.OrgIsSyntype == true && billInfo.IsSyntype == false)
                {
                    isAccordConfig = false;
                }

                //先删除掉原先的派工任务单，然后重新创建新的任务单
                if (workOrder.Id > 0)
                {
                    var dispatchTasks = Query<DispatchTask>().Where(p => p.WorkOrderId == workOrder.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    var dispatchTaskIds = dispatchTasks.Select(p => p.Id).Distinct().ToList();
                    //修改排程值
                    var schedulingInfValues_1 = Query<SchedulingInfValue>().Where(p => dispatchTaskIds.Contains((double)p.DispatchTask1Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    foreach (var schedulingInfValue_1 in schedulingInfValues_1)
                    {
                        schedulingInfValue_1.PersistenceStatus = PersistenceStatus.Modified;
                        schedulingInfValue_1.DispatchTask1Id = null;
                    }
                    RF.Save(schedulingInfValues_1);
                    //修改排程值
                    var schedulingInfValues_2 = Query<SchedulingInfValue>().Where(p => dispatchTaskIds.Contains((double)p.DispatchTask2Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    foreach (var schedulingInfValue_2 in schedulingInfValues_2)
                    {
                        schedulingInfValue_2.PersistenceStatus = PersistenceStatus.Modified;
                        schedulingInfValue_2.DispatchTask2Id = null;
                    }
                    RF.Save(schedulingInfValues_2);
                    //把任务单删掉
                    dispatchTasks.ForEach(p =>
                    {
                        p.PersistenceStatus = PersistenceStatus.Deleted;
                    });
                    RF.Save(dispatchTasks);
                    //把对应的排程导入都删掉
                    var schedulingInfs = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfsByWoIds(new List<double>() { workOrder.Id });
                    schedulingInfs.ForEach(p =>
                    {
                        p.PersistenceStatus = PersistenceStatus.Deleted;
                    });
                    RF.Save(schedulingInfs);
                    //DB.Delete<DispatchTask>().Where(p => p.WorkOrderId == workOrder.Id).Execute();
                }
                var tasks = RT.Service.Resolve<DispatchController>().CreateDispatch(workOrder, isAccordConfig, taskConfig, true);
                //获取工序属性
                var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

                //获取首工序
                var processId = workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault()?.StartProcess;
                if (processId == null)
                    processId = workOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault()?.ProcessId;

                foreach (var task in tasks)
                {
                    if (task.ProcessId == null)
                        throw new ValidationException("任务单未获取到工序，无法生成任务单!".L10N());
                    //派工管理的资源字段取值修改：由原来取工单的资源，改成取工单工艺路线中工序对应的工作中心字段
                    if (task.ProcessId != null)
                    {
                        //这个工序必须要是在该工厂(当下库存组织下的)
                        var info = workOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == task.Process.Code && p.Factory == invOrg.ExternalId);
                        if (info == null)
                            throw new ValidationException("未找到工序[{0}]工厂[{1}]的工艺路线信息,无法生成任务单".L10nFormat(task.Process.Code, invOrg.ExternalId));
                        if (info != null)
                        {
                            WipResource wipResource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(info.WorkCenterCode);
                            if (wipResource == null)
                                throw new ValidationException("生产资源未维护编码[{0}],无法生成任务单".L10nFormat(info.WorkCenterCode));
                            if (wipResource != null)
                            {
                                //将任务单的来源状态改为MES生成
                                task.SourceType = MES.TaskManagement.Dispatchs.SourceType.Mes;
                                task.ResourceId = wipResource.Id;
                                //task.PersistenceStatus = PersistenceStatus.Modified;
                                RF.Save(task);
                            }

                        }
                    }

                    //返工类型的工单要自动派工,不需要走下面的校验           
                    if (workOrder.Type == WorkOrderType.Rework)
                    {
                        //工序编码含有“包装”的，不自动派工
                        if (!task.Process.Code.Contains("包装"))
                        {
                            var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new List<double>() { task.Id });
                            if (errMsg.Length == 0)
                            { }
                            else
                                throw new ValidationException(errMsg);
                        }
                        continue;
                    }

                    EntityList<ProcessPty> processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { task.ProcessId.Value }, workOrder.ProductId);
                    if (processPtys.Count < 1)
                        continue;

                    var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(workOrder.ProductId);
                    var pps = new List<ProcessPty>();
                    if (kzItemCategory != null)
                    {
                        pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                    }
                    ////当找得到分类得时候，优先找到分类的，然后再找工序的
                    if (pps.Count == 0)
                        pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                    if (task.ProcessId != null && pps.Any(p => p.ProcessId == task.ProcessId && p.IsAutoDispatch == true))
                    //if(task.ProcessId != null)
                    {
                        //if (pps.Any(p => p.ProcessId == task.ProcessId && p.IsAutoDispatch == true))
                        //{
                            //当工序属性中维护了自动派工且资源类型必须是产线，那么生成派工单后，自动进行派工
                            if (task.Resource.SourceType == SyncSourceType.LineAndon)
                            {
                                var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new List<double>() { task.Id });
                                if (errMsg.Length == 0)
                                { }
                                else
                                    throw new ValidationException(errMsg);
                            }
                            //当资源类型不为产线时候，除了满足非排程点 + 首工序的任务之外，其他都可以自动派工
                            if (task.Resource.SourceType != SyncSourceType.LineAndon)
                            {
                                //非排程点 + 首工序 任务,不要自动派工(因为他们要选择资源产线后才可以派工)
                                if (processId == task.ProcessId && (pps.Any(p => p.ProcessId == task.ProcessId && p.Scheduling == false) || pps.All(p => p.ProcessId != task.ProcessId)))
                                { }
                                else
                                {
                                    var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(new List<double>() { task.Id });
                                    if (errMsg.Length == 0)
                                    { }
                                    else
                                        throw new ValidationException(errMsg);
                                }
                            }
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// 创建工艺路线
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="layoutInfs"></param>
        private void CreateLayout(WorkOrder workOrder, List<LayoutInf> layoutInfs)
        {
            //先排个序，因为原接口下来的他们可能没排序好的，且需要应该是1->2->3->0
            List<RoutingData> routingDatas = new List<RoutingData>();
            int seq = 1;
            int BackSeq = seq + 1;
            foreach (var layoutInf in layoutInfs.OrderBy(p => Convert.ToInt32(p.VORNR)))
            {
                RoutingData routingData = new RoutingData();
                //当前工序
                routingData.seq = seq;
                //指向下一个工序
                routingData.BackSeq = BackSeq;
                routingData.Layout = layoutInf;
                seq++;
                BackSeq++;
                routingDatas.Add(routingData);
            }
            //在最后一道工序没有指向下一个工序，所以应该是指向0
            routingDatas.LastOrDefault().BackSeq = 0;

            List<ProcessInfo> _processList = new List<ProcessInfo>();
            Dictionary<string, double?> _routingDic = new Dictionary<string, double?>();

            var processCodes = layoutInfs.Select(p => p.KTSCH).Distinct().ToList();
            var pros = RT.Service.Resolve<ProcessController>().GetProcessesList(processCodes);

            var cate = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyCateByName("工单");
            if (cate == null)
            {
                cate = new ProductFamilyCategory();
                cate.Code = "工单";
                cate.Name = "工单";
                cate.PersistenceStatus = PersistenceStatus.New;
                RF.Save(cate);
            }

            //根据接口传入，先创建各个工序信息、以及验证工序
            foreach (var data in routingDatas)
            {
                var layoutInf = data.Layout;
                var processInfo = _processList.FirstOrDefault(p => p.ProcessCode == layoutInf.KTSCH);
                if (processInfo == null)
                {
                    var process = pros.FirstOrDefault(p => p.Code == layoutInf.KTSCH);
                    if (process != null)
                    {
                        var info = new ProcessInfo() { Id = process.Id, ProcessCode = process.Code, ProcessName = process.Name, IsBatch = IsBatchProcess(process.Type.Value), Type = process.Type.Value };
                        process.ParameterList.ForEach(e => info.ParamterList.Add(new ParamterInfo() { Id = e.Id, Result = e.Type, Describe = e.Description, Script = e.Script, Condition = e.Condition }));
                        _processList.Add(info);
                    }
                    else
                    {
                        throw new ValidationException("工序[{0}]不存在".L10nFormat(layoutInf.KTSCH));
                    }
                }
            }

            var routing1 = RT.Service.Resolve<RoutingController>().GetRoutingByName(workOrder.No);
            if (routing1 != null)
                _routingDic.Add(workOrder.No, routing1.Id);
            else
            {
                //不存在工艺路线就创建新的
                routing1 = new Routing()
                {
                    CategoryId = cate.Id,
                    Name = workOrder.No,
                    Description = workOrder.No
                };
                routing1.GenerateId();
                RF.Save(routing1);
                _routingDic.Add(workOrder.No, routing1.Id);
            }

            RoutingImportSaveViewModel routing = new RoutingImportSaveViewModel();
            bool isBatch = false;
            var i = 0;
            //把旧的删除，创建新的覆盖
            workOrder.LayoutInfoList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(workOrder.LayoutInfoList);
            workOrder.LayoutInfoList.Clear();
            //开始创建工艺路线图，各个节点信息
            foreach (var data in routingDatas)
            {
                var layoutInf = data.Layout;
                var processInfo = _processList.FirstOrDefault(p => p.ProcessCode == layoutInf.KTSCH);

                routing.RowNum = i;
                routing.Category = "工单";
                routing.RoutingName = workOrder.No;
                routing.RoutingDesc = workOrder.No;
                routing.IsPass = true;
                routing.CategoryId = cate.Id;

                if (_routingDic.TryGetValue(workOrder.No, out double? routingId))
                    routing.RoutingId = routingId;
                else
                    routing.RoutingId = null;
                routing.IsPass = true;
                if (processInfo == null)
                {
                    routing.IsPass = false;
                    continue;
                }

                if (string.IsNullOrEmpty(routing.Category))
                    throw new ValidationException("产品族分类不能为空".L10N());
                if (string.IsNullOrEmpty(routing.RoutingName) && string.IsNullOrEmpty(workOrder.No))
                    throw new ValidationException("工艺路线名称不能为空".L10N());

                ProcessViewModel process = new ProcessViewModel();
                routing.RowNum = i;
                process.IsBatch = processInfo.IsBatch;
                process.ProcessId = processInfo.Id;
                process.ProcessType = processInfo.Type;
                process.ProcessName = processInfo.ProcessName;
                process.SortOrder = data.seq;
                process.SortOrderBack = data.BackSeq;
                ParamterInfo resultInfo = null;
                resultInfo = processInfo.ParamterList.FirstOrDefault(p => p.Result == ResultTypeForDesign.Pass);

                if (resultInfo == null)
                    throw new ValidationException("工序[{0}]的工序参数未配置[通过]结果!".L10nFormat(processInfo.ProcessName));
                var pro = pros.FirstOrDefault(p => p.Code == layoutInf.KTSCH);
                process.Result = ResultTypeForDesign.Pass;//resultInfo.Result;
                process.ParameterId = resultInfo.Id;
                process.ResultDesc = resultInfo.Describe;
                process.Script = resultInfo.Script;
                process.Condition = resultInfo.Condition;
                process.CanChoose = pro.CanChoose;//choose;
                process.IsRepeat = pro.IsRepeat;//repeat;
                process.IsCreateSku = pro.IsCreateSku; //sku;
                process.ParallelGroup = null;
                process.IsGenerateTask = true;

                //process.IsGenerateTask = generateTask;
                routing.ProcessDetailModelList.Add(process);
                i++;
                //创建工单的工艺路线信息明细
                LayoutInfo layoutInfo = new LayoutInfo();
                layoutInfo.Vornr = layoutInf.VORNR;
                layoutInfo.ProcessCode = layoutInf.KTSCH;
                layoutInfo.WorkCenterCode = layoutInf.ARBPL;
                layoutInfo.Steus = layoutInf.STEUS;
                layoutInfo.ProcessQty = layoutInf.MGVRG;
                layoutInfo.Zcode = layoutInf.ZCODE;
                layoutInfo.Vgw01 = layoutInf.VGW01;
                layoutInfo.Vgw02 = layoutInf.VGW02;
                layoutInfo.Vgw03 = layoutInf.VGW03;
                layoutInfo.Factory = layoutInf.WERKS;
                layoutInfo.PersistenceStatus = PersistenceStatus.New;
                layoutInfo.Aufpl = layoutInf.AUFPL;
                layoutInfo.Aplzl = layoutInf.APLZL;
                layoutInfo.GenerateId();
                workOrder.LayoutInfoList.Add(layoutInfo);

                process.LayoutInfoId = layoutInfo.Id;
                process.Vornr = layoutInfo.Vornr;
                process.Steus = layoutInfo.Steus;
            }

            var rv = RT.Service.Resolve<RoutingController>().ImportRouting(routing);

            rv.IsDefault = YesNo.Yes;
            RF.Save(rv);
            var version = RT.Service.Resolve<RoutingController>().ReleaseRoutingVersion(rv.Id, rv.Layout.Layout);

            workOrder.VersionId = rv.Id;
            workOrder.Version = rv;

            var woPackageGenerator = new WoPackageGenerator(new EntityList<Item> { workOrder.Product });
            woPackageGenerator.GenerateWorkOrderPackageRule(workOrder);
            var template = woPackageGenerator.GenerateProductLabelTemplate(workOrder);
            if (template == null)
            {
                template = new Core.Items.LabelPrintTemplate();
                workOrder.Template = template;
            }
            RF.Save(template);
            workOrder.TemplateId = template.Id;

            RT.Service.Resolve<WorkOrderPropertyChanged>().GenerateRoutingProcesss(workOrder);
            RT.Service.Resolve<WorkOrderPropertyChanged>().GenerateProcessBoms(workOrder);
            RT.Service.Resolve<WorkOrderPropertyChanged>().GenerateWorkOrderProcessPackingUnit(workOrder);

            if (workOrder.VersionId.HasValue)
            {
                RT.Service.Resolve<RoutingController>().UpdateVersionRefTimes(workOrder.VersionId.Value, 1);
            }
            if (workOrder.VersionId.HasValue && workOrder.Version != null)
            {
                var layout = new WorkOrderRoutingLayout();
                layout.Layout = workOrder.Version.Layout.Layout;
                layout.PersistenceStatus = PersistenceStatus.New;
                RF.Save(layout);
                workOrder.Layout = layout;
                workOrder.LayoutId = layout.Id;
            }

            //批次属性保存
            var attacWoWipBatch = workOrder.GetProperty(WoWipBatchExt.AttacWoWipBatchProperty);
            if (attacWoWipBatch != null)
            {
                attacWoWipBatch.WorkOrderId = workOrder.Id;
                RF.Save(attacWoWipBatch);
            }
        }



        /// <summary>
        /// 工单bom和包装规则数据处理(用于前端正确显示)
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void HandleBomAndPackData(WorkOrder workOrder)
        {
            var now = RF.Find<WorkOrder>().GetDbTime();
            workOrder.BomList.ForEach(p =>
            {
                p.ItemCode = p.Item.Code; p.ItemName = p.Item.Name;
                p.CreateDate = now; p.UpdateDate = now;
                p.ExtValues["ItemId_Display"] = p.Item.Code;
            });
            workOrder.PackageRuleDetailList.ForEach(p =>
            {
                p.ExtValues.Add("NumberRuleId_Display", p.NumberRule?.Name);
                p.PackageUnitName = p.PackageUnit.Name;
                p.CreateDate = now; p.UpdateDate = now;
                p.WorkOrderProcessPackingUnitList.ForEach(w => { w.UpdateDate = now; w.CreateDate = now; });
            });
        }

        /// <summary>
        /// 生成工序清单
        /// </summary>
        private WorkOrder CreateWorkOrderRoutingProcess(WorkOrder workOrder, string Layout)
        {
            ContainerModel container = new ContainerModel();
            container.Deserialize(Layout);
            container.ValidateSave();
            bool isPassRate = false;
            StringBuilder sb = new StringBuilder();
            //记录开始工序的Activity
            IActivity startActivity = null;
            //生成工序清单
            workOrder.RoutingProcessList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            foreach (var activity in container.Activitys.Where(p => p.Type == ActivityType.Interaction))
            {
                var process = RF.GetById<Process>(activity.ProcessId);
                var mAcModel = (ActivityModel)activity;
                if (process == null && mAcModel != null && !mAcModel.IsGroup)
                {
                    throw new ValidationException("工序：{0} 不存在".L10nFormat(activity.Text));
                }
                if (isPassRate && activity.IsPassRate)
                {
                    sb.Append(activity.Text);
                    throw new ValidationException("直通率工序【{0}】最多只能勾选一个".L10nFormat(sb.ToString()));
                }
                if (activity.IsPassRate)
                {
                    isPassRate = true;
                    sb.Append(activity.Text + ",");
                }
                WorkOrderRoutingProcess routingProcess = RT.Service.Resolve<WorkOrderController>().CreateRoutingProcess(workOrder, activity, process);
                var beginRule = activity.EndRules.FirstOrDefault(p => p.BeginActivity.Type == ActivityType.Initial);
                if (beginRule != null)
                {
                    routingProcess.Sign = RoutingProcessSign.Start;
                    startActivity = activity;
                }
                var endRule = activity.BeginRules.FirstOrDefault(p => p.EndActivity.Type == ActivityType.Completion);
                if (endRule != null)
                    routingProcess.Sign |= RoutingProcessSign.End;
                if (beginRule == null && endRule == null)
                    routingProcess.Sign = RoutingProcessSign.Normal;
                foreach (var item in activity.Bom)
                {
                    var routingProcessBomConfig = new WorkOrderRoutingProcessBom()
                    {
                        ItemId = item.ItemId,
                        ProcessId = routingProcess.Id,
                        ItemExtProp = item.ItemExtProp,
                        ItemExtPropName = item.ItemExtPropName
                    };
                    routingProcess.BomConfigList.Add(routingProcessBomConfig);
                }

                workOrder.RoutingProcessList.Add(routingProcess);
            }

            //获取工序属性维护
            var processIds = workOrder.RoutingProcessList.Select(p => p.ProcessId.Value).Distinct().ToList();
            var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(processIds);
            //获取工艺路线信息
            var layoutInfoIds = workOrder.RoutingProcessList.Select(p => p.LayoutInfoId).Distinct().ToList();
            var layoutInfos = workOrder.LayoutInfoList;//RT.Service.Resolve<WorkOrderController>().GetLayoutInfosByWorkOrderId(workOrder.Id);
            double? startProcess = null;
            double? endProcess = null;
            if (workOrder.Type == Core.WorkOrders.WorkOrderType.Rework)
            {
                //返工工单只有最后一个工序需要生成任务单不管是不是工序属性有没有配置
                //var layoutInfo = layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault();
                //var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode);
                //startProcess = processPty.ProcessId;
                //endProcess = processPty.ProcessId;
                //返工任务单只有最后一个工序,设置为首末工序
                //判定首工序的时候，排除PP04不参与首工序判定
                startProcess = workOrder.RoutingProcessList.Where(p => p.LayoutInfo == null || (p.LayoutInfo != null && p.LayoutInfo.Steus != "PP04")).OrderByDescending(p => p.Index).FirstOrDefault().ProcessId.Value;
                endProcess = workOrder.RoutingProcessList.OrderByDescending(p => p.Index).FirstOrDefault().ProcessId.Value;
            }
            else
            {
                //判断首工序
                //判定首工序的时候，排除PP04不参与首工序判定
                foreach (var layoutInfo in layoutInfos.Where(p=>p.Steus != "PP04").OrderBy(p => Convert.ToDecimal(p.Vornr)))
                {
                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
                    if (processPty != null)
                    {
                        startProcess = processPty.ProcessId;
                        break;
                    }
                }
                //判断尾工序
                foreach (var layoutInfo in layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)))
                {
                    var processPty = processPtys.FirstOrDefault(p => p.ProcessCode == layoutInfo.ProcessCode && (p.Scheduling == true || p.DispatchWork == true));
                    if (processPty != null)
                    {
                        endProcess = processPty.ProcessId;
                        break;
                    }
                }
            }
            foreach (var item in workOrder.RoutingProcessList)
            {
                item.StartProcess = startProcess;
                item.EndProcess = endProcess;
            }

            RF.Save(workOrder.RoutingProcessList);
            if (startActivity != null)
            {
                RT.Service.Resolve<WorkOrderController>().SaveNextProcess(workOrder, container, startActivity);
            }
            return workOrder;
        }

        /// <summary>
        /// 同步父级物料
        /// </summary>
        public virtual void SyncParentItem(ParentItemInf parentItemInf, Item item)
        {
            item.ParentItemList.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            ParentItem parentItem = null;//item.ParentItemList.FirstOrDefault(p => p.ParentItemCode == parentItemInf.SMATNR);
            if (parentItem == null)
            {
                parentItem = new ParentItem();
                parentItem.ParentItemCode = parentItemInf.SMATNR;
                parentItem.ItemId = item.Id;
                parentItem.PersistenceStatus = PersistenceStatus.New;
                item.ParentItemList.Add(parentItem);
            }
            parentItem.Bismt = parentItemInf.BISMT;
            parentItem.Werks = parentItemInf.WERKS;
            parentItem.Mtart = parentItemInf.MTART;
            RF.Save(item.ParentItemList);
        }

        public virtual bool IsBatchProcess(ProcessType type)
        {
            return type == ProcessType.BatchAssembly || type == ProcessType.BatchFix || type == ProcessType.BatchPacking || type == ProcessType.BatchPqc;
        }
    }

    [Serializable]
    public class RoutingData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int seq { get; set; }

        public LayoutInf Layout { get; set; }

        /// <summary>
        /// 返回序号(下个序号)
        /// </summary>
        public int BackSeq { get; set; }
    }

}
