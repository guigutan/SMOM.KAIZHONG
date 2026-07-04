using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品版本查询实体
    /// </summary>
    [Label("产品版本查询实体")]
    [QueryEntity, Serializable]
    public class WipProductReportCriteria : WipProductVersionCriteria
    {
        /// <summary>
        /// 获取产品版本列表
        /// </summary>
        /// <returns>产品版本列表</returns>
        protected override EntityList Fetch()
        {
            using (Diagnostics.DebugTrace.Start("生产通用报表查询：".L10N()))
            {
                return RT.Service.Resolve<WipProductReportController>().GetWipProductVersions(this);
            }
        }
    }
}
