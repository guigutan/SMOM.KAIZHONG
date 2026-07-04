using SIE.Core.Common;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Items;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 工单BOM下载控制器
    /// </summary>
    public class DownloadWorkOrderBomController : DomainController
    {
        /// <summary>
        /// 通用控制器
        /// </summary>
        private CommonController _commonController = RT.Service.Resolve<CommonController>();

        /// <summary>
        /// 从API下载工单BOM到业务表
        /// </summary>
        /// <param name="workOrderBomDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadWorkOrderBomToBusiness(List<WorkOrderBomData> workOrderBomDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<WorkOrderBomData>(
                workOrderBomDatas,
                p => this.SaveWorkOrderBoms(p),
                JobType.WorkOrderBom,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载工单BOM到业务表
        /// </summary>
        public virtual ProcessResult DownloadWorkOrderBomInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<WorkOrderBomInf>(
                () => ctl.GetUnprocessedDatas<WorkOrderBomInf>(),                      //工单BOM中间表数据
                p =>
                {
                    var paras = this.GenerateWorkOrderBomPara(p);
                    return this.SaveWorkOrderBoms(paras);
                },
                JobType.WorkOrderBom, isManual);
        }

        /// <summary>
        /// 生成工单BOM实体
        /// </summary>
        /// <param name="workOrderBomInfs">中间表实体数据</param>
        /// <returns></returns>
        public virtual List<WorkOrderBomData> GenerateWorkOrderBomPara(IEnumerable<WorkOrderBomInf> workOrderBomInfs)
        {
            var paras = new List<WorkOrderBomData>();

            workOrderBomInfs.ForEach(p =>
            {
                var data = new WorkOrderBomData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.ItemCode = p.ItemCode;
                data.WorkOrderNo = p.WoNo;
                data.RequireQty = p.RequireQty;
                data.SingleQty = p.SingleQty;
                data.Remark = p.Remark;
                data.IsRecoilItem = p.IsRecoilItem;
                data.IsVritualItem = p.IsVritualItem;

                paras.Add(data);
            });

            return paras.OrderBy(p => p.LastUpdateDate).ToList();
        }

        /// <summary>
        /// ERP保存工单BOM数据
        /// </summary>
        /// <param name="erpInfoDatas">ERP工单数据集合</param>
        /// <returns>错误信息列表</returns>
        public virtual List<ErpErrorData> SaveWorkOrderBoms(List<WorkOrderBomData> erpInfoDatas)
        {
            List<ErpErrorData> res = new List<ErpErrorData>();
            Dictionary<string, WorkOrder> dicWorkOrder;
            Dictionary<double, List<WorkOrderBom>> dicBom;
            Dictionary<string, Item> dicItem;
            GetBomRelatedDatas(erpInfoDatas, out dicWorkOrder, out dicBom, out dicItem);
            var dicErpBom = erpInfoDatas.GroupBy(p => p.WorkOrderNo).ToDictionary(p => p.Key, p => p.ToList());
            dicErpBom.ForEach(erpBom =>
            {
                var workOrderNo = erpBom.Key;
                var erpBomList = erpBom.Value;
                try
                {
                    if (workOrderNo.IsNullOrEmpty())
                        throw new ValidationException("工单号为空".L10N());
                    if (!dicWorkOrder.TryGetValue(workOrderNo, out WorkOrder workOrder))
                        throw new ValidationException("未找到工单{0}".L10nFormat(workOrderNo));
                    if (!dicBom.TryGetValue(workOrder.Id, out List<WorkOrderBom> boms))
                        boms = new List<WorkOrderBom>();
                    UpdateWorkOrderBom(erpBomList, boms, dicItem, workOrder);
                    workOrder.PropertyChanged += RT.Service.Resolve<ErpWorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
                    workOrder.NotifyPropertyChanged(WorkOrder.VersionIdProperty);  //触发属性变更重新更新工序BOM
                    workOrder.PropertyChanged -= RT.Service.Resolve<ErpWorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
                    RF.Save(workOrder);
                }
                catch (Exception exc)
                {
                    res.Add(new ErpErrorData()
                    {
                        ErrMsg = exc.Message,
                        Infkey = erpBomList.FirstOrDefault()?.Infkey,
                        IsChild = true
                    });
                }
            });

            return res;
        }

        /// <summary>
        /// 更新工单BOM
        /// </summary>
        /// <param name="erpBomList">ERP工单BOM数据</param>
        /// <param name="bomList">工单BOM集合</param>
        /// <param name="dicItem">物料数据字典</param>
        /// <param name="workOrder">工单</param>
        public virtual void UpdateWorkOrderBom(List<WorkOrderBomData> erpBomList, List<WorkOrderBom> bomList, Dictionary<string, Item> dicItem, WorkOrder workOrder)
        {
            //删除不存在的bom
            var deleteBoms = bomList.Where(p => !erpBomList.Select(f => f.ItemCode).Contains(p.ItemCode)).Select(p => p.Id);
            var workBomList = workOrder.BomList;
            workBomList.Where(p => deleteBoms.Contains(p.Id)).ForEach(deleteBom => deleteBom.PersistenceStatus = PersistenceStatus.Deleted);
            erpBomList.GroupBy(p => p.ItemCode).ForEach(gkey =>
            {
                var boms = gkey.ToList();
                var bom = boms.FirstOrDefault();
                decimal singleQty = boms.Sum(p => p.SingleQty);
                decimal requireQty = boms.Sum(p => p.RequireQty);
                var existBom = bomList.FirstOrDefault(p => p.ItemCode == bom.ItemCode);
                if (existBom != null)
                {
                    existBom.WorkOrder = workOrder;
                    existBom.SingleQty = singleQty;
                    existBom.RequireQty = requireQty;
                    existBom.IsRecoilItem = bom.IsRecoilItem;
                    existBom.IsVritualItem = bom.IsVritualItem;
                    existBom.IsByBill = bom.IsByBill;
                    existBom.Remark = bom.Remark;
                }
                else
                {
                    if (!dicItem.TryGetValue(bom.ItemCode, out Item item))
                        throw new ValidationException("未找到BOM物料{0}".L10nFormat(bom.ItemCode));
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
                }
            });
        }

        /// <summary>
        /// 获取工单BOM相关联数量集
        /// </summary>
        /// <param name="erpInfoDatas">ERP工单BOM数据集合</param>
        /// <param name="dicWorkOrder">工单数据字典</param>
        /// <param name="dicBom">工单BOM字典</param>
        /// <param name="dicItem">物料数据字典</param> 
        private void GetBomRelatedDatas(List<WorkOrderBomData> erpInfoDatas, out Dictionary<string, WorkOrder> dicWorkOrder, out Dictionary<double, List<WorkOrderBom>> dicBom, out Dictionary<string, Item> dicItem)
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
            List<string> itemCodeList = erpInfoDatas.Select(p => p.ItemCode).ToList();
            if (itemCodeList.Count > 0)
            {
                var itemExp = itemCodeList.CreateContainsExpression<Item>("x", nameof(Item.Code));
                dicItem = _commonController.GetDatas(itemExp).ToDictionary(p => p.Code);
            }
            else
                dicItem = new Dictionary<string, Item>();
        }
    }
}
