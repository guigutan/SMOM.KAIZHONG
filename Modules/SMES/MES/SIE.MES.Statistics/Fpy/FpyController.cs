using SIE.Defects;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Statistics.WIP;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Statistics.Fpy
{
    /// <summary>
    /// 直通率统计控制器
    /// </summary>
    public partial class FpyController : DomainController
    {
        /// <summary>
        /// 获取或更新工序直通率
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="passQty">一次通过数</param>
        /// <param name="failQty">一次不良数</param>
        public virtual void CreateOrUpdateProcessFpy(WipData data, decimal passQty, decimal failQty)
        {
            int count = DB.Update<ProcessFpyStatistics>()
                .Set(p => p.InputQty, p => p.InputQty + passQty + failQty)
                .Set(p => p.FailedQty, p => p.FailedQty + failQty)
                .Set(p => p.PassQty, p => p.PassQty + passQty)
                .Where(p => p.InvOrgId == data.InvOrgId
                && p.WorkOrderId == data.WorkOrderId
                && p.ProductId == data.ProductId
                && p.ModelId == data.ModelId
                && p.WorkShopId == data.WorkShopId
                && p.ResourceId == data.ResourceId
                && p.ProcessId == data.ProcessId
                && p.ShiftId == data.ShiftId
                && p.ShiftDate == data.ShiftDate
                && p.CollectedDate == data.CollectDate
                && p.Hour == data.Hour)
                .Execute();
            if (count == 0)
            {
                //更新失败，创建新数据 
                RF.Save(new ProcessFpyStatistics()
                {
                    InvOrgId = data.InvOrgId,
                    WorkOrderId = data.WorkOrderId,
                    WorkOrderNo = data.WorkOrderNo,
                    ProductId = data.ProductId,
                    ProductName = data.ProductName,
                    ModelId = data.ModelId,
                    ModelName = data.ModelName,
                    WorkShopId = data.WorkShopId,
                    WorkShopName = data.WorkShopName,
                    ResourceId = data.ResourceId,
                    ResourceName = data.ResourceName,
                    ProcessId = data.ProcessId,
                    ProcessName = data.ProcessName,
                    ShiftId = data.ShiftId,
                    ShiftName = data.ShiftName,
                    ShiftDate = data.ShiftDate,
                    CollectedDate = data.CollectDate,
                    Hour = data.Hour,
                    InputQty = passQty + failQty,
                    FailedQty = failQty,
                    PassQty = passQty
                });
            }
        }

        /// <summary>
        /// 获取或更新产品直通率
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="inputQty">投入数</param>
        /// <param name="passQty">一次通过数</param>
        /// <param name="failQty">一次不良数</param>
        public virtual void CreateOrUpdateProductFpy(WipData data, decimal inputQty, decimal passQty, decimal failQty)
        {
            int count = DB.Update<ProductFpyStatistics>()
                .Set(p => p.InputQty, p => p.InputQty + inputQty)
                .Set(p => p.FailedQty, p => p.FailedQty + failQty)
                .Set(p => p.PassQty, p => p.PassQty + passQty)
                .Where(p => p.InvOrgId == data.InvOrgId
                && p.WorkOrderId == data.WorkOrderId
                && p.ProductId == data.ProductId
                && p.ModelId == data.ModelId
                && p.WorkShopId == data.WorkShopId
                && p.ResourceId == data.ResourceId
                && p.ShiftId == data.ShiftId
                && p.ShiftDate == data.ShiftDate
                && p.CollectedDate == data.CollectDate
                && p.Hour == data.Hour)
                .Execute();
            if (count == 0)
            {
                //更新失败，创建新数据 
                RF.Save(new ProductFpyStatistics()
                {
                    InvOrgId = data.InvOrgId,
                    WorkOrderId = data.WorkOrderId,
                    WorkOrderNo = data.WorkOrderNo,
                    ProductId = data.ProductId,
                    ProductName = data.ProductName,
                    ModelId = data.ModelId,
                    ModelName = data.ModelName,
                    WorkShopId = data.WorkShopId,
                    WorkShopName = data.WorkShopName,
                    ResourceId = data.ResourceId,
                    ResourceName = data.ResourceName,
                    ShiftId = data.ShiftId,
                    ShiftName = data.ShiftName,
                    ShiftDate = data.ShiftDate,
                    CollectedDate = data.CollectDate,
                    Hour = data.Hour,
                    InputQty = inputQty,
                    FailedQty = failQty,
                    PassQty = passQty
                });
            }
        }

        /// <summary>
        /// 创建或更新不良缺陷采集信息
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="qty">数量</param>
        /// <param name="defects">缺陷信息</param>
        public virtual void CreateOrUpdateDefectStatics(WipData data, decimal qty, List<DefectData> defects)
        {
            foreach (DefectData defect in defects)
            {
                var fpyStatistics = GetDefectStatistics(data, defect.DefectId, defect.CategoryId);
                if (fpyStatistics == null)
                    CreateDefectStatics(data, qty, defect);
                else
                    UpdateDefectStatics(data, qty, defect);
            }
        }

        /// <summary>
        /// 更新不良缺陷采集信息
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="qty">数量</param>
        /// <param name="defect">缺陷信息</param>
        private void UpdateDefectStatics(WipData data, decimal qty, DefectData defect)
        {
            DB.Update<DefectStatistics>().Set(p => p.Qty, p => p.Qty + qty).Where(p => p.ResourceId == data.ResourceId && p.ProductId == data.ProductId && p.ProcessId == data.ProcessId && p.ShiftId == data.ShiftId && p.ShiftDate == data.ShiftDate && p.CollectedDate == data.CollectDate && p.DefectId == defect.DefectId && p.CategoryId == defect.CategoryId).Execute();
        }

        /// <summary>
        /// 创建不良缺陷采集信息
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="qty">数量</param>
        /// <param name="defect">缺陷信息</param>
        private void CreateDefectStatics(WipData data, decimal qty, DefectData defect)
        {
            var statics = new DefectStatistics()
            {
                ResourceId = data.ResourceId,
                ResourceName = data.ResourceName,
                ProductId = data.ProductId,
                ProductName = data.ProductName,
                ProcessId = data.ProcessId,
                ProcessName = data.ProcessName,
                ShiftId = data.ShiftId,
                ShiftName = data.ShiftName,
                ShiftDate = data.ShiftDate,
                CollectedDate = data.CollectDate,
                DefectId = defect.DefectId,
                DefectName = defect.DefectName,
                CategoryId = defect.CategoryId,
                CategoryName = defect.CategoryName,
                Qty = qty
            };
            RF.Save(statics);
        }

        /// <summary>
        /// 获取不良缺陷采集信息
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="defectId">缺陷代码ID</param>
        /// <param name="categoryId">缺陷代码分类ID</param>
        /// <returns>不良缺陷采集信息</returns>
        private DefectStatistics GetDefectStatistics(WipData data, double defectId, double categoryId)
        {
            return Query<DefectStatistics>().Where(p => p.ResourceId == data.ResourceId && p.ProductId == data.ProductId && p.ProcessId == data.ProcessId && p.ShiftId == data.ShiftId && p.ShiftDate == data.ShiftDate && p.CollectedDate == data.CollectDate && p.DefectId == defectId && p.CategoryId == categoryId).FirstOrDefault();
        }

        /// <summary>
        /// 获取车间直通率数据信息
        /// </summary>
        /// <param name="shopName">车间名称</param>
        /// <param name="lineName">产线名称</param>
        /// <param name="dateRange">采集日期范围</param>
        /// <param name="info">分页信息</param>
        /// <returns>直通率列表</returns>
        public virtual EntityList<ProcessFpyStatistics> GetShopFpyStatistics(string shopName = null, string lineName = null, DateRange dateRange = null, PagingInfo info = null)
        {
            var query = Query<ProcessFpyStatistics>().Where(p => p.InvOrgId == RT.InvOrg);
            var resController = RT.Service.Resolve<EnterpriseController>();
            if (shopName.IsNotEmpty())
            {
                var shop = resController.GetEnterprises(EnterpriseType.Shop, shopName, string.Empty).FirstOrDefault();
                if (shop == null)
                {
                    return new EntityList<ProcessFpyStatistics>();
                }
                var lineIds = RT.Service.Resolve<WipResourceController>().GetWipResources(new List<ResourceState> { }, shop.Id, pagInfo: null, keyword: string.Empty);
                List<double> idlist = new List<double>();
                if (lineName.IsNotEmpty())
                {
                    idlist = lineIds.Where(p => p.Name == lineName).Select(p => p.Id).ToList();
                }
                else
                {
                    idlist = lineIds.Select(p => p.Id).ToList();
                }
                if (lineIds.Count <= 0)
                {
                    return new EntityList<ProcessFpyStatistics>();
                }
                query.Where(p => idlist.Contains(p.ResourceId));
            }

            if (dateRange != null && dateRange.BeginValue != null)
            {
                query.Where(p => p.CollectedDate >= dateRange.BeginValue);
            }

            if (dateRange != null && dateRange.EndValue != null)
            {
                query.Where(p => p.CollectedDate < dateRange.EndValue);
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取产线直通率数据信息
        /// </summary>
        /// <param name="lineName">产线名称</param>
        /// <param name="shiftName">班次名称</param>
        /// <param name="dateRange">采集日期范围</param>
        /// <param name="info">分页信息</param>
        /// <returns>直通率列表</returns>
        public virtual EntityList<ProcessFpyStatistics> GetLineFpyStatistics(string lineName = null, string shiftName = null, DateRange dateRange = null, PagingInfo info = null)
        {
            var query = Query<ProcessFpyStatistics>().Where(p => p.InvOrgId == RT.InvOrg);
            if (lineName.IsNotEmpty())
            {
                query.Where(p => p.ResourceName.Contains(lineName));
            }

            if (shiftName.IsNotEmpty())
            {
                query.Where(p => p.ShiftName.Contains(shiftName));
            }

            if (dateRange != null && dateRange.BeginValue != null)
            {
                query.Where(p => p.CollectedDate >= dateRange.BeginValue);
            }

            if (dateRange != null && dateRange.EndValue != null)
            {
                query.Where(p => p.CollectedDate < dateRange.EndValue);
            }

            return query.ToList(info);
        }

        /// <summary>
        /// 获取产品直通率数据信息
        /// </summary>
        /// <param name="modelname">产品机型名称</param>
        /// <param name="prodname">产品名字</param>
        /// <param name="dateRange">采集日期范围</param>
        /// <param name="info">分页信息</param>
        /// <returns>直通率列表</returns>
        public virtual EntityList<ProcessFpyStatistics> GetProdcutFpyStatistics(string modelname = null, string prodname = null, DateRange dateRange = null, PagingInfo info = null)
        {
            var query = Query<ProcessFpyStatistics>()
                .Where(p => p.InvOrgId == RT.InvOrg);
            if (modelname.IsNotEmpty())
            {
                query.Join<Item>((x, y) => x.ProductId == y.Id);
                query.Join<Item, ProductModel>((x, y) => x.ModelId == y.Id && y.Name.Contains(modelname));
            }
            if (prodname.IsNotEmpty())
            {
                query.Where(p => p.ProductName.Contains(prodname));
            }

            if (dateRange != null && dateRange.BeginValue != null)
            {
                query.Where(p => p.CollectedDate >= dateRange.BeginValue);
            }

            if (dateRange != null && dateRange.EndValue != null)
            {
                query.Where(p => p.CollectedDate < dateRange.EndValue);
            }

            return query.ToList(info);
        }

        /// <summary>
        /// 获取产线工序缺陷
        /// </summary>
        /// <param name="dateRange">日期范围</param>
        /// <param name="lineName">产线名称</param>
        /// <param name="shiftName">班制名称</param>
        /// <returns>缺陷统计列表</returns>
        public virtual EntityList<DefectStatistics> GetDefectStatisticsForLine(Tuple<DateTime, DateTime> dateRange, string lineName = null, string shiftName = null)
        {
            var query = Query<DefectStatistics>().Where(p => p.CollectedDate >= dateRange.Item1 && p.CollectedDate < dateRange.Item2);

            if (lineName.IsNotEmpty())
                query.Where(p => p.ResourceName == lineName);
            if (shiftName.IsNotEmpty())
                query.Where(p => p.ShiftName == shiftName);

            return query.ToList();
        }

        /// <summary>
        /// 获取产品工序缺陷统计
        /// </summary>
        /// <param name="dateRange">日期范围</param>
        /// <param name="modelName">机型名称</param>
        /// <param name="productName">产品名称</param>
        /// <returns>缺陷统计列表</returns>
        public virtual EntityList<DefectStatistics> GetDefectStatisticsForProd(Tuple<DateTime, DateTime> dateRange, string modelName = null, string productName = null)
        {
            var query = Query<DefectStatistics>().Where(p => p.CollectedDate >= dateRange.Item1 && p.CollectedDate < dateRange.Item2);
            if (modelName.IsNotEmpty())
                query.Join<Item>((x, y) => x.ProductId == y.Id).Join<Item, ProductModel>((x, y) => x.ModelId == y.Id && y.Name == modelName);
            if (productName.IsNotEmpty())
                query.Where(p => p.ProductName == productName);

            return query.ToList();
        }

        /// <summary>
        /// 获取车间直通率数据信息
        /// </summary>
        /// <param name="shopName">车间名称</param>
        /// <param name="lineName">产线名称</param>
        /// <param name="dateRange">采集日期范围</param>
        /// <param name="info">分页信息</param>
        /// <returns>直通率列表</returns>
        public virtual EntityList<ProductFpyStatistics> GetShopProductFpyStatistics(string shopName = null, string lineName = null, DateRange dateRange = null, PagingInfo info = null)
        {
            var resController = RT.Service.Resolve<EnterpriseController>();

            var query = Query<ProductFpyStatistics>()
                .Where(p => p.InvOrgId == RT.InvOrg);
            if (shopName.IsNotEmpty())
            {
                var shop = resController.GetEnterprises(EnterpriseType.Shop, shopName, string.Empty).FirstOrDefault();
                if (shop == null)
                {
                    return new EntityList<ProductFpyStatistics>();
                }

                var lineIds = RT.Service.Resolve<WipResourceController>().GetWipResources(new List<ResourceState> { /*ResourceState.Actived*/ }, shop.Id, pagInfo: null, keyword: string.Empty).Select(p => p.Id).ToList();
                if (lineIds.Count <= 0)
                {
                    return new EntityList<ProductFpyStatistics>();
                }

                query.Where(p => lineIds.Contains(p.ResourceId));
            }

            if (lineName.IsNotEmpty())
            {
                query.Where(p => p.ResourceName.Contains(lineName));
            }

            if (dateRange != null && dateRange.BeginValue != null)
            {
                query.Where(p => p.CollectedDate >= dateRange.BeginValue);
            }

            if (dateRange != null && dateRange.EndValue != null)
            {
                query.Where(p => p.CollectedDate < dateRange.EndValue);
            }

            return query.ToList(info);
        }
    }
}