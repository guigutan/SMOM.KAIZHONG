using SIE.Barcodes;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Common.IService;
using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Edge.Models;
using SIE.MES.InspectionStandards;
using SIE.MES.PackingPrints;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 边缘在制数据访问
    /// </summary>
    public class EdgeWipDao : IEdgeWipDao
    {
        private readonly IRepositoryFactoryService rfs;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EdgeWipDao(IRepositoryFactoryService rfs)
        {
            this.rfs = rfs;
        }

        /// <summary>
        /// 按工单编码获取工单
        /// </summary>
        /// <param name="WorkOrderNo">工单编码</param>
        /// <returns></returns>
        public WorkOrder GetWorkOrder(string WorkOrderNo)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var wo = rfs.Query<WorkOrder>().Where(w => w.No == WorkOrderNo).FirstOrDefault();
                if (wo != null) 
                {
                    var singleBatchRule = rfs.Query<SIE.Core.Items.ItemBatchRule>().Where(p => p.ItemId == wo.ProductId && p.RetrospectType == Core.Items.RetrospectType.Single).FirstOrDefault();
                    return singleBatchRule != null ? wo : null;
                }
                return null;
            }
        }

        /// <summary>
        /// 按生产条码获取工单
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        public WorkOrder GetWorkOrderByBarcode(string barcode)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var bc = rfs.Query<Barcode>().Where(b => b.Sn == barcode).FirstOrDefault();
                if (bc == null)
                {
                    return null;
                }
                return GetWorkOrder(bc.WorkOrder.No);
            }
        }

        /// <summary>
        /// 取资源的工位数据
        /// </summary>
        /// <param name="resourceIds">资源ID</param>
        /// <returns></returns>
        public List<Station> GetStationsByResourceIds(List<double> resourceIds)
        {
            List<Station> dataList;
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                dataList = Core.Common.DataProcessEx.SplitContains(resourceIds, (Func<IEnumerable<double>, List<Station>>)((tmpIds) =>
                {
                    return rfs.Query<Station>().Where(p => tmpIds.Contains(p.ResourceId)).ToList(null,new EagerLoadOptions().LoadWith((IListProperty)Station.StationEquipmentListProperty)).ToList();
                }));
            }
            return dataList;
        }

        /// <summary>
        /// 根据登陆用户获取工序列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>工序列表</returns>
        public List<Tech.Processs.Process> GetProcesssByEmployeeId(double employeeId)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {

                return rfs.Query<Tech.Processs.Process>()
                .Exists<Tech.Processs.ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == employeeId))
                .ToList().ToList();
            }
        }

        /// <summary>
        /// 根据工单ID获取已打印的条码
        /// </summary>
        /// <param name="workorderId">工单ID</param> 
        /// <returns>条码信息</returns>
        public virtual List<Barcode> GetBarcodes(double workorderId)
        {
            List<Barcode> lst = new List<Barcode>();
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                PagingInfo pg = new PagingInfo();
                pg.PageSize = 3000;
                pg.PageNumber = 1;
                while (true)
                {
                    var barcodes = rfs.Query<Barcode>().Where(p => p.WorkOrderId == workorderId).ToList(pg).ToList();
                    pg.PageNumber++;
                    if (barcodes.Count == 0)
                    {
                        break;
                    }
                    lst.AddRange(barcodes);
                }
            }
            return lst;
        }

        /// <summary>
        /// 根据工单ID获取已打印的包装号
        /// </summary>
        /// <param name="workorderId">工单ID</param> 
        /// <returns>条码信息</returns>
        public virtual List<PackingBarcode> GetPackingBarcodes(double workorderId)
        {
            List<PackingBarcode> lst = new List<PackingBarcode>();
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                PagingInfo pg = new PagingInfo();
                pg.PageSize = 3000;
                pg.PageNumber = 1;
                while (true)
                {
                    var barcodes = rfs.Query<PackingBarcode>().Where(p => p.WorkOrderId == workorderId).ToList(pg).ToList();
                    pg.PageNumber++;
                    if (barcodes.Count == 0)
                    {
                        break;
                    }
                    lst.AddRange(barcodes);
                }
            }
            return lst;
        }

        /// <summary>
        /// 获取员工关联的生产资源列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>生产资源列表</returns>
        public virtual EntityList<WipResource> GetWipResources(double employeeId)
        {
            return rfs.Query<WipResource>()
                .Join<EmployeeResource>((x, y) => x.Id == y.ResourceId && y.EmployeeId == employeeId)
                .ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 取员工信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public virtual Employee GetEmployeeById(double employeeId)
        {
            return rfs.GetById<Employee>(employeeId);
        }

        /// <summary>
        /// 取生产资源班次信息
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public SIE.Resources.ShiftTypes.Shift GetShift(double resourceId, DateTime currentTime)
        {
            try
            {
                return RT.Service.Resolve<Resources.WipResources.WipResourceController>().GetWipResourceShift(resourceId, currentTime);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Item> GetItems(List<double> ids)
        {
            var exp = ids.CreateContainsExpression<Item>("x", "Id");
            if (exp == null)
            {
                return new List<Item>();
            }
            return rfs.Query<Item>().Where(exp).ToList().ToList();

        }

        /// <summary>
        /// 取机型检验项目
        /// </summary>
        /// <returns></returns>
        public List<ModelInspectionItem> GetInspectionItems()
        {
            return RT.Service.Resolve<WIP.Inspects.InspectByItemController>().GetInspectionItems().ToList();
        }

        /// <summary>
        /// 取缺陷代码
        /// </summary>
        /// <returns></returns>
        public List<Defect> GetAllDefects()
        {
            return rfs.GetAll<Defect>().ToList();
        }

        /// <summary>
        /// 取缺陷分类
        /// </summary>
        /// <returns></returns>
        public List<DefectCategory> GetAllDefectCategory()
        {
            return rfs.GetAll<DefectCategory>().ToList();
        }

        /// <summary>
        /// 获取所有的缺陷责任
        /// </summary>
        /// <returns></returns>
        public List<DefectResponsibility> GetAllDefectResponsibility()
        {
            return rfs.GetAll<DefectResponsibility>().ToList();
        }

        /// <summary>
        /// 获取所有的缺陷责任分类
        /// </summary>
        /// <returns></returns>
        public List<DefectResponsibilityCategory> GetAllDefectResponsibilityCategory()
        {
            return rfs.GetAll<DefectResponsibilityCategory>().ToList();
        }

        /// <summary>
        /// 获取所有维修措施
        /// </summary>
        /// <returns></returns>
        public List<RepairMeasure> GetAllRepairMeasure()
        {
            return rfs.GetAll<RepairMeasure>().ToList();
        }

        /// <summary>
        /// 更新条码信息
        /// </summary>
        /// <param name="edgeMaterials"> EdgeMaterial 的Qty为要更新的剩余数量</param>
        /// <returns></returns>
        public bool SetBarcodes(List<EdgeMaterial> edgeMaterials)
        {
            //Sn条码
            var sns = edgeMaterials.Where(m => m.SourceType == SingleLabels.LoadItemSourceType.SN);
            //物料标签
            var itemLabels = edgeMaterials.Where(m => m.SourceType == SingleLabels.LoadItemSourceType.ItemLabel);
            
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //SN
                if (sns.Any())
                {
                    var labels = sns.Select(k => k.Barcode).ToList();
                    DB.Update<WipProductProcessKeyItem>().Set(p => p.IsUnbound, true).Where(p => labels.Contains(p.SourceCode)).Execute();
                }
                
                //物料标签
                if (itemLabels.Any())
                {
                    foreach (var itemlabel in itemLabels)
                    {
                            //已更新为0 则更新状态为用毕
                            DB.Update<ItemLabel>().Set(p => p.Qty, itemlabel.Qty).Where(p => p.Label == itemlabel.Barcode).Execute();
                    }
                }

                tran.Complete();

            }

            return true;

        }

        /// <summary>
        /// 下料更新条码信息
        /// </summary>
        /// <param name="edgeMaterials">下料来源信息</param>
        /// <returns></returns>
        public bool UpdateUnLoadItemBarcodes(List<EdgeMaterial> edgeMaterials)
        {
            //Sn条码
            var sns = edgeMaterials.Where(m => m.SourceType == SingleLabels.LoadItemSourceType.SN);
            //物料标签
            var itemLabels = edgeMaterials.Where(m => m.SourceType == SingleLabels.LoadItemSourceType.ItemLabel);
            
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //SN
                if (sns.Any())
                {
                    var labels = sns.Select(k => k.Barcode).ToList();
                    DB.Update<WipProductProcessKeyItem>()
                        .Set(p => p.IsUnbound, true)
                        .Where(p => labels.Contains(p.SourceCode))
                        .Execute();
                }
               
                //物料标签
                if (itemLabels.Any())
                {
                    foreach (var itemlabel in itemLabels)
                    {
                        //itemlabel.Qty为边缘端剩余数量
                        DB.Update<ItemLabel>().Set(p => p.Qty, itemlabel.RemainQty)
                            .Where(p => p.Label == itemlabel.Barcode)
                            .Execute();
                    }
                }
                
                tran.Complete();

            }
            return true;
        }

        /// <summary>
        /// 获取工序缺陷信息
        /// </summary>
        /// <param name="processIds">工序ID</param>
        /// <returns>工序缺陷列表</returns>
        public EntityList<ProcessDefect> GetProcessDefects(IList<double> processIds)
        {
            return processIds.SplitContains(tempIds =>
            {
                return rfs.Query<ProcessDefect>().Where(p => tempIds.Contains(p.ProcessId)).ToList(null,new EagerLoadOptions().LoadWith(ProcessDefect.DefectProperty));
            });
        }

        /// <summary>
        /// 获取工序缺陷分类
        /// </summary>
        /// <param name="defects">工序缺陷代码</param>
        /// <returns>工序缺陷分类列表</returns>
        public List<EdgeDefectCategory> GetProcessDefectCategorys(IList<EdgeDefect> defects)
        {
            var edgeDefectCategorys = new List<EdgeDefectCategory>();
            var defectCategorys = defects.Select(p => p.CategoryId).Distinct().SplitContains(tempIds=> {
                return rfs.Query<DefectCategory>().Where(p=>tempIds.Contains(p.Id)).ToList();
            });

            var allDefectCategory = GetAllDefectCategory();
            var retDefectCategorys = new EntityList<DefectCategory>();

            retDefectCategorys.AddRange(defectCategorys);
            foreach (var category in defectCategorys)
            {
                CreateDefectCategoryListByCategoryId(category.TreePId,retDefectCategorys,allDefectCategory);
            }

            foreach (var category in retDefectCategorys)
            {
                var defectCategory = new EdgeDefectCategory();
                defectCategory.Id = category.Id;
                defectCategory.Code = category.Code;
                defectCategory.Desc = category.Description;
                defectCategory.TreePId = category.TreePId;
                edgeDefectCategorys.Add(defectCategory);
            }
            return edgeDefectCategorys;
        }

        /// <summary>
        /// 根据缺陷子类获取所有上层父类
        /// </summary>
        private void CreateDefectCategoryListByCategoryId(double? categoryId, EntityList<DefectCategory> retDefectCategorys, List<DefectCategory>  allDefectCategory)
        {
            if (categoryId != null)
            {
                var defectCategory = allDefectCategory.FirstOrDefault(p => p.Id == categoryId);
                if (defectCategory!=null && !retDefectCategorys.Any(p=>p.Id == defectCategory.Id)) 
                {
                    retDefectCategorys.Add(defectCategory);
                    CreateDefectCategoryListByCategoryId(defectCategory.TreePId, retDefectCategorys, allDefectCategory);
                }
            } 
        }

        /// <summary>
        /// 通过工序Id获取采集步骤列表
        /// </summary>
        /// <param name="processIds">工序Id</param>
        /// <returns>采集步骤列表</returns>
        public List<ProcessCollectStep> GetProcessCollectSteps(List<double> processIds)
        {
            List<double?> ids = new List<double?>();
            processIds.ForEach(d=>ids.Add(d));
            var lst = ids.SplitContains(tempIds =>
            {
                return rfs.Query<ProcessCollectStep>().Where(p => tempIds.Contains(p.ProcessId)).ToList();
            });
            return lst.ToList();
        }


        /// <summary>
        /// 获取当前时间计划排产的有效在制工单信息
        /// </summary>
        /// <returns></returns>
        public List<WorkOrder> GetPlannedWipWorkOrders(List<string> resourceNos)
        {
            var dtStart = DateTime.Now.AddDays(1);
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return rfs.Query<WorkOrder>().Where(w => resourceNos.Contains(w.Resource.Code) && w.PlanBeginDate <= dtStart && 
                    w.IsPause == YesNo.No &&
                    w.State != Core.WorkOrders.WorkOrderState.Finish &&
                    w.State != Core.WorkOrders.WorkOrderState.Close).ToList().ToList();
            }
        }


        /// <summary>
        /// 获取当前有效在制工单信息
        /// </summary>
        /// <param name="workOrderNo">工单编码</param>
        /// <returns></returns>
        public WorkOrder GetWipWorkOrderByNo(string workOrderNo)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return rfs.Query<WorkOrder>().Where(w => w.No == workOrderNo).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取包装号
        /// </summary>
        /// <param name="ruleId">包装规则Id</param> 
        /// <returns></returns>
        public string GetPackCode(double ruleId) 
        {
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(ruleId, 1).First();
        }
    }
}
