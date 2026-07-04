using Microsoft.Scripting.Utils;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.LoadItems;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Assemblys
{
    /// <summary>
    /// 扣料逻辑器
    /// </summary>
    public class UseLoadItemExecutor
    {
        /// <summary>
        /// 上料明细
        /// </summary>
        private readonly EntityList<LoadItem> loadItems;

        /// <summary>
        /// 上料明细的物料标签
        /// </summary>
        private EntityList<ItemLabel> LoadItemLabels;

        /// <summary>
        /// 工单BOM
        /// </summary>
        private readonly EntityList<WorkOrderBom> workOrderBoms;

        /// <summary>
        /// 关键件已上料数量
        /// </summary>
        private readonly List<KeyItemSumInfo> wipProductProcessKeyItems;

        /// <summary>
        /// 工序 BOM 管理 是否考虑工单BOM 配置项
        /// </summary>
        private readonly bool referenceWoBom;

        /// <summary>
        /// 生产采集运行时产品
        /// </summary>
        private readonly product product;

        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly Workcell workcell;
        private readonly ItemDataOwner itemDataOwner;

        /// <summary>
        /// 替代分组
        /// </summary>
        private string AlterGroup;

        /// <summary>
        /// 警告信息
        /// </summary>
        private string WarningMessage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="product">采集运行时产品</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="isBackFlush">是否扣料</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UseLoadItemExecutor(product product, Workcell workcell, bool isBackFlush)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (product.Routing is null)
            {
                throw new EntityNotFoundException("采集运行时产品的工艺路线为空！");
            }

            if (workcell is null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            this.product = product;
            this.workcell = workcell;

            loadItems = RT.Service.Resolve<LoadItemController>()
                    .GetLoadItemEntityList(workcell.ResourceId, workcell.StationId, product.WorkOrderId);

            workOrderBoms = DB.Query<WorkOrderBom>()
             .Where(x => x.WorkOrderId == product.WorkOrderId)
             .ToList();



            //获取工单已扣料数量
            //wipProductProcessKeyItems = DB.Query<WipProductProcessKeyItem>()
            //  .Where(x => x.Process.Version.WorkOrderId == product.WorkOrderId)
            //  .GroupBy(p => p.ItemId)
            //  .Select(p => new { Item_Id = p.ItemId, Qty = p.Qty.SUM() })
            //  .ToList<KeyItemSumInfo>().ToList();
            wipProductProcessKeyItems = new List<KeyItemSumInfo>();

            //工序 BOM 管理 是否考虑工单BOM 配置项
            referenceWoBom = true;
            var referenceWoBomConfigValue = ConfigService.GetConfig(new ReferenceWoBomConfig(), typeof(WorkOrder));
            if (referenceWoBomConfigValue != null)
            {
                referenceWoBom = referenceWoBomConfigValue.ReferenceWoBom;
            }

            //BOM和替代料的物料数据缓存
            IList<bom> boms;
            if (isBackFlush)
            {
                boms = product.Routing.Current.Boms.Where(p => p.IsBuckleMaterial).ToList();
            }
            else
            {
                var routingProcess = product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == workcell.ProcessId);
                if (routingProcess == null)
                {
                    throw new ValidationException("工序不在产品的后工序列表中！".L10N());
                }

                boms = routingProcess.Boms.Where(p => p.IsBuckleMaterial).ToList();
            }

            var itemIds = boms.Select(x => x.ItemId).ToList();
            itemIds.AddRange(boms.SelectMany(x => x.AltBom).Select(x => x.ItemId));
            itemDataOwner = new ItemDataOwner();
            itemDataOwner.GetItemsAndCache(itemIds);

            // 物料标签

            var itemLabelIds = loadItems.Select(p => p.SourceId).ToList();
            LoadItemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabelByIds(itemLabelIds);
        }

        /// <summary>
        /// 验证工序BOM是否够扣料
        /// </summary>
        public void ValidateProcessBom()
        {
            var process = product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == workcell.ProcessId);

            if (process == null)
            {
                process = product.Routing.GetNext().FirstOrDefault();
                if (process == null || (process.Sign & Tech.Routings.RoutingProcessSign.Start) != Tech.Routings.RoutingProcessSign.Start)
                {
                    throw new ValidationException("找不到工艺路线当前工序".L10N());
                }
            }

            foreach (var bom in process.Boms.Where(p => p.IsBuckleMaterial).OrderBy(p => p.ItemId))
            {
                //工序BOM物料既存在工单BOM又存在工序BOM中且是反冲物料,则过站时不触发齐套检
                if (workOrderBoms.FindIndex(x => x.ItemId == bom.ItemId&&x.IsRecoilItem) >= 0)
                {
                    continue;
                }

                if (!TryValidity(loadItems, out ItemLabel itemLabel))
                {
                    throw new LackItemException("标签[{0}]失效".L10nFormat(itemLabel.Label));
                }

                //尝试计算正常扣料
                //尝试计算拉式物料扣料
                if (!TryUseLoadItem(bom, loadItems))
                {
                    if (!WarningMessage.IsNullOrEmpty())
                    {
                        throw new LackItemException(WarningMessage);
                    }
                    else
                    {
                        throw new LackItemException("缺料:物料[{0}],用量[{1}]"
                            .L10nFormat(itemDataOwner.GetItem(bom.ItemId).Code, bom.Qty));
                    }
                }
            }
        }

        /// <summary>
        /// 装配扣料
        /// </summary>
        public void UseLoadItem(WipProductProcess wipProductProcess)
        {

            var needToRecoilIds = new List<double>();
            foreach (var bom in product.Routing.Current.Boms.Where(p => p.IsBuckleMaterial).OrderBy(p => p.ItemId))
            {
                this.WarningMessage = string.Empty;

                //BOM物料既存在工单BOM又存在工序BOM中且是反冲物料,则过站时不触发齐套检
                if (workOrderBoms.FindIndex(x => x.ItemId == bom.ItemId && x.IsRecoilItem) >= 0)
                {
                    //改为标记不扣料
                    bom.IsBuckleMaterial = false;
                    needToRecoilIds.Add(bom.ItemId);
                    continue;
                }

                //尝试正常扣料
                //尝试拉式物料扣料，可扣致负数
                if (!TryUseLoadItem(bom, loadItems, wipProductProcess))
                {
                    if (!WarningMessage.IsNullOrEmpty())
                    {
                        throw new LackItemException(WarningMessage);
                    }
                    else
                    {
                        throw new LackItemException("缺料:物料[{0}],用量[{1}]"
                            .L10nFormat(itemDataOwner.GetItem(bom.ItemId).Code, bom.Qty));
                    }
                }

                //过站后不清空工序bom了，改为标记不扣料
                bom.IsBuckleMaterial = false;
            }
        }

        /// <summary>
        /// 校验上料明细物料标签是否失效
        /// </summary>
        /// <param name="loadItems"></param>
        /// <param name="itemLabel"></param>
        /// <returns></returns>
        private bool TryValidity(ICollection<LoadItem> loadItems, out ItemLabel itemLabel)
        {
            
            foreach(var load in  loadItems)
            {
                itemLabel = LoadItemLabels.FirstOrDefault(p => p.Id == load.SourceId);
                var isVal = AssemblyValityManager.IsValidity(itemLabel);
                if (!isVal)
                    return false;
            }
            itemLabel = null;
            return true;
        }

        /// <summary>
        /// 尝试扣料:<para/>
        /// 1.找出满足BOM属性的上料bomItems。
        /// 2.在bomItems中找到属性相同且数量足够的进行扣料。
        /// 3.扣料失败尝试替代料扣料。
        /// </summary>
        /// <param name="bom">采集运行时工序BOM</param>
        /// <param name="loadItems">上料信息</param>
        /// <param name="wipProductProcess">过站记录，可以为空，为空则不扣料，只作验证计算</param>
        /// <returns>bool</returns>
        private bool TryUseLoadItem(bom bom, ICollection<LoadItem> loadItems, WipProductProcess wipProductProcess = null)
        {
            if (bom is null)
            {
                throw new ArgumentNullException(nameof(bom));
            }
            // 20240320 lyp上料考虑匹配物料扩展属性
            var requireQty = workOrderBoms.Where(x => x.ItemId == bom.ItemId && x.ItemExtProp == bom.ItemExtProp).Sum(x => x.RequireQty);
            var keyItemSumInfo = wipProductProcessKeyItems.FirstOrDefault(x => x.ItemId == bom.ItemId);

            if (keyItemSumInfo == null)
            {
                keyItemSumInfo = new KeyItemSumInfo { ItemId = bom.ItemId, Qty = 0 };
            }

            //满足工序BOM的物料（属性，值满足）
            var samePropertyItems = loadItems
                .Where(p => p.ItemId == bom.ItemId && p.ItemExtProp == bom.ItemExtProp).ToList();

            decimal lackQty = bom.Qty;

            //已使用量加上本次用量大于工单BOM需求量时，则不能再用这个料
            //这里暂不考虑一个物料用部分的问题
            if (samePropertyItems.Sum(p => p.Qty) >= lackQty && CanUseItem(bom, requireQty, keyItemSumInfo))
            {
                if (wipProductProcess != null)
                {
                    UseLoadItem(samePropertyItems, lackQty, wipProductProcess);
                }

                keyItemSumInfo.Qty += lackQty;

                if (!bom.AlterGroup.IsNullOrEmpty())
                {
                    this.AlterGroup = bom.AlterGroup;
                }

                return true;
            }

            if (bom.AltBom != null && bom.AltBom.Any())
            {
                var alterBoms = bom.AltBom.OrderBy(x => x.Priority).ToList();

                foreach (var alt in alterBoms)
                {
                    var success = TryUseLoadItem(alt, loadItems, wipProductProcess);
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
            // 工序 BOM 管理 是否考虑工单BOM 配置项的值为 true
            // 则物料的用量不能超过工单BOM
            if (referenceWoBom && keyItemSumInfo.Qty + bom.Qty > requireQty)
            {
                this.WarningMessage = "物料【{0}】已使用数量【{1}】加上本次用量【{2}】超过工单BOM需求量【{3}】".L10nFormat(
                    itemDataOwner.GetItem(bom.ItemId).Code, keyItemSumInfo.Qty, bom.Qty, requireQty);
                return false;
            }

            //不同替代分组不能同时使用
            if (!bom.AlterGroup.IsNullOrEmpty() && !this.AlterGroup.IsNullOrEmpty() && bom.AlterGroup != this.AlterGroup)
            {
                this.WarningMessage = "物料【{0}】的替代组为【{1}】与已经使用替代组【{2}】不相同"
                    .L10nFormat(itemDataOwner.GetItem(bom.ItemId).Code, bom.AlterGroup, this.AlterGroup);

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
        /// <param name="wipProductProcess">生产采集记录</param>
        private void UseLoadItem(ICollection<LoadItem> loadItems, decimal lackQty, WipProductProcess wipProductProcess)
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

                // 有效期记录开始时间
                var itemLabel = LoadItemLabels.FirstOrDefault(p => p.Id == loadItem.SourceId);
                AssemblyValityManager.ValidityStart(itemLabel, true);

                if (loadItem.Qty == 0)
                {
                    //删除上料记录
                    DB.Delete<LoadItem>()
                        .Where(x => x.Id == loadItem.Id)
                        .Execute();
                    
                    // 有效期记录结束时间
                    AssemblyValityManager.ValidityEnd(itemLabel);
                }
                else
                {
                    DB.Update<LoadItem>()
                        .Set(x => x.Qty, loadItem.Qty)
                        .Where(x => x.Id == loadItem.Id)
                        .Execute();
                }
                RT.Service.Resolve<WipController>().AddKeyItem(wipProductProcess, loadItem, useQty);

                if (itemLabel != null)
                {
                    RF.Save(itemLabel);
                }
            }
        }
    }
}
