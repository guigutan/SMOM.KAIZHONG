using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ShipPlan;
using SIE.ShipPlan.Datas;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ShipPlan.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddDeliveryPlanCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<DeliveryPlan>();
            data.OrderType = SIE.Core.Enums.OrderType.SaleOut;
            data.State = DeliveryState.Created;
            data.SourceType = DeliverySourceType.SelfBuild;
            data.DeliveryDate = DateTime.Now;
            data.NoCreateQty = 0;
            data.DeliveryQty = 0;
            data.CancelQty = 0;
            return data;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteDeliveryPlanCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveDeliveryPlanCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            EntityList<DeliveryPlan> planList = data.CastTo<EntityList<DeliveryPlan>>();
            planList.ForEach(p =>
            {
                if (p.PersistenceStatus == PersistenceStatus.New)
                {
                    p.NoCreateQty = p.RequireQty;
                }

                if (p.CreateQty > p.NoCreateQty)
                    throw new ValidationException("需创单数:[{0}]不能大于未建单数:[{1}]".L10nFormat(p.CreateQty, p.NoCreateQty));

                if (p.ItemId > 0 && p.Item.EnableExtendProperty && p.ItemExtProp.IsNullOrEmpty())
                    throw new ValidationException("物料启用了扩展属性,扩展属性栏位不能为空".L10N());
            });

            base.OnSaving(planList);
        }
    }

    /// <summary>
    /// 审核命令
    /// </summary>
    public class AuditDeliveryPlanCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 审核命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DeliveryPlanController>().AuditDeliveryPlans(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 分配仓库命令
    /// </summary>
    public class AssignWarehouseCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 审核命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DeliveryPlanController>().AssignWarehousePlans(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 强制完成命令
    /// </summary>
    public class ForceCompleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 审核命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DeliveryPlanController>().ForceCompleteDeliveryPlans(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 创建发运订单命令
    /// </summary>
    public class CreateSoCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 审核命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DeliveryPlanController>().CreateSoByDeliveryPlans(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 齐套分析
    /// </summary>
    public class KittingCommand : ViewCommand<KittingData>
    {
        /// <summary>
        /// 齐套分析
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(KittingData args, string scope)
        {
            if (args == null)
            {
                throw new ValidationException("数据有误".L10N());
            }
            var result = RT.Service.Resolve<DeliveryPlanController>().StockPlanAnalys(args.stockIds, args.BuyOnLoad, args.MakeOnLoad);
            return result;
        }
    }
}