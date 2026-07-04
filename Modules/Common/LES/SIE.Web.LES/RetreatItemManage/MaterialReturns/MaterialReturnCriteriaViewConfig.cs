using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.RetreatItemManage.MaterialReturns
{
    /// <summary>
    /// 退料查询
    /// </summary>
    public class MaterialReturnCriteriaViewConfig : WebViewConfig<MaterialReturnCriteria>
    {
        /// <summary>
        /// 配置查询
        /// </summary>

        protected override void ConfigQueryView()
        {
            View.Property(p => p.NO).HasLabel("退料单号");
            View.Property(p => p.ReturnType);
            View.Property(p => p.ReturnState);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.Label);
            View.Property(p => p.BatchNO);
            View.Property(p => p.WorkOrder);
            View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("关联工厂");
            View.Property(p => p.WipResource).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(pagingInfo, keyword);
            }).HasLabel("资源编码");
            View.Property(p => p.EmployeeId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            }).HasLabel("提交人");
            View.Property(p => p.SubmitDate).UseDateRangeEditor(p=>p.DateRangeType= ObjectModel.DateRangeType.All).HasLabel("提交时间");

        }

    }
}
