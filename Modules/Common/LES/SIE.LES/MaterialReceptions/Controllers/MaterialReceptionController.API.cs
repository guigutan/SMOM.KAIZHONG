using Newtonsoft.Json;
using SIE.Api;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.MaterialReceptions.Services;
using SIE.LES.StockOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.MaterialReceptions.Controllers
{
    /// <summary>
    /// API 相关调用
    /// </summary>
    public partial class MaterialReceptionController : DomainController
    {

        /// <summary>
        /// 按明细接收扫描
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="scanRecords"></param>
        /// <param name="resourcesId"></param>
        /// <returns></returns>
        [ApiService("按明细扫描")]
        [return: ApiReturn("返回可用扫描到数据集合：ScanParamters")]
        public virtual ScanParamters ScanByDetail([ApiParameter("查询关键字")] string keyword,
            [ApiParameter("已扫条码集合")] List<MaterialReceptionInfo> scanRecords, [ApiParameter("当前选择资源")] double? resourcesId)
        {
            var paramters = new ScanParamters(keyword, "", scanRecords, resourcesId,1);
            return RT.Service.Resolve<MaterialReceptionServices>().ScanByDetail(paramters);
        }

        /// <summary>
        /// 按备料单或发货单号获取数据
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [ApiService("按备料单或发货单号获取备料单数据")]
        [return: ApiReturn("返回可用扫描到数据集合：ScanParamters")]
        public virtual ScanParamters ScanOrder([ApiParameter("查询关键字")] string keyword)
        {
            var paramters = new ScanParamters(keyword, "", new List<MaterialReceptionInfo>(), null,2);
            RT.Service.Resolve<MaterialReceptionServices>().ValidateStockOrders(paramters);
            return paramters;
        }

        /// <summary>
        /// 选择或变更备料单后
        /// </summary>
        /// <param name="scanParamters"></param>
        /// <returns></returns>
        [ApiService("选择或变更备料单后")]
        [return: ApiReturn("返回APP上下文：ScanParamters")]
        public virtual ScanParamters ChangedStockOrder([ApiParameter("App上下文数据")] ScanParamters scanParamters)
        {
            RT.Service.Resolve<MaterialReceptionServices>().ValidateChangedStockOrder(scanParamters);
            return scanParamters;
        }

        /// <summary>
        /// 提交接收信息
        /// </summary>
        /// <param name="scanParamters"></param>
        [ApiService("提交接收信息")]
        public virtual void SubmitReceive([ApiParameter("App上下文数据")] List<MaterialReceptionInfo> scanParamters)
        {
            var changedRecord = scanParamters.FindAll(m => m.Qty > 0).Distinct((x,y)=>x.Id==y.Id && x.BillNo==y.BillNo).ToList();
            RT.Service.Resolve<MaterialReceptionServices>().Submit(changedRecord);
        }
    }
}
