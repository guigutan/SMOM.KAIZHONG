using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.LoadItems;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SIE.MES.BatchWIP.Assemlys
{
    /// <summary>
    /// 扣料逻辑器
    /// </summary>
    public class BatchUseLoadItemExecutor
    {
        /// <summary>
        /// 上料明细
        /// </summary>
        private readonly EntityList<LoadItem> loadItems;

        /// <summary>
        /// 工单BOM
        /// </summary>
        private readonly EntityList<WorkOrderBom> workOrderBoms;

        /// <summary>
        /// 关键件已上料数量
        /// </summary>
        private readonly List<KeyItemSumInfo> wipProductProcessKeyItems;

        /// <summary>
        /// 生产采集运行时产品
        /// </summary>
        private readonly product product;

        /// <summary>
        /// 返回关键件列表 交由外部保存
        /// </summary>
        private EntityList<BatchWipProductProcessKeyItem> ReturnBatchWipProductProcessKeyItems { get; set; }
        /// <summary>
        /// 替代分组
        /// </summary>
        private string AlterGroup;

        /// <summary>
        /// 获取生成的关键件
        /// </summary>
        /// <returns></returns>
        public EntityList<BatchWipProductProcessKeyItem> GetKeyItems()
        {
            return ReturnBatchWipProductProcessKeyItems;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="product"></param>
        /// <param name="workcell"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BatchUseLoadItemExecutor(product product, Workcell workcell)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (workcell is null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }
            if (ReturnBatchWipProductProcessKeyItems is null)
            {
                ReturnBatchWipProductProcessKeyItems = new EntityList<BatchWipProductProcessKeyItem>();
            }

            this.product = product;

            loadItems = RT.Service.Resolve<LoadItemController>()
                    .GetLoadItemEntityList(workcell.ResourceId, workcell.StationId, product.WorkOrderId);

            workOrderBoms = DB.Query<WorkOrderBom>()
             .Where(x => x.WorkOrderId == product.WorkOrderId)
             .ToList();

            //获取工单已扣料数量
            wipProductProcessKeyItems = DB.Query<BatchWipProductProcessKeyItem>()
              .Where(x => x.Detail.BatchVersion.WorkOrderId == product.WorkOrderId)
              .GroupBy(p => p.ItemId)
              .Select(p => new { Item_Id = p.ItemId, Qty = p.Qty.SUM() })
              .ToList<KeyItemSumInfo>().ToList();
        }

        /// <summary>
        /// 装配扣料
        /// </summary>
        public void UseLoadItem(OutputBatch outBatch, BatchWipRecord wipProductProcess)
        {
            if (outBatch is null)
            {
                throw new ArgumentNullException(nameof(outBatch));
            }

            if (wipProductProcess == null)
            {
                return;
            }

            foreach (var bom in product.Routing.Current.Boms.Where(p => p.IsBuckleMaterial).OrderBy(p => p.ItemId))
            {
                //尝试正常扣料
                //尝试拉式物料扣料，可扣致负数
                if (!TryUseLoadItem(bom, loadItems, outBatch.Qty, wipProductProcess))
                {
                    throw new LackItemException("缺料:物料[{0}],用量[{1}]"
                        .L10nFormat(RF.GetById<Item>(bom.ItemId).Code, bom.Qty));
                }

                //过站后不清空工序bom了，改为标记不扣料
                bom.IsBuckleMaterial = false;
            }
        }

        /// <summary>
        /// 尝试扣料:<para/>
        /// 1.找出满足BOM属性的上料bomItems。
        /// 2.在bomItems中找到属性相同且数量足够的进行扣料。
        /// 3.扣料失败尝试替代料扣料。
        /// </summary>
        /// <param name="bom">采集运行时工序BOM</param>
        /// <param name="loadItems">上料信息</param>
        /// <param name="batchQty">批次数量</param>
        /// <param name="wipProductProcess">过站记录，可以为空，为空则不扣料，只作验证计算</param>
        /// <returns>bool</returns>
        protected virtual bool TryUseLoadItem(bom bom, ICollection<LoadItem> loadItems, decimal batchQty, BatchWipRecord wipProductProcess = null)
        {
            if (bom is null)
            {
                throw new ArgumentNullException(nameof(bom));
            }

            if (loadItems is null)
            {
                throw new ArgumentNullException(nameof(loadItems));
            }

            var requireQty = workOrderBoms.Where(x => x.ItemId == bom.ItemId).Sum(x => x.RequireQty);
            if (requireQty <= 0)
            {
                throw new LackItemException("物料【{0}】在工序BOM中需扣料，但工单BOM需求数为0，扣料失败！"
                           .L10nFormat(RF.GetById<Item>(bom.ItemId).Name));
            }

            var keyItemSumInfo = wipProductProcessKeyItems.FirstOrDefault(x => x.ItemId == bom.ItemId);

            if (keyItemSumInfo == null)
            {
                keyItemSumInfo = new KeyItemSumInfo { ItemId = bom.ItemId, Qty = 0 };
            }

            //已使用量加上本次用量大于工单BOM需求量时，则不能再用这个料
            //这里暂不考虑一个物料用部分的问题
            if (CanUseItem(bom, requireQty, keyItemSumInfo))
            {
                //满足工序BOM的物料（属性，值满足）
                var samePropertyItems = loadItems.Where(p => p.ItemId == bom.ItemId && p.ItemExtProp == bom.ItemExtProp).ToArray();
                decimal lackQty = bom.Qty * batchQty;

                if (samePropertyItems.Sum(p => p.Qty) >= lackQty)
                {
                    if (wipProductProcess != null)
                    {
                        UseLoadItem(samePropertyItems, lackQty, bom.Qty, wipProductProcess);
                    }

                    return true;
                }
            }
            if (bom.AltBom != null && bom.AltBom.Any())
            {
                var alterBoms = bom.AltBom.OrderBy(x => x.Priority).ToList();
                foreach (var alt in alterBoms)
                {
                    var success = TryUseLoadItem(alt, loadItems, batchQty, wipProductProcess);
                    if (success)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CanUseItem(bom bom, decimal requireQty, KeyItemSumInfo keyItemSumInfo)
        {
            //扣料数量
            if (keyItemSumInfo.Qty + bom.Qty > requireQty)
            {
                return false;
            }

            //不同替代分组不能同时使用
            if (!bom.AlterGroup.IsNullOrEmpty() && !this.AlterGroup.IsNullOrEmpty() && bom.AlterGroup != this.AlterGroup)
            {
                //已经有使用替代分组，且当前BOM有替代分组属性，且替代分组与之前的替代分组不同，则不能使用这个物料
                return false;
            }

            return true;
        }


        /// <summary>
        /// 根据用量扣减上料，要求上料已按扣料顺序排序
        /// </summary>
        /// <param name="loadItems">上料信息</param>
        /// <param name="lackQty">缺料数量</param>
        /// <param name="singleQty">单位用量</param>
        /// <param name="wipProductProcess">生产采集记录明细</param>
        protected virtual void UseLoadItem(ICollection<LoadItem> loadItems, decimal lackQty, decimal singleQty,
            BatchWipRecord wipProductProcess)
        {
            if (loadItems is null)
            {
                throw new ArgumentNullException(nameof(loadItems));
            }

            foreach (var loadItem in loadItems)
            {
                if (lackQty == 0m)
                {
                    break;
                }

                var useQty = 0m;
                decimal qty = loadItem.Qty;
                if (qty >= lackQty)
                {
                    useQty = lackQty;
                }
                else
                {
                    useQty = qty;
                }

                lackQty = lackQty - useQty;
                loadItem.Qty = qty - useQty;

                if (loadItem.Qty == 0)
                {
                    //删除上料记录
                    DB.Delete<LoadItem>()
                        .Where(x => x.Id == loadItem.Id)
                        .Execute();
                }
                else
                {
                    DB.Update<LoadItem>()
                        .Set(x => x.Qty, loadItem.Qty)
                        .Where(x => x.Id == loadItem.Id)
                        .Execute();
                }

                AddKeyItem(wipProductProcess, loadItem, useQty, singleQty);

                var loadItemEvent = new LoadItemEvent()
                {
                    ItemCode = loadItem.Item.Code,
                    ItemName = loadItem.Item.Name,
                    Qty = useQty,
                    ItemLable = loadItem.SourceCode,
                    RemainQty = loadItem.Qty
                };

                RT.RemotingEventBus.Publish(loadItemEvent);
            }
        }

        /// <summary>
        /// 添加关键件到工序过站记录，添加物料属性到产品属性
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="loadItem">上料</param>
        /// <param name="qty">数量</param>
        /// <param name="singleQty">单位用量</param>
        private void AddKeyItem(BatchWipRecord wipProductProcess, LoadItem loadItem, decimal qty, decimal singleQty)
        {
            var wipProductProcessKeyItem = new BatchWipProductProcessKeyItem
            {
                ItemId = loadItem.ItemId,
                Item = loadItem.Item,
                ItemExtProp = loadItem.ItemExtProp,
                ItemExtPropName = loadItem.ItemExtPropName,
                SourceCode = loadItem.SourceCode,
                SourceId = loadItem.SourceId,
                SourceType = loadItem.SourceType,
                Qty = qty,
                SingleQty = singleQty,
                ProcessId = wipProductProcess.ProcessId,
                ResourceId = wipProductProcess.ResourceId,
                StationId = wipProductProcess.StationId,
            };

            wipProductProcessKeyItem.DetailId = wipProductProcess.Id;
            ReturnBatchWipProductProcessKeyItems.Add(wipProductProcessKeyItem);
            //RF.Save(wipProductProcessKeyItem);
        }

    }
}
