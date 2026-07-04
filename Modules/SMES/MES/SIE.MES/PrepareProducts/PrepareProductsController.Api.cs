using SIE.Api;
using SIE.Domain;
using SIE.LES.StockOrders.APIModels;
using SIE.MES.PrepareProducts.ApiModels;
using SIE.MES.PrepareProducts.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PrepareProductsController : DomainController
    {

        /// <summary>
        /// 获取产前准备单据
        /// </summary>
        /// <param name="prepareProductsFilter"></param>
        /// <returns></returns>
        [ApiService("获取产前准备单据")]
        public virtual List<PrepareProductsBill> GetBills([ApiParameter("过滤条件")] PrepareProductsFilter prepareProductsFilter)
        {
            return RT.Service.Resolve<PrepareRecordService>().GetBills(prepareProductsFilter);
        }

        /// <summary>
        /// 获取单据明细
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [ApiService("获取单据明细")]
        public virtual ProjectCheckInfo GetProjectCheckInfo([ApiParameter("单据Id")] double billId)
        {
            return RT.Service.Resolve<PrepareRecordService>().GetProjectCheckInfo(billId);
        }

        /// <summary>
        /// 提交明细数据
        /// </summary>
        /// <param name="ProjectInfos"></param>

        [ApiService("提交明细数据")]
        public virtual void SubmitData([ApiParameter("明细数据")] List<ProjectInfo> ProjectInfos)
        {
             RT.Service.Resolve<PrepareRecordService>().SubmitData(ProjectInfos);
        }

        
    }
}
