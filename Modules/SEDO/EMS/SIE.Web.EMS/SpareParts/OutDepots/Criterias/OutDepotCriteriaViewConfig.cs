using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Criterias;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.OutDepots
{
    /// <summary>
    /// 备件出库查询视图
    /// </summary>
    public class OutDepotCriteriaViewConfig : WebViewConfig<OutDepotCriteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.OutDepotType);
            View.Property(p => p.OutDepotState);
            View.Property(p => p.QualityStatus);
            View.Property(p => p.ReleDoc);
            View.Property(p => p.SourceNo);
            View.Property(p => p.GetDepartment).UseDataSource((e, pagingInfo, keyword) =>
            {
                EnterpriseController enterpriseController = RT.Service.Resolve<EnterpriseController>();
                return enterpriseController.GetDepartmentsWithParent(pagingInfo, keyword);
            });
            View.Property(p => p.WarehouseId);
            View.Property(p => p.SparePartId)
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    m.DicLinkField = keyValues;
                }).UseDataSource((e, p, o) =>
                {
                    return RT.Service.Resolve<SparePartController>().GetSparePartByEquipModelId(p, o, null);
                }).HasLabel("备件编码");
            View.Property(p => p.SparePartName);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; });
        }
    }
}