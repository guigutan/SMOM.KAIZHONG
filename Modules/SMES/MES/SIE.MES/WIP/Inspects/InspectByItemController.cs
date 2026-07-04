using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MES.InspectionStandards;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Inspects
{
    /// <summary>
    /// 检验(项目)采集控制器
    /// </summary>
    public class InspectByItemController : WipController
    {
        /// <summary>
        /// 过站时保存不良缺陷信息、检验项目信息
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">采集运行时产品模型, 记录产品在生产过程中的信息, 通过Puid产品全局ID关联生产信息</param>
        /// <param name="collectBarcodes">采集的条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        protected override void OnWipProductProcessFinished(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> collectBarcodes, CollectData collectData, Workcell workcell)
        {
            if (collectData == null)
            {
                throw new EntityNotFoundException(nameof(collectData));
            }

            if (workcell == null)
            {
                throw new EntityNotFoundException(nameof(workcell));
            }

            if (wipProductProcess == null)
            {
                throw new EntityNotFoundException(nameof(wipProductProcess));
            }

            //添加缺陷记录的方法移到基类WipController
            base.OnWipProductProcessFinished(wipProductProcess, product, collectBarcodes, collectData, workcell);
            var ids = collectData.InspectionItems.Select(m => m.InspectItemId).Distinct().ToList();
            var modelInspectionItemList = RT.Service.Resolve<ModelInspectionItemController>().GetModelInspectionItemByIds(ids);
            var wipProductInspectionItems = new EntityList<WipProductInspectionItem>();
            foreach (var item in collectData.InspectionItems)
            {
                var inspectionItem = modelInspectionItemList.FirstOrDefault(m => m.Id == item.InspectItemId);
                if (inspectionItem ==null)
                {
                    continue;
                }
                WipProductInspectionItem wipProductInspectionItem = new WipProductInspectionItem
                {
                    Name = inspectionItem.Name,
                    LimitLow = inspectionItem.LimitLow,
                    LimitMax = inspectionItem.LimitMax,
                    InspectionValue = item.InspectionValue,
                    Remarks = item.Remarks,
                    Result = item.ItemResult,
                    InspectionItemId = item.InspectItemId,
                    InspectById = workcell.EmployeeId,
                    StationId = workcell.StationId,
                    ProcessId = workcell.ProcessId,
                    ShiftId = wipProductProcess.ShiftId
                };

                wipProductInspectionItem.VersionId = wipProductProcess.VersionId;
                wipProductInspectionItems.Add(wipProductInspectionItem);
              
            }
            RF.Save(wipProductInspectionItems);
        }

        /// <summary>
        /// 根据机型和工序获取检验项目
        /// </summary>
        /// <returns>机型检验项目列表</returns>
        public virtual EntityList<ModelInspectionItem> GetInspectionItems()
        {
            var inspectionItemList = Query<ModelInspectionItem>()
                .Where(p => (p.EffectiveStartTime <= DateTime.Now && (p.EffectiveEndTime > DateTime.Now || p.EffectiveEndTime == null)))
                .ToList();
            return inspectionItemList;
        }

        /// <summary>
        /// 根据产品或产品机型(优先产品)和工序获取检验项目
        /// </summary>
        /// <param name="itemId">产品Id</param>        
        /// <param name="processId">工序ID</param>
        /// <returns>机型检验项目列表</returns>
        public virtual EntityList<ModelInspectionItem> GetInspectionItems(double itemId, double processId)
        {
            //按机型或产品带出检验项目。取值优先级，优先取产品的数据，匹配不到数据再去机型的数据。
            //如果产品有维护生效的数据，则不带出机型的数据。

            EntityList<ModelInspectionItem> inspectionItemList = Query<ModelInspectionItem>()
                .Where(p => p.ProcessId == processId && p.EffectiveStartTime <= DateTime.Now
                    && (p.EffectiveEndTime > DateTime.Now || p.EffectiveEndTime == null))
                .Where(p => p.ProductItemId == itemId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (!inspectionItemList.Any())
            {
                inspectionItemList = Query<ModelInspectionItem>()
                    .Join<Item>((x, y) => x.ModelId == y.ModelId && y.Id == itemId)
                .Where(p => p.ProcessId == processId && p.EffectiveStartTime <= DateTime.Now
                    && (p.EffectiveEndTime > DateTime.Now || p.EffectiveEndTime == null))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }

            return inspectionItemList.OrderBy(x => x.OrderNum).AsEntityList();
        }
    }
}
