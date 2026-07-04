using SIE.Core.Enums;
using SIE.Inventory.Transactions;
using SIE.LES.LesStockCounts;
using SIE.LES.LesStockCounts.ViewModels;
using SIE.Security;
using System.Collections.Generic;

namespace SIE.Web.LES.LesStockCounts.DataQueryer
{
    /// <summary>
    /// 线边仓盘点数据查询器
    /// </summary>
    [AllowAnonymous]
    public class LesStockCountDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取盘点单状态
        /// </summary>
        /// <param name="id">盘点单ID</param>
        /// <returns>盘点单状态</returns>
        public LesCountState GetLesStockCountState(double id)
        {
            return RT.Service.Resolve<LesStockCountController>().GetLesCountState(id);
        }

        /// <summary>
        /// 获取库存盘点数据
        /// </summary>
        /// <param name="stockCountId">库存盘点Id</param>
        /// <returns>库存盘点数据</returns>
        public object GetFinishCountDetail(double stockCountId)
        {
            return RT.Service.Resolve<LesStockCountController>().FinishedStockCount(stockCountId);
        }

        /// <summary>
        /// 执行完工
        /// </summary>
        /// <param name="stockCountId">库存盘点Id</param>
        /// <param name="viewModel">视图模型</param>   
        /// <returns></returns>
        public List<DiffAdjustViewModel> StockCountFinishDiff(double stockCountId, LesStockCountResult viewModel)
        {             
            return RT.Service.Resolve<LesStockCountController>().FinishedLesStockCount(stockCountId, viewModel);
        }

        /// <summary>
        /// 保存差异调账数据
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="adjustWorkOrderViewModels"></param>
        public bool SaveDiffAdjust(double billId, List<AdjustWorkOrderViewModel> adjustWorkOrderViewModels)
        {
            RT.Service.Resolve<LesStockCountController>().SaveDiffAdjust(billId, adjustWorkOrderViewModels);
            return true;
        }

        /// <summary>
        /// 获取调整数据
        /// </summary>
        /// <param name="countDtlId">盘点明细Id</param>
        /// <returns>调整数据</returns>
        public virtual List<AdjustWorkOrderViewModel> GetAdjustWorkOrderViewModels(double countDtlId)
        {
            return RT.Service.Resolve<LesStockCountController>().GetAdjustWorkOrderViewModels(countDtlId);
        }

        /// <summary>
        /// 获取配置的模板的值
        /// </summary>
        /// <returns>模板数据</returns>
        public StockCountTempleteData GetStockCountNumberRule(OrderType orderType)
        {
            var fun = RT.Service.Resolve<TransactionController>().GetFunctionByType(orderType);
            StockCountTempleteData rst = new StockCountTempleteData()
            {
                PrintBillRuleId = fun?.BillTemplateId,
                PrintBillRuleName = fun?.BillTemplate?.FileName,
            };
            return rst;
        }
    }
    /// <summary>
    /// 库存盘点配置模板
    /// </summary>
    public class StockCountTempleteData
    {
        /// <summary>
        /// 单据模板
        /// </summary>
        public double? PrintBillRuleId { get; set; }

        /// <summary>
        /// 单据模板
        /// </summary>
        public string PrintBillRuleName { get; set; }
    }
}
