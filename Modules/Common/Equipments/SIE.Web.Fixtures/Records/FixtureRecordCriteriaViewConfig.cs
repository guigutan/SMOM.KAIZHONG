using SIE.Fixtures;
using SIE.Fixtures.FixtureRecords;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.Records
{
    /// <summary>
    /// 
    /// </summary>
    public class FixtureRecordCriteriaViewConfig : WebViewConfig<FixtureRecordCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.RecordType);
            View.Property(p => p.BusinessType);
            View.Property(p => p.Code);
            View.Property(p => p.FixtureAccountCode);
            View.Property(p => p.FixtureEncodeId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodes(keyword, pagingInfo);
            });
            View.Property(p => p.FixtureWarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).HasLabel("仓库");
            View.Property(p => p.FixtureModelId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureModels(null,pagingInfo, keyword);
            }).HasLabel("工治具型号");
            View.Property(p => p.ApplyDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
            View.Property(p => p.ComplyDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
        }
    }


}
