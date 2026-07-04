using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Service;
using SIE.Warehouses;
using SIE.Web.Command;
using SIE.Web.LES.StockOrders.WorkOrders;
using System;
using System.Linq;

namespace SIE.Web.LES.StockOrders.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AddStockOrderCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var bill = args.Data.ToJsonObject<StockOrder>();

            bill.No = RT.Service.Resolve<StockOrderService>().GetStockOrderNo();
            bill.StockState = StockState.Created;
            bill.BillSource = BillSource.Manual;
            bill.TriggerMode = SIE.LES.Commons.TriggerMode.ManualModel;
            return bill;
        }
    }

    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditStockOrderCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 撤回命令
    /// </summary>
    public class ReCallStockOrderCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 撤回命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<StockOrderService>().ReCallStockOrders(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 审核确认命令
    /// </summary>
    public class AduitStockOrderCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 审核确认命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<StockOrderService>().AduitStockOrders(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 提交审核命令
    /// </summary>
    public class SubmitStockOrderCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 提交审核命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<StockOrderService>().SubmitStockOrders(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 强制关闭命令
    /// </summary>
    public class ForceCloseStockOrderCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 审核命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<StockOrderService>().ForceCloseStockOrders(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveStockOrderCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        protected override void OnSaving(Entity entity)
        {
            StockOrder bill = entity as StockOrder;
            var billData = RF.GetById<StockOrder>(bill.Id);
            var limitedMaximumStock = RT.Service.Resolve<StockOrderService>().GetLimitedMaximumStock();
            if (billData != null && billData.StockState != StockState.Created && billData.StockState != StockState.Audit)
            {
                throw new ValidationException("单号{0}的单据状态不是[创建,待审核]状态,无法保存!".L10nFormat(billData.No));
            }

            if (bill.PersistenceStatus != PersistenceStatus.New)
            {
                var delIdList = bill.StockOrderDetailList.DeletedList.Select(p => p.GetId()).ToList();
                var curDtlIdList = bill.StockOrderDetailList.Select(p => p.Id).ToList();
                var oldDtlIdList = RT.Service.Resolve<StockOrderDetailService>().GetStockOrderDetailList(bill.Id).Where(p => !curDtlIdList.Contains(p.Id) && !delIdList.Contains(p.Id)).ToList();
                bill.StockOrderDetailList.AddRange(oldDtlIdList);
                bill.StockOrderDetailList.Where(x => !curDtlIdList.Contains(x.Id)).ForEach(x => x.PersistenceStatus = PersistenceStatus.Unchanged);
            }

            if ((bill.StockType == PrepareItemType.Push || bill.StockType == PrepareItemType.OverBom) && !bill.WorkOrderId.HasValue)
            {
                throw new ValidationException("备料单的备料模式为[推式或超BOM],工单必须填写".L10N());
            }

            if (bill.DemandMode != SIE.LES.Commons.DemandMode.ManualFillIn && !bill.ResourceId.HasValue)
            {
                throw new ValidationException("备料单的需求计算方式不是[手工填写],生产资源必须填写".L10N());
            }

            if (!bill.StockOrderDetailList.Any())
            {
                throw new ValidationException("备料单的物料需求不能空".L10N());
            }

            if (bill.StockOrderDetailList.Any())
            {
                bill.StockOrderDetailList.ForEach(p =>
                {
                    if (bill.StockType == PrepareItemType.Pull && (!p.WarehouseId.HasValue || !p.StorageLocationId.HasValue))
                    {
                        throw new ValidationException("行号{0}备料单备料模式为拉式时,物料需求的仓库和库位不能为空".L10nFormat(p.LineNo));
                    }

                    if (p.Qty <= 0)
                    {
                        throw new ValidationException("行号{0}本次备料量必须大于0".L10nFormat(p.LineNo));
                    }
                    if (p.Item.EnableExtendProperty&&p.ItemExtProp.IsNullOrEmpty())
                    {
                        throw new ValidationException("行号{0}物料启用了扩展属性，必须填写".L10nFormat(p.LineNo));
                    }

                    if (p.WarehouseId.HasValue && !p.StorageLocationId.HasValue)
                    {
                        throw new ValidationException("行号{0}当备料接收仓库必填，则备料接收库位必填".L10nFormat(p.LineNo));
                    }
                    //如果配置项【推式备料是否限制最高存量】=是，则保存校验的本次需求量是否大于最高存量。大于则提示保存失败。

                    if (limitedMaximumStock)
                    {
                        var maximumStock = 0m;
                        if (bill.StockType == PrepareItemType.Pull)
                        {
                            maximumStock = RT.Service.Resolve<StockOrderService>().GetPrepareItemPullMaximumStock(p.ItemId, p.WarehouseId, p.ItemExtProp);
                        }
                        else// (bill.StockType == PrepareItemType.Push)
                        {
                            maximumStock = RT.Service.Resolve<StockOrderService>().GetBaseItemIoLimit(p.ItemId, p.WarehouseId, p.ItemExtProp);
                        }
                        if (maximumStock>=0&& maximumStock < p.Qty)
                        {
                            throw new ValidationException("行号{0}本次需求量大于最高存量,保存失败,请检查最高存量".L10nFormat(p.LineNo));
                        }
                    }

                });
            }

            //if (bill.PersistenceStatus == PersistenceStatus.New)
            //{
            //    var config = RT.Service.Resolve<StockOrderService>().GetStockOrderConfig();
            //    if (bill.StockType == PrepareItemType.OverBom)
            //    {
            //        bill.StockState = StockState.Audit;
            //    }

            //    if (config != null && config.IsAudit && bill.StockType != PrepareItemType.OverBom)
            //    {
            //        bill.StockState = StockState.Audit;
            //    }
            //}

            bill.StockOrderDetailList.ForEach(p => p.StockState = bill.StockState);

            base.OnSaving(bill);
        }
    }

    /// <summary>
    /// 明细添加命令
    /// </summary>
    public class AddStockOrderDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<StockOrderDetail>();
            if (entity.DemandMode == SIE.LES.Commons.DemandMode.ManualFillIn)
            {
                var receiveType = RT.Service.Resolve<StockOrderService>().GetReceiveType();
                entity.IsManualRec = receiveType == StockReceiveType.Hand;
            }
            return entity.IsManualRec;
        }
    }

    /// <summary>
    /// 批量添加明细
    /// </summary>
    public class MulAddStockOrderDetailCommand : ViewCommand
    {
        /// <summary>
        /// 实现
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<MultAddData>();
            var details = RT.Service.Resolve<StockOrderDetailService>().MultiAddDetail(data.ItemList, data.ResourceId, data.WoId);
            return details;
        }
    }

    /// <summary>
    /// 推式批量添加明细
    /// </summary>
    public class MulAddStockOrderDetailPushCommand : ViewCommand
    {
        /// <summary>
        /// 实现
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<MultAddData>();
            var details = RT.Service.Resolve<StockOrderDetailService>().MultiAddPushDetail(data.ResourceId, data.WoId);
            return details;
        }
    }

    /// <summary>
    /// 批量添加参数
    /// </summary>
    public class MultAddData
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WoId { get; set; }

        /// <summary>
        /// 物料列表
        /// </summary>
        public EntityList<StockOrderItemViewModel> ItemList { get; set; }
    }
}