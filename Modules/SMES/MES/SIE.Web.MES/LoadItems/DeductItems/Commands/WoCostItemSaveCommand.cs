using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.Common.WorkOrders;
using SIE.MES.LoadItems;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.LoadItems.DeductItems.Commands
{
    /// <summary>
    /// 工单耗用单保持命令
    /// </summary>
    public class WoCostItemSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var costItemEntityList = data as EntityList<WoCostItem>;
            //if (!costItemEntityList.Any())
            //{
            //    throw new ValidationException("无提交数据！");
            //}
            if (costItemEntityList.Any(p => p.RecordType == SIE.MES.LoadItems.Enum.WoCostItemType.DeductItem))
            {
                throw new ValidationException("物料倒扣类型单据不能手动创建".L10N());
            }
            if (costItemEntityList.Any(p => p.CostNo.IsNullOrEmpty()))
            {
                throw new ValidationException("耗用单号不能为空！".L10N());
            }
            if (costItemEntityList.Any(p => p.WorkOrderId == 0))
            {
                throw new ValidationException("工单不能为空！".L10N());
            }
            if (costItemEntityList.Any(p => p.ItemId == 0))
            {
                throw new ValidationException("耗用物料不能为空！".L10N());
            }
            if (costItemEntityList.Any(p => p.CostItemLabelId == 0 || p.CostItemLabelId == null))
            {
                throw new ValidationException("物料标签不能为空！".L10N());
            }
            if (costItemEntityList.Any(p => p.Qty <= 0 ))
            {
                throw new ValidationException("数量必须大于0！".L10N());
            }
            // 物料标签
            var itemLabelIds = costItemEntityList.Select(p => p.CostItemLabelId).ToList();
            var itemLabelList = RT.Service.Resolve<WoCostItemController>().GetItemLabelByIds(itemLabelIds);
            foreach ( var costItem in costItemEntityList)
            {
                var itemLabel = itemLabelList.FirstOrDefault(p => p.Id == costItem.CostItemLabelId);
                if (itemLabel == null)
                {
                    throw new ValidationException("物料标签不存在！".L10N());
                }
                else
                {
                    var qty = costItem.Qty;
                    if (qty > itemLabel.Qty)
                    {
                        throw new ValidationException("数量大于物料标签{0}的可用数量！".L10nFormat(itemLabel.Label));
                    }
                    if (itemLabel.IsSerialNumber.HasValue && itemLabel.IsSerialNumber.Value && (int)qty - qty != 0)
                    {
                        throw new ValidationException("序列号管控物料标签{0}数量必须为整数！".L10nFormat(itemLabel.Label));
                    }
                }
            }
            base.OnSaving(costItemEntityList);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            var dataList = data.Cast<WoCostItem>().ToList();
            var deleteList = data.DeletedList.Cast<WoCostItem>().ToList();
            base.DoSave(data);

            ////更新工单BOM
            dataList.AddRange(deleteList);
            dataList?.Where(p => p.WorkOrderId > 0).GroupBy(p => p.WorkOrderId).ForEach(p =>
            {
                var woId = p.Key;
                var itemIds = p.Select(p => p.ItemId).Distinct().ToList();
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woId, itemIds);
            });
            
        }
    }
}
