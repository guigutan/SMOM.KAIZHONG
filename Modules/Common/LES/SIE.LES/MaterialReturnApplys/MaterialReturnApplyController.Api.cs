using SIE.Api;
using SIE.Common.Catalogs;
using SIE.Core.ApiModels;
using SIE.Core.ProjectMaintains;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.LES;
using SIE.EventMessages.MES.WorkOrders;
using SIE.LES.LinesideWarehouses;
using SIE.LES.LinesideWarehouses.Models;
using SIE.LES.MaterialReturnApplys.ApiModels;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请控制器
    /// </summary>
    public partial class MaterialReturnApplyController : DomainController
    {
        /// <summary>
        /// 获取工单相关数据
        /// </summary>
        /// <param name="type">退料模式 0-工单 1-车间</param>
        /// <param name="key">查询关键字 0-工单号 1-车间</param>
        /// <returns></returns>
        [ApiService("获取工单相关数据")]
        [return: ApiReturn("返回退料单需求数据：MaterialWoData")]
        public virtual MaterialWoData GetMaterialReturnData([ApiParameter("退料模式")] int type, [ApiParameter("查询关键字")] string key)
        {
            MaterialWoData data = new MaterialWoData();
            if (type == 0)
            {
                var workOrderInfos = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderSimpleInfos(key);
                data.WorkOrderInfos = workOrderInfos;
                // 工单车间信息
                var workShopIds = data.WorkOrderInfos.Where(p => p.WorkShopId != null).Select(p => p.WorkShopId).Distinct().ToList();
                var resourceIds = data.WorkOrderInfos.Where(p => p.ResourceId != null).Select(p => p.ResourceId).Distinct().ToList();
                var workShopInfos = Query<Enterprise>().Where(p => workShopIds.Contains(p.Id))
                    .Select(p => new
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                    }).ToList<BaseDataInfo>().ToList();
                data.WorkShopInfos = workShopInfos;
                // 产线线边仓
                var linesideInfos = Query<LinesideWarehouse>()
                    .WhereIf(workShopIds.Any(), p => workShopIds.Contains(p.WorkShopId))
                    .WhereIf(resourceIds.Any(), p => resourceIds.Contains(p.WipResouceId))
                    .LeftJoin<Warehouse>((lw, w) => lw.WarehouseId == w.Id)
                    .Select<Warehouse>((lw, w) => new
                    {
                        Id = lw.Id,
                        WorkShopId = lw.WorkShopId,
                        WipResouceId = lw.WipResouceId,
                        WarehouseId = w.Id,
                        WarehouseName = w.Name,
                        StorageLocationId = lw.StorageLocationId,

                    }).ToList<LinesideWareBaseData>().ToList();
                data.WarehouseInfos = linesideInfos;
            }
            else
            {
                var workShopInfos = Query<Enterprise>()
                    .LeftJoin<EnterpriseLevel>((e, el) => e.LevelId == el.Id)
                    .Where<EnterpriseLevel>((e, el) => el.Type == EnterpriseType.Shop)
                    .WhereIf(key.IsNotEmpty(), e => e.Code.Contains(key) || e.Name.Contains(key))
                    .Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        Name = e.Name,
                    }).ToList<BaseDataInfo>().ToList();
                data.WorkShopInfos = workShopInfos;

                var workShopIds = workShopInfos.Select(p => p.Id).ToList();
                // 产线线边仓
                var linesideInfos = Query<LinesideWarehouse>()
                    .Where(p => p.WorkShopId != null && workShopIds.Contains((double)p.WorkShopId))
                    .LeftJoin<Warehouse>((lw, w) => lw.WarehouseId == w.Id)
                    .Select<Warehouse>((lw, w) => new
                    {
                        Id = lw.Id,
                        WorkShopId = lw.WorkShopId,
                        WipResouceId = lw.WipResouceId,
                        WarehouseId = w.Id,
                        WarehouseName = w.Name,
                        StorageLocationId = lw.StorageLocationId,

                    }).ToList<LinesideWareBaseData>().ToList();
                data.WarehouseInfos = linesideInfos;
            }
            // 原因
            var catalogs = RT.Service.Resolve<CatalogController>().GetCatalogList(MaterialReturnApply.MaterialReturnReasonStr);
            List<BaseDataInfo> reasonInfos = new List<BaseDataInfo>();
            foreach(var reason in catalogs)
            {
                BaseDataInfo baseDataInfo = new BaseDataInfo
                {
                    Code = reason.Code,
                    Name = reason.Name,
                };
                reasonInfos.Add(baseDataInfo);
            }
            data.ResonInfos = reasonInfos;

            return data;
        }

        /// <summary>
        /// 查询项目号信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [ApiService("查询项目号信息")]
        [return: ApiReturn("返回项目号列表信息 List<BaseDataInfo>")]
        public virtual List<BaseDataInfo> GetProjectMaintainList([ApiParameter("关键字")] string keyword)
        {
            return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintainBaseInfo(keyword);
        }

        /// <summary>
        /// 查询退料申请明细
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [ApiService("查询退料申请明细")]
        [return: ApiReturn("返回退料申请需求数据 MaterialDtlData")]
        public virtual MaterialDtlData GetMaterialReturnApplyDatas([ApiParameter("页码")] int pageNumber, [ApiParameter("页面大小")] int pageSize, [ApiParameter("退料查询")] MaterialReturnApplyDtlSelCriteria criteria)
        {
            PagingInfo pagingInfo = new PagingInfo
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                IsNeedCount = true,
            };
            criteria.PagingInfo = pagingInfo;
            var details = RT.Service.Resolve<MaterialReturnApplyController>().GetMaterialReturnApplyDetailSelects(criteria);
            var detailList = new List<MaterialReturnApplyDetailSelect>();
            if (criteria.WareId != null && criteria.WoId != null)
            {
                detailList = CaseLpnOnHand(details.ToList(), criteria.WareId, criteria.StorageId);
            }
            else
            {
                detailList.AddRange(details);
            }
            MaterialDtlData materialDtlData = new MaterialDtlData
            {
                TotalCount = details.TotalCount,
            };
            materialDtlData.DetailList.AddRange(detailList);
            return materialDtlData;
        }

        /// <summary>
        /// PDA提交退料数据
        /// </summary>
        /// <param name="mainInfo">主数据</param>
        /// <param name="detail">明细</param>
        [ApiService("PDA提交退料数据")]
        [return: ApiReturn("无")]
        public virtual void SubmitReturnApplyDatas([ApiParameter("主数据")] MaterialReturnApplyMainInfo mainInfo, [ApiParameter("明细")] List<MaterialReturnApplyDetail> detail)
        {
            MaterialReturnApply materialReturnApply = new MaterialReturnApply
            {
                WorkOrderId = mainInfo.WoId,
                WorkShopId = mainInfo.WorkShopId,
                WarehouseId = mainInfo.WarehouseId,
                StorageLocationId = mainInfo.StorageLocationId,
                Reason = mainInfo.Reason,
                ProjectId = mainInfo.ProjectId,
                PersistenceStatus = PersistenceStatus.New,
            };
            int lineNo = 1;
            detail.ForEach(p =>
            {
                p.LineNo = (lineNo++).ToString();
                p.PersistenceStatus = PersistenceStatus.New;
            });
            materialReturnApply.DetailList.AddRange(detail);

            // 校验
            RT.Service.Resolve<MaterialReturnApplyController>().ValidateBeforeSave(materialReturnApply);
            materialReturnApply.No = RT.Service.Resolve<MaterialReturnApplyController>().GetMrNoLists(1).FirstOrDefault();
            materialReturnApply.ReType = materialReturnApply.WorkOrderId != null ? Enums.ReType.WorkOrderReturn : Enums.ReType.WorkShopReturn;
            // 提交
            RT.Service.Resolve<MaterialReturnApplyController>().SaveSubmitReturnApply(materialReturnApply);
        }

        /// <summary>
        /// 扫描实物标签并转化
        /// </summary>
        /// <param name="label">实物标签</param>
        /// <returns></returns>
        [ApiService("扫描实物标签并转化")]
        [return: ApiReturn("扫描内容") ]
        public virtual ScanLabelData GetScanLabelData([ApiParameter("实物标签")] string label)
        {
            var invorg = RT.InvOrg != null ? RT.InvOrg.Value : 1;
            var labelData = RT.Service.Resolve<ILesShippingOrder>().ScanLabel(invorg, label);
            if (labelData != null && labelData.Count > 0)
            {
                return labelData.First();
            }
            else
            {
                return new ScanLabelData();
            }
        }
    }
}
