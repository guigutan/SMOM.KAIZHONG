using SIE.Common;
using SIE.Core.Common;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Download.WorkOrders;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 工单下载控制器
    /// </summary>
    public class DownloadWorkOrderController : DomainController
    {
        /// <summary>
        /// 通用控制器
        /// </summary>
        private CommonController _commonController = RT.Service.Resolve<CommonController>();

        /// <summary>
        /// 从API下载工单到业务表
        /// </summary>
        /// <param name="workOrderDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadWorkOrderToBusiness(List<WorkOrderData> workOrderDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<WorkOrderData>(
                workOrderDatas,
                p => this.SaveWorkOrders(p.OrderByLastUpdateDate()),
                JobType.WorkOrder,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载工单到业务表
        /// </summary>
        public virtual ProcessResult DownloadWorkOrderInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<WorkOrderInf, WorkOrderBomInf>(
                () => ctl.GetUnprocessedDatas<WorkOrderInf>(),                     //库位中间表数据
                p =>
                {
                    //发运单明细中间表数据
                    var nos = p.Select(y => y.No).Distinct().ToList();
                    var whereDtl = nos.CreateContainsExpression<WorkOrderBomInf>("x", WorkOrderBomInf.WoNoProperty.Name);
                    var dtlDatas = ctl.GetUnprocessedDatas(whereDtl);
                    return dtlDatas;
                },
                (x, y) =>
                {
                    //构建明细数据嵌套字典
                    var dtlDataDicts = ctl.GenerateDictionarys<string, WorkOrderBomInf>(y, WorkOrderBomInf.WoNoProperty);

                    //调用业务接口
                    var paras = this.GenerateWorkOrderPara(x, dtlDataDicts);
                    return this.SaveWorkOrders(paras.OrderByLastUpdateDate());
                },
                JobType.WorkOrder, JobType.WorkOrderBom, isManual);
        }

        /// <summary>
        /// 生成工单实体
        /// </summary>
        /// <param name="workOrderInfs">中间表实体数据</param>
        /// <param name="workOrderBomInfs">中间表明细实体数据</param>
        /// <returns></returns>
        private List<WorkOrderData> GenerateWorkOrderPara(IEnumerable<WorkOrderInf> workOrderInfs, Dictionary<string, List<WorkOrderBomInf>> workOrderBomInfs)
        {
            var paras = new List<WorkOrderData>();
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();
            var dtlCtl = RT.Service.Resolve<DownloadWorkOrderBomController>();

            workOrderInfs.ForEach(p =>
            {
                //构建子列表
                List<WorkOrderBomInf> details;
                if (workOrderBomInfs.TryGetValue(p.No, out details))
                    workOrderBomInfs.Remove(p.No);      //由于来源数据集允许重复数据，已取值明细清除，避免重复构建浪费资源
                else
                    details = new List<WorkOrderBomInf>();
                ctl.GenerateChildren(p, details);
                var dtlDatas = dtlCtl.GenerateWorkOrderBomPara(details);

                //构建主数据
                var data = new WorkOrderData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.WorkOrderNo = p.No;
                ////workOrderData.ErpWorkOrderNo = workOrderInf.No;
                data.CustomerCode = p.CustomerCode;
                data.CustomerOrderNo = p.CustomerOrderNo;
                data.SaleOrderNo = p.SaleOrderNo;
                ////data.WorkOrderType = (int)p.WorkOrderType;
                data.MakerCode = p.MakerCode;
                data.WorkshopCode = p.WorkshopCode;
                data.ResourceCode = p.ResourceCode;
                data.ProductCode = p.ProductCode;
                data.PlanQty = p.PlanQty;
                data.OrderQty = p.OrderQty;
                data.PlanBeginDate = p.PlanBeginDate;
                data.PlanEndDate = p.PlanEndDate;
                data.ErpKey = p.ErpKey;
                data.BomList.AddRange(dtlDatas);                            //附加字列表

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        public virtual string DownloadManual(string keyWord)
        {
            ProcessResult result = new ProcessResult();
            string resultMsg = string.Empty;

            try
            {
                if (keyWord.IsNullOrEmpty())
                    throw new ValidationException("唯一主键不能为空".L10N());
                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<SoapWorkOrderController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadWorkOrderInfToBusiness();           //执行业务表下载
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                result.AddFailMsg(ex.GetBaseException());
            }

            if (!result.Result) resultMsg = result.FailMsg.FirstOrDefault();
            return resultMsg;
        }

        /// <summary>
        /// ERP保存工单数据
        /// </summary>
        /// <param name="erpInfoDatas">ERP工单数据集合</param>
        /// <returns>错误信息列表</returns>
        public virtual List<ErpErrorData> SaveWorkOrders(List<WorkOrderData> erpInfoDatas)
        {
            List<ErpErrorData> res = new List<ErpErrorData>();
            var workOrderController = RT.Service.Resolve<WorkOrderController>();
            Dictionary<string, WorkOrder> dicWorkOrder;
            Dictionary<double, List<WorkOrderBom>> dicBom;
            Dictionary<string, Item> dicItem;
            Dictionary<string, ErpWorkOrder> dicErpWorkOrder;
            Dictionary<string, Enterprise> dicWorkshop;
            Dictionary<string, WipResource> dicResource;
            Dictionary<string, Employee> dicEmployee;
            //1 获取相关联数据
            GetWorkOrderRelatedDatas(erpInfoDatas, out dicWorkOrder, out dicBom, out dicItem, out dicErpWorkOrder, out dicWorkshop, out dicResource, out dicEmployee);
            DateTime? makeDate = null;
            erpInfoDatas.ForEach(data =>
            {
                try
                {
                    //2 验证数据合理性
                    WorkOrder workOrder; Item product; Enterprise workshop; WipResource resource; Employee maker; ErpWorkOrder erpWorkOrder;
                    ValidateWorkOrderData(data, dicWorkOrder, dicItem, dicErpWorkOrder, dicWorkshop, dicResource, dicEmployee, out workOrder, out product, out workshop, out resource, out maker, out erpWorkOrder);
                    if (workOrder == null)
                    {
                        //3.1 新增工单
                        if (makeDate == null)
                            makeDate = RF.Find<WorkOrder>().GetDbTime();
                        workOrder = CreateWorkOrder(data, dicItem, makeDate.Value, product, workshop, resource, maker, erpWorkOrder);
                        ValidateWorkOrderSave(workOrder);
                        workOrderController.SaveWorkOrder(workOrder, workOrder.Template, WorkOrderLogType.Release, "接口工单创建".L10N());
                    }
                    else
                    {
                        if (workOrder.State != Core.WorkOrders.WorkOrderState.Release)
                            throw new ValidationException("工单{0}非发放状态，不允许{1}".L10nFormat(workOrder.No, data.IsDelete ? "删除".L10N() : "修改".L10N()));
                        if (!dicBom.TryGetValue(workOrder.Id, out List<WorkOrderBom> boms))
                            boms = new List<WorkOrderBom>();
                        //3.2 删除工单
                        DeleteWorkOrder(data, workOrder);
                        //3.3 修改工单
                        UpdateWorkOrder(data, boms, dicItem, workOrder, workshop, resource, erpWorkOrder);
                    }
                }
                catch (Exception exc)
                {
                    res.Add(new ErpErrorData()
                    {
                        ErrMsg = exc.Message,
                        Infkey = data.Infkey,
                        IsChild = false
                    });
                }
            });
            return res;
        }

        /// <summary>
        /// 验证工单保存
        /// </summary>
        /// <param name="workOrder">工单</param>
        private void ValidateWorkOrderSave(WorkOrder workOrder)
        {
            if (workOrder.Version == null)
                throw new ValidationException("工艺路线版本".L10N());
        }

        /// <summary>
        /// 获取工单相关联数量集
        /// </summary>
        /// <param name="erpInfoDatas">ERP工单数据集合</param>
        /// <param name="dicWorkOrder">工单数据字典</param>
        /// <param name="dicBom">工单BOM字典</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="dicErpWorkOrder">ERP工单数据字典</param>
        /// <param name="dicWorkshop">车间数据字典</param>
        /// <param name="dicResource">资源数据字典</param>
        /// <param name="dicEmployee">员工数据字典</param>
        private void GetWorkOrderRelatedDatas(List<WorkOrderData> erpInfoDatas, out Dictionary<string, WorkOrder> dicWorkOrder, out Dictionary<double, List<WorkOrderBom>> dicBom, out Dictionary<string, Item> dicItem, out Dictionary<string, ErpWorkOrder> dicErpWorkOrder, out Dictionary<string, Enterprise> dicWorkshop, out Dictionary<string, WipResource> dicResource, out Dictionary<string, Employee> dicEmployee)
        {
            List<string> workOrderNoList = erpInfoDatas.Where(p => !p.WorkOrderNo.IsNullOrEmpty()).Select(p => p.WorkOrderNo).Distinct().ToList();
            if (workOrderNoList.Count > 0)
            {
                //工单集合
                var woExp = workOrderNoList.CreateContainsExpression<WorkOrder>("x", nameof(WorkOrder.No));
                var workOrderList = _commonController.GetDatas(woExp, null, new EagerLoadOptions().LoadWith(WorkOrder.BomListProperty));
                dicWorkOrder = workOrderList.ToDictionary(p => p.No);
                //工单BOM集合
                List<double> workOrderIdList = workOrderList.Select(p => p.Id).ToList();
                var bomExp = workOrderIdList.CreateContainsExpression<WorkOrderBom>("x", nameof(WorkOrderBom.WorkOrderId));
                dicBom = _commonController.GetDatas(bomExp, null, new EagerLoadOptions().LoadWithViewProperty()).GroupBy(p => p.WorkOrderId).ToDictionary(p => p.Key, p => p.ToList());
            }
            else
            {
                dicWorkOrder = new Dictionary<string, WorkOrder>();
                dicBom = new Dictionary<double, List<WorkOrderBom>>();
            }
            //产品集合
            List<string> productCodeList = erpInfoDatas.Select(p => p.ProductCode).ToList();
            List<string> itemCodeList = erpInfoDatas.SelectMany(p => p.BomList).Select(p => p.ItemCode).ToList();
            var sumItemCodeList = productCodeList.Union(itemCodeList).Where(p => !p.IsNullOrEmpty()).Distinct().ToList();
            if (sumItemCodeList.Count > 0)
            {
                var itemExp = sumItemCodeList.CreateContainsExpression<Item>("x", nameof(Item.Code));
                dicItem = _commonController.GetDatas(itemExp).ToDictionary(p => p.Code);
            }
            else
                dicItem = new Dictionary<string, Item>();
            //ERP工单集合
            List<string> erpWoNoList = erpInfoDatas.Where(p => !p.ErpWorkOrderNo.IsNullOrEmpty()).Select(p => p.ErpWorkOrderNo).Distinct().ToList();
            if (erpWoNoList.Count > 0)
            {
                var erpWoExp = erpWoNoList.CreateContainsExpression<ErpWorkOrder>("x", nameof(ErpWorkOrder.No));
                dicErpWorkOrder = _commonController.GetDatas(erpWoExp).ToDictionary(p => p.No);
            }
            else
                dicErpWorkOrder = new Dictionary<string, ErpWorkOrder>();
            //车间集合
            List<string> workshopCodeList = erpInfoDatas.Where(p => !p.WorkshopCode.IsNullOrEmpty()).Select(p => p.WorkshopCode).Distinct().ToList();
            if (workshopCodeList.Count > 0)
            {
                var workshopExp = workshopCodeList.CreateContainsExpression<Enterprise>("x", nameof(Enterprise.Code));
                dicWorkshop = _commonController.GetDatas(workshopExp).ToDictionary(p => p.Code);
            }
            else
                dicWorkshop = new Dictionary<string, Enterprise>();
            //资源集合
            List<string> resourceList = erpInfoDatas.Where(p => !p.ResourceCode.IsNullOrEmpty()).Select(p => p.ResourceCode).Distinct().ToList();
            if (resourceList.Count > 0)
            {
                var resourceExp = resourceList.CreateContainsExpression<WipResource>("x", nameof(WipResource.Code));
                dicResource = _commonController.GetDatas(resourceExp).ToDictionary(p => p.Code);
            }
            else
                dicResource = new Dictionary<string, WipResource>();
            //建单人集合
            List<string> employeeList = erpInfoDatas.Where(p => !p.MakerCode.IsNullOrEmpty()).Select(p => p.MakerCode).Distinct().ToList();
            if (employeeList.Count > 0)
            {
                var employeeExp = employeeList.CreateContainsExpression<Employee>("x", nameof(Employee.Code));
                dicEmployee = _commonController.GetDatas(employeeExp).ToDictionary(p => p.Code);
            }
            else
                dicEmployee = new Dictionary<string, Employee>();
        }

        /// <summary>
        /// 验证工单数据
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="dicWorkOrder">工单数据字典</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="dicErpWorkOrder">ERP工单数据字典</param>
        /// <param name="dicWorkshop">车间数据字典</param>
        /// <param name="dicResource">资源数据字典</param>
        /// <param name="dicEmployee">员工数据字典</param>
        /// <param name="workOrder">工单</param>
        /// <param name="product">产品</param>
        /// <param name="workshop">车间</param>
        /// <param name="resource">资源</param>
        /// <param name="maker">制单人</param>
        /// <param name="erpWorkOrder">ERP工单</param>
        private void ValidateWorkOrderData(WorkOrderData data, Dictionary<string, WorkOrder> dicWorkOrder, Dictionary<string, Item> dicItem, Dictionary<string, ErpWorkOrder> dicErpWorkOrder, Dictionary<string, Enterprise> dicWorkshop, Dictionary<string, WipResource> dicResource, Dictionary<string, Employee> dicEmployee, out WorkOrder workOrder, out Item product, out Enterprise workshop, out WipResource resource, out Employee maker, out ErpWorkOrder erpWorkOrder)
        {
            if (!dicWorkOrder.TryGetValue(data.WorkOrderNo, out workOrder) && data.IsDelete)
                throw new ValidationException("未找到工单{0}".L10nFormat(data.WorkOrderNo));
            if (data.WorkOrderType < 0 || data.WorkOrderType > 3)
                throw new ValidationException("工单类型错误".L10N());
            if (data.ProductCode.IsNullOrEmpty())
                throw new ValidationException("产品不能为空".L10N());
            if (!dicItem.TryGetValue(data.ProductCode, out product))
                throw new ValidationException("未找到产品{0}".L10nFormat(data.ProductCode));
            if (data.WorkshopCode.IsNullOrEmpty())
                throw new ValidationException("车间不能为空".L10N());
            if (!dicWorkshop.TryGetValue(data.WorkshopCode, out workshop))
                throw new ValidationException("未找到车间{0}".L10nFormat(data.WorkshopCode));
            if (data.ResourceCode.IsNullOrEmpty())
                throw new ValidationException("资源不能为空".L10N());
            if (!dicResource.TryGetValue(data.ResourceCode, out resource))
                throw new ValidationException("未找到资源{0}".L10nFormat(data.ResourceCode));
            if (resource.WorkShopId != workshop.Id)
                throw new ValidationException("资源{0}不属于车间{1}".L10nFormat(data.ResourceCode, data.WorkshopCode));
            if (data.MakerCode.IsNullOrEmpty())
                throw new ValidationException("制单人不能为空".L10N());
            if (!dicEmployee.TryGetValue(data.MakerCode, out maker))
                throw new ValidationException("未找到制单人{0}".L10nFormat(data.MakerCode));
            if (!dicErpWorkOrder.TryGetValue(data.ErpWorkOrderNo ?? "", out erpWorkOrder) && data.ErpWorkOrderNo != null)
                throw new ValidationException("未找到ERP工单{0}".L10nFormat(data.ErpWorkOrderNo));
        }

        /// <summary>
        /// 创建工单
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="dicItem">物料</param>
        /// <param name="makeDate">制单时间</param>
        /// <param name="product">产品</param>
        /// <param name="workshop">车间</param>
        /// <param name="resource">资源</param>
        /// <param name="maker">制单人</param>
        /// <param name="erpWorkOrder">ERP工单</param>
        /// <returns>工单</returns>
        private WorkOrder CreateWorkOrder(WorkOrderData data, Dictionary<string, Item> dicItem, DateTime makeDate, Item product, Enterprise workshop, WipResource resource, Employee maker, ErpWorkOrder erpWorkOrder)
        {
            var workOrderController = RT.Service.Resolve<WorkOrderController>();
            WorkOrder workOrder = new WorkOrder()
            {
                No = data.WorkOrderNo.IsNullOrEmpty() ? workOrderController.GetWorkOrderNo() : data.WorkOrderNo,
                Source = SourceType.External,
                State = Core.WorkOrders.WorkOrderState.Release,
                CustomerId = data.CustomerId,
                CustomerOrderNo = data.CustomerOrderNo,
                SaleOrderNo = data.SaleOrderNo,
                PlanQty = data.PlanQty,
                OrderQty = data.OrderQty,
                PlanBeginDate = data.PlanBeginDate,
                PlanEndDate = data.PlanEndDate,
                Type = (Core.WorkOrders.WorkOrderType)data.WorkOrderType,
                WorkShop = workshop,
                Resource = resource,
                MakerId = maker.Id,
                MakeDate = makeDate
            };
            workOrder.GenerateId();
            workOrder.LocalContext.SetExtendedProperty("IsExistBom", data.BomList.Count > 0);
            CreateWorkOrderBom(data, dicItem, workOrder);
            //在赋值产品时才添加属性变更，减少触发次数，减少数据查询
            workOrder.PropertyChanged += RT.Service.Resolve<ErpWorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
            workOrder.Product = product;
            workOrder.ErpWorkOrder = erpWorkOrder;
            workOrder.PropertyChanged -= RT.Service.Resolve<ErpWorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
            return workOrder;
        }

        /// <summary>
        /// 创建工单BOM
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="workOrder">工单</param>
        private void CreateWorkOrderBom(WorkOrderData data, Dictionary<string, Item> dicItem, WorkOrder workOrder)
        {
            data.BomList.GroupBy(p => p.ItemCode).ForEach(gkey =>
            {
                var itemCode = gkey.Key;
                if (!dicItem.TryGetValue(itemCode, out Item item))
                    throw new ValidationException("未找到物料{0}".L10nFormat(itemCode));
                var boms = gkey.ToList();
                decimal singleQty = boms.Sum(p => p.SingleQty);
                decimal requireQty = boms.Sum(p => p.RequireQty);
                var bom = boms.FirstOrDefault();
                workOrder.BomList.Add(new WorkOrderBom()
                {
                    Item = item,
                    WorkOrder = workOrder,
                    SingleQty = singleQty,
                    RequireQty = requireQty,
                    IsRecoilItem = bom.IsRecoilItem,
                    IsVritualItem = bom.IsVritualItem,
                    IsByBill = bom.IsByBill,
                    Remark = bom.Remark
                });
            });
        }

        /// <summary>
        /// 更新工单
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="boms">工单BOM集合</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="workOrder">工单</param>
        /// <param name="workshop">车间</param>
        /// <param name="resource">资源</param> 
        /// <param name="erpWorkOrder">ERP工单</param>
        private void UpdateWorkOrder(WorkOrderData data, List<WorkOrderBom> boms, Dictionary<string, Item> dicItem, WorkOrder workOrder, Enterprise workshop, WipResource resource, ErpWorkOrder erpWorkOrder)
        {
            if (data.IsDelete)
                return;
            var ctl = RT.Service.Resolve<DownloadWorkOrderBomController>();

            workOrder.CustomerId = data.CustomerId;
            workOrder.CustomerOrderNo = data.CustomerOrderNo;
            workOrder.SaleOrderNo = data.SaleOrderNo;
            workOrder.PlanQty = data.PlanQty;
            workOrder.OrderQty = data.OrderQty;
            workOrder.PlanBeginDate = data.PlanBeginDate;
            workOrder.PlanEndDate = data.PlanEndDate;
            workOrder.Type = (Core.WorkOrders.WorkOrderType)data.WorkOrderType;
            workOrder.WorkShop = workshop;
            ctl.UpdateWorkOrderBom(data.BomList, boms, dicItem, workOrder);
            workOrder.LocalContext.SetExtendedProperty("IsExistBom", data.BomList.Count > 0);
            workOrder.PropertyChanged += RT.Service.Resolve<ErpWorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
            workOrder.Resource = null;
            workOrder.Resource = resource;
            workOrder.ErpWorkOrder = erpWorkOrder;
            workOrder.PropertyChanged -= RT.Service.Resolve<ErpWorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
            workOrder.IsPause = YesNo.Yes;
            ValidateWorkOrderSave(workOrder);
            RT.Service.Resolve<WorkOrderController>().UpdateWorkOrder(workOrder, workOrder.Template, false, true);
        }

        /// <summary>
        /// 删除工单
        /// </summary>
        /// <param name="data">ERP工单数据</param>
        /// <param name="workOrder">工单</param>
        private void DeleteWorkOrder(WorkOrderData data, WorkOrder workOrder)
        {
            if (data.IsDelete)
            {
                workOrder.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(workOrder);
            }
        }

    }
}
