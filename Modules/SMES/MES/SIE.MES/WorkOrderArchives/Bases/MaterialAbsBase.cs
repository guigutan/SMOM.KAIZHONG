using SIE.Core.Items;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.LES.StockOrders;
using SIE.MES.LoadItems.Models;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WorkOrderArchives.Bases
{
    /// <summary>
    /// 物料抽象基类
    /// </summary>
    public abstract class MaterialAbsBase
    {
        #region 属性
        /// <summary>
        /// 物料追溯方式
        /// </summary>
        protected List<ItemBatchRule> ItemBatchRules { get; set; }

        /// <summary>
        /// 单体关键件
        /// </summary>
        protected List<WipProductKeyItem> SingleKeyItemList { get; set; }

        /// <summary>
        /// 批次关键件
        /// </summary>
        protected List<WipProductKeyItem> BatchKeyItemList { get; set; }

        /// <summary>
        /// 工单耗用单
        /// </summary>
        protected List<WoCostItemBaseData> WoCostList { get; set; }

        /// <summary>
        /// 物料需求明细
        /// </summary>
        protected EntityList<StockOrderDetail> StockOrderDetails { get; set; }

        /// <summary>
        /// 计算参数集
        /// </summary>
        protected CalculateParameterInfo CalculateParameterInfo { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="calculateParameterInfo"></param>
        protected void Init(CalculateParameterInfo calculateParameterInfo) 
        {
            CalculateParameterInfo = calculateParameterInfo;
        }

        /// <summary>
        /// 获取追溯方式
        /// </summary>
        /// <returns></returns>
        protected RetrospectType GetRetrospectType(double parameter)
        {
            return ItemBatchRules.FirstOrDefault(p => p.ItemId == parameter).RetrospectType;
        }

        /// <summary>
        /// 计算缺料数量
        /// </summary>
        /// <param name="resideQty">(推式)工单剩余需求数量 (拉式)相同线边仓工单剩余需求数量</param>
        /// <param name="availableQty">库存可用量</param>
        /// <param name="feedQty">已上料量</param>
        /// <returns></returns>
        protected decimal CalculateShortQty(decimal resideQty, decimal availableQty, decimal feedQty)
        {
            var shortQty = resideQty - availableQty - feedQty;
            return shortQty > 0 ? shortQty : 0;
        }

        /// <summary>
        /// 计算工单需求数量
        /// </summary>
        /// <param name="singleQty">工单bom单位耗用量</param>
        /// <returns></returns>
        protected decimal CalculateTotalNeedQty(decimal singleQty)
        {
            return CalculateParameterInfo.WorkOrder.PlanQty * singleQty;
        }

        /// <summary>
        /// 计算剩余需求数量
        /// </summary>
        /// <param name="totalNeedQty">工单需求数量</param>
        /// <param name="hasCostQty">工单已耗用数量</param>
        /// <returns></returns>
        protected decimal CalculateResidueNeedQty(decimal totalNeedQty, decimal hasCostQty)
        {
            return totalNeedQty - hasCostQty;
        }
        #endregion

        #region 接口(抽象方法)
        /// <summary>
        /// 准备计算数据
        /// </summary>
        protected abstract void PrepareData();

        /// <summary>
        /// 计算可用量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected abstract decimal CalculateUseAvailableQty(double itemId, string itemExtProp);

        /// <summary>
        /// 计算已上料量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected abstract decimal CalculateFeedQty(double itemId, string itemExtProp);

        /// <summary>
        /// 计算已建备料量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected abstract decimal CalculateStockOrderQty(double itemId, string itemExtProp);

        /// <summary>
        /// 计算备料待接收量
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected abstract decimal CalculateToTakeQty(double itemId, string itemExtProp);

        /// <summary>
        /// 计算已耗用数量
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="productId">工单产品Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        protected abstract decimal CalculateHasCostQty(double woId, double productId, double itemId, string itemExtProp);
        #endregion
    }
}
